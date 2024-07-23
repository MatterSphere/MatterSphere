using System;
using System.Linq;
using System.Text;

namespace Fwbs.Office.Outlook
{
    using System.Globalization;
    using System.Runtime.InteropServices;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public static class OutlookUtils
    {
        internal static string GenerateChecksum(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            var enc = new System.Text.UnicodeEncoding();
            var input = enc.GetBytes(text);

            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(input);

            var buff = new StringBuilder();

            foreach (var hashByte in hash)
                buff.Append(String.Format(CultureInfo.InvariantCulture, "{0:X1}", hashByte));
            return buff.ToString();
        }

        internal static Fwbs.WinFinder.Window FindWindow(object obj, bool wrapLocal = false)
        {
            const string winclass = "rctrl_renwnd32";

            string inspcaption = String.Empty;
            string expcaption = String.Empty;

            Fwbs.WinFinder.Window win = null;


            MSOutlook.Application app = obj as MSOutlook.Application;
            MSOutlook.Inspector insp = obj as MSOutlook.Inspector;
            MSOutlook.Explorer exp = obj as MSOutlook.Explorer;
            MSOutlook.DocumentItem di = obj as MSOutlook.DocumentItem;
            OutlookItem oi = obj as OutlookItem;

            try
            {
                inspcaption = Convert.ToString(obj.GetType().InvokeMember("Caption", System.Reflection.BindingFlags.GetProperty, null, obj, null));
                if (!String.IsNullOrEmpty(inspcaption))
                    win = Fwbs.WinFinder.WindowList.Find(winclass, inspcaption).FirstOrDefault();
            }
            catch (COMException comex)
            {
                if (comex.ErrorCode != HResults.E_DISP_UNKNOWN)
                    throw;
            }
            catch (MissingMemberException) { }

            if (win == null)
            {
                if (app != null)
                    exp = app.ActiveExplorer();
                if (oi != null && !oi.IsDetached)
                {
                    bool pinned = oi.IsPinned;
                    insp = oi.GetInspector;
                    oi.IsPinned = pinned;
                }
                if (di != null)
                    inspcaption = "Untitled - Message";

                if (insp != null)
                    inspcaption = insp.Caption;
                else if (exp != null)
                    expcaption = exp.Caption;

                if (win == null)
                {
                    if (String.IsNullOrEmpty(inspcaption))
                        win = Fwbs.WinFinder.WindowList.Find(winclass, inspcaption).FirstOrDefault();
                }
                if (win == null)
                {
                    if (String.IsNullOrEmpty(expcaption))
                        win = Fwbs.WinFinder.WindowList.Find(winclass, expcaption).FirstOrDefault();
                }
            }

            if (win != null)
            {
                if (wrapLocal && win.IsLocal)
                    return new Fwbs.WinFinder.LocalWindow(win.Handle);
            }

            return win;

        }

        private static Fwbs.WinFinder.Window FindWindow(string caption)
        {
            var win = Fwbs.WinFinder.WindowList.Find("rctrl_renwnd32", caption).FirstOrDefault();
            if (win == null)
                win = Fwbs.WinFinder.WindowList.Find("rctrl_renwnd32", String.Format(CultureInfo.InvariantCulture, "{0} - Microsoft Word", caption)).FirstOrDefault();
            return win;
        }
      

    }

}
