using System;

namespace Fwbs.Office.Outlook
{
    using System.Runtime.InteropServices;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public static class OutlookExtensions
    {
        public static void Printable(this MSOutlook.UserProperty prop, bool value)
        {
            RedemptionUserProperty rprop = prop as RedemptionUserProperty;
            if (rprop != null)
            {
                rprop.Printable(value);
                return;
            }

            long dispidMember = 107;
            long ulPropPrintable = 0x4;
            string dispMemberName = String.Format("[DispID={0}]", dispidMember);
            object[] dispParams;

            var userPropertyType = prop.GetType();

            // Call IDispatch::Invoke to get the current flags
            object flags = userPropertyType.InvokeMember(dispMemberName, System.Reflection.BindingFlags.GetProperty, null, prop, null);
            long lFlags = long.Parse(flags.ToString());

            // Remove the hidden property Printable flag
            if ( ! value)
                lFlags &= ~ulPropPrintable;
            else
                lFlags |= ulPropPrintable;

            // Place the new flags property into an argument array
            dispParams = new object[] { lFlags };

            // Call IDispatch::Invoke to set the current flags
            userPropertyType.InvokeMember(dispMemberName,
            System.Reflection.BindingFlags.SetProperty, null, prop, dispParams);
        }

        public static string GetVersion(this MSOutlook.Application app)
        {
            
            var ver = app.Version;

            var vers = ver.Split('.');
            

            if (vers.Length == 1)
                ver = vers[0];
            else if (vers.Length > 1)
                ver = string.Join(".", vers, 0, 2);

           
            return ver;
            
        }

        public static MSOutlook.MAPIFolder GetRootFolder(this MSOutlook.MAPIFolder folder)
        {
            if (folder == null)
                return null;

            var temp = folder as MSOutlook.MAPIFolder;

            while (temp != null)
            {
                temp = OutlookItem.GetPropertyEx<object>(temp, "Parent") as MSOutlook.MAPIFolder;
                if (temp != null)
                    folder = temp;
            }

            return folder;
        }

        public static MSOutlook.MAPIFolder CreateFolder(this MSOutlook.Application app, MSOutlook.MAPIFolder folder, string[] path, bool startFromRoot, out bool created)
        {
            if (app == null)
                throw new ArgumentNullException("app");

            if (folder == null)
                folder = (MSOutlook.MAPIFolder)app.Session.GetDefaultFolder(MSOutlook.OlDefaultFolders.olFolderInbox);

            var parent = folder as object;

            if (startFromRoot)
            {
                try
                {
                    parent = folder.Parent;
                    Microsoft.Office.Interop.Outlook.MAPIFolder publicfolders;
                    try
                    {
                        publicfolders = app.Session.GetDefaultFolder(MSOutlook.OlDefaultFolders.olPublicFoldersAllPublicFolders);
                    }
                    catch
                    {
                        publicfolders = null;
                    }
                    if (publicfolders != null && publicfolders.StoreID == folder.StoreID)
                        parent = folder.GetRootFolder().Parent;
                    else
                        parent = folder.GetRootFolder();
                }
                catch (COMException comex)
                {
                    ValidateFolderExistenceException(comex);
                }
            }
     
            return CreateFolder(parent, path, out created);
        }


        public static MSOutlook.MAPIFolder CreateFolder(this MSOutlook.MAPIFolder folder, string path, out bool created)
        {
            return CreateFolder((object)folder, path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries), out created);
        }

        private static MSOutlook.MAPIFolder CreateFolder(object root, string[] path, out bool created)
        {
            created = false;
            var folder = root;

            foreach (string s in path)
            {
                if (String.IsNullOrEmpty(s))
                    break;

                MSOutlook.Folders folders = null;

                try
                {
                    folders = OutlookItem.GetPropertyEx<MSOutlook.Folders>(folder, "Folders");
                }
                catch (COMException)
                {
                }

                if (folders == null)
                    break;

                object f = null;

                try
                {
                    f = (MSOutlook.MAPIFolder)folders[s];
                }
                catch (ArgumentOutOfRangeException)
                {
                }
                catch (COMException comex)
                {
                    ValidateFolderExistenceException(comex);
                }

                if (f == null)
                {
                    f = folders.Add(s, Type.Missing);
                    created = true;
                }

                folder = f;

            }

            
            return (MSOutlook.MAPIFolder)folder;

        }

        /// <summary>
        /// A replacement for Microsoft.Office.Interop.Outlook.Folders'  MAPIFolder this[object Index] { get; }
        /// Returns a Microsoft.Office.Interop.Outlook.MAPIFolder object from the collection by Name.
        /// This is a workaround to native MSO Folders collection implementation that may return folder by numerical index when lookup by name was unsuccessful.
        /// For example, Folders["1.234"] will act as Folders[1] if a folder named "1.234" doesn't exist in Folders collection.
        /// </summary>
        /// <param name="folders">This folders collection.</param>
        /// <param name="index">A value used to match the default property of an object in the collection, i.e. Name</param>
        /// <returns>A Folder object that represents the specified object.</returns>
        public static MSOutlook.MAPIFolder GetItem(this MSOutlook.Folders folders, string index)
        {
            MSOutlook.MAPIFolder folder = folders[index];
            if (folder.Name != index)
            {
                throw new COMException(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("PRPRFLDCDNTBFND", "The attempted operation failed.  An object could not be found.", "").Text, HResults.E_OBJECT_NOT_FOUND);
            }
            return folder;
        }

        private static void ValidateFolderExistenceException(COMException exception)
        {
            if (exception.ErrorCode == HResults.E_OBJECT_NOT_FOUND 
                || exception.ErrorCode == HResults.E_ARRAY_OUT_OF_BOUNDS
                || exception.Message.StartsWith("The operation failed. An object could not be found.", StringComparison.OrdinalIgnoreCase)
                || exception.Message.StartsWith("Array index out of bounds", StringComparison.OrdinalIgnoreCase))
            {
                //Errors known about.
                return;
            }
            
                
            Framework.EventLogger.Write(exception);
        }

        internal static void ValidateItemExistenceException(COMException exception)
        {
            if (exception.ErrorCode == HResults.E_OBJECT_NOT_FOUND
                || exception.Message.StartsWith("The operation failed. An object could not be found.", StringComparison.OrdinalIgnoreCase)
                )
            {
                //Errors known about.
                return;
            }


            Framework.EventLogger.Write(exception);
        }
    }
}
