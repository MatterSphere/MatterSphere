namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public sealed class MoveDocumentCommand : FWBS.OMS.DocumentManagement.MoveDocumentCommand
    {
        #region Properties

     
        public System.Windows.Forms.IWin32Window Owner {get;set;}

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

        #endregion

        #region IProcess Members

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


            //Canceled
            if (ToAssociate == Associate.Private || ToAssociate == null)
            {
                Commands.ExecuteResult res = new FWBS.OMS.Commands.ExecuteResult();
                res.Status = Commands.CommandStatus.Canceled;
                return res;
            }

            return base.Execute();

        }



        #endregion
    }
}
