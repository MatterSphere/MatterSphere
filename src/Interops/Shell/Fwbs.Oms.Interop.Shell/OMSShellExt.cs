using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;

namespace FWBS.OMS.Shell
{

    internal struct MenuItem
    {
        public string Text;
        public System.Diagnostics.ProcessStartInfo Command;
        public string HelpText;
    }

	[GuidAttribute(OMSShellExt.CLSID), ProgId(OMSShellExt.PROGID)]
	[ComVisible(true)]
	public class OMSShellExt : IShellExtInit, IContextMenu
	{
		private const string CLSID ="FB6D8570-49B2-4fcf-9BE4-79A74369CF4D";
        private const string PROGID = "FWBS.OMS.UI.Windows.OMSShellExt";

		[System.Runtime.InteropServices.ComRegisterFunctionAttribute()]
		static void RegisterServer(String zRegKey) 
		{
			try 
			{
				RegistryKey root;
				RegistryKey rk;

                root = Registry.ClassesRoot;
                rk = root.OpenSubKey(@"*\shellex\ContextMenuHandlers", true);
                rk.CreateSubKey("{" + CLSID + "}").SetValue("", "OMS Context Menu Handler");
                rk.Close();

			}
			catch(Exception e) 
			{
				System.Console.Error.WriteLine(e.ToString());
			}
		}

		[System.Runtime.InteropServices.ComUnregisterFunctionAttribute()]
		static void UnregisterServer(String zRegKey) 
		{
			try 
			{
				RegistryKey root;
				RegistryKey rk;

				root = Registry.ClassesRoot;
				rk = root.OpenSubKey(@"*\shellex\ContextMenuHandlers", true);
                rk.DeleteSubKey("{" + CLSID + "}");
				rk.Close();

			}
			catch(Exception e) 
			{
				System.Console.Error.WriteLine(e.ToString());
			}
        }


        #region Fields

        private List<string> files = new List<string>();
        private List<MenuItem> menus = new List<MenuItem>();

        #endregion

        public OMSShellExt()
        {
        }

        #region IShellExtInit Implementation

        public int Initialize(IntPtr pidlFolder,  IntPtr lpdobj,  uint hKeyProgID)
		{
			try
			{

                IDataObject m_dataObject = (IDataObject)Marshal.GetObjectForIUnknown(lpdobj);
                FORMATETC fmt = new FORMATETC();
                fmt.cfFormat = (short)NativeMethods.CLIPFORMAT.CF_HDROP;
                fmt.ptd = IntPtr.Zero;
                fmt.dwAspect = DVASPECT.DVASPECT_CONTENT;
                fmt.lindex = -1;
                fmt.tymed = TYMED.TYMED_HGLOBAL;
                STGMEDIUM medium;
                m_dataObject.GetData(ref fmt, out medium);

				MenuItem mi1 = new MenuItem();
                mi1.Command = new System.Diagnostics.ProcessStartInfo("OMS.UTILS.EXE", "SAVE");
                mi1.Text = Global.RegistryRes("Save", "Save");
                mi1.HelpText = Global.RegistryRes("SaveHelp", String.Format(System.Globalization.CultureInfo.CurrentCulture, "Saves the selected files to {0}", Global.ApplicationName));
                MenuItem mi2 = new MenuItem();
                mi2.Text = Global.RegistryRes("SaveAs", "Save As");
                mi2.Command = new System.Diagnostics.ProcessStartInfo("OMS.UTILS.EXE", "SAVEAS");
                mi2.HelpText = Global.RegistryRes("SaveAsHelp", String.Format(System.Globalization.CultureInfo.CurrentCulture, "Saves the selected files to {0}", Global.ApplicationName));

                menus.Add(mi1);
                menus.Add(mi2);

                uint filecount = NativeMethods.DragQueryFileW(medium.unionmember, 0xFFFFFFFF, new System.Text.StringBuilder(0), 0);

                for (uint ctr = 0; ctr < filecount; ctr++)
                {
                    System.Text.StringBuilder sbfile = new System.Text.StringBuilder(256);

                    uint i = NativeMethods.DragQueryFileW(medium.unionmember, ctr, null, 0);
                    NativeMethods.DragQueryFileW(medium.unionmember, ctr, sbfile, i+1);
                    files.Add(sbfile.ToString());
                }

                NativeMethods.ReleaseStgMedium(ref medium);

			}
			catch (Exception ex)
			{
				Console.Write(ex);
			}

            return 0;
		}

		#endregion

		#region IContextMenu Implentation


        int IContextMenu.QueryContextMenu(IntPtr hMenu, uint iMenu, int idCmdFirst, int idCmdLast, uint uFlags)
        {
            int id = 1;
            if ((uFlags & 0xf) == 0 || (uFlags & (uint)NativeMethods.CMF.CMF_EXPLORE) != 0)
            {
                if (files.Count > 0)
                {
                    IntPtr hmnuPopup = NativeMethods.CreatePopupMenu();

                    uint ctr = 0;
                    foreach (MenuItem menu in menus)
                    {
                       
                        AddMenuItem(hmnuPopup, menu.Text, idCmdFirst + id, ctr);
                        ctr++;
                        id++;
                    }

                    // Add the popup to the context menu
                    NativeMethods.MENUITEMINFO mii = new NativeMethods.MENUITEMINFO();
                    mii.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO));
                    mii.fMask = (uint)NativeMethods.MIIM.TYPE | (uint)NativeMethods.MIIM.STATE | (uint)NativeMethods.MIIM.SUBMENU;
                    mii.hSubMenu = hmnuPopup;
                    mii.fType = (uint)NativeMethods.MF.STRING;
                    mii.dwTypeData = Global.ApplicationName;
                    mii.fState = (uint)NativeMethods.MF.ENABLED;
                    NativeMethods.InsertMenuItem(hMenu, (uint)iMenu, 1, ref mii);

                    // Add a separator
                    NativeMethods.MENUITEMINFO sep = new NativeMethods.MENUITEMINFO();
                    sep.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO));
                    sep.fMask = (uint)NativeMethods.MIIM.TYPE;
                    sep.fType = (uint)NativeMethods.MF.SEPARATOR;
                    NativeMethods.InsertMenuItem(hMenu, iMenu + 1, 1, ref sep);


                }
            }
            return id;

		}

        private static void AddMenuItem(IntPtr hMenu, string text, int id, uint position)
        {
            NativeMethods.MENUITEMINFO mii = new NativeMethods.MENUITEMINFO();
            mii.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO));
            mii.fMask = (uint)NativeMethods.MIIM.ID | (uint)NativeMethods.MIIM.TYPE | (uint)NativeMethods.MIIM.STATE;
            mii.wID = id;
            mii.fType = (uint)NativeMethods.MF.STRING;
            mii.dwTypeData = text;
            mii.fState = (uint)NativeMethods.MF.ENABLED;
            NativeMethods.InsertMenuItem(hMenu, position, 1, ref mii);
        }



        int IContextMenu.InvokeCommand(IntPtr pici)
        {

            try
            {
                
                NativeMethods.INVOKECOMMANDINFO ici = (NativeMethods.INVOKECOMMANDINFO)Marshal.PtrToStructure(pici, typeof(NativeMethods.INVOKECOMMANDINFO));

                if (ici.verb > menus.Count)
                    return -1;

                MenuItem menu = menus[ici.verb - 1];
                menu.Command.Arguments += " \"" + String.Join("\" \"", files.ToArray()) + "\"";
                menu.Command.CreateNoWindow = true;
                menu.Command.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                System.Diagnostics.Process.Start(menu.Command);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }

        }

        void IContextMenu.GetCommandString(int idCmd, uint uFlags, int pwReserved, StringBuilder commandString, int cchMax)
        {
            switch (uFlags)
            {
                case (uint)NativeMethods.GCS.HELPTEXT:
                    commandString = new StringBuilder(menus[idCmd - 1].HelpText.Substring(1, cchMax));
                    break;
                case (uint)NativeMethods.GCS.VALIDATE:
                    break;
            }
		}

		#endregion
	}
}
