using System.Collections.Generic;

namespace FWBS.OMS.Script
{
    public interface IScriptManager
    {
        IEnumerable<IReferenceLibrary> ReferenceLibraries{get;}

    }
}
