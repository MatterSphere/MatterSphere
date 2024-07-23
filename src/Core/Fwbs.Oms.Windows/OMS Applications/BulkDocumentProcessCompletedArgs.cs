using System;
using System.Collections.Generic;

namespace FWBS.OMS.UI.OMS_Applications
{
    public class BulkDocumentProcessCompletedArgs : EventArgs
    {
        private List<string> _errorFiles;
        private List<Exception> _exceptions;

        public List<string> ErrorFiles
        {
            get
            {
                return _errorFiles;
            }
        }

        public List<Exception> Exceptions
        {
            get
            {
                return _exceptions;
            }
        }

        public BulkDocumentProcessCompletedArgs(List<string> errorfiles, List<Exception> exceptions)
        {
            _errorFiles = errorfiles;
            _exceptions = exceptions;
        }
    }
}
