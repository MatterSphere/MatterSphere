using System;

namespace FWBS.OMS.Utils.Commands
{
    public abstract class FileCommand : RunCommand
    {

        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, System.IO.Path.GetFileName(Param));
        }


 
    }
}
