using System;

namespace PackageUpgradeAnalyzer
{
    class OmsObjectInfo
    {
        public OmsObjectInfo(string omsType, string code)
        {
            OmsType = omsType;
            Code = code;
        }

        public string OmsType { get; private set; }
        public string Code { get; private set; }
        public int Version { get; set; }
        public DateTime? Updated { get; set; }
        public int? UpdatedBy { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-15} {1,6} {2,20} {3,6}", Code, Version, Updated?.ToString(@"yyyy-MM-dd_HH\:mm\:ss"), UpdatedBy);
        }

        public UpgradeStatus CanBeUpgradedBy(OmsObjectInfo other)
        {
            if (this.OmsType != other.OmsType || this.Code != other.Code)
                throw new ArgumentException();

            if (this.Version > other.Version)
                return UpgradeStatus.No;

            if (this.Version == other.Version && this.Updated != other.Updated)
                return UpgradeStatus.Ignore;

            return UpgradeStatus.Yes;
        }

        public enum UpgradeStatus
        {
            Yes = '.',
            No = '!',
            Ignore = '?'
        }
    }
}
