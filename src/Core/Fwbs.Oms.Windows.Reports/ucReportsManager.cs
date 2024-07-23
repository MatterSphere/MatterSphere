using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI.Windows;
using FWBS.OMS.SearchEngine;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for ucReportManager.
    /// </summary>
    internal class ucReportsManager : System.Windows.Forms.UserControl
	{
		#region Auto Fields
		private System.Windows.Forms.Panel pnlHeading;
		private System.Windows.Forms.Label labHeading;
		private System.Windows.Forms.Button btnClose;
		private FWBS.Common.UI.Windows.eXPPanel pnlBack;
		private FWBS.Common.UI.Windows.eXPFrame pnlMain;
		private FWBS.OMS.UI.Windows.EnquiryForm enqReports;
		private System.Windows.Forms.Label labCount;
		#endregion

		#region Fields
		/// <summary>
		/// The Search List Object used for search the Contacts
		/// </summary>
		private FWBS.OMS.Report _report = null;
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Panel pnlBorder;
		private System.Windows.Forms.Button btnReset;
		private CrystalDecisions.CrystalReports.Engine.ReportDocument crpe;
        private Timer timWait;
        private IContainer components;
        private Button btnOpenSearch;
        private Button btnSaveSearch;
        private frmHourGlass hrg;
        private TableLayoutPanel tableLayoutPanel;

        /// <summary>
        /// The ID of the last opened search
        /// </summary>
        private long _lastOpenedSearch = -1;

		#endregion

		#region Contructors


		public ucReportsManager()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            this.SuspendLayout();
            this.pnlMain.FrameBackColor.SettingsChanged +=new EventHandler(FrameBackColor_SettingsChanged);
			FrameBackColor_SettingsChanged(this,EventArgs.Empty);
			enqReports.TabIndex = 0;
            enqReports.BackColor = Color.Transparent;
			enqReports.Rendered += new EventHandler(enqReports_Rendered);
            ExtColor col = new ExtColor(ExtColorPresets.TaskPainBackColor, ExtColorTheme.Auto);
            this.BackColor = col.Color;
            this.labCount.BackColor = Color.Transparent;
            this.ResumeLayout();
		}


		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlHeading = new System.Windows.Forms.Panel();
            this.labHeading = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.timWait = new System.Windows.Forms.Timer(this.components);
            this.pnlBack = new FWBS.Common.UI.Windows.eXPPanel();
            this.pnlMain = new FWBS.Common.UI.Windows.eXPFrame();
            this.enqReports = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.btnSaveSearch = new System.Windows.Forms.Button();
            this.btnOpenSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.labCount = new System.Windows.Forms.Label();
            this.pnlHeading.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeading
            // 
            this.pnlHeading.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeading.Controls.Add(this.labHeading);
            this.pnlHeading.Controls.Add(this.btnClose);
            this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeading.Location = new System.Drawing.Point(0, 0);
            this.pnlHeading.Name = "pnlHeading";
            this.pnlHeading.Padding = new System.Windows.Forms.Padding(3);
            this.pnlHeading.Size = new System.Drawing.Size(222, 424);
            this.pnlHeading.TabIndex = 99;
            // 
            // labHeading
            // 
            this.labHeading.BackColor = System.Drawing.SystemColors.Control;
            this.labHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labHeading.Location = new System.Drawing.Point(3, 3);
            this.labHeading.Name = "labHeading";
            this.labHeading.Size = new System.Drawing.Size(198, 418);
            this.labHeading.TabIndex = 0;
            this.labHeading.Text = "Report Manager";
            this.labHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Image = global::FWBS.OMS.UI.Windows.Reports.Properties.Resources.BlackColapse;
            this.btnClose.Location = new System.Drawing.Point(201, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(18, 418);
            this.btnClose.TabIndex = 4;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timWait
            // 
            this.timWait.Interval = 1000;
            this.timWait.Tick += new System.EventHandler(this.timWait_Tick);
            // 
            // pnlBack
            // 
            this.pnlBack.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlBack.Controls.Add(this.pnlMain);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBack.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.pnlBack.Size = new System.Drawing.Size(222, 424);
            this.pnlBack.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.enqReports);
            this.pnlMain.Controls.Add(this.tableLayoutPanel);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlMain.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlMain.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlMain.Location = new System.Drawing.Point(4, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlMain.Size = new System.Drawing.Size(214, 420);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            // 
            // enqReports
            // 
            this.enqReports.AutoScroll = true;
            this.enqReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enqReports.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enqReports.IsDirty = false;
            this.enqReports.Location = new System.Drawing.Point(0, 54);
            this.enqReports.Name = "enqReports";
            this.enqReports.Size = new System.Drawing.Size(214, 356);
            this.enqReports.TabIndex = 10;
            this.enqReports.ToBeRefreshed = false;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.pnlBorder, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.btnSaveSearch, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.btnOpenSearch, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.btnReset, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.btnApply, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labCount, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(214, 54);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pnlBorder
            // 
            this.pnlBorder.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel.SetColumnSpan(this.pnlBorder, 3);
            this.pnlBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBorder.Location = new System.Drawing.Point(0, 45);
            this.pnlBorder.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(214, 1);
            this.pnlBorder.TabIndex = 5;
            // 
            // btnSaveSearch
            // 
            this.btnSaveSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSaveSearch.Location = new System.Drawing.Point(164, 0);
            this.btnSaveSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSaveSearch.Name = "btnSaveSearch";
            this.btnSaveSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSaveSearch.TabIndex = 7;
            this.btnSaveSearch.Text = "Save";
            this.btnSaveSearch.Click += new System.EventHandler(this.btnSaveSearch_Click);
            // 
            // btnOpenSearch
            // 
            this.btnOpenSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpenSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpenSearch.Location = new System.Drawing.Point(114, 0);
            this.btnOpenSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpenSearch.Name = "btnOpenSearch";
            this.btnOpenSearch.Size = new System.Drawing.Size(50, 22);
            this.btnOpenSearch.TabIndex = 8;
            this.btnOpenSearch.Text = "Open";
            this.btnOpenSearch.Click += new System.EventHandler(this.btnOpenSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReset.Location = new System.Drawing.Point(114, 20);
            this.btnReset.Margin = new System.Windows.Forms.Padding(0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(50, 22);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Location = new System.Drawing.Point(164, 20);
            this.btnApply.Margin = new System.Windows.Forms.Padding(0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(50, 22);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Search";
            this.btnApply.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // labCount
            // 
            this.labCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labCount.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labCount.Location = new System.Drawing.Point(0, 0);
            this.labCount.Margin = new System.Windows.Forms.Padding(0);
            this.labCount.Name = "labCount";
            this.tableLayoutPanel.SetRowSpan(this.labCount, 2);
            this.labCount.Size = new System.Drawing.Size(114, 40);
            this.labCount.TabIndex = 4;
            this.labCount.Text = "0";
            this.labCount.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labCount.UseMnemonic = false;
            // 
            // ucReportsManager
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBack);
            this.Controls.Add(this.pnlHeading);
            this.Name = "ucReportsManager";
            this.Size = new System.Drawing.Size(222, 424);
            this.pnlHeading.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Threaded To Main Thread
		/// <summary>
		/// The Background Thread that fires this Event into the WinUI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _searchlist_Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
		{
			try
			{
				SearchedEventHandler sch = new SearchedEventHandler(this.Main_searchlist_Searched);
				this.Invoke(sch, new object [2] {sender, e});
			}
			catch
			{}
		}

		/// <summary>
		/// The Background Thread that fires this Before the Search
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _searchlist_Searching(object sender, CancelEventArgs e)
		{
			try
			{
				CancelEventHandler sch = new CancelEventHandler(this.Main_searchlist_Searching);
				this.Invoke(sch,new object [2] {sender, e});
			}
			catch
			{}

		}

		/// <summary>
		/// In Debug mode break on this exception
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _searchlist_Error(object sender, MessageEventArgs e)
		{
            try
            {
                MessageEventHandler sch = new MessageEventHandler(this.Main_searchlist_Error);
                this.Invoke(sch, new object[2] { sender, e });
            }
            catch
            { }
		}

        /// <summary>
        /// In Debug mode break on this exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_searchlist_Error(object sender, MessageEventArgs e)
        {
            StopTimer();

            ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("UNXPERR", "Unexpected error in Report ''%1%''. Please contact support.", "", _report.Code).Text, e.Exception));
            
            labCount.Text = Session.CurrentSession.Resources.GetResource("ERROR", "ERROR", "").Text;
            ParentForm.Cursor = Cursors.Default;
        }

		/// <summary>
		/// Returns the Number of Matches and starts the Auto Display if criteria match
		/// </summary>
		/// <param name="sender">Search List Object</param>
		/// <param name="e">Returned DataTable</param>
		private void Main_searchlist_Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
		{
            StopTimer();
            ParentForm.Cursor = Cursors.WaitCursor;
			try
			{
				labCount.Text = e.Data.Rows.Count.ToString();
				crpe = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
				if (_report.ReportLocation != null)
				{
					crpe.Load(_report.ReportLocation.FullName);
					if (e.DataSet != null)
					{
                        //UTCFIX: DM - 06/12/06 = Make sure that the dates are displayed as local time.
                        DataSet data = e.DataSet.Clone();

                        foreach (DataTable dt in data.Tables)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                if (col.DataType == typeof(DateTime))
                                {
                                    col.DateTimeMode = DataSetDateTime.Local;
                                }
                            }
                        }
                        data.Merge(e.DataSet, false, MissingSchemaAction.Ignore);
                        data.AcceptChanges();

						string[] _tables = _report.SearchList.Tables;
						for(int i = 0; i < data.Tables.Count ; i++)
						{
							try
							{
								data.Tables[i].TableName = _tables[i];
							}
							catch
							{
								data.Tables[i].TableName = "REPORTS"+ (i+1).ToString();
                            }
                            try
                            {
                                crpe.Database.Tables[i].SetDataSource(data.Tables[i]);
					        }
                            catch
                            { }
						}
					}
					else if (e.Data != null)
					{
                        //UTCFIX: DM - 06/12/06 = Make sure that the dates are displayed as local time.
                        DataTable data = e.Data.Clone();
                        foreach (DataColumn col in data.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Local;
                            }
                        }

                        data.Merge(e.Data, false, MissingSchemaAction.Ignore);
                        data.AcceptChanges();
                        
						string[] _tables = _report.SearchList.Tables;
						try
						{
							data.TableName = _tables[0];
						}
						catch
						{
							data.TableName = "REPORTS1";
						}
						if (crpe.Database.Tables.Count > 0)
							crpe.Database.Tables[0].SetDataSource(data);
					}
					else
						throw new Exception(Session.CurrentSession.Resources.GetMessage("DBCRERR", "Error creating Database Connection for Report Link!", "").Text);
				
					// Replace alias fields

				
					// Possible BaseObject/Parent Object to be passed in here.
					FWBS.OMS.FieldParser fparse = new FWBS.OMS.FieldParser(_report.SearchList.Parent);

					foreach (CrystalDecisions.CrystalReports.Engine.ReportObject repObj in crpe.ReportDefinition.ReportObjects)
					{
						// Text Field so parse for update.
						if (repObj is CrystalDecisions.CrystalReports.Engine.TextObject)
						{
							CrystalDecisions.CrystalReports.Engine.TextObject txtObj = repObj as CrystalDecisions.CrystalReports.Engine.TextObject;
							if (txtObj.Text.StartsWith("@"))
							{
								txtObj.Text = txtObj.Text.Substring(1);
								txtObj.Text = Session.CurrentSession.Terminology.Parse(txtObj.Text,true);
								if (enqReports.Enquiry != null)
								{
									DataRow dr = enqReports.Enquiry.Source.Tables["DATA"].NewRow();
									dr.ItemArray = enqReports.Enquiry.Source.Tables["DATA"].Rows[0].ItemArray;
									foreach (Control ctrl in enqReports.Controls)
									{
										if (ctrl is FWBS.Common.UI.IListEnquiryControl)
										{
											FWBS.Common.UI.IListEnquiryControl list = ctrl as FWBS.Common.UI.IListEnquiryControl;
											dr[ctrl.Name] = list.DisplayValue;
										}
									}								
									// Replaces any %Fields with the matching Search Criterai Data
									txtObj.Text = fparse.ParseString(txtObj.Text,dr);
								}
								// Replace any %Fields with any Parent for Parameters
								txtObj.Text = fparse.ParseString(txtObj.Text);
							}
						}
						// Sub Report Object so process loop of Text Fields on this one.
						if (repObj is CrystalDecisions.CrystalReports.Engine.SubreportObject)
						{
							CrystalDecisions.CrystalReports.Engine.SubreportObject subrep = repObj as CrystalDecisions.CrystalReports.Engine.SubreportObject;
						
							foreach (CrystalDecisions.CrystalReports.Engine.ReportObject repObj2 in subrep.OpenSubreport(subrep.SubreportName).ReportDefinition.ReportObjects)
							{
								if (repObj2 is CrystalDecisions.CrystalReports.Engine.TextObject)
								{
									CrystalDecisions.CrystalReports.Engine.TextObject txtObj = repObj2 as CrystalDecisions.CrystalReports.Engine.TextObject;
									if (txtObj.Text.StartsWith("@"))
									{
										txtObj.Text = txtObj.Text.Substring(1);
										txtObj.Text = Session.CurrentSession.Terminology.Parse(txtObj.Text,true);
										// Same as above
										if (enqReports.Enquiry != null)
										{
											DataRow dr = enqReports.Enquiry.Source.Tables["DATA"].NewRow();
											dr.ItemArray = enqReports.Enquiry.Source.Tables["DATA"].Rows[0].ItemArray;
											foreach (Control ctrl in enqReports.Controls)
											{
												if (ctrl is FWBS.Common.UI.IListEnquiryControl)
												{
													FWBS.Common.UI.IListEnquiryControl list = ctrl as FWBS.Common.UI.IListEnquiryControl;
													dr[ctrl.Name] = list.DisplayValue;
												}

											}
											txtObj.Text = fparse.ParseString(txtObj.Text,dr);
										}
										txtObj.Text = fparse.ParseString(txtObj.Text);
									}
								}
							}
						}
					}


					crystalReportViewer.ReportSource = crpe;
					crystalReportViewer.Show();
					OnReportedShowed();
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				ParentForm.Cursor = Cursors.Default;
			}
			
		}

		/// <summary>
		/// Not Implemented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Main_searchlist_Searching(object sender, CancelEventArgs e)
		{
			ParentForm.Cursor = Cursors.WaitCursor;
            timWait.Enabled = true;
        }
		

		#endregion
		
		#region Properties
		[DefaultValue("")]
		public Report Reports 
		{
			get
			{
				return _report;
			}
			set
			{
				if (value != _report)
				{
					if (Session.CurrentSession.IsLoggedIn)
					{
						_report = value;
						if (_report.SearchList != null)
						{
							enqReports.Enquiry = _report.SearchList.CriteriaForm;
							if (enqReports.Enquiry != null)
							{
								DataView dvr = new DataView(enqReports.Enquiry.Source.Tables["QUESTIONS"]);
								dvr.RowFilter = "quRequired = true";
								if (dvr.Count > 0) enqReports.DockPadding.Right = 13;

								bool flag = false;
								foreach (DataColumn cm in enqReports.Enquiry.Source.Tables["DATA"].Columns)
									if (Convert.ToString(enqReports.Enquiry.Source.Tables["DATA"].Rows[0][cm.ColumnName]) == "")
									{
										enqReports.Enquiry.Source.Tables["DATA"].Rows[0][cm.ColumnName] = DBNull.Value;
										flag = true;
									}
								if (flag) enqReports.RenderControls(true);

                                //*** Hide 'Open' and 'Save' buttons when Save Search Type is 'Never'
                                this.btnOpenSearch.Visible = !(_report.SearchList.SaveSearch == SaveSearchType.Never.ToString());
                                this.btnSaveSearch.Visible = !(_report.SearchList.SaveSearch == SaveSearchType.Never.ToString());
							}
							_report.SearchList.Searched -= new FWBS.OMS.SearchEngine.SearchedEventHandler(_searchlist_Searched);
							_report.SearchList.Searching -= new CancelEventHandler(_searchlist_Searching);
							_report.SearchList.Error -= new MessageEventHandler(_searchlist_Error);

							_report.SearchList.Searched += new FWBS.OMS.SearchEngine.SearchedEventHandler(_searchlist_Searched);
							_report.SearchList.Searching += new CancelEventHandler(_searchlist_Searching);
							_report.SearchList.Error += new MessageEventHandler(_searchlist_Error);
						}
					}
				}
			}
		}

		public CrystalDecisions.Windows.Forms.CrystalReportViewer CrystalReportViewer
		{
			get
			{
				return crystalReportViewer;
			}
			set
			{
				crystalReportViewer = value;
			}
		}

		public bool CloseVisible
		{
			get
			{
				return btnClose.Visible;
			}
			set
			{
				btnClose.Visible = value;
			}
		}

		public string Heading
		{
			get
			{
				return labHeading.Text;
			}
			set
			{
				labHeading.Text = value;
			}
		}

		public Button ApplyButton
		{
			get
			{
				return btnApply;
			}
		}
		#endregion

		#region Public Methods
		public void Close()
		{
			OnClosed();
		}

		public void Run()
		{
			btnReports_Click(this,EventArgs.Empty);
		}
		
		#endregion

		#region Events
		public event EventHandler Closed;
		public event EventHandler Apply;
		public event EventHandler ReportsForChanged;
		public event EventHandler CreateNewClick;
		public event EventHandler ReportedShowed;

		public void OnReportedShowed()
		{
			if (ReportedShowed != null)
				ReportedShowed(this,EventArgs.Empty);
		}
		

		public void OnCreateNewClick()
		{
			if (CreateNewClick != null)
				CreateNewClick(this,EventArgs.Empty);
		}

		public void OnReportsForChanged()
		{
			if (ReportsForChanged != null)
				ReportsForChanged(this,EventArgs.Empty);
		}
		
		public void OnClosed()
		{
			if (Closed != null)
				Closed(this,EventArgs.Empty);
		}

		public void OnApply()
		{
			if (Apply != null)
				Apply(this,EventArgs.Empty);
		}
		#endregion

		#region Private Events
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			OnClosed();
		}

		private void btnReports_Click(object sender, System.EventArgs e)
		{
			btnApply.Focus();
			Application.DoEvents();
			try
			{
				enqReports.UpdateItem();
				labCount.Text = Session.CurrentSession.Resources.GetResource("PROCESSING","Processing","").Text;
                if (_report.SearchList != null)
                {
                    //Automatically save the search if SaveSearchType is set to Always
                    if (_report.SearchList.SaveSearch == SaveSearchType.Always.ToString())
                    {
                        string _obj = "";
                        long? _objID = 0;
                        SavedSearches.Tools.GetParentObjectTypeAndID(this.Reports.Code, _report.SearchList.Parent, ref _obj, ref _objID);
                        SavedSearches.SaveForcedSearch(SavedSearches.Tools.BuildSearchCriteriaXML(this.enqReports), this._report.Code, "REPORT", _obj, _objID);
                    }
                    _report.SearchList.Search(true);
                }
				OnApply();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

		private void lnkCreateNew_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			OnCreateNewClick();
		}

		private void FrameBackColor_SettingsChanged(object sender, EventArgs e)
		{
			enqReports.BackColor = pnlMain.FrameBackColor.Color;
			labCount.BackColor = pnlMain.FrameBackColor.Color;
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			foreach (DataColumn cm in enqReports.Enquiry.Source.Tables["DATA"].Columns)
				enqReports.Enquiry.Source.Tables["DATA"].Rows[0][cm.ColumnName] = DBNull.Value;

			enqReports.RenderControls(true);
		
		}

		private void enqReports_Rendered(object sender, EventArgs e)
		{
		}

        /// <summary>
        /// Stops the timer that is used to display the hourglass wait window.
        /// </summary>
        private void StopTimer()
        {
            //Stop the timer and close the form if exists.
            timWait.Enabled = false;
            if (hrg != null)
            {
                Form p = FWBS.OMS.UI.Windows.Global.GetParentForm(this);
                if (p != null) p.BringToFront();
                hrg.Close();
                hrg = null;
            }
        }


        /// <summary>
        /// Captures the timer event so that the hourglass wait form can appear whilst 
        /// the search occurs, but only if a certain amount of time has already elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timWait_Tick(object sender, System.EventArgs e)
        {
            timWait.Enabled = false;
            hrg = new frmHourGlass(this);
            hrg.Closed += new EventHandler(hrg_Closed);
            hrg.ShowDialog(FWBS.OMS.UI.Windows.Global.GetParentForm(this));
        }

        /// <summary>
        /// Captures the hourglass wait windows closed event.
        /// </summary>
        /// <param name="sender">The current wait window.</param>
        /// <param name="e">Empty event arguments.</param>
        private void hrg_Closed(object sender, EventArgs e)
        {
        }

		#endregion





        private void btnSaveSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("btnSaveSearch_Click");
                string _xml = SavedSearches.Tools.BuildSearchCriteriaXML(this.enqReports);

                if (ConvertDef.ToInt64(this._lastOpenedSearch, -1) != -1 && (SavedSearches.Tools.IsUpdateToLastLoadedSearchRequired()))
                {
                    Debug.WriteLine(string.Format("--cmdSaveSearch_Click - already saved as ID {0}", this._lastOpenedSearch));
                    SavedSearch ss = SavedSearch.GetSavedSearch(this._lastOpenedSearch);
                    ss.CriteriaXML = _xml;
                    ss.Update();
                    Debug.WriteLine("Updated");
                }
                else
                {
                    Debug.WriteLine(string.Format("--cmdSaveSearch_Click - new search"));
                    
                    string _desc = SavedSearches.Tools.SaveSearchDescription(this);
                    if (String.IsNullOrWhiteSpace(_desc))
                        return;
                    
                    bool _globalsave = SavedSearches.Tools.IsGlobalSearchRequired();    
                    
                    string _obj = "";
                    long? _objID = 0;
                    SavedSearches.Tools.GetParentObjectTypeAndID(this.Reports.Code, _report.SearchList.Parent, ref _obj, ref _objID);
                    SavedSearches.SaveSearch(_desc, _xml, this.Reports.Code, "REPORT", _obj, _objID, _globalsave); 

                    Debug.WriteLine("Saved");
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }




        private void btnOpenSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SavedSearches.OpenSavedSearchAndPopulateForm(this, this._report.SearchList.Code, this.enqReports, ref this._lastOpenedSearch);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        
        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }


	}
}
