using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using FWBS.OMS.Data;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.ColumnSettings
{
    public class ColumnSettingsBuilder
    {
        private Dictionary<XmlNode, System.Windows.Forms.DataGridViewColumn> _columns;
        private XmlDocument _xmlDocListView;
        private XmlNode _xmlColumn;
        private string _searchListCode;

        public ColumnSettingsBuilder(string searchListCode)
        {
            _searchListCode = searchListCode;
            _columns = new Dictionary<XmlNode, System.Windows.Forms.DataGridViewColumn>();
        }

        /// <summary>
        /// Loads columns collection from search list xml
        /// </summary>
        /// <param name="columns"></param>
        internal void LoadNodes(IEnumerable<System.Windows.Forms.DataGridViewColumn> columns)
        {
            _xmlDocListView = new XmlDocument();
            _xmlDocListView.PreserveWhitespace = false;
            _xmlDocListView.LoadXml(Convert.ToString
                (Session.CurrentSession.CurrentConnection.ExecuteSQL
                (string.Format("select schListView from dbsearchlistconfig where schcode = '{0}'", _searchListCode), null)
                .Rows[0][0]));
            _xmlColumn = _xmlDocListView.DocumentElement.SelectSingleNode("/searchList/listView");
            CreateDictionary(columns);
        }

        private void CreateDictionary(IEnumerable<System.Windows.Forms.DataGridViewColumn> columns)
        {
            foreach (XmlNode item in _xmlColumn.ChildNodes)
            {
                var column = columns.FirstOrDefault(col =>
                    col.DataPropertyName == item.Attributes["mappingName"].Value && 
                    (!string.IsNullOrWhiteSpace(col.DataPropertyName) || col.Name != "ActionColumn"));
                _columns.Add(item, column);
            }
        }

        private void WriteAttribute(XmlNode Node, string Name, string Value)
        {
            if (Node.SelectSingleNode("@" + Name) != null)
                Node.SelectSingleNode("@" + Name).Value = Value;
            else
                Node.Attributes.Append(CreateAttribute(Node, Name, Value));
        }

        private XmlAttribute CreateAttribute(XmlNode Node, string Name, string Value)
        {
            var attribute = Node.OwnerDocument.CreateAttribute(Name);
            attribute.Value = Value;
            return attribute;
        }

        private void DeleteCacheFile(string fileName)
        {
            try
            {
                System.IO.FileInfo file = Global.GetCacheFile("searchlist", fileName, true, true);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Updates Search List Column Configuraion in Database
        /// </summary>
        internal void UpdateColumnSettings()
        {
            var xml = string.Empty;
            UpdateColumns();
            if (IsColumnsCustomized())
            {
                //Replace last xml node which representes search list column configuration
                _xmlDocListView.DocumentElement.RemoveChild(_xmlDocListView.DocumentElement.LastChild);
                _xmlDocListView.DocumentElement.AppendChild(_xmlColumn);
                using (var stringWriter = new System.IO.StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    _xmlDocListView.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xml = stringWriter.GetStringBuilder().ToString();
                }
            }
            UpdateDatabaseSettings(xml);
            //Delete cached file for associated search list to apply new configuration settings
            DeleteCacheFile(_searchListCode);
        }

        private void UpdateDatabaseSettings(string xml)
        {
            var connection = Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("Code", _searchListCode));
            parList.Add(connection.CreateParameter("listview", xml));
            connection.ExecuteProcedure("sprUpdateUserSearchListColumns", parList);
        }

        private void UpdateColumns()
        {
            foreach (XmlNode xmlNode in _xmlColumn.ChildNodes)
            {
                if (_columns[xmlNode] != null)
                {
                    WriteAttribute(xmlNode, "visible", _columns[xmlNode].Visible.ToString());
                    WriteAttribute(xmlNode, "orderIndex", _columns[xmlNode].DisplayIndex.ToString());
                }
            }
        }

        internal bool IsColumnsCustomized()
        {
            int defaultIndex = 0;
            foreach (XmlNode node in _xmlColumn.ChildNodes)
            {
                var column = _columns[node];
                if ((column != null && !column.Visible && column.Width > DataGridViewEx.MinimumColumnWidth) || 
                    (column != null && column.Visible && column.DisplayIndex != defaultIndex))
                {
                    return true;
                }
                defaultIndex++;
            }
            return false;
        }
    }
}
