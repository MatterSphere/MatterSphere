using System;
using System.Windows.Input;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class WPFViewAction : IDisposable
    {
        public Action Action { get; set; }

        public WPFViewAction(Action a)
        {
            Action = a;
            TaskActionClicked = new Command<Action>(OnTaskActionClicked);
        }

        public ICommand TaskActionClicked
        {
            get;
            private set;
        }

        public void OnTaskActionClicked(Action action)
        {
            try
            {
                action.Stage.Application.Execute(action);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        public virtual string Description
        {
            get
            {
                return Action.Description;
            }
        }


        internal void ActionFMAction(Action action)
        {
            

        }

        public void Dispose()
        {
            Action = null;
            TaskActionClicked = null;
        }
    }

    public class WPFSplitterViewAction : WPFViewAction
    {
        public WPFSplitterViewAction()
            : base(null)
        {
        }

        public override string Description
        {
            get
            {
                return "";
            }
        }
    }

    public class WPFDisabledViewAction : WPFViewAction
    {
        string description;

        public WPFDisabledViewAction(string description)
            : base(null)
        {
            this.description = description;
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }
    }
}
