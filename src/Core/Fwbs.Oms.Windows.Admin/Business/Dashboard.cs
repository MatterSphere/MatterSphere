using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Admin.Business
{
    class Dashboard : DashboardSysObject
    {

        private string _description = string.Empty;
        private string _code;

        public Dashboard() : base()
        { }

        public Dashboard(string code) : base(code)
        {
            _code = code;
            _description = CodeLookup.GetLookup("DASHBOARDS", _code);
        }

        public Dashboard(Dashboard clone) : base(clone)
        {
            _code = base.Code;
            _description = CodeLookup.GetLookup("DASHBOARDS", _code);
        }

        public event EventHandler CodeChanged;

        protected virtual void OnCodeChanged(EventArgs e)
        {
            CodeChanged?.Invoke(this, e);
        }

        [LocCategory("(Details)")]
        [Lookup("Code")]
        [RefreshProperties(RefreshProperties.All)]
        public override string Code
        {
            get
            {
                return base.Code;
            }
            set
            {
                if (IsNew)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        MessageBox.ShowInformation("DSHCODEEMPTY", "The Dashboard Code cannot be empty.");
                    }
                    else if (!Exists(value = value.Trim()))
                    {
                        base.Code = value;
                        Description = CodeLookup.GetLookup("DASHBOARDS", value);
                        OnCodeChanged(EventArgs.Empty);
                    }
                    else
                    {
                        if (value != _code)
                            MessageBox.ShowInformation("DSHCODEEXISTS", "The Dashboard Code [%1%] already exists.", value);
                    }
                }
                else
                    MessageBox.ShowInformation("CANNOTCHGCODE", "The Code cannot be changed when set.");
            }
        }

        [LocCategory("(Details)")]
        [Lookup("Description")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (!string.IsNullOrEmpty(base.Code))
                {
                    if (_description != value)
                    {
                        _description = value;
                        FWBS.OMS.CodeLookup.Create("DASHBOARDS", base.Code, value, "", CodeLookup.DefaultCulture, true, true, true);
                    }
                }
                else
                    MessageBox.ShowInformation("ENSRCODEBFRDSC", "Please ensure that the 'Code' property has a value before configuring the 'Description'.");
            }
        }

        [Lookup("DashboardType")]
        public override DashboardTypes DashboardType
        {
            get
            {
                return base.DashboardType;
            }
            set
            {
                if (!string.IsNullOrEmpty(base.Code))
                {
                    if (value != base.DashboardType)
                    {
                        base.DashboardType = value;
                    }
                }
                else
                    MessageBox.ShowInformation("ENSRCODEBFRTYPE",
                        "Please ensure that the 'Code' property has a value before configuring the 'CompatibleType'.");
            }
        }

        public static Dashboard Clone(string code)
        {
            Session.CurrentSession.CheckLoggedIn();
            return new Dashboard(new Dashboard(code));
        }

    }
}
