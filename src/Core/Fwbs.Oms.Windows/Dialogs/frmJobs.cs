using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A form that displays a precedent job list on a popup form.
    /// </summary>
    internal class frmJobs : frmNewBrandIdent
	{
		private System.Windows.Forms.Panel pnlContainer;
		private System.Windows.Forms.Panel pnlTools;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCancel;
	
		#region Controls

		private FWBS.OMS.UI.Windows.ucJoblistRepeaterContainer _joblist;
        private System.ComponentModel.IContainer components;
        private Button btnPrecLib;
		private FWBS.OMS.UI.Windows.ResourceLookup _res;
		
		#endregion


		#region Constructors

		private frmJobs(){}

		public frmJobs(PrecedentJobList jobList)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Text = Session.CurrentSession.Resources.GetResource("JOBLIST", "Job List", "").Text + " - " + jobList.Owner.FullName;
			_joblist.RefreshJobList(jobList);

			if (jobList == Session.CurrentSession.CurrentPrecedentJobList)
			{
				_res.SetLookup(btnSelect, new ResourceLookupItem("PROCESS", "Process", ""));
				btnSelect.Tag = "PROCESS";
				this._joblist.MultiSelect = false;
				this._joblist.ShowToolBar = true;
				this._joblist.ToggleSelection = false;
			}
			else
			{
				_res.SetLookup(btnSelect, new ResourceLookupItem("SELECT", "Select", ""));
				btnSelect.Tag = "SELECT";
			}
			SetIcon(Images.DialogIcons.Task);

		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this._joblist = new FWBS.OMS.UI.Windows.ucJoblistRepeaterContainer();
            this.pnlTools = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnPrecLib = new System.Windows.Forms.Button();
            this._res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlContainer.SuspendLayout();
            this.pnlTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this._joblist);
            this.pnlContainer.Controls.Add(this.pnlTools);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(5);
            this.pnlContainer.Size = new System.Drawing.Size(764, 391);
            this.pnlContainer.TabIndex = 0;
            // 
            // _joblist
            // 
            this._joblist.AutoScroll = true;
            this._joblist.Dock = System.Windows.Forms.DockStyle.Fill;
            this._joblist.Location = new System.Drawing.Point(5, 5);
            this._joblist.MultiSelect = true;
            this._joblist.Name = "_joblist";
            this._joblist.Padding = new System.Windows.Forms.Padding(3);
            this._joblist.ShowToolBar = false;
            this._joblist.Size = new System.Drawing.Size(664, 381);
            this._joblist.TabIndex = 1;
            this._joblist.ToggleSelection = true;
            // 
            // pnlTools
            // 
            this.pnlTools.Controls.Add(this.btnCancel);
            this.pnlTools.Controls.Add(this.btnSelect);
            this.pnlTools.Controls.Add(this.btnPrecLib);
            this.pnlTools.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTools.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTools.Location = new System.Drawing.Point(669, 5);
            this.pnlTools.Name = "pnlTools";
            this.pnlTools.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlTools.Size = new System.Drawing.Size(90, 381);
            this.pnlTools.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(5, 25);
            this._res.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("CANCELBTN", "&Cancel", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Location = new System.Drawing.Point(5, 0);
            this._res.SetLookup(this.btnSelect, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNSELECT", "&Select", ""));
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(85, 25);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Tag = "SELECT";
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnPrecLib
            // 
            this.btnPrecLib.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPrecLib.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrecLib.Location = new System.Drawing.Point(5, 356);
            this._res.SetLookup(this.btnPrecLib, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECEDENTSB", "%PRECEDENTS%...", ""));
            this.btnPrecLib.Name = "btnPrecLib";
            this.btnPrecLib.Size = new System.Drawing.Size(85, 25);
            this.btnPrecLib.TabIndex = 4;
            this.btnPrecLib.Text = "Precedents...";
            this.btnPrecLib.Click += new System.EventHandler(this.btnPrecLib_Click);
            // 
            // frmJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(764, 391);
            this.Controls.Add(this.pnlContainer);
            this.Name = "frmJobs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmJobs";
            this.pnlContainer.ResumeLayout(false);
            this.pnlTools.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		

		#endregion

        private System.Collections.Generic.List<PrecedentJob> _selectedjobs = new System.Collections.Generic.List<PrecedentJob>();
        public PrecedentJob[] SelectedJobs
        {
            get
            {
                return _selectedjobs.ToArray();
            }
        }

		private void btnSelect_Click(object sender, System.EventArgs e)
		{
            _selectedjobs.Clear();

			if (Convert.ToString(btnSelect.Tag) == "SELECT")
			{

				if (_joblist.SelectedCount > 0)
				{
					DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("REMJOBCFM", "Are you sure that you want to remove the selected jobs from '%1%' to your job list?", "", _joblist.JobList.Owner.FullName), null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					switch (res)
					{
						case DialogResult.Yes:
							try
							{
								foreach (ucPrecJobSelector job_ctrl in _joblist.Selected)
								{
									PrecedentJob job = job_ctrl.Object as PrecedentJob;
									if (job != null)
									{
                                        _selectedjobs.Add(job);
									}
								}
								DialogResult = DialogResult.Yes;
							}
							catch (Exception ex)
							{
								MessageBox.Show(ex);
							}
							break;
						case DialogResult.Cancel:
							return;
						default:
							DialogResult = DialogResult.No;
							break;
					}
				}
				else
					DialogResult = DialogResult.No;
			}
			else
			{
				DialogResult = DialogResult.OK;
			}
		}

		private void btnPrecLib_Click(object sender, System.EventArgs e)
		{
            DialogResult = DialogResult.No;
            Services.ShowPrecedentLibrary(null, null, "");
        }
	}
}
