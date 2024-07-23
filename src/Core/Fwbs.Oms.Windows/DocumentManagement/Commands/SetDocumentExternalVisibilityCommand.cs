using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.DocumentManagement
{
    public class SetDocumentExternalVisibilityCommand : FWBS.OMS.DocumentManagement.SetDocumentExternalVisibilityCommand
    {
        
        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            FWBS.OMS.Commands.ExecuteResult result = new Commands.ExecuteResult();

            if (MessageBox.ShowYesNoQuestion("CNFRMSETVIS", "Are you sure you want to change the external visibility of the selected document(s)?") != System.Windows.Forms.DialogResult.Yes)
            {
                result.Status = Commands.CommandStatus.Canceled;
                return result;
            }
           
            result = base.Execute();
           
            return result;
        }

    }
}
