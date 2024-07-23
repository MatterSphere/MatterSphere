using System;
using System.Collections.Generic;

namespace Fwbs.Oms.DialogInterceptor
{
    internal sealed class ProcessDialogDictionary : Dictionary<string, DialogConfigDictionary>        
    {
        public ProcessDialogDictionary() : base(StringComparer.OrdinalIgnoreCase)
        { }
    }
}
