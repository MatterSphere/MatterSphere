using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    public partial class DMTreeViewXMLManager
    {
        // Xml tag for node, e.g. 'node' in case of <node></node>
        private const string XmlNodeTag = "node";
        private const string XmlNodeSystemAtt = "system";
        private const string XmlNodeSystemIdAtt = "SystemID";
        private const string XmlNodeFolderCodeAtt = "FolderCode";
        private const string XmlNodeFolderGUIDAtt = "FolderGUID";
        private const string XMLNodeDocWalletsAtt = "DocWallets";


        string treeXml = "";
        DataTable walletsForMatter;
        string folderXML;

        DMTreeNodeTagData newNodeData;
        RadTreeView treeView;
        string strRoot;
        bool usecheckbox;

        public DMTreeViewXMLManager(RadTreeView TreeView)
        {
            newNodeData = new DMTreeNodeTagData();
            treeView = TreeView;
        }

        public DMTreeViewXMLManager()
        {
            newNodeData = new DMTreeNodeTagData();
        }


        public string SerializeTreeView(RadTreeView TreeView)
        {
            treeView = TreeView;
            return SerializeTreeView();
        }

        public void DeserialiseXML(string xml, string rootString, bool useCheckBox)
        {
            folderXML = xml;
            strRoot = rootString;
            usecheckbox = useCheckBox;
            DeserializeTreeView();
        }


        //*****************************************************************
        // deserialize the tree
        //*****************************************************************

        public void DeserializeTreeView()
        {
            XmlReader reader = null;

            if (!string.IsNullOrWhiteSpace(folderXML))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(folderXML);

                try
                {
                    // disabling re-drawing of treeview till all nodes are added
                    treeView.BeginUpdate();
                    reader = XmlReader.Create(new StringReader(folderXML));

                    RadTreeNode parentNode = null;
                    //create root node and add it to the tree
                    RadTreeNode rootNode = Windows.TreeViewNavigation.TreeViewFormatter.NewTreeNode();
                    rootNode.Text = strRoot;
                    treeView.Nodes.Add(rootNode);
                    parentNode = rootNode;

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == XmlNodeTag && reader.AttributeCount > 0)
                            {
                                RadTreeNode newNode = Windows.TreeViewNavigation.TreeViewFormatter.NewTreeNode();
                                if (treeView.Nodes.Count > 0)
                                {
                                    if (usecheckbox)
                                        newNode.CheckType = CheckType.CheckBox;
                                    newNode.Image = treeView.ImageList.Images[0];
                                }

                                bool isEmptyElement = reader.IsEmptyElement;

                                // loading node attributes
                                int attributeCount = reader.AttributeCount;
                                if (attributeCount > 0)
                                {
                                    for (int i = 0; i < attributeCount; i++)
                                    {
                                        reader.MoveToAttribute(i);
                                        SetAttributeValue(newNode, reader.Name, reader.Value);
                                    }
                                    if (!string.IsNullOrWhiteSpace(newNodeData.folderCode))
                                    {
                                        DMTreeNodeTagData newNodeTag = new DMTreeNodeTagData() { folderCode = newNodeData.folderCode,
                                                                                                 folderGUID = newNodeData.folderGUID,
                                                                                                 system = newNodeData.system,
                                                                                                 systemID = newNodeData.systemID,
                                                                                                 docWallets = newNodeData.docWallets
                                                                                               };
                                        newNode.Tag = newNodeTag;
                                        newNode.Name = Convert.ToString(newNodeTag.folderGUID);
                                        newNodeData.system = false;
                                        newNodeData.systemID = String.Empty;
                                        newNodeData.folderCode = null;
                                        newNodeData.folderGUID = Guid.Empty;
                                        newNodeData.docWallets = String.Empty;
                                    }
                                }
                                // add new node to Parent Node or TreeView
                                if (parentNode != null)
                                    parentNode.Nodes.Add(newNode);
                                
                                // making current node 'ParentNode' if its not empty
                                if (!isEmptyElement)
                                {
                                    parentNode = newNode;
                                }
                            }
                        }
                        // moving up to in TreeView if end tag is encountered
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if (reader.Name == XmlNodeTag)
                            {
                                parentNode = parentNode.Parent;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                        {
                            //Ignore Xml Declaration                    
                        }
                        else if (reader.NodeType == XmlNodeType.None)
                        {
                            return;
                        }
                        else if (reader.NodeType == XmlNodeType.Text)
                        {
                            parentNode.Nodes.Add(reader.Value);
                        }
                    }
                }
                finally
                {
                    // enabling redrawing of treeview after all nodes are added
                    treeView.EnableTheming = true;
                    treeView.ThemeName = "Windows8";
                    treeView.EndUpdate();
                    reader.Close();
                }
            }
        }


        /// &lt;span class="code-SummaryComment">&lt;summary>&lt;/span>
        /// Used by Deserialize method for setting properties of
        /// TreeNode from xml node attributes
        /// &lt;span class="code-SummaryComment">&lt;/summary>&lt;/span>
        private void SetAttributeValue(RadTreeNode node, string propertyName, string value)
        {
            if (propertyName == XmlNodeSystemIdAtt)
            {
                newNodeData.systemID = Convert.ToString(value);
            } 
            if (propertyName == XmlNodeSystemAtt)
            {
                newNodeData.system = Convert.ToBoolean(value);
            }
            else if (propertyName == XmlNodeFolderCodeAtt)
            {
                newNodeData.folderCode = value;
                node.Text = FWBS.OMS.CodeLookup.GetLookup("DFLDR_MATTER", newNodeData.folderCode);
            }
            else if (propertyName == XmlNodeFolderGUIDAtt)
            {
                newNodeData.folderGUID = new Guid(value);
            }
            else if (propertyName == XMLNodeDocWalletsAtt)
            {
                newNodeData.docWallets = value;
            }
        }


        public void AssignTreeStructureToMatter(long id, string rootString, out IFolderMapper mapper)
        {
            mapper = null;
            FileType ft = FWBS.OMS.FileType.GetFileType(FWBS.OMS.OMSFile.GetFile(id).FileTypeCode);
            if (string.IsNullOrWhiteSpace(ft.TemplateCode))
            {
                FolderMappingWithoutTemplate(id, rootString, ref mapper);
            }
            else
            {
                FolderMappingWithTemplate(rootString, ft, ref mapper, id);
            }
        }

        private void FolderMappingWithTemplate(string rootString, FileType ft, ref IFolderMapper mapper, long fileID)
        {
            string xml = new DocumentFolderRepositoryXML().Get("dbFileFolderTreeTemplates", ft.TemplateCode, "sprRetrieveFileFolderTemplate");

            if (!string.IsNullOrWhiteSpace(xml))
            {
                DeserialiseXML(xml, rootString, true);
                UpdateFolderGUIDs();
                var xmlAfterGuidsUpdated = SerializeTreeView();
                mapper = new TemplateFolderMapper(new Guid(CheckTreeForSystemNode(xmlAfterGuidsUpdated, "Correspondence")), 
                                                  new Guid(CheckTreeForSystemNode(xmlAfterGuidsUpdated, "Email")), 
                                                  treeView, 
                                                  fileID);
           }
        }

        private void FolderMappingWithoutTemplate(long id, string rootString, ref IFolderMapper mapper)
        {
            DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();
            treeXml = repository.CreateDefaultTreeXML();

            CreateAdditionalFoldersBasedUponWallets(repository, id);

            if (walletsForMatter != null && walletsForMatter.Rows.Count > 1)
            {
                mapper = new WalletDrivenMapper(id, 
                                                walletsForMatter,
                                                new Guid(CheckTreeForSystemNode(treeXml, "Correspondence")), 
                                                new Guid(CheckTreeForSystemNode(treeXml, "Email")));
            }

            if (mapper == null)
                mapper = new DefaultTreeMapper(id, 
                                               new Guid(CheckTreeForSystemNode(treeXml, "Correspondence")), 
                                               new Guid(CheckTreeForSystemNode(treeXml, "Email")));

            DeserialiseXML(treeXml, rootString, true);
        }

        private void CreateAdditionalFoldersBasedUponWallets(DocumentFolderRepositoryXML repository, long id)
        {
            walletsForMatter = GetWalletCodesForCreationAsFolders(repository, id);

            if (walletsForMatter != null && walletsForMatter.Rows.Count > 1)
            {
                AddFoldersForWallets(walletsForMatter);
            }
        }


        private DataTable GetWalletCodesForCreationAsFolders(DocumentFolderRepositoryXML repository, long id)
        {
            walletsForMatter = repository.GetWalletCodesForCreationAsFolders(id, FWBS.OMS.Session.CurrentSession.DefaultCulture);

            if (walletsForMatter != null && walletsForMatter.Rows.Count > 1)
            {
                return walletsForMatter;
            }

            walletsForMatter = repository.GetWalletCodesForCreationAsFolders(id, "{default}");

            return walletsForMatter;
        }


        private void AddFoldersForWallets(DataTable walletsForMatter)
        {
            //new process
            var nodesToAddToXml = new List<XElement>();
            bool systemGuidsAdded = false;

            foreach (DataRow wallet in walletsForMatter.Rows)
            {
                if (wallet["folderCode"].ToString() == "GENERAL" || wallet["folderCode"].ToString() == "EMAIL")
                {
                    if (!systemGuidsAdded)
                    {
                        UpdateFolderCodeTable("GENERAL", CheckTreeForSystemNode(treeXml, "Correspondence"));
                        UpdateFolderCodeTable("EMAIL", CheckTreeForSystemNode(treeXml, "Email"));
                        systemGuidsAdded = true;
                    }
                    continue;
                }

                var newNode = CreateNewNode(wallet["folderCode"].ToString());
                UpdateFolderCodeTable(Convert.ToString(newNode.Attribute("FolderCode").Value), Convert.ToString(newNode.Attribute("FolderGUID").Value));
                nodesToAddToXml.Add(newNode);
            }

            if (nodesToAddToXml.Count > 0)
            {
                AddNodesToXml(nodesToAddToXml);
            }
        }

        private void UpdateFolderCodeTable(string foldercode, string folderguid)
        {
            foreach (DataRow r in walletsForMatter.Rows)
            {
                if (Convert.ToString(r["FolderCode"]) == foldercode)
                {
                    r["FolderGUID"] = new Guid(folderguid);
                }
            }
        }

        private XElement CreateNewNode(string folderCode)
        {
            return new XElement("node",
                                    new XAttribute("SystemID", ""),
                                    new XAttribute("system", false),
                                    new XAttribute("FolderCode", folderCode),
                                    new XAttribute("FolderGUID", Guid.NewGuid()));
        }


        private void AddNodesToXml(IEnumerable<XElement> nodes)
        {
            var xDocument = XDocument.Parse(treeXml);

            var parentNode = xDocument.Descendants("node").FirstOrDefault();

            foreach (var node in nodes)
            {
                parentNode.Add(node);
            }

            treeXml = xDocument.ToString();
        }


        public string CheckTreeForSystemNode(string treeXML, string systemID)
        {
            string result = "";
            XDocument doc = XDocument.Parse(treeXML);
            var elements = doc.Descendants("node").Descendants("node");

            if (elements != null)
            {
                result = GetSystemNodeFromDescendants(elements, systemID);
            }
            return result;
        }


        public string GetSystemNodeFromDescendants(IEnumerable<XElement> nodes, string systemID)
        {
            foreach (XElement x in nodes)
            {
                if (x.Attribute("SystemID").Value == systemID)
                {
                    return x.Attribute("FolderGUID").Value;
                }

                if (x.HasElements)
                {
                    GetSystemNodeFromDescendants(x.Descendants("node"), systemID);
                }
            }

            return "";
        }


        private void UpdateFolderGUIDs()
        {
            foreach (RadTreeNode n in CollectNodes(treeView.Nodes))
            {
                DMTreeNodeTagData tagdata = (DMTreeNodeTagData)n.Tag;
                tagdata.folderGUID = Guid.NewGuid();
                n.Tag = tagdata;
                n.Name = Convert.ToString(tagdata.folderGUID);
            }
        }


        public IEnumerable<RadTreeNode> CollectNodes(RadTreeNodeCollection nodes)
        {
            foreach (RadTreeNode node in nodes)
            {
                if (node.Tag != null)
                {
                    yield return node;
                }

                foreach (var child in CollectNodes(node.Nodes))
                    if (child.Tag != null)
                    {
                        yield return child;
                    }
            }
        }


        private ArrayList GetGUIDsFromTreeView()
        {
            ArrayList guids = new ArrayList();
            DMTreeNodeTagData correspondence = (DMTreeNodeTagData)treeView.Nodes[0].Nodes[0].Tag;
            guids.Add(correspondence.folderGUID);
            DMTreeNodeTagData email = (DMTreeNodeTagData)treeView.Nodes[0].Nodes[1].Tag;
            guids.Add(email.folderGUID);
            return guids;
        }


        //*****************************************************************
        // serialize the tree
        //*****************************************************************

        public string SerializeTreeView()
        {
            using (TextWriter writer = new Utf8StringWriter())
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(writer))
                {
                    textWriter.Formatting = Formatting.Indented;
                    textWriter.WriteStartDocument();
                    // writing the main tag that encloses all node tags
                    textWriter.WriteStartElement("TreeView");

                    // save the nodes, recursive method
                    SaveNodes(treeView.Nodes, textWriter);

                    textWriter.WriteEndElement();

                    textWriter.Close();

                    return writer.ToString();
                }
            }
        }


        private void SaveNodes(RadTreeNodeCollection nodesCollection, XmlTextWriter textWriter)
        {
            for (int i = 0; i < nodesCollection.Count; i++)
            {
                RadTreeNode node = nodesCollection[i];
                textWriter.WriteStartElement(XmlNodeTag);
                if (node.Tag != null)
                {
                    DMTreeNodeTagData nodeData = (DMTreeNodeTagData)node.Tag;
                    textWriter.WriteAttributeString(XmlNodeSystemIdAtt, Convert.ToString(nodeData.systemID));
                    textWriter.WriteAttributeString(XmlNodeSystemAtt, Convert.ToString(nodeData.system));
                    textWriter.WriteAttributeString(XmlNodeFolderCodeAtt, nodeData.folderCode);
                    textWriter.WriteAttributeString(XmlNodeFolderGUIDAtt, nodeData.folderGUID.ToString());
                    textWriter.WriteAttributeString(XMLNodeDocWalletsAtt, nodeData.docWallets);
                }
                // add other node properties to serialize here  
                if (node.Nodes.Count > 0)
                {
                    SaveNodes(node.Nodes, textWriter);
                }
                textWriter.WriteEndElement();
            }
        }

    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
