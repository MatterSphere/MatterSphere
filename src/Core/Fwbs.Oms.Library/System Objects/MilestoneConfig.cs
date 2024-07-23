using System;
using System.Data.SqlClient;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS
{
    public class MilestoneConfig : CommonObject
    {
        public MilestoneConfig() : base()
        {
            SetExtraInfo("MSAll", false);       
        }

        [EnquiryUsage(true)]
        public MilestoneConfig(string MSCode)
        {
            Fetch(MSCode);
        }
        
        protected override string SelectStatement
        {
            get { return "SELECT * FROM dbMSConfig_OMS2K"; }
        }

        protected override string PrimaryTableName
        {
            get { return "MSConfig"; }
        }

        protected override string DefaultForm
        {
            get { return "SCRSYSMSCONFIG2"; }
        }

        public override object Parent
        {
            get { return null; }
        }

        public override string FieldPrimaryKey
        {
            get { return "MSCode"; }
        }

        [EnquiryUsage(true)]
        public string Code
        {
            get
            {
                return Convert.ToString(GetExtraInfo("MSCode"));
            }
            set
            {
                SetExtraInfo("MSCode", value);
            }
        }

        public static MilestoneConfig Clone(string OldCode, string NewCode)
        {
            MilestoneConfig p = new MilestoneConfig(OldCode);
            MilestoneConfig n = p.Clone() as MilestoneConfig;
            n.Code = NewCode;
            return n;
        }

		public override void Update()
		{
			try
			{
				base.Update();
			}
			catch (ConnectionException cex)
			{
				SqlException sqlex = cex.InnerException as SqlException;
				if (sqlex != null && sqlex.Message.StartsWith("Violation of PRIMARY KEY constraint", StringComparison.InvariantCultureIgnoreCase))
					throw new OMSException2("ERRDUPMSCODE", "MileStone Config '%1%' already exists please choose another.", sqlex, true, Code);

				throw;				
			}
		}
    }
}
