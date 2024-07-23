using System;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class ClientDocumentFolderBuilderXML : IDocumentFolderBuilder
    {
        public void Build(long id, RadTreeView treeView, string rootString, bool useCheckBox)
        {
            //FWBS.OMS.Client.GetClient(id) - does this expose a Matter collection as a DataTable??
            //query dB get Matters & XML - return as DataTable   

            //create root node - append to treeView
            //loop through Matter table triaging XML
                //append Matter XML to treeView
                //if there is no XML for a Matter
                    //auto-create the Matter's tree 
                    //save the tree to dbFileFolderTreeData
                    //auto-assign the relevant GUIDs to its documents (if any)
                    

        }

        public void Build(string code, RadTreeView treeView, string rootString)
        {
            throw new NotImplementedException();
        }
    }
}
