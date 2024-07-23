using System;
using System.IO;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.DocumentManagement
{
    public abstract class ImportDocumentCommand : FWBS.OMS.Commands.Command
    {

        protected IStorageItem parent;
        public IStorageItem Parent
        {
            set { parent = value; }
        }

        

        protected Associate toAssociate;
        public Associate ToAssociate
        {
            set { toAssociate = value; }
        }

        protected string fileName;
        public string FileName
        {
            set { fileName = value; }
        }


        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            else if (toAssociate == null)
                throw new ArgumentNullException("toAssociate");
            else if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);

            throw new NotImplementedException();

            //Due to running through shell oms being 'the best' way to do this - there is no way to do this currently
        }
    }
}
