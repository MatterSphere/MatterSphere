using System;
using System.Collections.Generic;
using FWBS.OMS.Commands;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.DocumentManagement
{
    public abstract class CheckInDocumentCommand : Command
    {
        private readonly List<IStorageItem> docs = new List<IStorageItem>();
        public List<IStorageItem> Docs
        {
            get { return docs; }
        } 



        public override ExecuteResult Execute()
        {
            throw new NotImplementedException();
        }
    }
}
