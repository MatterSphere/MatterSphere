// FolderHomepageCtl.h : Declaration of the CFolderHomepageCtl

#pragma once

#include "resource.h"       // main symbols
#include "FolderHomepageControlShim_i.h"

//#import "libid:00020430-0000-0000-C000-000000000046" raw_interfaces_only  // OLE Automation
#import "libid:2DF8D04C-5BFA-101B-BDE5-00AA0044DE52" raw_interfaces_only auto_rename // Microsoft Office Object Library
#import "libid:00062FFF-0000-0000-C000-000000000046" raw_interfaces_only auto_rename // Microsott Outlook Object Library

#import <mscorlib.tlb> rename_namespace("Mscorlib") raw_interfaces_only auto_rename // For IDisposable

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace Office;

#define IfPtrNullFail(p) { if(p == NULL) return E_UNEXPECTED; }

// CFolderHomepageCtl
class ATL_NO_VTABLE CFolderHomepageCtl : 
    public CComObjectRootEx<CComSingleThreadModel>,
    public CComCoClass<CFolderHomepageCtl, &CLSID_FolderHomepageCtl>,
    public CComControl<CFolderHomepageCtl>,
    public IOleObject,
    public IOleControl,
    public IOleInPlaceObject,
    public IOleInPlaceActiveObject,
    public IViewObject2,
    public Mscorlib::IDisposable
{
public:
    CFolderHomepageCtl();
    ~CFolderHomepageCtl();

DECLARE_OLEMISC_STATUS(OLEMISC_RECOMPOSEONRESIZE |
    OLEMISC_CANTLINKINSIDE |
    OLEMISC_INSIDEOUT |
    OLEMISC_ACTIVATEWHENVISIBLE |
    OLEMISC_SETCLIENTSITEFIRST
)

DECLARE_REGISTRY_RESOURCEID(IDR_FOLDERHOMEPAGECTL)

BEGIN_COM_MAP(CFolderHomepageCtl)
    COM_INTERFACE_ENTRY(Mscorlib::IDisposable)
    COM_INTERFACE_ENTRY(IViewObject2)
    COM_INTERFACE_ENTRY(IViewObject)
    COM_INTERFACE_ENTRY(IOleInPlaceObject)
    COM_INTERFACE_ENTRY(IOleInPlaceActiveObject)
    COM_INTERFACE_ENTRY(IOleControl)
    COM_INTERFACE_ENTRY(IOleObject)
END_COM_MAP()

    STDMETHOD (GetTypeInfoCount)(UINT* pctinfo);
    HRESULT STDMETHODCALLTYPE GetTypeInfo( 
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ __RPC__deref_out_opt ITypeInfo **ppTInfo);
        
    HRESULT STDMETHODCALLTYPE GetIDsOfNames( 
            /* [in] */ __RPC__in REFIID riid,
            /* [size_is][in] */ __RPC__in_ecount_full(cNames) LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ __RPC__out_ecount_full(cNames) DISPID *rgDispId);
        
    HRESULT STDMETHODCALLTYPE Invoke( 
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);


    DECLARE_PROTECT_FINAL_CONSTRUCT()

    HRESULT FinalConstruct()
    {
        return S_OK;
    }

    void FinalRelease()
    {
		//HRESULT hr = S_OK;

		//DISPPARAMS dp = {NULL, NULL, 0, 0}; 

		// Call Dispose on WinFroms control
		Dispose();

		// m_srpAddInObject is NULL when the addin is disabled
		if(m_srpAddInObject != NULL)
		{
			// Invoke clean up method on WinForms control
			m_srpAddInObject.Invoke0(L"WinFormsControlCleanUp");
		}
    }

public: 

    // IPersist
    STDMETHOD(GetClassID)(CLSID* pClassID);

    // IOleControl implementation
    STDMETHOD(GetControlInfo)(_Out_ CONTROLINFO *pCI);
    STDMETHOD(OnMnemonic)(MSG *pMsg);
    STDMETHOD(OnAmbientPropertyChange)(DISPID dispID);
    STDMETHOD(FreezeEvents)(BOOL bFreeze);

    // IOleWindow
    STDMETHOD(GetWindow)(_Out_ HWND *phwnd);
    STDMETHOD(ContextSensitiveHelp)(BOOL fEnterMode);

    // IOleInPlaceActiveObject 
    STDMETHOD(TranslateAccelerator)(LPMSG lpmsg);
    STDMETHOD(OnFrameWindowActivate)(BOOL fActivate);
    STDMETHOD(OnDocWindowActivate)(BOOL fActivate);
    STDMETHOD(ResizeBorder)(LPCRECT prcBorder, IOleInPlaceUIWindow *pUIWindow, BOOL fFrameWindow);
    STDMETHOD(EnableModeless)(BOOL fEnable);

    // IOleInPlaceObject
    STDMETHOD(InPlaceDeactivate)( void);
    STDMETHOD(UIDeactivate)( void);
    STDMETHOD(SetObjectRects)(LPCRECT lprcPosRect, LPCRECT lprcClipRect);
    STDMETHOD(ReactivateAndUndo)( void);

    //IOleObject
    STDMETHOD(SetClientSite)(IOleClientSite *pClientSite);
    STDMETHOD(GetClientSite)(_Deref_out_ IOleClientSite **ppClientSite);
    STDMETHOD(SetHostNames)(LPCOLESTR szContainerApp, LPCOLESTR szContainerObj);
    STDMETHOD(Close)(DWORD dwSaveOption);
    STDMETHOD(SetMoniker)(DWORD dwWhichMoniker, IMoniker *pmk);
    STDMETHOD(GetMoniker)(DWORD dwAssign, DWORD dwWhichMoniker, _Deref_out_ IMoniker **ppmk);
    STDMETHOD(InitFromData)(IDataObject *pDataObject, BOOL fCreation, DWORD dwReserved);
    STDMETHOD(GetClipboardData)(DWORD dwReserved, _Deref_out_ IDataObject **ppDataObject);
    STDMETHOD(DoVerb)(LONG iVerb, LPMSG lpmsg, IOleClientSite *pActiveSite, LONG lindex, HWND hwndParent, LPCRECT lprcPosRect);
    STDMETHOD(EnumVerbs)(_Deref_out_ IEnumOLEVERB **ppEnumOleVerb);
    STDMETHOD(Update)(void);
    STDMETHOD(IsUpToDate)(void);
    STDMETHOD(GetUserClassID)(_Out_ CLSID *pClsid);
    STDMETHOD(GetUserType)(DWORD dwFormOfType, _Deref_out_ LPOLESTR *pszUserType);
    STDMETHOD(SetExtent)(DWORD dwDrawAspect, SIZEL *psizel);
    STDMETHOD(GetExtent)(DWORD dwDrawAspect, _Out_ SIZEL *psizel);
    STDMETHOD(Advise)(IAdviseSink *pAdvSink, _Out_ DWORD *pdwConnection);
    STDMETHOD(Unadvise)(DWORD dwConnection);
    STDMETHOD(EnumAdvise)(_Deref_out_ IEnumSTATDATA **ppenumAdvise);
    STDMETHOD(GetMiscStatus)(DWORD dwAspect, _Out_ DWORD *pdwStatus);
    STDMETHOD(SetColorScheme)(LOGPALETTE *pLogpal);

    // IOleViewObject
    STDMETHOD(Draw)(DWORD dwDrawAspect, LONG lindex, void *pvAspect, DVTARGETDEVICE *ptd, HDC hdcTargetDev, HDC hdcDraw, LPCRECTL lprcBounds, LPCRECTL lprcWBounds, BOOL ( STDMETHODCALLTYPE *pfnContinue )(ULONG_PTR dwContinue), ULONG_PTR dwContinue);
    STDMETHOD(GetColorSet)(DWORD dwDrawAspect, LONG lindex, void *pvAspect, DVTARGETDEVICE *ptd, HDC hicTargetDev, _Deref_out_ LOGPALETTE **ppColorSet);
    STDMETHOD(Freeze)(DWORD dwDrawAspect, LONG lindex, void *pvAspect, _Out_ DWORD *pdwFreeze);
    STDMETHOD(Unfreeze)(DWORD dwFreeze);
    STDMETHOD(SetAdvise)(DWORD aspects, DWORD advf, IAdviseSink *pAdvSink);
    STDMETHOD(GetAdvise)(_Out_ DWORD *pAspects, _Out_ DWORD *pAdvf, _Deref_out_ IAdviseSink **ppAdvSink);

    // IOleViewObject2
    STDMETHOD(GetExtent)(DWORD dwDrawAspect, LONG lindex, DVTARGETDEVICE *ptd, LPSIZEL lpsizel);

    // IDisposable 
    STDMETHOD(Dispose)();

    STDMETHODIMP Initialize(IOleClientSite* pClientSite);
    STDMETHOD(GetWinFormsControl)(Outlook::_Application* srpApp);
    STDMETHOD(QueryAndGetObjects)(void);

private:
    CComPtr<IUnknown> m_srpWinFormsDashboardControl;
    CComPtr<IDispatch> m_srpAddInObject;

    CComPtr<IOleControl>                m_srpOleControl;
    CComPtr<IOleInPlaceObject>          m_srpOleInPlaceObject;
    CComPtr<IOleObject>                 m_srpOleObject;
    CComPtr<IOleInPlaceActiveObject>    m_srpOleInPlaceActiveObject;
    CComPtr<IViewObject>                m_srpViewObject;
    CComPtr<IViewObject2>               m_srpViewObject2;
    CComPtr<IDisposable>                m_srpDisposable;

    // For IOleObject::SetHostNames
    CComBSTR                            m_cbstrContainerApp;
    CComBSTR                            m_cbstrContainerObj;

    // For IOleObject::SetClientSite
    CComPtr<IOleClientSite>             m_srpClientSite;

    // CONSIDER: This field is just used for debug tracing.
    int m_id;

    //--------------------------------------------------------------------------------
    // IUnknown
    //--------------------------------------------------------------------------------        

    // Reference count for this object
    long m_cRef;

    static int s_cInstances;
};

OBJECT_ENTRY_AUTO(__uuidof(FolderHomepageCtl), CFolderHomepageCtl)