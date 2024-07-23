using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FWBS.OMS.FileManagement;

namespace FWBS.OMS.Teams
{

    public class PrecedentsTeamsAccessItem : LookupTypeDescriptor
    {
        #region Fields

        private DataRow _info;
        internal DataRow Info => _info;
        private Precedent _precedent = null;
        private int _teamId;
        private long _precId;
        private string _teamName;

        #endregion

        #region Constructors

        internal PrecedentsTeamsAccessItem(Precedent precedent, DataRow info)
        {
            _info = info;
            _precedent = precedent;
            _teamId = TeamId;
            _precId = PrecedentId;
            Team team = Team.GetTeam(_teamId);
            _teamName = team.Name;
        }

        internal PrecedentsTeamsAccessItem(Precedent precedent, DataRow info, Team team)
        {
            _info = info;
            _precedent = precedent;
            TeamId = team.ID;
            PrecedentId = precedent.ID;
            _teamName = team.Name;
        }

        public static PrecedentsTeamsAccessItem Create(Precedent precedent, Team team)
        {
            DataTable table = new DataTable("#tempTeamsAccess");
            table.Columns.AddRange(new [] { new DataColumn("teamID"), new DataColumn("precID") }); 
            DataRow info = table.NewRow();
            return new PrecedentsTeamsAccessItem(precedent, info, team: team);
        }
        #endregion

        #region Methods

        internal object GetExtraInfo(string fieldName)
        {
            return _info[fieldName];
        }

        internal void SetExtraInfo(string fieldName, object value)
        {
            if (_info[fieldName] != value)
            {
                _info[fieldName] = value;
            }
        }

        public override string ToString()
        {
            return string.Format(_teamName?? "<unknown>");
        }


        #endregion

        #region Properties

        internal int TeamId
        {
            get
            {
                return Convert.ToInt32(GetExtraInfo("teamID"));
            }
            set
            {
                SetExtraInfo("teamID", value);
            }
        }

        internal long PrecedentId
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("precID"));
            }
            set
            {
                SetExtraInfo("precID", value);
            }
        }

        #endregion
    }
}
