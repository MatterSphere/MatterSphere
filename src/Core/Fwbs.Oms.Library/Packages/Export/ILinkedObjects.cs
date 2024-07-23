using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public interface ILinkedObjects
    {
        List<LinkedObject> LinkedObjects {get;}
    }

    public struct LinkedObject
    {
        public LinkedObject(string searchListCode, string ID, Export.PackageTypes PackageType)
        {
            this.searchlistcode = searchListCode;
            this.packagetype = PackageType;
            this.id = ID;
        }

        private Export.PackageTypes packagetype; 
        private string searchlistcode;
        private string id;

        public string SearchListCode 
        { 
            get
            {
                return searchlistcode;
            }
        }

        public string ID 
        {
            get
            {
                return id;
            }
        }

        public Export.PackageTypes packageType 
        {
            get
            {
                return packagetype;   
            }
        }
    }


    public static class LinkedObjectCollector
    {


        public static List<LinkedObject> BuildLinkedObjectList(string sql, string code, string tablename)
        {
            List<LinkedObject> linkedobjects = new List<LinkedObject>();
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);
            System.Data.DataTable oms = OMS.Session.CurrentSession.Connection.ExecuteSQLTable(sql, tablename, false, paramlist);

            if (oms != null && oms.Rows.Count > 0)
            {
                string slcode = OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageObjects);
                LinkedObject obj = new LinkedObject(slcode, Convert.ToString(oms.Rows[0]["ObjCode"]), PackageTypes.OMSObjects);
                linkedobjects.Add(obj);
            }
            return linkedobjects;
        }

    }

}
