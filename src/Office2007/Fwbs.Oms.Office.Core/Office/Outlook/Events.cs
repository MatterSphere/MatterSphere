using System;
using System.ComponentModel;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public class BeforeManipulateItemsEventArgs : CancelEventArgs
    {
        private readonly MSOutlook.Explorer _explorer;
        private readonly IOutlookItems _items;
        private readonly bool _requiresaction;
        private readonly EventSource _source;

        public BeforeManipulateItemsEventArgs(MSOutlook.Explorer explorer, IOutlookItems items, bool requiresAction, EventSource source)
        {
            if (explorer == null)
                throw new ArgumentNullException("explorer");

            if (items == null)
                throw new ArgumentNullException("items");

            this._explorer = explorer;
            this._items = items;
            this._requiresaction = requiresAction;
            this._source = source;
        }

        public MSOutlook.Explorer Explorer
        {
            get
            {
                return _explorer;
            }
        }

        public IOutlookItems Items
        {
            get
            {
                return _items;
            }
        }

        public bool RequiresAction { get { return _requiresaction; } }

        public bool Handled { get; set; }

        private Action onaction;
        public Action OnAction
        {
            get
            {
                return onaction;
            }
            set
            {
                onaction = value;
            }
        }

        public EventSource Source
        {
            get
            {
                return _source;
            }
        }
    }

    public enum EventSource
    {
        None = 0,
        Event,
        Button,
        KeyHook
    }

    public class BeforeItemEventArgs : CancelEventArgs
    {
        private readonly OutlookItem _item;
        private readonly bool _requiresaction;

        public BeforeItemEventArgs(OutlookItem item, bool requiresAction)
        {
            this._item = item;
            this._requiresaction = requiresAction;
        }

        public OutlookItem Item
        {
            get
            {
                return _item;
            }
        }

        public bool RequiresAction { get { return _requiresaction; } }

        public bool Handled { get; set; }

    }

    public class BeforeSaveItemsEventArgs : BeforeManipulateItemsEventArgs
    {
        public BeforeSaveItemsEventArgs(MSOutlook.Explorer explorer, IOutlookItems items, bool requiresAction, EventSource source)
            : base(explorer, items, requiresAction, source)
        {
        }
    }

    public class BeforeDeleteItemsEventArgs : BeforeManipulateItemsEventArgs
    {

        public BeforeDeleteItemsEventArgs(MSOutlook.Explorer explorer, IOutlookItems items, bool requiresAction, EventSource source, bool permanent)
            : base(explorer, items, requiresAction, source)
        {
            this.Permanent = permanent;
        }

        public bool Permanent { get; set; }
       

    }

    public class BeforePrintItemsEventArgs : BeforeManipulateItemsEventArgs
    {

        public BeforePrintItemsEventArgs(MSOutlook.Explorer explorer, IOutlookItems items, bool requiresAction, EventSource source)
            : base(explorer, items, requiresAction, source)
        {
        }

    }

    public class BeforeOpenItemsEventArgs : BeforeManipulateItemsEventArgs
    {

        public BeforeOpenItemsEventArgs(MSOutlook.Explorer explorer, IOutlookItems items, bool requiresAction, EventSource source)
            : base(explorer, items, requiresAction, source)
        {
        }

    }


    public class BeforeMoveItemsEventArgs : BeforeManipulateItemsEventArgs
    {
        private readonly OutlookFolder _source;
        private readonly OutlookFolder _target;
        private readonly bool _copy;

        public BeforeMoveItemsEventArgs(MSOutlook.Explorer explorer, IOutlookItems items, bool requiresAction, EventSource eventSource, OutlookFolder source, OutlookFolder target, bool copy)
            : base(explorer, items, requiresAction, eventSource)
        {
            this._source = source;
            this._target = target;
            this._copy = copy;
        }

        public bool Copy { get { return _copy; } }

        public OutlookFolder SourceFolder
        {
            get
            {
                return _source;
            }
        }

        public OutlookFolder TargetFolder
        {
            get
            {
                return _target;
            }
        }

    }

   
    public sealed class BeforeForwardItemEventArgs : BeforeItemEventArgs
    {
        private readonly OutlookItem _forward;

        public BeforeForwardItemEventArgs(OutlookItem item, bool requiresAction, OutlookItem forward)
            :base(item, requiresAction)
        {
            this._forward = forward;
        }

        public OutlookItem ForwardItem
        {
            get
            {
                return _forward;
            }
        }
    }

    public sealed class BeforeReplyItemEventArgs : BeforeItemEventArgs
    {
        private readonly OutlookItem _reply;

        public BeforeReplyItemEventArgs(OutlookItem item, bool requiresAction, OutlookItem reply)
            : base(item, requiresAction)
        {
            this._reply = reply;
        }

        public OutlookItem ReplyItem
        {
            get
            {
                return _reply;
            }
        }
    }

}
