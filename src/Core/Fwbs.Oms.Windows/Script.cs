using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.Script;

namespace FWBS.OMS.UI.Windows.Script
{
    /// <summary>
    /// Describes a windows UI enquiry form client scriplet.
    /// </summary>
    public class EnquiryFormScriptType : FWBS.OMS.Script.EnquiryScriptType
    {
        /// <summary>
        /// A reference to the enquiry from hosting the scriptlet.
        /// </summary>
        private EnquiryForm _enqForm = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EnquiryFormScriptType()
            : base()
        {
        }

        /// <summary>
        /// Gets a string array of assembly references to include.
        /// </summary>
        protected override string[] AssemblyReferences
        {
            get
            {
                string[] a = base.AssemblyReferences;
                string[] b = new string[a.Length + 2];
                a.CopyTo(b, 0);
                b[b.GetUpperBound(0) - 1] = "FWBS.Common.UI.dll";
                b[b.GetUpperBound(0)] = "System.Drawing.dll";
                return b;
            }
        }

        new protected EnquiryFormScriptType CurrentScript
        {
            get
            {
                return (EnquiryFormScriptType)base.CurrentScript;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    UnbindEnquiryFormObject(this._enqForm);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        /// <summary>
        /// Unbind the Reference to the enqury form object
        /// </summary>
        /// <param name="enquiryForm"></param>
        internal void UnbindEnquiryFormObject(EnquiryForm enquiryForm)
        {
            _enqForm = enquiryForm;
            if (_enqForm != null)
            {
                _enqForm.Cancelled -= new EventHandler(CurrentScript.Cancelled);
                _enqForm.Finishing -= new CancelEventHandler(CurrentScript.Finishing);
                _enqForm.Finished -= new EventHandler(CurrentScript.Finished);
                _enqForm.Rendering -= new CancelEventHandler(CurrentScript.Rendering);
                _enqForm.Rendered -= new EventHandler(CurrentScript.Rendered);
                _enqForm.PageChanging -= new PageChangingEventHandler(CurrentScript.PageChanging);
                _enqForm.PageChanged -= new PageChangedEventHandler(CurrentScript.PageChanged);
                _enqForm.EnquiryLoaded -= new EventHandler(CurrentScript.EnquiryLoaded);

                foreach (Control ctrl in _enqForm.Controls)
                {
                    EventInfo[] events = ctrl.GetType().GetEvents();
                    foreach (EventInfo ev in events)
                    {
                        string methodname = ctrl.Name + "_" + ev.Name;
                        RemoveEvent(ctrl, ev, methodname);
                        methodname = ctrl.Name + "_" + ev.Name;
                        RemoveEvent(ctrl, ev, methodname);
                    }
                }
            }
            _enqForm = null;
        }

        private void RemoveEvent(Control ctrl, EventInfo ev, string methodname)
        {
            MethodInfo meth = GetMethod(methodname);

            if (meth != null)
            {
                Delegate del = Delegate.CreateDelegate(ev.EventHandlerType, CurrentScript, meth.Name, false);
                ev.RemoveEventHandler(ctrl, del);
            }
        }

        /// <summary>
        /// Sets a reference to the enquiry form being used.
        /// </summary>
        /// <param name="enquiryForm">The enquiry form hosting the script.</param>
        internal void SetEnquiryFormObject(EnquiryForm enquiryForm)
        {
            _enqForm = enquiryForm;
            if (_enqForm != null)
            {
                _enqForm.EnquiryLoaded -= new EventHandler(CurrentScript.EnquiryLoaded);
                _enqForm.Cancelled -= new EventHandler(CurrentScript.Cancelled);
                _enqForm.Finishing -= new CancelEventHandler(CurrentScript.Finishing);
                _enqForm.Finished -= new EventHandler(CurrentScript.Finished);
                _enqForm.Rendering -= new CancelEventHandler(CurrentScript.Rendering);
                _enqForm.Rendered -= new EventHandler(CurrentScript.Rendered);
                _enqForm.PageChanging -= new PageChangingEventHandler(CurrentScript.PageChanging);
                _enqForm.PageChanged -= new PageChangedEventHandler(CurrentScript.PageChanged);

                _enqForm.EnquiryLoaded += new EventHandler(CurrentScript.EnquiryLoaded);
                _enqForm.Cancelled += new EventHandler(CurrentScript.Cancelled);
                _enqForm.Finishing += new CancelEventHandler(CurrentScript.Finishing);
                _enqForm.Finished += new EventHandler(CurrentScript.Finished);
                _enqForm.Rendering += new CancelEventHandler(CurrentScript.Rendering);
                _enqForm.Rendered += new EventHandler(CurrentScript.Rendered);
                _enqForm.PageChanging += new PageChangingEventHandler(CurrentScript.PageChanging);
                _enqForm.PageChanged += new PageChangedEventHandler(CurrentScript.PageChanged);
            }
        }

        internal void SetEnquiryFormObjectControls(EnquiryForm enquiryForm)
        {
            _enqForm = enquiryForm;
            if (_enqForm == null)
                return;

            MethodInfo[] meths = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo meth in meths)
            {
                if (meth.ReturnType != typeof(void))
                    continue;

                var ctrlmeth = ParseEventMethod(meth.Name);

                if (ctrlmeth == null)
                    continue;


                Control ctrl = _enqForm.Controls[ctrlmeth.Item1];
                if (ctrl != null)
                {
                    EventInfo ev = ctrl.GetType().GetEvent(ctrlmeth.Item2);
                    if (ev != null)
                    {
                        Delegate del = Delegate.CreateDelegate(ev.EventHandlerType, CurrentScript, meth.Name, false);
                        ev.RemoveEventHandler(ctrl, del);
                        ev.AddEventHandler(ctrl, del);
                    }
                }
            }
        }

        internal static Tuple<string, string> ParseEventMethod(string methodName)
        {
            if (String.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            var idx = methodName.LastIndexOf("_");
            if (idx < 0)
                return null;

            return new Tuple<string, string>(methodName.Substring(0, idx), methodName.Substring(idx + 1));
        }

        /// <summary>
        /// Gets the reference to the enquiry form hosting the script.
        /// </summary>
        public EnquiryForm EnquiryForm
        {
            get
            {
                return _enqForm;
            }
        }

        /// <summary>
        /// Gets the Current Display windows.
        /// </summary>
        public Interfaces.IOMSTypeDisplay CurrentDisplay
        {
            get
            {
                Control parent = _enqForm.Parent;
                while (parent != null)
                {
                    if (parent is Interfaces.IOMSTypeDisplay)
                        return (Interfaces.IOMSTypeDisplay)parent;
                    else
                        parent = parent.Parent;
                }

                return parent as Interfaces.IOMSTypeDisplay;
            }
        }

        #region Script Virtual Methods
        [ScriptMethodOverridable()]
        protected virtual void EnquiryLoaded(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Rendering(object sender, CancelEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Rendered(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Finishing(object sender, CancelEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Finished(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Cancelled(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void PageChanging(object sender, PageChangingEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void PageChanged(object sender, PageChangedEventArgs e)
        {
        }



        #endregion
    }

    /// <summary>
    /// Describes a windows UI search list client scriplet.
    /// </summary>
    public class SearchListScriptType : FWBS.OMS.Script.SearchListScriptType
    {
        #region Fields

        private ucSearchControl _search = null;
        public bool EnableBeforeDisplayCellEvent { get; set; }

        #endregion

        #region Constructors

        public SearchListScriptType()
            : base()
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    DisconnectEvents();
                    _search = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        #endregion


        protected override string[] AssemblyReferences
        {
            get
            {
                string[] a = base.AssemblyReferences;
                string[] b = new string[a.Length + 1];
                a.CopyTo(b, 0);
                b[b.GetUpperBound(0)] = "System.Drawing.dll";
                return b;
            }
        }

        new protected SearchListScriptType CurrentScript
        {
            get
            {
                return (SearchListScriptType)base.CurrentScript;
            }
        }

        /// <summary>
        /// Sets a reference to the search list being used.
        /// </summary>
        internal void SetSearchListControl(ucSearchControl search)
        {
            DisconnectEvents();
            _search = search;
            if (_search != null)
            {
                _search.CommandExecuting += new CommandExecutingEventHandler(CurrentScript.CommandExecuting);
                _search.CommandExecuted += new CommandExecutedEventHandler(CurrentScript.CommandExecuted);
                _search.SearchButtonCommands += new SearchButtonEventHandler(CurrentScript.CommandButtonClick);
                _search.ItemSelected += new EventHandler(CurrentScript.ItemSelected);
                _search.ItemHover += new SearchItemHoverEventHandler(CurrentScript.ItemClick);
                _search.SelectedItemDoubleClick += new EventHandler(CurrentScript.ItemDblClick);
                _search.FilterChanged += new EventHandler(CurrentScript.FilterChanged);
                _search.SearchCompleted += new SearchCompletedEventHandler(CurrentScript.SearchCompleted);
                _search.BeforeCellDisplay += new CellDisplayEventHandler(CurrentScript.BeforeCellDisplay);
                _search.SearchListLoad += new EventHandler(CurrentScript.SearchListLoaded);
            }
        }

        private void DisconnectEvents()
        {
            if (_search != null)
            {
                _search.CommandExecuting -= new CommandExecutingEventHandler(CurrentScript.CommandExecuting);
                _search.CommandExecuted -= new CommandExecutedEventHandler(CurrentScript.CommandExecuted);
                _search.SearchButtonCommands -= new SearchButtonEventHandler(CurrentScript.CommandButtonClick);
                _search.ItemSelected -= new EventHandler(CurrentScript.ItemSelected);
                _search.ItemHover -= new SearchItemHoverEventHandler(CurrentScript.ItemClick);
                _search.SelectedItemDoubleClick -= new EventHandler(CurrentScript.ItemDblClick);
                _search.FilterChanged -= new EventHandler(CurrentScript.FilterChanged);
                _search.SearchCompleted -= new SearchCompletedEventHandler(CurrentScript.SearchCompleted);
                _search.BeforeCellDisplay -= new CellDisplayEventHandler(CurrentScript.BeforeCellDisplay);
                _search.SearchListLoad -= new EventHandler(CurrentScript.SearchListLoaded);
            }
        }



        /// <summary>
        /// Gets the reference to the search list control hosting the script.
        /// </summary>
        public ucSearchControl SearchControl
        {
            get
            {
                return _search;
            }
        }


        private ContextFactory _contextfactory = new ContextFactory();

        public IContext Context
        {
            get
            {
                return _contextfactory.CreateContext(SearchControl.SelectedItems, _search.Parent);
            }
        }



        /// <summary>
        /// Gets the Current Display windows.
        /// </summary>
        public Interfaces.IOMSTypeDisplay CurrentDisplay
        {
            get
            {
                Control parent = _search.Parent;
                while (parent != null)
                {
                    if (parent is Interfaces.IOMSTypeDisplay)
                        return (Interfaces.IOMSTypeDisplay)parent;
                    else
                        parent = parent.Parent;
                }

                return parent as Interfaces.IOMSTypeDisplay;
            }
        }

        #region Script Virtual Methods


        #endregion

        [ScriptMethodOverridable()]
        [VersionConditional("5.0.0.0")]
        protected virtual void SearchListLoaded(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void CommandExecuting(object sender, CommandExecutingEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void CommandExecuted(object sender, CommandExecutedEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void BeforeCellDisplay(object sender, FWBS.Common.UI.Windows.CellDisplayEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void CommandButtonClick(object sender, SearchButtonEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void ItemSelected(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void ItemClick(object sender, SearchItemHoverEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void ItemDblClick(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void FilterChanged(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
        }
    }
}
