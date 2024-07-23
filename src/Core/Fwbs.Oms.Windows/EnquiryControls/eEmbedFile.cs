using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class eEmbedFile : FWBS.Common.UI.Windows.eBase2
    {
        #region Consts
        private const string header =
@"Content-Type: text/plain; charset=US-ASCII; {0}
Content-transfer-encoding: base64

";
        #endregion

        #region Fields
        private Panel pnlMain;
        private Button btnView;
        private Button btnClear;
        private Button btnInsert;
        private TextBox txtFilename;
        private OpenFileDialog openFilename;
        private string _value;
        private bool _readonly;
        #endregion

        public eEmbedFile() : base()
        {
            InitializeComponent();
            if (Session.CurrentSession.IsLoggedIn)
            {
                btnClear.Text = Session.CurrentSession.Resources.GetResource("CLEAR", "Clear", "").Text;
                btnInsert.Text = Session.CurrentSession.Resources.GetResource("INSERT", "Insert", "").Text;
                btnView.Text = Session.CurrentSession.Resources.GetResource("VIEW", "View", "").Text;
            }
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnView = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.openFilename = new System.Windows.Forms.OpenFileDialog();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.AutoSize = true;
            this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMain.Controls.Add(this.txtFilename);
            this.pnlMain.Controls.Add(this.btnView);
            this.pnlMain.Controls.Add(this.btnInsert);
            this.pnlMain.Controls.Add(this.btnClear);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(451, 23);
            this.pnlMain.TabIndex = 0;
            // 
            // txtFilename
            // 
            this.txtFilename.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFilename.Location = new System.Drawing.Point(0, 0);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(315, 23);
            this.txtFilename.TabIndex = 0;
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(44, 23);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.AutoSize = true;
            this.btnInsert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnInsert.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnInsert.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(47, 23);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnClear
            // 
            this.btnClear.AutoSize = true;
            this.btnClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(45, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // openFilename
            // 
            this.openFilename.Filter = "All Files (*.*)|*.*";
            this.openFilename.Title = "Embed File";
            // 
            // eEmbedFile
            // 
            this.Controls.Add(this.pnlMain);
            this.Size = new System.Drawing.Size(451, 23);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        #region Properties
        [Category("Settings")]
        [Description("The Open Dialog Filter e.g All Files (*.*)|*.*")]
        public string DialogFilter
        {
            get
            {
                return openFilename.Filter;
            }
            set
            {
                openFilename.Filter = value;
            }
        }

        [Category("Settings")]
        [Description("The Default Folder that will be open when the Dialog Opens")]
        public string DialogDefaultFolder
        {
            get
            {
                return openFilename.InitialDirectory;
            }
            set
            {
                if (value != "")
                {
                    try
                    {
                        openFilename.InitialDirectory = value;
                    }
                    catch
                    { }
                }
            }
        }

        public override bool ReadOnly
        {
            get
            {
                return _readonly;
            }
            set
            {
                _readonly = value;
                if (_readonly)
                {
                    btnClear.Enabled = false;
                    btnInsert.Enabled = false;
                }
            }
        }

        private int maxSize = 0;

        [DefaultValue(0)]
        [Category("Settings")]
        public int MaximumSizeInKB
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        public override int CaptionWidth
        {
            get
            {
                return base.CaptionWidth;
            }
            set
            {
                base.CaptionWidth = value;
                this.Padding = new Padding(value, 0, 0, 0);
            }
        }

        [Browsable(false)]
        public override bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Convert.ToString(value);
                if (_value != "")
                {
                    System.IO.DirectoryInfo directory = FWBS.OMS.Global.GetTempPath();
                    string getheader = _value.Substring(0, _value.IndexOf(Environment.NewLine + Environment.NewLine) + 4);
                    int pos2 = getheader.IndexOf(Environment.NewLine);
                    txtFilename.Text = getheader.Substring("Content-Type: text/plain; charset=US-ASCII;".Length, pos2 - "Content-Type: text/plain; charset=US-ASCII;".Length).Trim();
                    btnClear.Enabled = (_readonly == false);
                    btnView.Enabled = true;
                    btnInsert.Enabled = false;
                }
                else
                {
                    btnClear.Enabled = false;
                    btnView.Enabled = false;
                    btnInsert.Enabled = (_readonly == false);
                }
            }
        }
        #endregion

        #region Public
        public void ViewEmbedFile()
        {
            if (_value != "")
            {
                string getheader = "";
                string fileName = "";
                string getvalue = "";
                System.IO.DirectoryInfo directory;

                try
                {
                    directory = FWBS.OMS.Global.GetTempPath();
                    directory = directory.CreateSubdirectory("Embeded");
                    directory.Delete(true);
                }
                catch { }

                directory = FWBS.OMS.Global.GetTempPath();
                directory = directory.CreateSubdirectory("Embeded");
                
                getheader = _value.Substring(0, _value.IndexOf(Environment.NewLine + Environment.NewLine) + 4);
                getvalue = _value.Substring(getheader.Length);
                int pos2 = getheader.IndexOf(Environment.NewLine);
                fileName = directory.FullName + "\\" + getheader.Substring("Content-Type: text/plain; charset=US-ASCII;".Length, pos2 - "Content-Type: text/plain; charset=US-ASCII;".Length).Trim();

                try
                {
                    using (System.IO.FileStream reader = System.IO.File.Create(fileName))
                    {
                        byte[] buffer = Convert.FromBase64String(getvalue);
                        reader.Write(buffer, 0, buffer.Length);
                    }
                }
                catch (System.IO.IOException)
                { }

                ProcessStartInfo i = new ProcessStartInfo(fileName);
                if (string.IsNullOrEmpty(i.Arguments))
                {
                    Process p = new Process {StartInfo = i};
                    p.Start();
                }
            }
        }
        #endregion

        #region Private
        private void btnInsert_Click(object sender, EventArgs e)
        {
            Insert();
        }

        public void Insert()
        {
            try
            {
                if (openFilename.ShowDialog() == DialogResult.OK)
                {
                    FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.ValidateFileExtension(openFilename.FileName);
                    Insert(openFilename.FileName);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(TopLevelControl, ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            _value = "";
            txtFilename.Text = "";
            btnClear.Enabled = false;
            btnView.Enabled = false;
            btnInsert.Enabled = (_readonly == false);
            base.IsDirty = true;
            base.OnActiveChanged();
            base.OnChanged();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                ViewEmbedFile();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(TopLevelControl, ex);
            }
        }

        public void Insert(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            if (maxSize > 0)
            {
                if ((file.Length / 1024) > maxSize)
                    throw new OMSException2("ERRMAXSIZE", "The file '%1%' exceeds the Maximum size of %2% KB", new Exception(), false, file.Name, maxSize.ToString());
            }
            if (file.Exists)
            {
                using (System.IO.FileStream reader = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open,FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[reader.Length];
                    reader.Read(buffer, 0, (int)reader.Length);
                    string base64 = Convert.ToBase64String(buffer);
                    _value = String.Format(header, file.Name) + base64;
                    txtFilename.Text = file.Name;
                    base.IsDirty = true;
                    base.OnActiveChanged();
                    base.OnChanged();
                }
            }
            btnClear.Enabled = (_readonly == false);
            btnView.Enabled = true;
            btnInsert.Enabled = false;
        }
        #endregion

        public override int PreferredHeight
        {
            get
            {
                return txtFilename.PreferredHeight;
            }
        }
    }
}