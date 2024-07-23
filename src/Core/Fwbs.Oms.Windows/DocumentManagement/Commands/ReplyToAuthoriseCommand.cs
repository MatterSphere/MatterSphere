using System;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.DocumentManagement
{
    public class ReplyToAuthoriseCommand : FWBS.OMS.DocumentManagement.ReplyToAuthoriseCommand
    {

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            Send = MessageBox.ShowYesNoQuestion("ASKEDITBEFSEND", "Would you like to edit the response before sending?") == System.Windows.Forms.DialogResult.No;

            FWBS.OMS.Commands.ExecuteResult result = base.Execute();

            switch (result.Status)
            {
                case FWBS.OMS.Commands.CommandStatus.Success:
                    MessageBox.ShowInformation("AUTHMAILRPLY", "Your response has been sent");
                    break;
                case FWBS.OMS.Commands.CommandStatus.None:
                    DisplayReply();
                    break;
                case FWBS.OMS.Commands.CommandStatus.Failed:
                    if (result.Errors.Count > 0)
                    {
                        MessageBox.Show(result.Errors[0]);
                    }
                    break;

            }

            return result;
        }

    }
}
