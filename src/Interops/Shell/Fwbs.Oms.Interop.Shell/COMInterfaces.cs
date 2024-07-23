using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FWBS.OMS.Shell
{
    [ComImport(),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     GuidAttribute("000214e8-0000-0000-c000-000000000046")]
    internal interface IShellExtInit
    {
        [PreserveSig()]
        int Initialize(
            IntPtr pidlFolder,
            IntPtr lpdobj,
            uint hKeyProgID);
    }

    [ComImport(),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     GuidAttribute("000214e4-0000-0000-c000-000000000046")]
    internal interface IContextMenu
    {
        [PreserveSig()]
        int QueryContextMenu(
            IntPtr hmenu,
            uint iMenu,
            int idCmdFirst,
            int idCmdLast,
            uint uFlags);

        [PreserveSig()]
        int InvokeCommand(IntPtr pici);

        [PreserveSig()]
        void GetCommandString(
            int idcmd,
            uint uflags,
            int reserved,
            StringBuilder commandstring,
            int cch);
    }

}
