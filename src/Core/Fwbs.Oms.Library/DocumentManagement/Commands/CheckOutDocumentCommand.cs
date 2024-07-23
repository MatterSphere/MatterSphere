using System;
using System.Collections.Generic;
using FWBS.OMS.Commands;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.DocumentManagement
{
    class CheckOutDocumentCommand : FWBS.OMS.Commands.Command
    {
        private readonly List<IStorageItem> docs = new List<IStorageItem>();
        public List<IStorageItem> Docs
        {
            get { return docs; }
        }


        public override ExecuteResult Execute()
        {
            ExecuteResult result = new ExecuteResult();
            result.Status = CommandStatus.Success;

            if (docs.Count <= 0)
                return result;

            foreach (IStorageItem doc in docs)
            {
                try
                {
                    FetchResults results = doc.GetStorageProvider().Fetch(doc, true, FWBS.Common.TriState.True);

                    IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);

                    lockdoc.CheckOut(results.LocalFile);
                }
                catch (Exception ex)
                {
                    result.Errors.Add(ex);
                }

            }

            return result;
        }
    }
}
