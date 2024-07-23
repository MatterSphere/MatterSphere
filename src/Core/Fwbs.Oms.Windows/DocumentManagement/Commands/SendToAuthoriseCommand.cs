namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    class SendToAuthoriseCommand : FWBS.OMS.DocumentManagement.SendToAuthoriseCommand
    {

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {

            FWBS.OMS.Commands.ExecuteResult result = base.Execute();

            if (result.Status == FWBS.OMS.Commands.CommandStatus.Success)
            {
                if (string.IsNullOrEmpty(sentToFullName))
                    sentToFullName = To;

                MessageBox.ShowInformation("AUTHEMAILSENT", "Your document has been sent to '%1%' for authorisation.", sentToFullName);
            }
            
            return result;

        }

    }
}
