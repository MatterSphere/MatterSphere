namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    using FWBS.OMS.UI.Windows;

    public sealed class CopyDocumentCommand : FWBS.OMS.DocumentManagement.CopyDocumentCommand
    {
        #region Properties

        public bool DisplaySaveWizard { get; set; }

        public System.Windows.Forms.IWin32Window Owner { get; set; }

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

                    bool defaultAssoc = Session.CurrentSession.UseDefaultAssociate;
                    switch (Session.CurrentSession.CurrentUser.UseDefaultAssociate)
                    {
                        case FWBS.Common.TriState.False:
                            {
                                defaultAssoc = false;
                                break;
                            }
                        case FWBS.Common.TriState.True:
                            {
                                defaultAssoc = true;
                                break;
                            }
                    }
                    assocpicker.UseDefault = defaultAssoc;
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

        protected override void Execute(FWBS.OMS.DocumentManagement.DocumentVersion originalversion, FWBS.OMS.DocumentManagement.DocumentVersion newversion, FWBS.OMS.Commands.ExecuteResult res)
        {
            //Display the save wizard if it is opted to do so.
            OMSDocument newdoc = newversion.ParentDocument;

            if (DisplaySaveWizard)
            {
                if (!Services.Wizards.SaveDocument(Owner, ref newdoc))
                {
                    newdoc.Cancel();
                    newdoc.ClearSettings();
                    return;
                }
            }

            base.Execute(originalversion, newversion, res);
        }

        #endregion
    }
}
