using System;
using System.Data;
using FWBS.Common;

namespace FWBS.OMS.Caching
{
    [Obsolete("Use the one in Queries")]
    internal sealed class CodeLookupQueryCache : BaseQueryCache, Queries.ICodeLookupQueryCache
    {
        private const string NULL = "{NULL}";
        private const string SQL = "sprCodeLookupList/Type={0}/Code={1}/UI={2}/Brief={3}/IncludeNull={4}";

        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            //Only capture the code lookup list stored procedure if there are two parameters
            //passed.  Type and UI.
            switch (args.SQL.ToUpperInvariant())
            {
                case "SPRCODELOOKUPLIST":
                    return true;
            }

            return false;

        }

        public override bool Handles(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            //Only capture the code lookup list stored procedure if there are two parameters
            //passed.  Type and UI.
            switch (args.SQL.ToUpperInvariant())
            {
                case "SPRCODELOOKUPCACHE":
                    return true;
            }

            return false;

        }

        protected override DataTableCache Cache
        {
            get { return (DataTableCache)Session.CurrentSession.CachedItems["OMS:CODELOOKUPS"]; }
        }

        public override void SetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            if (args == null || args.Data == null)
                return;

            if (Handles(args))
            {
                if (args.Data.Tables.Count > 0)
                {
     
                    DataTable names = args.Data.Tables[0];
                    for (int ctr = 0; ctr < names.Rows.Count; ctr++)
                    {
                        DataRow name = names.Rows[ctr];
                        string cachename = String.Format(SQL,
                        GetValue(name["type"]),
                        GetValue(name["code"]),
                        GetValue(name["ui"]),
                        ConvertDef.ToBoolean(name["brief"], true),
                        ConvertDef.ToBoolean(name["includenull"], false));

                        int idx = ctr + 1;
                        if (idx < args.Data.Tables.Count)
                            Cache.Set(cachename, args.Data.Tables[idx]);
                    }
                }
            }
            

        }


        protected override string GetName(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            IDataParameter type = args.GetParameterByName("TYPE");
                        IDataParameter code = args.GetParameterByName("CODE");
                        IDataParameter ui = args.GetParameterByName("UI");
                        IDataParameter brief = args.GetParameterByName("BRIEF");
                        IDataParameter includenull = args.GetParameterByName("INCLUDENULL");

            return String.Format(SQL,
                type == null ? NULL : GetValue(type.Value),
                code == null ? NULL : GetValue(code.Value),
                ui == null ? NULL : GetValue(ui.Value),
                brief == null ? true : ConvertDef.ToBoolean(brief.Value, true),
                includenull == null ? false : ConvertDef.ToBoolean(includenull.Value, false));
        }

        private string GetValue(object val)
        {
            if (val == null || val == DBNull.Value)
                return NULL;
            else
                return Convert.ToString(val);
        }

        #endregion

    }
}
