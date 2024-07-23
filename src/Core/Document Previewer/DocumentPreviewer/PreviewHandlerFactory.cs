using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;


namespace Fwbs.Documents.Preview
{
    using System.Runtime.InteropServices;
    using Fwbs.Framework.ComponentModel.Composition;
    using Handlers;

    internal sealed class PreviewHandlerInfo
    {
        public PreviewHandlerInfo()
        {
            ResolvedHandler = new Func<IPreviewHandler>(() => PreviewHandlerFactory.CreatePreviewHandler(Id));
        }

        public string Extension { get; internal set; }

        public Guid Id { get; internal set; }

        public bool? Supported { get; internal set; }

        public bool? CopyFile { get; internal set; }

        public bool? AlwaysUnload { get; internal set; }

        public Func<IPreviewHandler> ResolvedHandler { get; internal set; }
    }

    internal static class PreviewHandlerFactory
    {

        private static void Log(object post)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("PREVIEWER:PERFORMANCE: {0}", post));
        }

        private const string PreviewHandlerExtensionSubKey = "ShellEx\\{8895b1c6-b41f-4c1c-a562-0d564250836f}";
        internal static Dictionary<string, PreviewHandlerInfo> ShellExtensions = new Dictionary<string, PreviewHandlerInfo>();


        public static PreviewHandlerInfo GetPreviewHandlerId(string extension, IContainer container)
        {
            PreviewHandlerInfo previewHandler = null;

            extension = extension.TrimStart('.').ToUpperInvariant();

            lock (ShellExtensions)
            {
                if (ShellExtensions.ContainsKey(extension))
                    return ShellExtensions[extension];
            }

            try
            {
                if (container != null)
                {
                    var factory = container.TryResolve<IPreviewHandlerFactory>(extension);
                    
                    if (factory != null)
                        return new PreviewHandlerInfo() { Extension = extension, ResolvedHandler = factory.CreateHandler, Id = factory.ID, Supported = true };
                }               

                using (RegistryKey localkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes"))
                {
                    previewHandler = LocateShellExtension(localkey, "." + extension, container);
                    if (previewHandler != null && previewHandler.Extension == extension)
                        return previewHandler;
                }

                using (RegistryKey classeskey = Registry.ClassesRoot)
                {
                    previewHandler = LocateShellExtension(classeskey, extension, container);
                    if (previewHandler != null && previewHandler.Extension == extension)
                        return previewHandler;
                }

                previewHandler = QueryAssociations.GetPreviewHandler(extension);
                if (previewHandler != null && previewHandler.Extension == extension)
                    return previewHandler;

                return new PreviewHandlerInfo() { Extension = extension, Id = Guid.Empty, Supported = false };
            }

            finally
            {               
                lock (ShellExtensions)
                {
                    if (previewHandler != null)
                    {
                        if (!ShellExtensions.ContainsKey(extension))
                        {
                            previewHandler.Extension = extension;
                            ShellExtensions.Add(extension, previewHandler);
                        }
                    }
                }
            }
        }       

        private static PreviewHandlerInfo CheckForGuid(ref string classSubKey, RegistryKey fileClassKey)
        {
            if (fileClassKey != null)
            {
                using (RegistryKey previewHandlerKey = fileClassKey.OpenSubKey(PreviewHandlerExtensionSubKey))
                {
                    if (previewHandlerKey != null)
                    {
                        var previewHandlerGuid = (string)previewHandlerKey.GetValue(null);

                        Guid previewHandlerGuidObject = new Guid(previewHandlerGuid);

                        var supportedval = (string)previewHandlerKey.GetValue("IsSupported");
                        var copyfileval = (string)previewHandlerKey.GetValue("CopyFile");
                        var alwaysunloadval = (string)previewHandlerKey.GetValue("AlwaysUnload");

                        PreviewHandlerInfo info = new PreviewHandlerInfo();


                        if (String.IsNullOrEmpty(supportedval) 
                            || String.IsNullOrEmpty(copyfileval)
                            || String.IsNullOrEmpty(alwaysunloadval)
                            )
                        {
                            using (var handlerkey = Registry.ClassesRoot.OpenSubKey(String.Format(System.Globalization.CultureInfo.InvariantCulture,"CLSID\\{0:B}", previewHandlerGuidObject)))
                            {
                                if (handlerkey != null)
                                {
                                    if (String.IsNullOrEmpty(supportedval))
                                        supportedval = (string)handlerkey.GetValue("IsSupported");

                                    if (String.IsNullOrEmpty(copyfileval))
                                        copyfileval = (string)handlerkey.GetValue("CopyFile");

                                    if (String.IsNullOrEmpty(alwaysunloadval))
                                        alwaysunloadval = (string)handlerkey.GetValue("AlwaysUnload");
                                }

                            }
                        }

                        bool supported;
                        if (bool.TryParse(supportedval, out supported))
                            info.Supported = supported;

                        bool copyfile;
                        if (bool.TryParse(copyfileval, out copyfile))
                            info.CopyFile = copyfile;

                        bool alwaysunload;
                        if (bool.TryParse(alwaysunloadval, out alwaysunload))
                            info.AlwaysUnload = alwaysunload;

                        info.Id = previewHandlerGuidObject;
                        
                        return info;
                    }
                }
                classSubKey = (string)fileClassKey.GetValue(null);
            }
            else
            {
                classSubKey = null;
            }
            return null;
        }


        private static PreviewHandlerInfo LocateShellExtension(RegistryKey key, string classSubKey, IContainer container)
        {
            string regkey = classSubKey;

            while (!string.IsNullOrEmpty(classSubKey))
            {
                using (RegistryKey fileClassKey = key.OpenSubKey(classSubKey)) //Registry.ClassesRoot.OpenSubKey(classSubKey))
                {
                    var value = CheckForGuid(ref classSubKey, fileClassKey);

                    if (value != null)
                        return value;
                }
            }

            using (RegistryKey fileClassKey = key.OpenSubKey(regkey))
            {
                if (fileClassKey != null)
                {
                    string subType = (string)fileClassKey.GetValue(null);
                    if (!string.IsNullOrEmpty(subType))
                        return GetPreviewHandlerId(subType, container);
                }
            }

            return null;
        }


        public static IPreviewHandler CreatePreviewHandler(Guid handlerGuid)
        {
            Type handlerType = Type.GetTypeFromCLSID(handlerGuid);
            return (IPreviewHandler)Activator.CreateInstance(handlerType);
        }

    }

    internal static class QueryAssociations
    {
        const string PreviewerGuid = "{8895b1c6-b41f-4c1c-a562-0d564250836f}";
        #region PInvoke
        [DllImport("shlwapi.dll")]
        extern static int AssocCreate(
           Guid clsid,
           ref Guid riid,
           [MarshalAs(UnmanagedType.Interface)] out object ppv);

        [Flags]
        enum ASSOCF
        {
            INIT_NOREMAPCLSID = 0x00000001,
            INIT_BYEXENAME = 0x00000002,
            OPEN_BYEXENAME = 0x00000002,
            INIT_DEFAULTTOSTAR = 0x00000004,
            INIT_DEFAULTTOFOLDER = 0x00000008,
            NOUSERSETTINGS = 0x00000010,
            NOTRUNCATE = 0x00000020,
            VERIFY = 0x00000040,
            REMAPRUNDLL = 0x00000080,
            NOFIXUPS = 0x00000100,
            IGNOREBASECLASS = 0x00000200,
            INIT_IGNOREUNKNOWN = 0x00000400
        }

        enum ASSOCSTR
        {
            COMMAND = 1,
            EXECUTABLE,
            FRIENDLYDOCNAME,
            FRIENDLYAPPNAME,
            NOOPEN,
            SHELLNEWVALUE,
            DDECOMMAND,
            DDEIFEXEC,
            DDEAPPLICATION,
            DDETOPIC,
            INFOTIP,
            QUICKTIP,
            TILEINFO,
            CONTENTTYPE,
            DEFAULTICON,
            SHELLEXTENSION
        }

        enum ASSOCKEY
        {
            SHELLEXECCLASS = 1,
            APP,
            CLASS,
            BASECLASS
        }

        enum ASSOCDATA
        {
            MSIDESCRIPTOR = 1,
            NOACTIVATEHANDLER,
            QUERYCLASSSTORE,
            HASPERUSERASSOC,
            EDITFLAGS,
            VALUE
        }

        [Guid("c46ca590-3c3f-11d2-bee6-0000f805ca57"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IQueryAssociations
        {
            void Init(
              [In] ASSOCF flags,
              [In, MarshalAs(UnmanagedType.LPWStr)] string pszAssoc,
              [In] UIntPtr hkProgid,
              [In] IntPtr hwnd);

            void GetString(
              [In] ASSOCF flags,
              [In] ASSOCSTR str,
              [In, MarshalAs(UnmanagedType.LPWStr)] string pwszExtra,
              [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszOut,
              [In, Out] ref int pcchOut);

            void GetKey(
              [In] ASSOCF flags,
              [In] ASSOCKEY str,
              [In, MarshalAs(UnmanagedType.LPWStr)] string pwszExtra,
              [Out] out UIntPtr phkeyOut);

            void GetData(
              [In] ASSOCF flags,
              [In] ASSOCDATA data,
              [In, MarshalAs(UnmanagedType.LPWStr)] string pwszExtra,
              [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] out byte[] pvOut,
              [In, Out] ref int pcbOut);

            void GetEnum(); // not used actually
        }

        static Guid CLSID_QueryAssociations = new Guid("a07034fd-6caa-4954-ac3f-97a27216f98a");
        static Guid IID_IQueryAssociations = new Guid("c46ca590-3c3f-11d2-bee6-0000f805ca57");
        #endregion

        /// <summary>
        /// Use IQueryAssociations interface and shlwapi.dll to get the previewer that the Operating System has associated with the file extension
        /// </summary>   
        internal static PreviewHandlerInfo GetPreviewHandler(string extension)
        {
            object obj = null;
            int size = 0;
            StringBuilder sb;

            try
            {
                Marshal.ThrowExceptionForHR(AssocCreate(CLSID_QueryAssociations, ref IID_IQueryAssociations, out obj), (IntPtr)(-1));

                IQueryAssociations qa = (IQueryAssociations)obj;

                //Initialise
                qa.Init(0, "." + extension, UIntPtr.Zero, IntPtr.Zero);

                //Call to get the size
                qa.GetString(0, ASSOCSTR.SHELLEXTENSION, PreviewerGuid, null, ref size);
                
                //Construct a string builder that is the correct length
                sb = new StringBuilder(size);
                
                //Call to get the value
                qa.GetString(0, ASSOCSTR.SHELLEXTENSION, PreviewerGuid, sb, ref size);
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Unable to determine Previewer for file extension '{0}'.{1}", extension, ex.Message), "GetPreviewHandler");
                return null;
            }
            finally
            {
                if (obj != null)
                    Marshal.ReleaseComObject(obj);
            }

            return new PreviewHandlerInfo() { Extension = extension, Id = new Guid(sb.ToString()), Supported = true };
        }
    }
}
