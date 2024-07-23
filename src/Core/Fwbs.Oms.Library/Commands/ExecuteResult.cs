using System;
using System.Collections.Generic;

namespace FWBS.OMS.Commands
{
    public class ExecuteResult
    {
        public ExecuteResult()
        {
        }

        public ExecuteResult(CommandStatus status)
        {
            this.Status = status;
        }

        private readonly List<Exception> errors = new List<Exception>();
        public List<Exception> Errors
        {
            get
            {
                return errors;
            }
        }

        public CommandStatus Status {get;set;}
        
    }
}
