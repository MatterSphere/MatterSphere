using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.FileManagement.Addins
{


    public partial class MilestonePlan : FWBS.OMS.UI.Windows.ucBaseAddin
    {
        private FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout.MilestonePlan milestonePlan1;

        private ucPanelNav FileActionsPanel
        {
            get
            {
                ucPanelNav pnl = GetPanel("pnlFileActions");
                if (pnl == null)
                    pnl = pnlFileActions;
                return pnl;
            }
        }

        private ucNavPanel ActionsContainer
        {
            get
            {
                foreach (Control ctrl in pnlActions.Controls)
                {
                    if (ctrl is ucNavPanel)
                        return (ucNavPanel)ctrl;
                }

                return null;
            }
        }

        private ucNavPanel FileActionsContainer
        {
            get
            {
                Control pnlActions = FileActionsPanel;

                if (pnlActions != null)
                {
                    foreach (Control ctrl in pnlActions.Controls)
                    {
                        if (ctrl is ucNavPanel)
                            return (ucNavPanel)ctrl;
                    }
                }

                return null;
            }
        }

        MilestonePlanVM vm;

        public MilestonePlan()
        {
            InitializeComponent();
        }



        void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "SelectedStage":
                    {
                        RefreshActions(vm.SelectedStage);
                        break;
                    }
                case "SelectedTask":
                    {
                        RefreshActions(vm.SelectedTask);
                        break;
                    }
                case "IsDirty":
                    {
                        if (vm.IsDirty)
                            OnDirty();
                        break;
                    }
            }

            
        }

        internal void RefreshActions(FileManagement.Milestones.Task tsk)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                ucNavPanel taskContainer = ActionsContainer;

                if (taskContainer != null)
                {
                    if (tsk != null)
                    {
                        Action[] actions = tsk.Application.GetAvailableActions(tsk);

                        Tasks.RefreshActions(pnlActions, taskContainer, actions, new EventHandler(ActionFMAction), true);
                    }
                    else
                    {
                        Tasks.RefreshActions(pnlActions, taskContainer, null, new EventHandler(ActionFMAction), true);
                    }
                }
            }
        }

        private void RefreshActions(FileManagement.Milestones.MilestoneStage stage)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                ucNavPanel container = FileActionsContainer;

                if (container != null)
                {
                    Action[] actions = null;
                    if (stage != null)
                        actions = stage.Application.GetAvailableActions(stage);

                    Tasks.RefreshActions(FileActionsPanel, container, actions, new EventHandler(ActionFMAction), true);
                }
            }
        }

        private void ActionFMAction(object sender, EventArgs e)
        {
            try
            {
               

                Action action = null;
                if (sender is MenuItem)
                    action = (Action)((MenuItem)sender).Tag;
                else
                    action = (Action)((Control)sender).Tag;

                vm.ActionFMAction(action);

               


            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        public override void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
        {
            if(vm != null)
                vm.Initialise(obj);
        }

        public override bool Connect(Interfaces.IOMSType obj)
        {
            this.milestonePlan1 = new FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout.MilestonePlan();
            this.elementHost1.Child = this.milestonePlan1;
            vm = new MilestonePlanVM();
            vm.PropertyChanged += new PropertyChangedEventHandler(vm_PropertyChanged); 
            milestonePlan1.SetupData(vm);
            return vm.Connect(obj);
        }

        public override void RefreshItem()
        {
            if (ToBeRefreshed)
            {
                if(vm !=null)
                    vm.RefreshItem();
            }
            ToBeRefreshed = false;
        }

        public override void UpdateItem()
        {
            if (vm != null)
                vm.UpdateItem();
        }

        public override void CancelItem()
        {
            if (vm != null)
                vm.CancelItem();
        }

        public override bool IsDirty
        {
            get
            {
                if (vm == null)
                    return false;

                return vm.IsDirty;
            }
        }
        
    }
}
