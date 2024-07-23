using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Fwbs.Documents.Preview.Zip
{
    using Handlers;

    [System.Runtime.InteropServices.Guid(ZipPreviewHandlerFactory.ClassID)]
    internal partial class ZipPreviewHandlerControl : PreviewHandlerFromFile
    {
        private IconImageList icons;

        public ZipPreviewHandlerControl()
        {
            InitializeComponent();
            icons = new IconImageList(this.iconList);
        }

        private void SetTreeViewImages()
        {
            Files.Indent = LogicalToDeviceUnits(20);
            Files.ImageList = icons.GetIcons(LogicalToDeviceUnits(this.iconList.ImageSize));
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            SetTreeViewImages();
            base.OnDpiChangedAfterParent(e);
        }

        public override void DoPreview()
        {
            Files.Nodes.Clear();
            TreeNode baseNode = new TreeNode(file.Name);

            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string entryName = entry.FullName;
                        string[] pathParts = entryName.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        TreeNode parent = baseNode;
                        for (int i = 0; i < pathParts.Length; i++)
                        {
                            string pathPart = pathParts[i];
                            if (pathPart == string.Empty)
                                break;

                            TreeNode[] foundNodes = parent.Nodes.Find(pathPart, false);
                            TreeNode partNode;
                            if (foundNodes.Length == 0 || i == pathParts.Length - 1)
                            {
                                partNode = new TreeNode(pathPart);
                                partNode.Name = pathPart;
                                parent.Nodes.Add(partNode);
                                if (i == pathParts.Length - 1)
                                {
                                    partNode.ImageIndex = icons.GetIconIndexForFile(pathPart);
                                    partNode.Tag = entryName;
                                }
                                else
                                    partNode.ImageIndex = 0;
                            }
                            else
                            {
                                partNode = foundNodes[0];
                            }
                            partNode.SelectedImageIndex = partNode.ImageIndex;
                            parent = partNode;
                        }
                    }
                }
            }

            SetTreeViewImages();
            baseNode.Expand();
            Files.Nodes.Add(baseNode);
        }

    }
}
