// CFolderHomepageCtl.cpp : Implementation of CFolderHomepageCtl

#include "stdafx.h"
#include "FolderHomepageCtl.h"
#include <SHLGUID.h>

#define IfFailGo(expr) { hr = (expr); if (FAILED(hr)) { goto Error; } }

int CFolderHomepageCtl::s_cInstances;

// CFolderHomepageCtl
CFolderHomepageCtl::CFolderHomepageCtl()
:
    m_cRef(1)
{
    ATLTRACE(L"In CFolderHomepageCtl[%d]::CFolderHomepageCtl\n", m_id);

    m_id = s_cInstances++;
}

CFolderHomepageCtl::~CFolderHomepageCtl()
{
    ATLTRACE( L"In CFolderHomepageCtl[%d]::~CFolderHomepageCtl\n", m_id);
}

STDMETHODIMP CFolderHomepageCtl::GetWinFormsControl(Outlook::_Application* pApp)
{
    ATLTRACE( L"In function CFolderHomepageCtl[%d]::GetWinFormsControl\n", m_id);

    HRESULT hr = S_OK;

    CComPtr<COMAddIns> srpAddIns;
    CComPtr<COMAddIn> srpAddIn = NULL;
    VARIANT varResult;

    // Get Outlook Com AddIn Collection
    hr = pApp->get_COMAddIns(&srpAddIns);
	IfFailGo(hr)

    // Find the FolderHomepage Addin
    //hr = srpAddIns->Item(&CComVariant(L"FolderHomepageSampleAddIn"), &srpAddIn);
	hr = srpAddIns->Item(&CComVariant(L"OMSOffice2007"), &srpAddIn);
	IfFailGo(hr)

    // Get the addin object
    ATLASSERT( m_srpAddInObject == NULL);
    if (m_srpAddInObject != NULL) {
        m_srpAddInObject.Release();
    }

    hr = srpAddIn->get_Object(&m_srpAddInObject);
	IfFailGo(hr)

    if (m_srpAddInObject == NULL) {
        hr = E_FAIL;
        return hr;
    }

    //hr = m_srpAddInObject.Invoke0(L"GetWinFormsControl", &varResult);
	hr = m_srpAddInObject.Invoke0(L"GetFolderHomePage", &varResult);
	IfFailGo(hr)

    ATLASSERT( V_VT(&varResult) == VT_UNKNOWN || V_VT(&varResult) == VT_DISPATCH );

    if (!(V_VT(&varResult) == VT_UNKNOWN || V_VT(&varResult) == VT_DISPATCH))  {
        hr = E_UNEXPECTED;
        goto Error;
    }

    // Save the reference into the global variable
    m_srpWinFormsDashboardControl = V_UNKNOWN(&varResult);
	
    hr = CFolderHomepageCtl::QueryAndGetObjects();
	IfFailGo(hr)

Error:
    return hr;
}

STDMETHODIMP CFolderHomepageCtl::Initialize(IOleClientSite* pClientSite)
{
    ATLTRACE( L"In function CFolderHomepageCtl[%d]::Initialize\n", m_id);

    HRESULT hr = S_OK;

    CComPtr<IWebBrowserApp> srpWebBrowserApp;
    CComPtr<IDispatch> srpDocument;
    CComPtr<IServiceProvider> srpSP;
    CComPtr<IDispatch> srpScript;
    CComVariant cvarExternal;
    CComPtr<IDispatch> srpExternal;
    CComVariant cvarOutlookApplication;
    CComPtr<Outlook::_Application> m_srpApp;
    CComVariant cvarScript;

    hr = pClientSite->QueryInterface(__uuidof(srpSP), (void**)&srpSP);
	IfFailGo(hr)

    hr = srpSP->QueryService(SID_SWebBrowserApp, IID_IWebBrowserApp, (void **)&srpWebBrowserApp);
	IfFailGo(hr)

    hr = srpWebBrowserApp->get_Document(&srpDocument);
	IfFailGo(hr)

    hr = srpDocument.GetPropertyByName(L"Script", &cvarScript);
	IfFailGo(hr)

    srpScript = V_DISPATCH(&cvarScript);
    hr = srpScript.GetPropertyByName(L"external", &cvarExternal);
	IfFailGo(hr)

    srpExternal = V_DISPATCH(&cvarExternal);
    hr = srpExternal.GetPropertyByName(L"OutlookApplication", &cvarOutlookApplication);
	IfFailGo(hr)

    hr = V_DISPATCH(&cvarOutlookApplication)->QueryInterface(__uuidof(m_srpApp), (void**)&m_srpApp);
	IfFailGo(hr)

    hr = CFolderHomepageCtl::GetWinFormsControl(m_srpApp);
	IfFailGo(hr)

Error:
    return hr;
}

STDMETHODIMP CFolderHomepageCtl::QueryAndGetObjects()
{
    ATLTRACE( L"In function CFolderHomepageCtl[%d]::QueryAndGetObjects\n", m_id);
    HRESULT hr = S_OK;
    
    CComPtr<IUnknown> srpControl;

	ATLASSERT(m_srpWinFormsDashboardControl);

    srpControl = m_srpWinFormsDashboardControl;

    hr = srpControl->QueryInterface(&m_srpOleControl);
	IfFailGo(hr)

    hr = srpControl->QueryInterface(&m_srpOleInPlaceObject);
	IfFailGo(hr)

    hr = srpControl->QueryInterface(&m_srpOleObject);
	IfFailGo(hr)

    hr = srpControl->QueryInterface(&m_srpOleInPlaceActiveObject);
	IfFailGo(hr)

    hr = srpControl->QueryInterface(&m_srpViewObject);
	IfFailGo(hr)

    hr = srpControl->QueryInterface(&m_srpViewObject2);
	IfFailGo(hr)

    hr = srpControl->QueryInterface(&m_srpDisposable);
	IfFailGo(hr)

Error:
   return hr;
}

// IDispatch
HRESULT __stdcall CFolderHomepageCtl::GetTypeInfoCount(UINT* pctinfo)
{
    ATLTRACE(_T("CFolderHomepageCtl(IDispatch)::GetTypeInfoCount\n"));

    *pctinfo = 0;

    return S_OK;
}

HRESULT __stdcall CFolderHomepageCtl::GetTypeInfo(UINT itinfo, LCID lcid, ITypeInfo** pptinfo)
{    
    ATLTRACE(_T("CFolderHomepageCtl(IDispatch)::GetTypeInfo\n"));
    return E_NOTIMPL;
}

HRESULT __stdcall CFolderHomepageCtl::GetIDsOfNames(REFIID riid, LPOLESTR* rgszNames, UINT cNames,
        LCID lcid, DISPID* rgdispid)
{
    ATLTRACE(_T("CFolderHomepageCtl(IDispatch)::GetIDsOfNames\n"));
    return E_NOTIMPL;
}

HRESULT __stdcall CFolderHomepageCtl::Invoke(DISPID dispidMember, REFIID riid,
        LCID lcid, WORD wFlags, DISPPARAMS* pdispparams, VARIANT* pvarResult,
        EXCEPINFO* pexcepinfo, UINT* puArgErr)
{        
    ATLTRACE(_T("CFolderHomepageCtl(IDispatch)::Invoke\n"));    
    return E_NOTIMPL;
}

// IPersist
STDMETHODIMP CFolderHomepageCtl::GetClassID(CLSID *pClassID)
{
    ATLTRACE(_T("CFolderHomepageCtl(IPersist)::GetClassID\n"));

    *pClassID = IID_NULL;
    return E_NOTIMPL;
}

// IOleControl
STDMETHODIMP CFolderHomepageCtl::GetControlInfo(_Out_ CONTROLINFO *pCI)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleControl)::GetControlInfo\n" , m_id);
   return m_srpOleControl->GetControlInfo(pCI);
}

STDMETHODIMP CFolderHomepageCtl::OnMnemonic(MSG *pMsg)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleControl)::OnMnemonic\n" , m_id);

   IfPtrNullFail(m_srpOleControl)
   return m_srpOleControl->OnMnemonic(pMsg);
}

STDMETHODIMP CFolderHomepageCtl::OnAmbientPropertyChange(DISPID dispID)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleControl)::OnAmbientPropertyChange\n" , m_id);

   IfPtrNullFail(m_srpOleControl)
   return m_srpOleControl->OnAmbientPropertyChange(dispID);
}

STDMETHODIMP CFolderHomepageCtl::FreezeEvents(BOOL bFreeze)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleControl)::FreezeEvents\n" , m_id);

   // FreezeEvents tells the control that container will not accept any events
   // Not forwarding this call is just fine if we do not have anyone to forward it to
   if (m_srpOleControl == NULL)
       return S_OK;

   return m_srpOleControl->FreezeEvents(bFreeze);
}


// IOleWindow
STDMETHODIMP CFolderHomepageCtl::GetWindow(_Out_ HWND *phwnd)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleWindow)::GetWindow\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceObject)
   return m_srpOleInPlaceObject->GetWindow(phwnd);
}

STDMETHODIMP CFolderHomepageCtl::ContextSensitiveHelp(BOOL fEnterMode)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleWindow)::ContextSensitiveHelp\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceObject)
   return m_srpOleInPlaceObject->ContextSensitiveHelp(fEnterMode);
}


// IOleInPlaceActiveObject
STDMETHODIMP CFolderHomepageCtl::TranslateAccelerator(LPMSG lpmsg)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceActiveObject)::TranslateAccelerator\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceActiveObject)
   return m_srpOleInPlaceActiveObject->TranslateAccelerator(lpmsg);
}

STDMETHODIMP CFolderHomepageCtl::OnFrameWindowActivate(BOOL fActivate)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceActiveObject)::OnFrameWindowActivate\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceActiveObject)
   return m_srpOleInPlaceActiveObject->OnFrameWindowActivate(fActivate);
}

STDMETHODIMP CFolderHomepageCtl::OnDocWindowActivate(BOOL fActivate)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceActiveObject)::OnDocWindowActivate\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceActiveObject)
   return m_srpOleInPlaceActiveObject->OnDocWindowActivate(fActivate);
}

STDMETHODIMP CFolderHomepageCtl::ResizeBorder(LPCRECT prcBorder, IOleInPlaceUIWindow *pUIWindow, BOOL fFrameWindow)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceActiveObject)::ResizeBorder\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceActiveObject)
   return m_srpOleInPlaceActiveObject->ResizeBorder(prcBorder, pUIWindow, fFrameWindow);
}

STDMETHODIMP CFolderHomepageCtl::EnableModeless(BOOL fEnable)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceActiveObject)::EnableModeless\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceActiveObject)
   return m_srpOleInPlaceActiveObject->EnableModeless(fEnable);
}


// IOleInPlaceObject
STDMETHODIMP CFolderHomepageCtl::InPlaceDeactivate( void)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceObject)::InPlaceDeactivate\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceObject)
   return m_srpOleInPlaceObject->InPlaceDeactivate();
}

STDMETHODIMP CFolderHomepageCtl::UIDeactivate( void)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceObject)::UIDeactivate\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceObject)
   return m_srpOleInPlaceObject->UIDeactivate();
}

STDMETHODIMP CFolderHomepageCtl::SetObjectRects(LPCRECT lprcPosRect, LPCRECT lprcClipRect)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceObject)::SetObjectRects\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceObject)
   return m_srpOleInPlaceObject->SetObjectRects(lprcPosRect, lprcClipRect);
}

STDMETHODIMP CFolderHomepageCtl::ReactivateAndUndo( void)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleInPlaceObject)::ReactivateAndUndo\n" , m_id);

   IfPtrNullFail(m_srpOleInPlaceObject)
   return m_srpOleInPlaceObject->ReactivateAndUndo();
}

// IOleObject
STDMETHODIMP CFolderHomepageCtl::SetClientSite(IOleClientSite *pClientSite)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::SetClientSite\n" , m_id);

   HRESULT hr = S_OK;

   if (pClientSite  != NULL) {
       hr = CFolderHomepageCtl::Initialize(pClientSite);
       if (FAILED(hr))
           return hr;
   }

   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->SetClientSite(pClientSite);
}

STDMETHODIMP CFolderHomepageCtl::GetClientSite(_Deref_out_ IOleClientSite **ppClientSite)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetClientSite\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->GetClientSite(ppClientSite);
}

STDMETHODIMP CFolderHomepageCtl::SetHostNames(LPCOLESTR szContainerApp, LPCOLESTR szContainerObj)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::SetHostNames\n" , m_id);
   if (m_srpOleObject == NULL)
   {
      m_cbstrContainerApp = szContainerApp;
      m_cbstrContainerObj = szContainerObj;

      return S_OK;
   }
   else
   {
      return m_srpOleObject->SetHostNames(szContainerApp, szContainerObj);
   }
}

STDMETHODIMP CFolderHomepageCtl::Close(DWORD dwSaveOption)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::Close\n" , m_id);   
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->Close(dwSaveOption);
}

STDMETHODIMP CFolderHomepageCtl::SetMoniker(DWORD dwWhichMoniker, IMoniker *pmk)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::SetMoniker\n" , m_id);   
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->SetMoniker(dwWhichMoniker, pmk);
}

STDMETHODIMP CFolderHomepageCtl::GetMoniker(DWORD dwAssign, DWORD dwWhichMoniker, _Deref_out_ IMoniker **ppmk)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetMoniker\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->GetMoniker(dwAssign, dwWhichMoniker, ppmk);
}

STDMETHODIMP CFolderHomepageCtl::InitFromData(IDataObject *pDataObject, BOOL fCreation, DWORD dwReserved)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::InitFromData\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->InitFromData(pDataObject, fCreation, dwReserved);
}

STDMETHODIMP CFolderHomepageCtl::GetClipboardData(DWORD dwReserved, _Deref_out_ IDataObject **ppDataObject)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetClipboardData\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->GetClipboardData(dwReserved, ppDataObject);
}

STDMETHODIMP CFolderHomepageCtl::DoVerb(LONG iVerb, LPMSG lpmsg, IOleClientSite *pActiveSite, LONG lindex, HWND hwndParent, LPCRECT lprcPosRect)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::DoVerb\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->DoVerb(iVerb, lpmsg, pActiveSite, lindex, hwndParent, lprcPosRect);
}

STDMETHODIMP CFolderHomepageCtl::EnumVerbs(_Deref_out_ IEnumOLEVERB **ppEnumOleVerb)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::EnumVerbs\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->EnumVerbs(ppEnumOleVerb);
}

STDMETHODIMP CFolderHomepageCtl::Update(void)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::Update\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->Update();
}

STDMETHODIMP CFolderHomepageCtl::IsUpToDate(void)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::IsUpToDate\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->IsUpToDate();
}

STDMETHODIMP CFolderHomepageCtl::GetUserClassID(_Out_ CLSID *pClsid)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetUserClassID\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->GetUserClassID(pClsid);
}

STDMETHODIMP CFolderHomepageCtl::GetUserType(DWORD dwFormOfType, _Deref_out_ LPOLESTR *pszUserType)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetUserType\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->GetUserType(dwFormOfType, pszUserType);
}

STDMETHODIMP CFolderHomepageCtl::SetExtent(DWORD dwDrawAspect, SIZEL *psizel)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::SetExtent\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->SetExtent(dwDrawAspect, psizel);
}

STDMETHODIMP CFolderHomepageCtl::GetExtent(DWORD dwDrawAspect, _Out_ SIZEL *psizel)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetExtent\n" , m_id);
   if (m_srpOleObject == NULL)
   {
      return OLE_E_BLANK;
   }
   else
   {
      return m_srpOleObject->GetExtent(dwDrawAspect, psizel);
   }
}

STDMETHODIMP CFolderHomepageCtl::Advise(IAdviseSink *pAdvSink, _Out_ DWORD *pdwConnection)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::Advise\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->Advise(pAdvSink, pdwConnection);
}

STDMETHODIMP CFolderHomepageCtl::Unadvise(DWORD dwConnection)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::Unadvise\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->Unadvise(dwConnection);
}

STDMETHODIMP CFolderHomepageCtl::EnumAdvise(_Deref_out_ IEnumSTATDATA **ppenumAdvise)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::EnumAdvise\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->EnumAdvise(ppenumAdvise);
}

STDMETHODIMP CFolderHomepageCtl::GetMiscStatus(DWORD dwAspect, _Out_ DWORD *pdwStatus)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::GetMiscStatus\n" , m_id);
   *pdwStatus = OLEMISC_ACTIVATEWHENVISIBLE | OLEMISC_INSIDEOUT | OLEMISC_SETCLIENTSITEFIRST | OLEMISC_RECOMPOSEONRESIZE;

   return S_OK;
}

STDMETHODIMP CFolderHomepageCtl::SetColorScheme(LOGPALETTE *pLogpal)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IOleObject)::SetColorScheme\n" , m_id);
    
   IfPtrNullFail(m_srpOleObject)
   return m_srpOleObject->SetColorScheme(pLogpal);
}


// IViewObject
STDMETHODIMP CFolderHomepageCtl::Draw(DWORD dwDrawAspect, LONG lindex, void *pvAspect, DVTARGETDEVICE *ptd, HDC hdcTargetDev, HDC hdcDraw, LPCRECTL lprcBounds, LPCRECTL lprcWBounds, BOOL ( STDMETHODCALLTYPE *pfnContinue )(ULONG_PTR dwContinue), ULONG_PTR dwContinue)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject)::Draw\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject)
   return m_srpViewObject->Draw(dwDrawAspect, lindex, pvAspect, ptd, hdcTargetDev, hdcDraw, lprcBounds, lprcWBounds, pfnContinue, dwContinue);
}

STDMETHODIMP CFolderHomepageCtl::GetColorSet(DWORD dwDrawAspect, LONG lindex, void *pvAspect, DVTARGETDEVICE *ptd, HDC hicTargetDev, _Deref_out_ LOGPALETTE **ppColorSet)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject)::GetColorSet\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject)
   return m_srpViewObject->GetColorSet(dwDrawAspect, lindex, pvAspect, ptd, hicTargetDev, ppColorSet);
}

STDMETHODIMP CFolderHomepageCtl::Freeze( DWORD dwDrawAspect, LONG lindex, void *pvAspect, _Out_ DWORD *pdwFreeze)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject)::Freeze\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject)
   return m_srpViewObject->Freeze(dwDrawAspect, lindex, pvAspect, pdwFreeze);
}

STDMETHODIMP CFolderHomepageCtl::Unfreeze(DWORD dwFreeze)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject)::Unfreeze\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject)
   return m_srpViewObject->Unfreeze(dwFreeze);
}

STDMETHODIMP CFolderHomepageCtl::SetAdvise(DWORD aspects, DWORD advf, IAdviseSink *pAdvSink)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject)::SetAdvise\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject)
   return m_srpViewObject->SetAdvise(aspects, advf, pAdvSink);
}

STDMETHODIMP CFolderHomepageCtl::GetAdvise(_Out_ DWORD *pAspects, _Out_ DWORD *pAdvf, _Deref_out_ IAdviseSink **ppAdvSink)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject)::GetAdvise\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject)
   return m_srpViewObject->GetAdvise(pAspects, pAdvf, ppAdvSink);
}

// IViewObject2
STDMETHODIMP CFolderHomepageCtl::GetExtent(DWORD dwDrawAspect, LONG lindex, DVTARGETDEVICE *ptd, LPSIZEL lpsizel)
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IViewObject2)::GetExtent\n" , m_id);
    
   IfPtrNullFail(m_srpViewObject2)
   return m_srpViewObject2->GetExtent(dwDrawAspect, lindex, ptd, lpsizel);
}

// IDisposable
STDMETHODIMP CFolderHomepageCtl::Dispose()
{
   ATLTRACE( L"In function CFolderHomepageCtl[%d](IDisposable)::Dispose\n" , m_id);

   IfPtrNullFail(m_srpDisposable)
   HRESULT hr = m_srpDisposable->Dispose();
   m_srpDisposable.Release();

   return hr;
}


