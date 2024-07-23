using System;
using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement
{
    public class SetDocumentExternalVisibilityCommand : FWBS.OMS.Commands.Command
    {

        #region Properties

        private readonly List<OMSDocument> docs = new List<OMSDocument>();
        public List<OMSDocument> Documents
        {
            get
            {
                return docs;
            }
        }
       
        public bool? Visible { get; set; }        

        #endregion

        #region IProcess Members

        protected InvalidOperationException CreateVisibleNotSetException()
        {
            throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("MSGVSPRPMBST", "Visible property must be set.", "").Text);
        }        

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            FWBS.OMS.Commands.ExecuteResult res = new FWBS.OMS.Commands.ExecuteResult();

            if (!Visible.HasValue)
                throw CreateVisibleNotSetException();

            Documents.Reverse(); //Attempt to keep the document ordered correctly (at least within the selected range)

            foreach (OMSDocument document in Documents)
            {
                if (document == null)
                    continue;

                try
                {
                    Execute(document, Visible.Value, res);

                    if (res.Status == FWBS.OMS.Commands.CommandStatus.Canceled)
                        return res;
                }
                catch (Exception ex)
                {
                    if (ContinueOnError)
                    {
                        res.Errors.Add(ex);
                    }
                    else
                        throw;
                }
            }

            res.Status = FWBS.OMS.Commands.CommandStatus.Success;
            
            return res;

        }


        protected virtual void Execute(OMSDocument document, bool visible, FWBS.OMS.Commands.ExecuteResult res)
        {
            if (document == null)
                return;

            try
            {
                if (document.IsExternallyVisible != visible)
                {
                    document.IsExternallyVisible = visible;
                    document.Update();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("MSGNBLCHVSDC", "Unable to change visibility of document %1%: ''%2%''", "", document.ID.ToString(), document.Description).Text, ex);
            }
        }



        #endregion
    }
}
