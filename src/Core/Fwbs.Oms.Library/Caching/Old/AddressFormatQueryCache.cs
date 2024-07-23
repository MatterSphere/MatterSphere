using System;
using System.Data;
using FWBS.Common;

namespace FWBS.OMS.Caching
{
    [Obsolete("Use the one in Queries")]
    internal sealed class AddressFormatQueryCache : BaseQueryCache
    {
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            switch (args.SQL.ToUpperInvariant())
            {
                case "SPRADDRESSFORMAT":
                        return true;
            }

            return false;
        }

        protected override DataTableCache Cache
        {
            get { return (DataTableCache)Session.CurrentSession.CachedItems["OMS:CONFIGS"]; }
        }

        protected override string GetName(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            IDataParameter country = args.GetParameterByName("COUNTRY");
            IDataParameter ui = args.GetParameterByName("UI");
            IDataParameter uitype = args.GetParameterByName("UITYPE");

            return String.Format("sprAddressFormat/Country={0}/UI={1}/UIType={2}",
                country == null ? String.Empty : Convert.ToString(country.Value),
                ui == null ? String.Empty : Convert.ToString(ui.Value),
                uitype == null ? (short)0 : ConvertDef.ToInt16(uitype.Value, 0));
        }

        #endregion
    }
}
