using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
    public enum ListViewGroupMask
    {
        None = 0x00000,
        Header = 0x00001,
        Footer = 0x00002,
        State = 0x00004,
        Align = 0x00008,
        GroupId = 0x00010,
        SubTitle = 0x00100,
        Task = 0x00200,
        DescriptionTop = 0x00400,
        DescriptionBottom = 0x00800,
        TitleImage = 0x01000,
        ExtendedImage = 0x02000,
        Items = 0x04000,
        Subset = 0x08000,
        SubsetItems = 0x10000
    }

    public enum ListViewGroupState
    {
        /// <summary>
        /// Groups are expanded, the group name is displayed, 
        /// and all items in the group are displayed.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The group is collapsed.
        /// </summary>
        Collapsed = 1,
        /// <summary>
        /// The group is hidden.
        /// </summary>
        Hidden = 2,
        /// <summary>
        /// Version 6.00 and Windows Vista. The group does not display a header.
        /// </summary>
        NoHeader = 4,
        /// <summary>
        /// Version 6.00 and Windows Vista. The group can be collapsed.
        /// </summary>
        Collapsible = 8,
        /// <summary>
        /// Version 6.00 and Windows Vista. The group has keyboard focus.
        /// </summary>
        Focused = 16,
        /// <summary>
        /// Version 6.00 and Windows Vista. The group is selected.
        /// </summary>
        Selected = 32,
        /// <summary>
        /// Version 6.00 and Windows Vista. The group displays only a portion of its items.
        /// </summary>
        SubSeted = 64,
        /// <summary>
        /// Version 6.00 and Windows Vista. The subset link of the group has keyboard focus.
        /// </summary>
        SubSetLinkFocused = 128,
    }

    public static class ListViewExtensions
    {
        public static void ToCSV(this ListViewEx list, StringBuilder sb)
        {
             foreach (ListViewItem a in list.Items)
            {
                foreach (ListViewItem.ListViewSubItem si in a.SubItems)
                {
                    sb.Append(si.Text);
                    sb.Append((char)9);
                }
                sb.Append(Environment.NewLine);
            }
        }
    }

    public class ListViewEx : FWBS.OMS.UI.ListView
    {
        #region Constants

        private const int LVM_FIRST = 0x1000;                    // ListView messages
        private const int LVM_SETGROUPINFO = (LVM_FIRST + 147);  // ListView messages 
        // Setinfo on Group
        private const int WM_LBUTTONUP = 0x0202;                 // Windows message 
        // left button

        #endregion

        #region Delegates

        private delegate void CallBackSetGroupState
            (ListViewGroup lstvwgrp, ListViewGroupState state);
        private delegate void CallbackSetGroupString(ListViewGroup lstvwgrp, string value);

        #endregion

        #region Native Methods

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref LVGROUP lParam);

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct LVGROUP
        {

            public int CbSize;

            public ListViewGroupMask Mask;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string PszHeader;

            public int CchHeader;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string PszFooter;

            public int CchFooter;

            public int IGroupId;

            public int StateMask;

            public ListViewGroupState State;

            public uint UAlign;

            public IntPtr PszSubtitle;

            public uint CchSubtitle;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string PszTask;

            public uint CchTask;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string PszDescriptionTop;

            public uint CchDescriptionTop;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string PszDescriptionBottom;

            public uint CchDescriptionBottom;

            public int ITitleImage;

            public int IExtendedImage;

            public int IFirstItem;

            public IntPtr CItems;

            public IntPtr PszSubsetTitle;

            public IntPtr CchSubsetTitle;
        }

        #endregion
        
        private static int? GetGroupID(ListViewGroup lstvwgrp)
        {
            int? rtnval = null;
            Type GrpTp = lstvwgrp.GetType();
            if (GrpTp != null)
            {
                PropertyInfo pi = GrpTp.GetProperty("ID", BindingFlags.NonPublic |
                                BindingFlags.Instance);
                if (pi != null)
                {
                    object tmprtnval = pi.GetValue(lstvwgrp, null);
                    if (tmprtnval != null)
                    {
                        rtnval = tmprtnval as int?;
                    }
                }
            }
            return rtnval;
        }
        private static void setGrpState(ListViewGroup lstvwgrp, ListViewGroupState state)
        {
            if (Environment.OSVersion.Version.Major < 6)   //Only Vista and forward 
                // allows collapse of ListViewGroups
                return;
            if (lstvwgrp == null || lstvwgrp.ListView == null)
                return;
            if (lstvwgrp.ListView.InvokeRequired)
                lstvwgrp.ListView.Invoke(new CallBackSetGroupState(setGrpState),
                                lstvwgrp, state);
            else
            {
                int? GrpId = GetGroupID(lstvwgrp);
                int gIndex = lstvwgrp.ListView.Groups.IndexOf(lstvwgrp);
                LVGROUP group = new LVGROUP();
                group.CbSize = Marshal.SizeOf(group);
                group.State = state;
                group.Mask = ListViewGroupMask.State;
                if (GrpId != null)
                {
                    group.IGroupId = GrpId.Value;
                    var ptr = GetPtr(group);
                    SendMessage(lstvwgrp.ListView.Handle, LVM_SETGROUPINFO, GrpId.Value, ref ptr);
                    SendMessage(lstvwgrp.ListView.Handle, LVM_SETGROUPINFO, GrpId.Value, ref ptr);
                }
                else
                {
                    group.IGroupId = gIndex;

                    var ptr = GetPtr(group);
                    SendMessage(lstvwgrp.ListView.Handle, LVM_SETGROUPINFO, gIndex, ref ptr);
                    SendMessage(lstvwgrp.ListView.Handle, LVM_SETGROUPINFO, gIndex, ref ptr);
                }
                lstvwgrp.ListView.Refresh();
            }
        }

        private static LVGROUP GetPtr(LVGROUP group)
        {
            return group;
        }

        private static void setGrpFooter(ListViewGroup lstvwgrp, string footer)
        {
            if (Environment.OSVersion.Version.Major < 6)   //Only Vista and forward 
                //allows footer on ListViewGroups
                return;
            if (lstvwgrp == null || lstvwgrp.ListView == null)
                return;
            if (lstvwgrp.ListView.InvokeRequired)
                lstvwgrp.ListView.Invoke(new CallbackSetGroupString(setGrpFooter),
                                lstvwgrp, footer);
            else
            {
                int? GrpId = GetGroupID(lstvwgrp);
                int gIndex = lstvwgrp.ListView.Groups.IndexOf(lstvwgrp);
                LVGROUP group = new LVGROUP();
                group.CbSize = Marshal.SizeOf(group);
                group.PszFooter = footer;
                group.Mask = ListViewGroupMask.Footer;
                if (GrpId != null)
                {
                    group.IGroupId = GrpId.Value;
                    var ptr = GetPtr(group);
                    SendMessage(lstvwgrp.ListView.Handle,LVM_SETGROUPINFO, GrpId.Value, ref ptr);
                }
                else
                {
                    group.IGroupId = gIndex;
                    var ptr = GetPtr(group);
                    SendMessage(lstvwgrp.ListView.Handle, LVM_SETGROUPINFO, gIndex, ref ptr);
                }
            }
        }
        public void SetGroupState(ListViewGroupState state)
        {
            foreach (ListViewGroup lvg in this.Groups)
                setGrpState(lvg, state);
        }
        public void Expand(ListViewGroup group)
        {
            setGrpState(group,  ListViewGroupState.Collapsible | ListViewGroupState.Normal);
        }

        public void Collapse(ListViewGroup group)
        {
            setGrpState(group, ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);
        }
        public void SetGroupFooter(ListViewGroup lvg, string footerText)
        {
            setGrpFooter(lvg, footerText);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP)
            {
                base.DefWndProc(ref m);
                Refresh();
                Invalidate();
                Update();
            }
            base.WndProc(ref m);
        }

    }
}
