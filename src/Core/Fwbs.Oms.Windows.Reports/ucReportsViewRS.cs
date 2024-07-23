using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for ucReportsView.
    /// </summary>
    public class ucReportsViewRS : System.Windows.Forms.UserControl
	{
		#region Auto Controls

        private FWBS.OMS.UI.Windows.omsSplitter splitter1;
        private ucReportsManagerRS ucReportsManager1;
        public Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private Button _searchbutton = null;
        private object _parent = null;
        private FWBS.Common.KeyValueCollection _param = null;
		#endregion

		#region Contructors
		public ucReportsViewRS()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.splitter1 = new FWBS.OMS.UI.Windows.omsSplitter();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ucReportsManager1 = new FWBS.OMS.UI.Windows.Reports.ucReportsManagerRS();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(222, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 507);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            this.splitter1.Visible = false;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportPath = "";
            this.reportViewer1.Location = new System.Drawing.Point(227, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(711, 507);
            this.reportViewer1.TabIndex = 17;
            this.reportViewer1.RenderingComplete += new Microsoft.Reporting.WinForms.RenderingCompleteEventHandler(this.reportViewer1_RenderingComplete);
            // 
            // ucReportsManager1
            // 
            this.ucReportsManager1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucReportsManager1.Location = new System.Drawing.Point(0, 0);
            this.ucReportsManager1.Name = "ucReportsManager1";
            this.ucReportsManager1.Size = new System.Drawing.Size(222, 507);
            this.ucReportsManager1.TabIndex = 3;
            this.ucReportsManager1.VisibleChanged += new System.EventHandler(this.ucReportsManager1_VisibleChanged);
            // 
            // ucReportsViewRS
            // 
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.ucReportsManager1);
            this.Name = "ucReportsViewRS";
            this.Size = new System.Drawing.Size(938, 507);
            this.Load += new System.EventHandler(this.ucReportsViewRS_Load);
            this.ResumeLayout(false);

		}
		#endregion

        #region Public
        public void OpenReport(string ReportServer, string ReportPath, object Parent, FWBS.Common.KeyValueCollection param)
        {
            try
            {
                _param = param;
                _parent = Parent;
                reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
                reportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServer);
                reportViewer1.ServerReport.ReportPath = ReportPath;
                reportViewer1.RefreshReport();
                ReportParameterInfoCollection rparams = reportViewer1.ServerReport.GetParameters();
                
                List<Microsoft.Reporting.WinForms.ReportParameter> listParams = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                if (Parent != null || _param != null)
                {
                    FWBS.OMS.FieldParser parse = new FieldParser(_parent);
                    parse.ChangeParameters(_param);
                    
                    foreach (ReportParameterInfo rparam in rparams)
                    {
                        if (_param.Contains(rparam.Name))
                        {

                            DateTime? isdate = _param[rparam.Name].Value as DateTime?;
                            ReportParameter rparameter;
                            if (isdate.HasValue)
                            {
                                rparameter = new Microsoft.Reporting.WinForms.ReportParameter(rparam.Name, isdate.Value.ToString("o"));
                            }
                            else
                            {
                                rparameter = new Microsoft.Reporting.WinForms.ReportParameter(rparam.Name, parse.ParseString(Convert.ToString(_param[rparam.Name].Value)));
                            }
                            listParams.Add(rparameter);
                        }
                    }
                    reportViewer1.ServerReport.SetParameters(listParams);
                }


                ucReportsViewRS_Load(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }
        #endregion

        #region Private
        private void ucReportsManager1_VisibleChanged(object sender, System.EventArgs e)
		{
			splitter1.Visible = ucReportsManager1.Visible;
		}

        private void ucReportsViewRS_Load(object sender, EventArgs e)
        {
            ucReportsManager1.SearchButton.Click += new EventHandler(SearchButton_Click);
            try
            {
                System.Windows.Forms.SplitContainer n = reportViewer1.Controls["paramsSplitContainer"] as System.Windows.Forms.SplitContainer;
                if (n == null) throw new Exception(Session.CurrentSession.Resources.GetMessage("OSMHCIC", "Invalid Microsoft Report Viewer. Please contact support. Quote OSMHCIC%1%", "", "3").Text);
                for (int i = n.Panel2.Controls.Count - 1; i >= 0; i--)
                {
                    Control c = n.Panel2.Controls[i] as Control;
                    c.Parent = this;
                    c.BringToFront();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }

            reportViewer1.RefreshReport();
            StealControls();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Reflection.MethodInfo mi = typeof(Button).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                mi.Invoke(_searchbutton, new object[] { EventArgs.Empty });
            }
            catch
            {
                ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("OSMHCIC", "Invalid Microsoft Report Viewer. Please contact support. Quote OSMHCIC%1%", "", "2").Text));
            }
        }

        private void StealControls()
        {
            try
            {
                System.Windows.Forms.SplitContainer n = reportViewer1.Controls["paramsSplitContainer"] as System.Windows.Forms.SplitContainer;
                if (n == null) return;
                
                n.Panel1Collapsed = true;
                UserControl n1 = n.Panel1.Controls[0] as UserControl;
                if (n1 == null) throw new Exception(Session.CurrentSession.Resources.GetMessage("OSMHCIC", "Invalid Microsoft Report Viewer. Please contact support. Quote OSMHCIC%1%", "", "4").Text);
                if (n1.Controls[2] != null && n1.Controls[2].Controls[1] != null)                
                    _searchbutton = n1.Controls[2].Controls[1] as Button;
                else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("OSMHCIC", "Invalid Microsoft Report Viewer. Please contact support. Quote OSMHCIC%1%", "", "5").Text);
                Panel n2 = n1.Controls[0] as Panel;
                if (n2 == null) return;
                if (n2.Controls.Count > 0)
                {
                    ucReportsManager1.Visible = true;
                    if (n2.Controls[0].Controls.Count > 0)
                    {
                        ucReportsManager1.Panel.Visible = false;
                        Global.RemoveAndDisposeControls(ucReportsManager1.Panel);
                    }
                    for (int pi = n2.Controls.Count - 1; pi >= 0; pi--)
                    {
                        Panel n3 = n2.Controls[pi] as Panel;
                        for (int i = n3.Controls.Count - 1; i >= 0; i--)
                        {
                            Control c = n3.Controls[i] as Control;
                            if (c is Label)
                                ((Label)c).AutoSize = true;
                            c.Parent = ucReportsManager1.Panel;
                            c.Dock = DockStyle.Top;
                        }
                    }
                }
                else
                    ucReportsManager1.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
            finally
            {
                ucReportsManager1.Panel.Visible = true;
            }
        }

        private void reportViewer1_RenderingComplete(object sender, Microsoft.Reporting.WinForms.RenderingCompleteEventArgs e)
        {
            StealControls();
        }
        #endregion
    }
}
