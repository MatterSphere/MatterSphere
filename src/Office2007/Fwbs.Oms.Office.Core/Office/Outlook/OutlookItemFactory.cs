namespace Fwbs.Office.Outlook
{

    using System.Runtime.InteropServices;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    internal static class OutlookItemFactory
    {
        public static Redemption._ISafeItem CreateSafeItem(OutlookItem obj)
        {
            return CreateSafeItem(obj.InternalItem);
        }
        public static Redemption._ISafeItem CreateSafeItem(object obj)
        {
            var mail = obj as MSOutlook.MailItem;
            if (mail != null)
            {
                var smail = Redemption.RedemptionFactory.Default.CreateSafeMailItem();
                smail.Item = obj;
                return smail;
            }

            var si = obj as MSOutlook.StorageItem;
            if (si != null)
            {
                if (si.Class == MSOutlook.OlObjectClass.olMail)
                {
                    var smail = Redemption.RedemptionFactory.Default.CreateSafeMailItem();
                    smail.Item = obj;
                    return smail;
                }
            }

            var app = obj as MSOutlook.AppointmentItem;
            if (app != null)
            {
                var sapp = Redemption.RedemptionFactory.Default.CreateSafeAppointmentItem();
                sapp.Item = obj;
                return sapp;
            }

            var task = obj as MSOutlook.TaskItem;
            if (task != null)
            {
                var stask = Redemption.RedemptionFactory.Default.CreateSafeTaskItem();
                stask.Item = obj;
                return stask;
            }

            var cont = obj as MSOutlook.ContactItem;
            if (cont != null)
            {
                var scont = Redemption.RedemptionFactory.Default.CreateSafeContactItem();
                scont.Item = obj;
                return scont;
            }

            var meet = obj as MSOutlook.MeetingItem;
            if (meet != null)
            {
                var smeet = Redemption.RedemptionFactory.Default.CreateSafeMeetingItem();
                smeet.Item = obj;
                return smeet;
            }

            var report = obj as MSOutlook.ReportItem;
            if (report != null)
            {
                var sreport = Redemption.RedemptionFactory.Default.CreateSafeReportItem();
                sreport.Item = obj;
                return sreport;
            }

            var post = obj as MSOutlook.PostItem;
            if (post != null)
            {
                var spost = Redemption.RedemptionFactory.Default.CreateSafePostItem();
                spost.Item = obj;
                return spost;
            }

            var journal = obj as MSOutlook.JournalItem;
            if (journal != null)
            {
                var sjournal = Redemption.RedemptionFactory.Default.CreateSafeJournalItem();
                sjournal.Item = obj;
                return sjournal;
            }

            var distlist = obj as MSOutlook.DistListItem;
            if (distlist != null)
            {
                var sdistlist = Redemption.RedemptionFactory.Default.CreateSafeDistList();
                sdistlist.Item = obj;
                return sdistlist;
            }

            var catchall = Redemption.RedemptionFactory.Default.CreateSafeMailItem();
            catchall.Item = obj;
            return catchall;



        }


        public static OutlookItem Create(object obj, bool pin)
        {           
            var ret = InternalCreate(obj);

            if (ret == null)
                return null;

            ret.IsPinned = pin;

            AttachOrDispose(obj, ret);

            return ret;

        }

        internal static void AttachOrDispose(object obj, OutlookItem ret)
        {
            var disp = new COMDisposable(obj);

            if (ret == null)
                disp.Dispose();
            else if (ret.IsNew || ret.IsPinned || ret.Inspector != null)
                ret.Attach(obj);
            else
                disp.Dispose();

            disp = null;
            obj = null;
        }

  
        private static OutlookItem InternalCreate(object obj)
        {
            try
            {
                var oi = obj as OutlookItem;
                if (oi != null)
                    return oi;

                var mail = obj as MSOutlook.MailItem;
                if (mail != null)
                    return new OutlookMail(mail);

                var appnt = obj as MSOutlook.AppointmentItem;
                if (appnt != null)
                    return new OutlookAppointment(appnt);

                var task = obj as MSOutlook.TaskItem;
                if (task != null)
                    return new OutlookTask(task);

                var report = obj as MSOutlook.ReportItem;
                if (report != null)
                    return new OutlookReport(report);

                var post = obj as MSOutlook.PostItem;
                if (post != null)
                    return new OutlookPost(post);

                var ie = obj as MSOutlook.ItemEvents_10_Event;
                if (ie != null)
                    return new OutlookItem(ie);
            }
            catch (COMException comex)
            {
                OutlookExtensions.ValidateItemExistenceException(comex);

                return null;
            }

            return null;
        }

        public static object Parse(object obj)
        {
            return obj;
        }
    }
}
