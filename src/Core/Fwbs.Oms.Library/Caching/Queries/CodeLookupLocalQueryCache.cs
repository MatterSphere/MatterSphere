using System;
using System.Data;
using FWBS.Common;

namespace FWBS.OMS.Caching.Queries
{
    internal interface ICodeLookupQueryCache : IQueryCache
    {
    }

    internal sealed class CodeLookupLocalQueryCache : DataTableLocalQueryCache, ICodeLookupQueryCache
    {
        private const string NULL = "{NULL}";
        private const string SQL = "sprCodeLookupList#Type={0}#Code={1}#UI={2}#Brief={3}#IncludeNull={4}";

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

        public override DataTable GetData(Data.ExecuteTableEventArgs args)
        {
            if (Handles(args))
            {
                string cachename = GetName(args);
                IDataParameter type = args.GetParameterByName("TYPE");
                var dt = Cache.Get(Type, GetValue(type.Value), cachename);
                if (dt != null)
                {
                    return dt.Copy();
                }
            }
            return null;
        }

        public override void SetData(Data.ExecuteTableEventArgs args)
        {
            if (args == null || args.Data == null)
                return;

            if (Handles(args))
            {
                string cachename = GetName(args);
                IDataParameter type = args.GetParameterByName("TYPE");
                Cache.Set(Type, GetValue(type.Value), cachename, args.Data);
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

        protected override string Type
        {
            get { return "dbCodeLookup"; }
        }

        #endregion

    }
}
