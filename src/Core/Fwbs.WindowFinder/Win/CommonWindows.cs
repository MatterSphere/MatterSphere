namespace Fwbs
{
    namespace WinFinder
    {
        public static class CommonWindows
        {
            public static WindowList MainWindows
            {
                get
                {
                    return WindowList.Find(Desktop, null as WindowFilter, false);
                }
            }

            private static Window windesktop;
            public static Window Desktop
            {
                get
                {
                    if (windesktop == null || !windesktop.IsValid)
                    {
                        windesktop = WindowFactory.GetWindow(NativeMethods.GetDesktopWindow());
                        if (!windesktop.IsValid)
                            windesktop = null;
                    }

                    return windesktop;
                }
            }

            public static Window ActiveWindow
            {
                get
                {
                    return WindowFactory.GetWindow(NativeMethods.GetActiveWindow());
                }
            }

            public static Window FocusedWindow
            {
                get
                {
                    return WindowFactory.GetWindow(NativeMethods.GetFocus());
                }
            }

            public static Window ForegroundWindow
            {
                get
                {
                    return WindowFactory.GetWindow(NativeMethods.GetForegroundWindow());
                }
            }

            private static Window wintaskbar;
            public static Window TaskBar
            {
                get
                {
                    Window desktop = Desktop;
                    if (wintaskbar == null || !wintaskbar.IsValid)
                    {
                        if (desktop != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Class = Properties.Settings.Default.WindowClassTaskBar;
                            WindowList wins = WindowList.Find(desktop, filter, true);
                            if (wins.Count > 0)
                                wintaskbar = wins[0];
                            else
                                wintaskbar = null;
                        }
                    }
                    return wintaskbar;
                }
            }

            private static Window wintrayclock;
            public static Window TrayClock
            {
                get
                {
                    if (wintrayclock == null || !wintrayclock.IsValid)
                    {
                        Window nb = NotificationBar;

                        if (nb != null)
                        {

                            WindowFilter filter = new WindowFilter();
                            filter.Class = Properties.Settings.Default.WindowClassTray;
                            WindowList wins = WindowList.Find(nb, filter, true);

                            if (wins.Count > 0)
                                wintrayclock = wins[0];
                            else
                                wintrayclock = null;
                        }
                    }

                    return wintrayclock;
                }
            }

            private static Window winquicklaunch;
            public static Window QuickLaunchBar
            {
                get
                {
                    if (winquicklaunch == null || !winquicklaunch.IsValid)
                    {
                        Window tb = TaskBar;
                        if (tb != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Title = Properties.Resources.WindowTextQuickLaunch;
                            filter.Class = Properties.Settings.Default.WindowClassQuickLaunch;
                            WindowList wins = WindowList.Find(tb, filter, true);

                            if (wins.Count > 0)
                                winquicklaunch = wins[0];
                            else
                                winquicklaunch = null;
                        }
                    }
                    return winquicklaunch;
                }
            }

            private static Window winrunningapps;
            public static Window RunningApplications
            {
                get
                {
                    if (winrunningapps == null || !winrunningapps.IsValid)
                    {
                        Window tb = TaskBar;
                        if (tb != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Title = Properties.Resources.WindowTextRunningApps;
                            filter.Class = Properties.Settings.Default.WindowClassRunningApps;

                            WindowList wins = WindowList.Find(tb, filter, true);

                            if (wins.Count > 0)
                                winrunningapps = wins[0];
                            else
                                winrunningapps = null;
                        }
                    }

                    return winrunningapps;

                }
            }

            private static Window winnotifybar;
            public static Window NotificationBar
            {
                get
                {
                    if (winnotifybar == null || !winnotifybar.IsValid)
                    {
                        Window tb = TaskBar;
                        if (tb != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Class = Properties.Settings.Default.WindowClassNotificationBar;
                            WindowList wins = WindowList.Find(tb, filter, true);
                            if (wins.Count > 0)
                                winnotifybar = wins[0];
                            else
                                winnotifybar = null;
                        }
                    }

                    return winnotifybar;
                }
            }

            private static Window winnotifyarea;
            public static Window NotificationArea
            {
                get
                {
                    if (winnotifyarea == null || !winnotifyarea.IsValid)
                    {
                        Window tb = NotificationBar;
                        if (tb != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Title = Properties.Resources.WindowTextNotificationArea;
                            filter.Class = Properties.Settings.Default.WindowClassNotificationArea;
                            WindowList wins = WindowList.Find(tb, filter, true);
                            if (wins.Count > 0)
                                winnotifyarea = wins[0];
                            else
                                winnotifyarea = null;
                        }
                    }

                    return winnotifyarea;
                }
            }

            private static Window winsysctrlarea;
            public static Window SystemControlArea
            {
                get
                {
                    if (winsysctrlarea == null || !winsysctrlarea.IsValid)
                    {
                        Window tb = NotificationBar;
                        if (tb != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Title = Properties.Resources.WindowTextSystemControlArea;
                            filter.Class = Properties.Settings.Default.WindowClassSystemControlArea;
                            WindowList wins = WindowList.Find(tb, filter, true);
                            if (wins.Count > 0)
                                winsysctrlarea = wins[0];
                            else
                                winsysctrlarea = null;
                        }
                    }

                    return winsysctrlarea;
                }
            }

            #region Vista Specific

            private static Window winsidebar;
            public static Window Sidebar
            {
                get
                {
                    Window desktop = Desktop;
                    if (winsidebar == null || !winsidebar.IsValid)
                    {
                        if (desktop != null)
                        {
                            WindowFilter filter = new WindowFilter();
                            filter.Class = Properties.Settings.Default.WindowClassSideBar;
                            WindowList wins = WindowList.Find(desktop, filter, true);
                            if (wins.Count > 0)
                                winsidebar = wins[0];
                            else
                                winsidebar = null;
                        }
                    }
                    return winsidebar;
                }
            }

            public static WindowList SidebarGadgets
            {
                get
                {
                    Window desktop = Desktop;
                    if (desktop != null)
                    {
                        WindowFilter filter = new WindowFilter();
                        filter.Class = Properties.Settings.Default.WindowClassGadgets;
                        return WindowList.Find(Desktop, filter, true);
                    }
                    else
                        return null;
                }
            }

            #endregion

        }
    }
}