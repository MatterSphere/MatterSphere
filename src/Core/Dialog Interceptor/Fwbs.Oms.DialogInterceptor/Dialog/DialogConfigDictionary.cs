using System;
using System.Collections.Generic;

namespace Fwbs.Oms.DialogInterceptor
{
    internal sealed class DialogConfigDictionary : Dictionary<string, DialogConfig>
    {
        public DialogConfigDictionary() : base(StringComparer.OrdinalIgnoreCase)
        { }

    }
}
