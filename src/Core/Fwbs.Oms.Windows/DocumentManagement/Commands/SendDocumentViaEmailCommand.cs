using System;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public class SendDocumentViaEmailCommand : FWBS.OMS.DocumentManagement.SendDocumentViaEmailCommand
    {
        public SendDocumentViaEmailCommand()
        {
        }

        private Commands.DisplayWhen displayAssociatePicker;
        public Commands.DisplayWhen DisplayAssociatePicker
        {
            get
            {
                return AllowUI ? displayAssociatePicker : Commands.DisplayWhen.Never;
            }
            set
            {
                displayAssociatePicker = value;
            }
        }

        private FWBS.OMS.UI.SelectAssociate assocpicker;
        public FWBS.OMS.UI.SelectAssociate AssociatePicker
        {
            get
            {
                if (assocpicker == null)
                {
                    assocpicker = new FWBS.OMS.UI.SelectAssociate();
                    assocpicker.UseDefault = true;
                }
                return assocpicker;
            }
        }


        public System.Windows.Forms.IWin32Window Owner { get; set; }


        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {

            //Makes sure that a default associate is specified.
            if (ToAssociate == null)
            {
                switch (DisplayAssociatePicker)
                {
                    case Commands.DisplayWhen.Never:
                        throw CreateAssociateMissingException();
                    case Commands.DisplayWhen.Always:
                    case Commands.DisplayWhen.ValueNotSpecified:
                        ToAssociate = AssociatePicker.Show(Owner);
                        break;
                }
            }
            else
            {
                switch (DisplayAssociatePicker)
                {
                    case Commands.DisplayWhen.Never:
                    case Commands.DisplayWhen.ValueNotSpecified:
                        break;
                    case Commands.DisplayWhen.Always:
                        ToAssociate = AssociatePicker.Show(Owner);
                        break;
                }

            }

            Commands.ExecuteResult res;

            //Canceled
            if (ToAssociate == Associate.Private || ToAssociate == null)
            {
                res = new FWBS.OMS.Commands.ExecuteResult();
                res.Status = Commands.CommandStatus.Canceled;
                return res;
            }

            res = base.Execute();

            if (res.Status == Commands.CommandStatus.Success)
            {
                try
                {
                    Services.ProcessJob(null, JobResult);

                    if (JobResult.HasError)
                        throw new InvalidOperationException(JobResult.ErrorMessage);
                }
                catch (Exception ex)
                {
                    if (ContinueOnError)
                    {
                        res.Errors.Add(ex);
                        res.Status = FWBS.OMS.Commands.CommandStatus.Failed;
                    }
                    else
                        throw;
                }

            }


            return res;

        }

    }


}
