using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucTimeStats.
    /// </summary>
    public class ucTimeStats : System.Windows.Forms.UserControl, ISupportRightToLeft
	{
		#region Controls
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel pnlStats;
		private System.Windows.Forms.Timer timFlash;
		private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup cmbView;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
        private System.Collections.Generic.Dictionary<ucTimeStat, Flash> flashing = new System.Collections.Generic.Dictionary<ucTimeStat, Flash>();
        private bool firstrun = false;
		#endregion

		#region Fields
		/// <summary>
		/// The OMSField Object
		/// </summary>
		private OMSFile _omsfile = null;
		/// <summary>
		/// The Flash Color Position from Red to White
		/// </summary>
		private int flashcolorpos = 0;
		/// <summary>
		/// If True Move from Red to White else White to Red
		/// </summary>
		private bool flashcolorfwd = true;
		/// <summary>
		/// The Availiable Credit Limit ucTimeStat Control used to Flash from Red to White
		/// </summary>
		private ucTimeStat avli = null;

        /// <summary>
        /// Returns the True or False if a Warning is Active
        /// </summary>
        private bool _warning = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor a Blank Time Stats Container
        /// </summary>
        public ucTimeStats() : this(false)
        {
        }

        /// <summary>
        /// Constructor a Blank Time Stats Container
        /// </summary>
        /// <param name="largeFont">Indicates whether to use large font or normal</param>
        public ucTimeStats(bool largeFont)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            this.Font = new Font("Segoe UI", largeFont ? 10.5F : 9F);
            if (Flashing)
            {
                this.timFlash.Tick += new System.EventHandler(this.timFlash_Tick);
            }

            pnlStats.ControlAdded += new ControlEventHandler(pnlStats_ControlAdded);
			pnlStats.ControlRemoved += new ControlEventHandler(pnlStats_ControlRemoved);
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_omsfile != null)
				{
					try
					{ _omsfile.Refreshed -= new EventHandler(File_Refreshed); }
					catch { }
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTimeStats));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlStats = new System.Windows.Forms.Panel();
            this.timFlash = new System.Windows.Forms.Timer(this.components);
            this.cmbView = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            // 
            // pnlStats
            // 
            this.pnlStats.AutoSize = true;
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnlStats.Location = new System.Drawing.Point(0, 27);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Size = new System.Drawing.Size(146, 0);
            this.pnlStats.TabIndex = 8;
            // 
            // cmbView
            // 
            this.cmbView.ActiveSearchEnabled = false;
            this.cmbView.AddNotSet = false;
            this.cmbView.CaptionWidth = 0;
            this.cmbView.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbView.IsDirty = false;
            this.cmbView.Location = new System.Drawing.Point(0, 0);
            this.cmbView.Name = "cmbView";
            this.cmbView.NotSetCode = "NOTSET";
            this.cmbView.NotSetType = "RESOURCE";
            this.cmbView.Size = new System.Drawing.Size(146, 27);
            this.cmbView.TabIndex = 7;
            this.cmbView.TerminologyParse = true;
            this.cmbView.Type = "TIMESTATS";
            this.cmbView.ActiveChanged += new System.EventHandler(this.cmbView_Changed);
            // 
            // ucTimeStats
            // 
            this.AutoSize = true;
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.cmbView);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucTimeStats";
            this.Size = new System.Drawing.Size(146, 34);
            this.Load += new System.EventHandler(this.ucTimeStats_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region IOMSTypeAddin Implementation Partial

		/// <summary>
		/// Allows the calling OMS dialog to connect to the addin for the configurable type object.
		/// </summary>
		/// <param name="obj">OMS Configurable type object to use.</param>
		/// <returns>A flag that tells the dialog that the connection has been successfull.</returns>
		public bool Connect(IOMSType obj)
		{
			_omsfile = obj as OMSFile;
			if (obj == null || !Session.CurrentSession.IsPackageInstalled("TIMERECORDING") || !Session.CurrentSession.IsLicensedFor("TIMEREC"))
				return false;
			else if (_omsfile != null)
			{
				_omsfile.Refreshed += new EventHandler(File_Refreshed);
				return true;
			}
			else
			{
				if (Session.CurrentSession.IsInDebug)
					MessageBox.ShowInformation("TIMESTATSERR", "Time Statistics Panel cannot be used on any object other than %FILE%. Please Remove from the Configurable Type.");
				this.Visible = false;
				return false;
			}
		}


		/// <summary>
		/// Captures the Refresh of the File and then refreshes the Client
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void File_Refreshed(object sender, EventArgs e)
		{
			if (_omsfile.Client.IsDirty == false)
				_omsfile.Client.Refresh();
			cmbView_Changed(this, EventArgs.Empty);
		}

		/// <summary>
		/// Refreshes the addin visual look and data contents.
		/// </summary>
		public void RefreshItem()
		{
			// THIS NO LONGER FORCES A REFRESH OF
			// THE FILE AND CLIENT OBJECTS
			if (firstrun == true)
				cmbView_Changed(this, EventArgs.Empty);
		}

		#endregion

		#region Private
		/// <summary>
		/// The Flashing Color Timer
		/// </summary>
		/// <param name="sender">this</param>
		/// <param name="e">Empty</param>
		private void timFlash_Tick(object sender, System.EventArgs e)
		{
			if (flashcolorfwd) flashcolorpos += 20; else flashcolorpos -= 20;
			if (flashcolorpos > 255)
			{
				flashcolorpos = 255;
				flashcolorfwd = false;
			}
			if (flashcolorpos < 0)
			{
				flashcolorpos = 0;
				flashcolorfwd = true;
			}
            avli.BackColor = Color.FromArgb(255, flashcolorpos, flashcolorpos);
        }


		/// <summary>
		/// The Combo Box Change Event
		/// </summary>
		/// <param name="sender">The Combo Box</param>
		/// <param name="e">Empty</param>
		private void cmbView_Changed(object sender, System.EventArgs e)
		{
			// Retreave the Back Color of the Panel if available
			if (Parent != null && Parent is ucNavPanel)
				((ucNavPanel)Parent).PanelBackColor = Color.Empty;
            this.SuspendLayout();
            this.Controls.Remove(pnlStats);
            if (_omsfile == null)
            {
                return;
            }
            try
			{
				_warning = false;				

				Global.RemoveAndDisposeControls(pnlStats);
                flashing.Clear();
                ReportingServer Current = new ReportingServer("FWBS Limited 2005");
				IDataParameter[] param = new IDataParameter[2];
				param[0] = Current.Connection.CreateParameter("objectID", _omsfile.ID);
				param[1] = Current.Connection.CreateParameter("@ObjectType", Convert.ToString(cmbView.SelectedValue));
				DataTable stats = Current.Connection.ExecuteProcedureTable("sprExternalBalances", "BALANCES", param);

				foreach (DataRow row in stats.Rows)
				{
					if (Convert.ToString(row[0]) == "-")
					{
						System.Windows.Forms.Panel sp = new System.Windows.Forms.Panel();
						sp.Height = 10;
						pnlStats.Controls.Add(sp);
					}
					else if (Convert.ToBoolean(row[2]))
					{
                        var flash = new Timer();
                        ucTimeStat stat = new ucTimeStat(imageList1, 0, Convert.ToString(row[0]), String.Format(_omsfile.CurrencyFormat, "{0:C}", row[1]), "") { Dock = DockStyle.Top };
                        flashing.Add(stat, new Flash(0, true));
                        stat.BackColor = Color.FromArgb(208, 2, 27);
                        flash.Tag = stat;
                        flash.Enabled = true;
                        flash.Tick += new EventHandler(flash_Tick);
                        pnlStats.Controls.Add(stat);
						_warning = true;
					}
					else
					{
						pnlStats.Controls.Add(new ucTimeStat(imageList1, 0, Convert.ToString(row[0]), String.Format(_omsfile.CurrencyFormat, "{0:C}", row[1]), "") {Dock = DockStyle.Top});
					}
				}
			}
			catch (ConnectionException cex)
			{
				SqlException sqlex = cex.InnerException as SqlException;
				if (sqlex == null || sqlex.Number != 2812)
				{
					ErrorBox.Show(ParentForm, cex);
					return;
				}

				// If File Code Lookup
				if (Convert.ToString(cmbView.SelectedValue) == "FILE")
				{
					Global.RemoveAndDisposeControls(pnlStats);

                    Panel padding = new Panel();
                    padding.Height = 10;
                    pnlStats.Controls.Add(padding);

                    pnlStats.Controls.Add(new ucTimeStat(imageList1, 0, ResourceLookup.GetLookupText("FILEWIP", "%FILE% WIP:", "%FILE% Work in Progress"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.TimeWIP), ResourceLookup.GetLookupHelp("FILEWIP")));
					pnlStats.Controls.Add(new ucTimeStat(imageList1, 2, ResourceLookup.GetLookupText("FILEBTD", "%FILE% BTD:", "%FILE% Balance to Date"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.TimeBilled), ResourceLookup.GetLookupHelp("FILEBTD")));
					pnlStats.Controls.Add(new ucTimeStat(imageList1, 3, ResourceLookup.GetLookupText("TOTALWIPBTD", "Total : ", "Work in Progress + Balance to Date"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.TimeWIP + _omsfile.TimeBilled), ResourceLookup.GetLookupHelp("TOTALWIPBTD")));

					Panel sp = new Panel();
					sp.Height = 20;
					pnlStats.Controls.Add(sp);

					pnlStats.Controls.Add(new ucTimeStat(imageList1, 4, CodeLookup.GetLookup("FTCLDESC", _omsfile.FundingType.CreditLimitCode), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.CreditLimit), ""));
					avli = new ucTimeStat(imageList1, 3, ResourceLookup.GetLookupText("AVAILABLE", "Available : ", "Credit Limit - Work in Progress + Balance to Date"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.CreditLimit - (_omsfile.TimeWIP + _omsfile.TimeBilled)), ResourceLookup.GetLookupHelp("AVAILABLE"));
					pnlStats.Controls.Add(avli);

                    Panel spacing = new Panel();
                    spacing.Height = 10;
                    pnlStats.Controls.Add(spacing);
				}
				// If Client Code Lookup
				else if (Convert.ToString(cmbView.SelectedValue) == "CLIENT")
				{
					Global.RemoveAndDisposeControls(pnlStats);

                    Panel padding = new Panel();
                    padding.Height = 10;
                    pnlStats.Controls.Add(padding);

                    pnlStats.Controls.Add(new ucTimeStat(imageList1, 0, ResourceLookup.GetLookupText("CLIENTWIP", "%CLIENT% WIP:", "%CLIENT% Work in Progress"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.Client.TimeWIP), ResourceLookup.GetLookupHelp("CLIENTWIP")));
					pnlStats.Controls.Add(new ucTimeStat(imageList1, 2, ResourceLookup.GetLookupText("CLIENTBTD", "%CLIENT% BTD:", "%CLIENT% Billed to Date"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.Client.TimeBilled), ResourceLookup.GetLookupHelp("CLIENTBTD")));
					pnlStats.Controls.Add(new ucTimeStat(imageList1, 3, ResourceLookup.GetLookupText("TOTALWIPBTD", "Total : ", "Work in Progress + Balance to Date"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.Client.TimeWIP + _omsfile.Client.TimeBilled), ResourceLookup.GetLookupHelp("TOTALWIPBTD")));
					Panel sp = new Panel();
					sp.Height = 20;
					pnlStats.Controls.Add(sp);
					pnlStats.Controls.Add(new ucTimeStat(imageList1, 2, ResourceLookup.GetLookupText("CLIENTCLIM", "%CLIENT% CLimit:", "%CLIENT% Credit Limit on all %FILES%"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.Client.TimeCreditLimit), ResourceLookup.GetLookupHelp("CLIENTCLIM")));
					avli = new ucTimeStat(imageList1, 3, ResourceLookup.GetLookupText("AVAILABLE", "Available Credit: ", "Credit Limit Less all Work in Progress + Billed to Date"), String.Format(_omsfile.CurrencyFormat, "{0:C}", _omsfile.Client.TimeCreditLimit - (_omsfile.Client.TimeWIP + _omsfile.Client.TimeBilled)), ResourceLookup.GetLookupHelp("AVAILABLE"));
					pnlStats.Controls.Add(avli);

                    Panel spacing = new Panel();
                    spacing.Height = 10;
                    pnlStats.Controls.Add(spacing);
				}
				else // If File Detailed 
				{
					//TODO: Finish File Detailed Stats
					Global.RemoveAndDisposeControls(pnlStats);
				}
			}
            finally
            {
                if (_omsfile.IsCreditWarning && avli != null)
                {
                    avli.ForeColor = Color.White;
                    avli.BackColor = Color.FromArgb(208, 2, 27);
                    _warning = true;
                }
                else
                {
                    _warning = false;
                }

                pnlStats.Dock = DockStyle.Top;
                this.Controls.Add(pnlStats, true);
                pnlStats.BringToFront();
                this.ResumeLayout();
                this.Refresh();
            }
        }

        private void flash_Tick(object sender, EventArgs e)
        {
            try
            {
                Flash iflash = flashing[(ucTimeStat)((Timer)sender).Tag];

                if (iflash.Direction) iflash.ColorID += 20; else iflash.ColorID -= 20;
                if (iflash.ColorID > 255)
                {
                    iflash.ColorID = 255;
                    iflash.Direction = false;
                }
                if (iflash.ColorID < 0)
                {
                    iflash.ColorID = 0;
                    iflash.Direction = true;
                }
                ((ucTimeStat)((Timer)sender).Tag).BackColor = Color.FromArgb(255, iflash.ColorID, iflash.ColorID);
            }
            catch
            {
                ((Timer)sender).Enabled = false;
            }
        }

        /// <summary>
        /// Set The Default Stats Information on Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucTimeStats_Load(object sender, System.EventArgs e)
		{
			firstrun = true;
			if (_omsfile != null)
			{
				cmbView.Value = "FILE";
			}
			else
			{
				cmbView_Changed(sender, e);
			}

			Refresh();
		}

		/// <summary>
		/// Sets Property of the Controls added to this Container
		/// Then Resizes to fit
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlStats_ControlAdded(object sender, ControlEventArgs e)
		{
			e.Control.Dock = DockStyle.Top;
			e.Control.BringToFront();
			pnlStats.Height = pnlStats.Height + e.Control.Height;
		}

		/// <summary>
		/// If the Controls are Remove resize the Container
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlStats_ControlRemoved(object sender, ControlEventArgs e)
		{
			pnlStats.Height = pnlStats.Height - e.Control.Height;
		}
		#endregion

		#region Public
		/// <summary>
		/// A Force Resize of the Container
		/// </summary>
		public new void Refresh()
		{
			int h = 0;
			foreach (Control ctrl in this.Controls)
			{
				h = h + ctrl.Height;
			}
			h = h + this.DockPadding.Top + this.DockPadding.Bottom;
			this.Height = h;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Warning Statis
		/// </summary>
		public bool Warning
		{
			get
			{
				return _warning;
			}
		}

        /// <summary>
        /// The flag for flashing feature
        /// </summary>
        public bool Flashing { get; set; }
        #endregion

        public void SetRTL(Form parentform)
        {
        }
    }

    internal class Flash
    {
        internal Flash(int colorID, bool Direction)
        {
            colorno = colorID;
            direction = Direction;
        }

        private int colorno;
        internal int ColorID
        {
            get { return colorno; }
            set { colorno = value; }
        }

        private bool direction;
        internal bool Direction
        {
            get { return direction; }
            set { direction = value; }
        }
    }
}
