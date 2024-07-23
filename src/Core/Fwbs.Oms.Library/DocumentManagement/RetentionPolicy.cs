using System;

namespace FWBS.OMS
{
    /// <summary>
    /// A structure that holds retention information for a document.
    /// This may be a policy that is used for archiving documents by an external program
    /// after a file is terminated.
    /// </summary>
    public struct RetentionPolicy
    {
        public readonly string Name;
        public readonly int Period;

        public RetentionPolicy(string name, int period)
        {
            Name = name;
            Period = period;
        }

        public bool IsValid
        {
            get
            {
                if (Name != String.Empty && Period >= 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
