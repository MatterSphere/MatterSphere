using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public abstract class OutlookObject : OfficeObject
    {
        protected OutlookObject() : base()
        {
        }

        protected OutlookObject(bool needsReleasing)
            : base(needsReleasing)
        {
        }

        protected sealed override object UnwrapValue(object value)
        {
            var folder = value as OutlookFolder;
            if (folder != null)
                return folder.InternalItem;

            var item = value as OutlookItem;
            if (item != null)
                return item.InternalItem;

            var session = value as OutlookSession;
            if (session != null)
                return session.InternalItem;

            var app = value as OutlookApplication;
            if (app != null)
                return app.InternalItem;

            var insp = value as OutlookInspector;
            if (insp != null)
                return insp.InternalItem;

            var exp = value as OutlookExplorer;
            if (exp != null)
                return exp.InternalItem;

            return base.UnwrapValue(value);
        }

        protected sealed override object WrapValue(object value)
        {
            var app = value as MSOutlook.Application;
            if (app != null)
                return OutlookApplication.GetApplication(app);

            var folder = value as MSOutlook.MAPIFolder;
            if (folder != null)
                return Application.GetFolder(folder);

            var item = value as MSOutlook.ItemEvents_10_Event;
            if (item != null)
                return Application.GetItem(item);

            var session = value as MSOutlook.NameSpace;
            if (session != null)
                return Application.GetSession(session);


            var insp = value as MSOutlook.Inspector;
            if (insp != null)
                return Application.GetInspector(insp);

            var exp = value as MSOutlook.Explorer;
            if (exp != null)
                return Application.GetExplorer(exp);


            return base.WrapValue(value);
        }

        public abstract OutlookApplication Application { get; }



        protected OutlookItem GetItem(Func<object> action)
        {
            return Application.LoadedItems.GetItem(action, false);
        }


        protected static void EnsureRaiseEvent(Delegate ev, params object[] args)
        {
            if (ev != null)
            {
                foreach (var del in ev.GetInvocationList())
                {
                    try
                    {
                        del.DynamicInvoke(args);
                    }
                    catch { }
                }
            }
        }
    }
}
