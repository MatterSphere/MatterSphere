using System.Collections.Generic;

namespace FWBS.OMS.Design.Export
{
    public abstract class ExportBase
    {
        #region Fields
        protected int treeviewParentID = -1;
        protected TreeView _treeview = null;
        protected bool rootimportable = true;
        protected bool active = true;
        protected bool runonce = false;
        protected List<LinkedObject> linkedobjects;
        #endregion

        #region Properties
        public List<LinkedObject> LinkedObjects
        {
            get
            {
                return linkedobjects;
            }
        }
        public int TreeViewParentID
        {
            get
            {
                return treeviewParentID;
            }
            set
            {
                treeviewParentID = value;
            }
        }

        public TreeView TreeView
        {
            get
            {
                return _treeview;
            }
            set
            {
                _treeview = value;
            }
        }

        public bool RootImportable
        {
            get
            {
                return rootimportable;
            }
            set
            {
                rootimportable = value;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public bool RunOnce
        {
            get
            {
                return runonce;
            }
            set
            {
                runonce = value;
            }
        }

        public abstract void ExportTo(string Directory);
        #endregion
    }
}
