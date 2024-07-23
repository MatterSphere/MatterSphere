// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#define _SILENCE_STDEXT_HASH_DEPRECATION_WARNINGS
#include <hash_map>

static const DWORD MAX_DIALOG_TYPE = 12;
static const DWORD MAX_WNDCLASS_NAME = 256;
static const DWORD MAX_TITLE_NAME = 64;
static const DWORD MAX_REG_BUFF = 64;

typedef struct tagDLGCREATION
{
	BOOL init;
	BOOL shown;
	WNDPROC proc;
} DLGCREATION;

typedef struct tagDIALOGCONFIG
{
	TCHAR Type[MAX_DIALOG_TYPE];
	TCHAR Class[MAX_WNDCLASS_NAME];
	TCHAR Title[MAX_TITLE_NAME];
} DIALOGCONFIG, *LPDIALOGCONFIG;

typedef struct tagPROCESSCONFIG
{
	LPTSTR ProcessName;
	DIALOGCONFIG ** Dialogs;
	DWORD DialogCount;
} PROCESSCONFIG;


static stdext::hash_map<HWND, DLGCREATION*> activewindows;

static HHOOK hcbthook = NULL;
static PROCESSCONFIG *ProcessConfig = NULL;
static UINT WM_DLGINIT = 0;
static UINT WM_DLGSHOW = 0;
static UINT WM_DLGDESTROYED = 0;
static const LPCTSTR WINCAPTURE = _T("{9029E4B1-6954-4b8b-B57F-7ABE954A9D4A}");
static LPTSTR procname = NULL;

static const LPCTSTR OMSUTILS_PROCESSNAME = _T("OMS.UTILS.EXE");					// uppercase as subroutine converts to uppercase
static BOOL isValidProcess = FALSE;													// indicates if this is a process we need to handle the events for
static HMODULE Module = NULL;														// this DLL handle
static HANDLE hEvent = NULL;														// event handle

LRESULT CALLBACK CBTProc(int nCode, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK WinProc(HWND hWnd,UINT Msg,WPARAM wParam,LPARAM lParam);
LPTSTR GetProcessName(HANDLE hProcess);
bool IsValidProcess(LPCTSTR processName);
bool IsValidWindow(HWND hWnd, LPCREATESTRUCT pCreateWnd);
void CentreIfOffScreen(HWND hwnd);
void MoveOffScreen(HWND hwnd);

PROCESSCONFIG * GetProcessConfig();
PROCESSCONFIG * GetProcessConfigByKey(CRegKey * key);
void MergeDialogConfigByKey(CRegKey key, LPTSTR name, DIALOGCONFIG * config);
void MergeDialogConfig(DIALOGCONFIG * config, DIALOGCONFIG * master);
void MergeProcessConfig(PROCESSCONFIG * config, PROCESSCONFIG * master);
bool MatchConfigKey(LPTSTR name, CRegKey * key, CRegKey * childkey);
void FreeProcessConfig(PROCESSCONFIG * config);


BOOL APIENTRY DllMain( HMODULE hModule,
					   DWORD  ul_reason_for_call,
					   LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
			{
				Module = hModule;
				procname = GetProcessName(NULL);
				ProcessConfig = GetProcessConfig();

				// is this a process we should handle?
				isValidProcess = IsValidProcess(procname);
				if (isValidProcess)
				{
					//Register the message that will be thrown back to the client.
					WM_DLGINIT = RegisterWindowMessage(_T("WM_DLGINIT"));
					WM_DLGSHOW = RegisterWindowMessage(_T("WM_DLGSHOW"));
					WM_DLGDESTROYED = RegisterWindowMessage(_T("WM_DLGDESTROYED"));
				}

				// is OMS.Utils.exe loading us? 
				if (lstrcmp(procname, OMSUTILS_PROCESSNAME) == 0)
				{
					// yes, set up hook
					hcbthook = SetWindowsHookEx(WH_CBT, CBTProc, hModule, 0);
				}

				// disable DLL_THREAD_ATTACH and DLL_THREAD_DETACH callbacks for performance
				DisableThreadLibraryCalls(hModule);

				// we are ready to go...
				return TRUE;
		}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		// should not get called, disabled above ...
		break;
	case DLL_PROCESS_DETACH:
		{
			if (procname != NULL)
			{
				delete[] procname;
				procname = NULL;
			}
			FreeProcessConfig(ProcessConfig);
			if (hcbthook != NULL)
			{
				// unhook since OMS.Utils.exe is unloading
				UnhookWindowsHookEx(hcbthook);
				hcbthook = NULL;
			}

			// close event handle
			if (hEvent != NULL)
			{
				CloseHandle(hEvent);
				hEvent = NULL;
			}
		}
		break;
	}
	return TRUE;
}

// Injects secondary DialogInterceptor Hook for another bitness processes.
// >rundll32.exe OMS.DialogInterceptor.Hook.dll,SetSecondaryHook UtilsPID
void CALLBACK SetSecondaryHook(HWND hWnd, HINSTANCE hInstance, LPSTR lpszCmdLine, int nCmdShow)
{
	DWORD dwProcessId = atol(lpszCmdLine);
	HANDLE hProcess = dwProcessId != 0 ? OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION | SYNCHRONIZE, FALSE, dwProcessId) : NULL;
	if (hProcess == NULL) return;

	LPTSTR lpProcessName = GetProcessName(hProcess);
	if (lstrcmp(lpProcessName, OMSUTILS_PROCESSNAME) == 0)
	{
		TCHAR szEventName[40]; // Manual reset event should have been be created in OMS.Utils and it will be set to signaled state to indicate unhook.
		wsprintf(szEventName, _T("Local\\%s:%u"), OMSUTILS_PROCESSNAME, dwProcessId);
		hEvent = OpenEvent(SYNCHRONIZE, FALSE, szEventName);
		if (hEvent != NULL)
		{
			hcbthook = SetWindowsHookEx(WH_CBT, CBTProc, Module, 0);
			if (hcbthook != NULL)
			{
				HANDLE handles[2] = { hEvent, hProcess }; // Wait until Monitor Save As becomes unchecked or OMS.Utils process exits.
				WaitForMultipleObjects(2, handles, FALSE, INFINITE);
			}
		}
	}
	delete[] lpProcessName;
	CloseHandle(hProcess);
}
  
LRESULT CALLBACK CBTProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	// Only handle the callback if we are interested in this process
	if (isValidProcess)
	{
		switch (nCode)
		{
			case HCBT_CREATEWND:
				{
					if (IsValidWindow((HWND)wParam, ((LPCBT_CREATEWND)lParam)->lpcs))
					{
						decltype(activewindows)::const_iterator it = activewindows.find((HWND)wParam);
						if (it == activewindows.cend())
						{
							activewindows.emplace((HWND)wParam,
								new DLGCREATION { FALSE, FALSE, (WNDPROC)SetWindowLongPtr((HWND)wParam, GWLP_WNDPROC, (LONG_PTR)WinProc) });
						}
					}
				}
				break;

			case HCBT_DESTROYWND:
				{
					decltype(activewindows)::const_iterator it = activewindows.find((HWND)wParam);
					if (it != activewindows.cend())
					{
						SetWindowLongPtr(it->first, GWLP_WNDPROC, (LONG_PTR)it->second->proc);
						WinProc(it->first, WM_DESTROY, 0, 0); // Fake call to send a notification
						delete it->second;
						activewindows.erase(it);
					}
				}
				break;
		}
	}

	return CallNextHookEx(hcbthook, nCode, wParam, lParam);
}


LRESULT CALLBACK WinProc(HWND hWnd,UINT Msg,WPARAM wParam,LPARAM lParam)
{
	DLGCREATION* dg = activewindows.at(hWnd);
	WNDPROC proc = dg->proc;


	switch (Msg)
	{
		case WM_CREATE:
			{
				dg->init = TRUE;
				HWND h = FindWindow(NULL,WINCAPTURE );

				if (h != NULL)
				{
					MoveOffScreen(hWnd);
				}
			}
			break;
		case WM_INITDIALOG:
			{
				dg->init = TRUE;
				HWND h = FindWindow(NULL,WINCAPTURE );			

				if (h != NULL)
				{
					LRESULT res = CallWindowProc(proc, hWnd, Msg, wParam, lParam);

					MoveOffScreen(hWnd);

					PostMessage(h, WM_DLGINIT, (WPARAM)hWnd, NULL);
					return res;
				}
			}
			break;
		case WM_SETTEXT:
			{
			}
			break;
		case WM_DESTROY:
			{
				HWND h = FindWindow(NULL,WINCAPTURE);

				if (h != NULL)
				{
					PostMessage(h, WM_DLGDESTROYED, (WPARAM)hWnd, NULL);
				}
				return 0;
			}
			break;
		
		case WM_SHOWWINDOW:
		{
			if (wParam == TRUE && dg->shown == FALSE)
			{
				dg->shown = TRUE;
				HWND h = FindWindow(NULL, WINCAPTURE);

				if (h != NULL)
				{				
					PostMessage(h, WM_DLGSHOW, (WPARAM)hWnd, NULL);
				}

			}
		}
		break;
	}


	return CallWindowProc(proc, hWnd, Msg, wParam, lParam);

}

void CentreIfOffScreen(HWND hWnd)
{
	RECT rectwork;
	HMONITOR hmon = MonitorFromWindow(GetParent(hWnd), MONITOR_DEFAULTTONEAREST);
	if (hmon == NULL)
	{
		SystemParametersInfo(SPI_GETWORKAREA, 0, (PVOID)&rectwork, 0);
	}
	else
	{
		MONITORINFO moninfo;
		moninfo.cbSize = sizeof(MONITORINFO);
		GetMonitorInfo(hmon, &moninfo);
		rectwork = moninfo.rcWork;
	}


	RECT parentrect;
	GetWindowRect(GetParent(hWnd), &parentrect);

	RECT winrect;
	GetWindowRect(hWnd, &winrect);

	RECT rect;

	rect.left = parentrect.left + ((parentrect.right - parentrect.left) - (winrect.right - winrect.left)) / 2;
	rect.top = parentrect.top +((parentrect.bottom - parentrect.top) - (winrect.bottom - winrect.top)) / 2;
	rect.right = (winrect.right - winrect.left);
	rect.bottom = (winrect.bottom - winrect.top);

	/*if (rect.left < rectwork.left)
	{
		LONG lOffset = (rectwork.left - rect.left);
		rect.left += lOffset;
		rect.right += lOffset;
	}
	if (rect.right > rectwork.right)
	{
		LONG lOffset = (rect.right - rectwork.right);
		rect.left -= lOffset;
		rect.right -= lOffset;
	}
	if (rect.top < rectwork.top)
	{
		LONG lOffset = (rectwork.top - rect.top);
		rect.top += lOffset;
		rect.bottom += lOffset;
	}
	if (rect.bottom > rect.bottom)
	{
		LONG lOffset = (rect.bottom - rectwork.bottom);
		rect.top -= lOffset;
		rect.bottom -= lOffset;
	}*/

	MoveWindow(hWnd, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, TRUE);

}

void MoveOffScreen(HWND hWnd)
{
	RECT rect;
	GetWindowRect(hWnd, &rect);

	SIZE screen;
	screen.cx = GetSystemMetrics(SM_CXSCREEN);
	screen.cy = GetSystemMetrics(SM_CYSCREEN);

	MoveWindow(hWnd, screen.cx, screen.cy, rect.right - rect.left, rect.bottom - rect.top, TRUE);
}


void FreeProcessConfig(PROCESSCONFIG * config)
{
	if (config != NULL)
	{
		for(DWORD ctr = 0; ctr < config->DialogCount; ctr++)
		{
			DIALOGCONFIG * dlgconfig = config->Dialogs[ctr];
			free(dlgconfig);
		}

		if (config->Dialogs != NULL)
			free(config->Dialogs);

		free(config);
		config = NULL;
	}
}

PROCESSCONFIG * GetProcessConfig()
{
	CRegKey lm_pol_key;
	CRegKey cu_pol_key;
	CRegKey lm_key;
	CRegKey cu_key;

	PROCESSCONFIG * lm_pol_cfg = NULL;
	PROCESSCONFIG * cu_pol_cfg = NULL;
	PROCESSCONFIG * lm_cfg = NULL;
	PROCESSCONFIG * cu_cfg = NULL;

	if (lm_pol_key.Open(HKEY_LOCAL_MACHINE, _T("Software\\Policies"), KEY_READ) == ERROR_SUCCESS)
	{
		lm_pol_cfg = GetProcessConfigByKey(&lm_pol_key);
		lm_pol_key.Close();
	}
	if (cu_pol_key.Open(HKEY_CURRENT_USER, _T("Software\\Policies"), KEY_READ) == ERROR_SUCCESS)
	{
		cu_pol_cfg = GetProcessConfigByKey(&cu_pol_key);
		cu_pol_key.Close();
	}
	if (lm_key.Open(HKEY_LOCAL_MACHINE, _T("Software"), KEY_READ) == ERROR_SUCCESS)
	{
		lm_cfg = GetProcessConfigByKey(&lm_key);
		lm_key.Close();
	}
	if (cu_key.Open(HKEY_CURRENT_USER, _T("Software"), KEY_READ) == ERROR_SUCCESS)
	{
		cu_cfg = GetProcessConfigByKey(&cu_key);
		cu_key.Close();
	}

	PROCESSCONFIG * config = (PROCESSCONFIG*)calloc(1, sizeof(PROCESSCONFIG));

	MergeProcessConfig(config, lm_pol_cfg);
	MergeProcessConfig(config, cu_pol_cfg);
	MergeProcessConfig(config, lm_cfg);
	MergeProcessConfig(config, cu_cfg);

	FreeProcessConfig(lm_pol_cfg);
	FreeProcessConfig(cu_pol_cfg);
	FreeProcessConfig(lm_cfg);
	FreeProcessConfig(cu_cfg);

	return config;
}

PROCESSCONFIG * GetProcessConfigByKey(CRegKey * key)
{
	PROCESSCONFIG * config = NULL;

	CRegKey dialogsKey;
	
	long dlgskeyres = dialogsKey.Open(*key, _T("FWBS\\OMS\\DialogInterceptor\\Dialogs"), KEY_READ);
	
	CRegKey procskey;
	if (procskey.Open(*key, _T("FWBS\\OMS\\DialogInterceptor\\Processes"), KEY_READ) == ERROR_SUCCESS)
	{
		CRegKey processKey;

		if (MatchConfigKey(procname, &procskey, &processKey))
		{
			
			config = (PROCESSCONFIG*)calloc(1, sizeof(PROCESSCONFIG));
			config->ProcessName = procname;


			CRegKey procdialogKey;
			DWORD iIndex = 0;
			DWORD iKeyNameLen = MAX_REG_BUFF;
			TCHAR KeyName[MAX_REG_BUFF];

			while (processKey.EnumKey(iIndex++, KeyName, &iKeyNameLen) == ERROR_SUCCESS)
			{
				if (procdialogKey.Open(processKey, KeyName, KEY_READ) == ERROR_SUCCESS)
				{
					DIALOGCONFIG * procdlgconfig = (DIALOGCONFIG*)calloc(1, sizeof(DIALOGCONFIG));

					MergeDialogConfigByKey(procdialogKey, KeyName, procdlgconfig);

					if (dlgskeyres == ERROR_SUCCESS)
					{
						CRegKey dlgkey;
						if (MatchConfigKey(KeyName, &dialogsKey, &dlgkey))
						{
							DIALOGCONFIG * dlgconfig = (DIALOGCONFIG*)calloc(1, sizeof(DIALOGCONFIG));

							MergeDialogConfigByKey(dlgkey, KeyName, dlgconfig);
							MergeDialogConfig(procdlgconfig, dlgconfig);

							dlgkey.Close();

							free(dlgconfig);

						}			
					}

					config->Dialogs = (DIALOGCONFIG **)realloc(config->Dialogs, iIndex * sizeof(DIALOGCONFIG*));
					config->Dialogs[iIndex-1] = procdlgconfig;			

					procdialogKey.Close();

				}

				iKeyNameLen = MAX_REG_BUFF;

			}
			
			config->DialogCount = iIndex -1;

			processKey.Close();
		}

		procskey.Close();
	}

	dialogsKey.Close();

	return config;
}

bool MatchConfigKey(LPTSTR name, CRegKey * key, CRegKey * childkey)
{
	DWORD iIndex = 0;
	DWORD iKeyNameLen=MAX_REG_BUFF;
	TCHAR KeyName[MAX_REG_BUFF];

	while (key->EnumKey(iIndex++, KeyName, &iKeyNameLen) == ERROR_SUCCESS)
	{
		_tcsupr_s(name, MAX_REG_BUFF);
		_tcsupr_s(KeyName, MAX_REG_BUFF);
		if (lstrcmp(name, KeyName) == 0)
		{
			if (childkey->Open(*key, KeyName, KEY_READ) == ERROR_SUCCESS)
			{
				return true;
			}
		}

		iKeyNameLen = MAX_REG_BUFF;
	}

	return false;
}

void MergeProcessConfig(PROCESSCONFIG * config, PROCESSCONFIG * master)
{
	if (config == NULL || master == NULL)
		return;

	if (config->ProcessName == NULL || _tcslen(config->ProcessName) == 0)
		config->ProcessName = master->ProcessName;

	if (config->Dialogs == NULL)
		config->Dialogs = (DIALOGCONFIG **)calloc(max(config->DialogCount, 1), sizeof(DIALOGCONFIG*));

	for(DWORD ctr = 0; ctr < master->DialogCount; ctr++)
	{
		DIALOGCONFIG * dlgmaster = master->Dialogs[ctr];
		DIALOGCONFIG * dlgconfig = NULL;

		for(DWORD ctr2 = 0; ctr2 < config->DialogCount; ctr2++)
		{
			if (lstrcmp(config->Dialogs[ctr2]->Type, dlgmaster->Type) == 0)
			{
				dlgconfig = config->Dialogs[ctr2];
				break;
			}
		}

		if (dlgconfig == NULL)
		{
			dlgconfig = (DIALOGCONFIG*)calloc(1, sizeof(DIALOGCONFIG));
	
			config->DialogCount++;
			config->Dialogs = (DIALOGCONFIG **)realloc(config->Dialogs, (config->DialogCount) * sizeof(DIALOGCONFIG*));
			config->Dialogs[config->DialogCount-1] = dlgconfig;
		}

		MergeDialogConfig(dlgconfig, dlgmaster);		
	}

}

void MergeDialogConfig(DIALOGCONFIG * config, DIALOGCONFIG * master)
{
	if (_tcslen(config->Type) == 0)
	{
		_tcscpy_s(config->Type, MAX_DIALOG_TYPE, master->Type);
	}

	if (_tcslen(config->Class) == 0)
	{
		_tcscpy_s(config->Class, MAX_WNDCLASS_NAME, master->Class);
	}

	if (_tcslen(config->Title) == 0)
	{
		_tcscpy_s(config->Title, MAX_TITLE_NAME, master->Title);
	}
}

void MergeDialogConfigByKey(CRegKey key, LPTSTR name, DIALOGCONFIG * config)
{
	if (_tcslen(config->Type) == 0)
	{
		_tcscpy_s(config->Type, MAX_DIALOG_TYPE, name);
		_tcsupr_s(config->Type, MAX_DIALOG_TYPE);
	}

	if (_tcslen(config->Class) == 0)
	{
		TCHAR classname[MAX_WNDCLASS_NAME];
		ULONG classnamelen = MAX_WNDCLASS_NAME;

		if (key.QueryStringValue(_T("WindowClass"), classname, &classnamelen) == ERROR_SUCCESS)
		{
			_tcsupr_s(classname, MAX_WNDCLASS_NAME);
			_tcscpy_s(config->Class, MAX_WNDCLASS_NAME, classname);
		}
	}

	if (_tcslen(config->Title) == 0)
	{
		TCHAR title[MAX_TITLE_NAME];
		ULONG titlelen = MAX_TITLE_NAME;

		if (key.QueryStringValue(_T("WindowTitle"), title, &titlelen) == ERROR_SUCCESS)
		{
			_tcsupr_s(title, MAX_TITLE_NAME);
			_tcscpy_s(config->Title, MAX_TITLE_NAME, title);
		}
	}

}

bool IsValidProcess(LPCTSTR processName)
{
	if (ProcessConfig == NULL)
		return false;

	return (lstrcmp(processName, ProcessConfig->ProcessName) == 0);
}

bool IsValidWindow(HWND hWnd, LPCREATESTRUCT pCreateWnd)
{
	if (ProcessConfig == NULL)
		return false;

	if ((pCreateWnd->style & WS_CHILD) != 0)
		return false; // Ignore child controls

	TCHAR ClassName[MAX_WNDCLASS_NAME];
	if (RealGetWindowClass(hWnd, ClassName, MAX_WNDCLASS_NAME))
	{
		_tcsupr_s(ClassName, MAX_WNDCLASS_NAME);
		for(DWORD ctr = 0; ctr < ProcessConfig->DialogCount; ctr++)
		{
			if (lstrcmp(ClassName, ProcessConfig->Dialogs[ctr]->Class) == 0)
			{
				// Ignore MessageBoxes
				return (pCreateWnd->hInstance != GetModuleHandle(_T("User32.dll")) && pCreateWnd->hInstance != GetModuleHandle(_T("Comctl32.dll")));
			}
		}
	}

	return false;
}


LPTSTR GetProcessName(HANDLE hProcess)
{
	LPTSTR lpModuleName = new TCHAR[MAX_PATH];
	DWORD nChars = MAX_PATH;
	if (hProcess == NULL)
	{
		nChars = GetModuleFileName(NULL, lpModuleName, nChars);
	}
	else if (!QueryFullProcessImageName(hProcess, 0, lpModuleName, &nChars))
	{
		lpModuleName[0] = _T('\0');
	}
	TCHAR* pSlash = _tcsrchr(lpModuleName, _T('\\'));
	if (pSlash != NULL)
	{
#ifdef _UNICODE
#define tmemmove wmemmove
#else
#define tmemmove memmove
#endif
		tmemmove(lpModuleName, pSlash + 1, nChars - (pSlash - lpModuleName));
		_tcsupr_s(lpModuleName, MAX_PATH);
	}
	return lpModuleName;
}