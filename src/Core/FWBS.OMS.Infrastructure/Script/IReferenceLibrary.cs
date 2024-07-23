using System;
using System.Collections.Generic;

namespace FWBS.OMS.Script
{
    public interface IReferenceLibrary
    {
        event EventHandler Loaded;

        string Name { get; }

        void Load();

        void Refresh();

        bool IsLoading{get;}

        bool IsLoaded { get; }

        IEnumerable<IReference> References { get; }

        IEnumerable<IReference> GetByDefinition(IScriptDefinition definition);

        int Count { get; }
    }
}
