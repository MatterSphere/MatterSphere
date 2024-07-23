using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class TreeViewItem
    {
        #region Fields
        private DataRow _parent;
        #endregion

        #region Constructors
        public TreeViewItem(DataRow Parent)
        {
            _parent = Parent;
        }
        #endregion

        #region Private
        private void CascadeUpdate(DataRow row, string fieldname, object value)
        {
            DataView dv = new DataView(row.Table);
            dv.RowFilter = String.Format("[Parent] = {0}", row["ID"]);
            if (dv.Count > 0)
            {
                foreach (DataRowView drv in dv)
                {
                    drv[fieldname] = value;
                    CascadeUpdate(drv.Row, fieldname, value);
                }
            }
        }
        #endregion

        #region Properties
        [System.ComponentModel.Category("Data")]
        public bool InstallByDefault
        {
            get
            {
                return Convert.ToBoolean(_parent["Active"]);
            }
            set
            {
                if (Convert.ToBoolean(_parent["Active"]) != value)
                {
                    _parent["Active"] = value;
                    CascadeUpdate(_parent, "Active", value);
                }
            }
        }

        [System.ComponentModel.Category("Data")]
        public bool InstallOnce
        {
            get
            {
                return Convert.ToBoolean(_parent["InstallOnce"]);
            }
            set
            {
                _parent["InstallOnce"] = value;
                CascadeUpdate(_parent, "InstallOnce", value);
            }
        }

        [System.ComponentModel.Category("(Details)")]
        public FWBS.OMS.Design.Export.PackageTypes Type
        {
            get
            {
                return (FWBS.OMS.Design.Export.PackageTypes)FWBS.Common.ConvertDef.ToEnum(_parent["Type"], FWBS.OMS.Design.Export.PackageTypes.None);
            }
        }

        [System.ComponentModel.Category("(Details)")]
        public string Code
        {
            get
            {
                return _parent["Code"] as string;
            }
        }

        [System.ComponentModel.Category("(Details)")]
        public string Name
        {
            get
            {
                return _parent["Name"] as string;
            }
        }

        [System.ComponentModel.Category("(Details)")]
        public string Description
        {
            get
            {
                return _parent["Description"] as string;
            }
        }
        #endregion
    }
}
