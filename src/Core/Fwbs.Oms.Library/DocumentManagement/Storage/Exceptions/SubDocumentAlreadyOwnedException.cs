using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    /// <summary>
    /// An exception that can is raised when a document is already checked out.
    /// </summary>
    public class SubDocumentAlreadyOwnedException : StorageException, EnquiryEngine.IAssociatedEnquiryPage
    {

        private const string MESSAGE = "The sub document has already been saved under a different %FILE%";

        private const string CODE = "SUBDOCDIFFOWNER";

        public SubDocumentAlreadyOwnedException()
            : base(CODE, MESSAGE, null) { }

        public SubDocumentAlreadyOwnedException(Exception innerException)
            : base(CODE, MESSAGE, innerException) { }

        #region IAssociatedEnquiryPage Members

        public string PageName
        {
            get { return "ATTACH"; }
        }

        #endregion
    }
}
