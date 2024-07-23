using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace FWBS.OMS.Script
{
    [Export(typeof(IScriptManager))]
    internal sealed class ScriptManager : IScriptManager
    {
        #region Properites

        public IEnumerable<IReferenceLibrary> ReferenceLibraries
        {
            get 
            {
                if (ImportedReferenceLibraries == null)
                    return new IReferenceLibrary[0];

                return ImportedReferenceLibraries.Reverse(); 
            }
        }

        [ImportMany(AllowRecomposition=true)]
        internal IEnumerable<IReferenceLibrary> ImportedReferenceLibraries { get; set; }
        
        #endregion




  
    }
}
