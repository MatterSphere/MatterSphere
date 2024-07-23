using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FWBS.OMS.FileManagement;

namespace FWBS.OMS.Teams
{
    public class PrecedentsTeamsAccessCollection : Crownwood.Magic.Collections.CollectionWithEvents
    {
        private DataTable _info;
        private Precedent _precedent;

        private const string Table = "TEAMSACCESS";
        private const string Sql = "select * from dbPrecedentTeamsAccess";

        internal PrecedentsTeamsAccessCollection(Precedent precedent, DataTable info)
        {
            _info = info.Copy();
            _precedent = precedent;
            BuildCollection();
        }

        internal PrecedentsTeamsAccessCollection(Precedent precedent)
        {
            _precedent = precedent;
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.AddParameter("prec", precedent.ID);
            _info = Session.CurrentSession.Connection.ExecuteSQLTable(@"select pta.* from dbPrecedentTeamsAccess pta
                                                                            inner join dbPrecedents p
                                                                            on p.precID = pta.precID
                                                                            where pta.precId = @prec"
                , Table, pars);
            BuildCollection();
        }

        private void BuildCollection()
        {
            foreach (DataRow row in _info.Rows)
            {
                Add(new PrecedentsTeamsAccessItem(_precedent, row));
            }

            //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
            if (_info.PrimaryKey == null || _info.PrimaryKey.Length == 0)
                _info.PrimaryKey = new DataColumn[2] {_info.Columns["precID"], _info.Columns["teamID"] };

        }

        
        public PrecedentsTeamsAccessItem New(Team team)
        {
            DataRow row = _info.NewRow();
            return new PrecedentsTeamsAccessItem(_precedent, row, team);
        }

        public PrecedentsTeamsAccessItem Add(PrecedentsTeamsAccessItem value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value);
            return value;
        }
        
        public void Remove(PrecedentsTeamsAccessItem value)
        {
            // Use base class to process actual collection operation
            if (this.Contains(value))
            {
                base.List.Remove(value);
            }
        }

        public void Insert(int index, PrecedentsTeamsAccessItem value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(PrecedentsTeamsAccessItem value)
        {
            return base.List.Any<PrecedentsTeamsAccessItem>(
                i => 
                        i.PrecedentId == value.PrecedentId 
                     && i.TeamId == value.TeamId);
        }

        public PrecedentsTeamsAccessItem this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as PrecedentsTeamsAccessItem); }
        }


        internal void Update()
        {
            bool added = false;
            foreach (PrecedentsTeamsAccessItem team in this)
            {
                if (team.Info.RowState == DataRowState.Detached)
                {
                    added = true;
                    _info.Rows.Add(team.Info);
                }
            }

            foreach (DataRow row in _info.Rows)
            {
                bool exists = false;
                for (int idx = 0; idx < this.Count; idx++)
                {
                    PrecedentsTeamsAccessItem team = this[idx];
                    if (team.Info == row)
                    {
                        team.SetExtraInfo("precID", _precedent.ID);
                        team.SetExtraInfo("teamID", team.TeamId);
                        exists = true;
                        break;
                    }
                }
                if (exists == false) row.Delete();
            }

            if (_info.GetChanges() != null)
            {
                Session.CurrentSession.Connection.Update(_info, Sql + " where precId = " + _precedent.ID.ToString());
                if (added)
                    _info.AcceptChanges();
            }
        }
    }
}
