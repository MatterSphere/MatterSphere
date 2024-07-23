// MessageInterceptor.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"


typedef LRESULT (CALLBACK *MSG_CALLBACK)(UINT msg, WPARAM wParam, LPARAM lParam);

//
// PInvoke API
//
//	C# Equivalent
//
//	public delegate IntPtr WndProc(uint msg, IntPtr wParam, IntPtr lParam);
//
//	[DllImport("MessageInterceptor.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//	public static extern bool HookWndProc(IntPtr hWnd);
//
//	[DllImport("MessageInterceptor.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//	public static extern void RegisterWndProc(WndProc msgCallback);
//
//	[DllImport("MessageInterceptor.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//	public static extern void UnregisterWndProc();
//

//
// Prototypes
//
LRESULT CALLBACK WndProc(HWND hWindow,
	UINT message, 
	WPARAM wParam, 
	LPARAM lParam, 
	UINT_PTR uIdSubclass, 
	DWORD_PTR dwRefData);

//
// Globals
//
static bool g_initialised = false;
static MSG_CALLBACK g_callback = NULL;

//
// API
//
//
// Hooks
//
extern "C" BOOL WINAPI HookWndProc(HWND hWnd)
{
	if (!g_initialised)
	{
		UINT_PTR uIdSubclass = 0;
		return g_initialised = SetWindowSubclass(hWnd, WndProc, uIdSubclass, 0) == TRUE;
	}

	return FALSE;
}

void WINAPI RegisterWndProc2(void (CALLBACK *mgdFunc)(const char* str))
{
	mgdFunc("Call to managed function");
}

extern "C" void WINAPI RegisterWndProc(MSG_CALLBACK mgdFunc)
{
	g_callback = mgdFunc;
}

extern "C" void WINAPI UnregisterWndProc(void)
{
	g_callback = NULL;
}


//
// Windows Proc
//
LRESULT CALLBACK WndProc(HWND hWnd, 
	UINT message, 
	WPARAM wParam, 
	LPARAM lParam, 
	UINT_PTR uIdSubclass, 
	DWORD_PTR dwRefData)
{
	switch (message)
	{
	case WM_CLOSE:
		// do we have a callback?
		if (g_callback != NULL)
		{
			LRESULT result = FALSE;
			// yes, callback - cater for any exceptions!
			try
			{
				result = g_callback(message, wParam, lParam);
			}
			catch(...)
			{
				// remove callback
				g_callback = NULL;
				result = FALSE;
			}

			// do we pass the message on?
			if (result == TRUE)
			{
				// no, don't pass it on
				return result;
			}
		}
		break;

	case WM_NCDESTROY:
		// We must remove the window subclass before the window being subclassed is destroyed
		if (!RemoveWindowSubclass(hWnd, WndProc, uIdSubclass))
		{
			// Error in removing!
			// L"RemoveWindowSubclass in handling WM_NCDESTROY"
		}
		else
		{
			g_initialised = false;
		}
		break;
	}

	// pass it on
	return DefSubclassProc(hWnd, message, wParam, lParam);
}

