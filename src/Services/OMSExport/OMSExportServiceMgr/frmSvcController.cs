using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;


namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// This form is a service controller to connect to the OMSExchange service
	/// If using the service application as a service set this to be the startup object
	/// For details of other alterations then see header of frmManager
	/// </summary>
	public class frmSvcController : System.Windows.Forms.Form
	{
		
		#region Fields

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Label label3;

		/// <summary>
		/// flag to indicate that a service is in it's started state
		/// </summary>
		private bool _blnRunning;
		/// <summary>
		/// variable for displaying icon in systray
		/// </summary>
		private System.Windows.Forms.NotifyIcon _notifyIcon1;
		/// <summary>
		/// contect menu for tray icon
		/// </summary>
		private System.Windows.Forms.ContextMenu contextMenu1;
		/// <summary>
		/// forms caption
		/// </summary>
		private string _caption;
		/// <summary>
		/// Popup menu items
		/// </summary>
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemStart;
		private System.Windows.Forms.MenuItem menuItemStop;
		private System.Windows.Forms.MenuItem menuItemPause;
		private System.Windows.Forms.MenuItem menuItemRestore;
		private System.Windows.Forms.MenuItem menuSpacer1;
		private System.Windows.Forms.MenuItem menuSpacer2;
		/// <summary>
		/// Main menu items
		/// </summary>
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mmenuItemSettings;
		private System.Windows.Forms.MenuItem mmenuItemExit;
		private System.Windows.Forms.MenuItem mmenuItemRefresh;
		/// <summary>
		/// Service controller component
		/// </summary>
		private System.ServiceProcess.ServiceController _svcController;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		
		private System.Windows.Forms.Timer _statusTimer;
		private System.Windows.Forms.Timer animate_timer;
		
		private string _servicename = "3EMatterSphereOMSExportService";
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblFinancials;
		private System.Windows.Forms.Label lblMatUpd;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Label lblMatAdd;
		private System.Windows.Forms.Label lbClientAdd;
		private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.Label lblClientUpd;
		private System.Windows.Forms.Label lblLastError;
        private MenuItem menuItem5;
        private MenuItem menuLastError;
        private MenuItem menuStats;
        private Label label10;
        private Label lblLookup;
        private Label label13;
        private Label lblUsers;
        private MenuItem menuViewLog;
		/// <summary>
		/// Used to track icon number for animation
		/// </summary>
		private int _iIndex = 2;


		#endregion

		#region Constructors and Destructors

		public frmSvcController()
		{
			InitializeComponent();

			SetUpMenu();

			SetUpTrayIcon();			

		}
		
		
		~frmSvcController()
		{}
		
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		#endregion
		
		#region Windows Form Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSvcController));
            this.btnStop = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mmenuItemSettings = new System.Windows.Forms.MenuItem();
            this.mmenuItemRefresh = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mmenuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuLastError = new System.Windows.Forms.MenuItem();
            this.menuStats = new System.Windows.Forms.MenuItem();
            this.menuViewLog = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this._statusTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblLookup = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblErrors = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblFinancials = new System.Windows.Forms.Label();
            this.lblMatUpd = new System.Windows.Forms.Label();
            this.lblClientUpd = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblMatAdd = new System.Windows.Forms.Label();
            this.lbClientAdd = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLastError = new System.Windows.Forms.Label();
            this.animate_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.Location = new System.Drawing.Point(8, 97);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(32, 32);
            this.btnStop.TabIndex = 5;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnGo
            // 
            this.btnGo.Enabled = false;
            this.btnGo.Image = ((System.Drawing.Image)(resources.GetObject("btnGo.Image")));
            this.btnGo.Location = new System.Drawing.Point(8, 17);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(32, 32);
            this.btnGo.TabIndex = 6;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(42, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(42, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "Stop";
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.Location = new System.Drawing.Point(8, 57);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(32, 32);
            this.btnPause.TabIndex = 10;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(42, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Pause";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem5,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mmenuItemSettings,
            this.mmenuItemRefresh,
            this.menuItem4,
            this.mmenuItemExit});
            this.menuItem1.Text = "&Options";
            // 
            // mmenuItemSettings
            // 
            this.mmenuItemSettings.Index = 0;
            this.mmenuItemSettings.Text = "&Settings";
            this.mmenuItemSettings.Click += new System.EventHandler(this.mmenuItemSettings_Click);
            // 
            // mmenuItemRefresh
            // 
            this.mmenuItemRefresh.Index = 1;
            this.mmenuItemRefresh.Text = "&Reset Stats";
            this.mmenuItemRefresh.Click += new System.EventHandler(this.mmenuItemRefresh_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // mmenuItemExit
            // 
            this.mmenuItemExit.Index = 3;
            this.mmenuItemExit.Text = "E&xit";
            this.mmenuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLastError,
            this.menuStats,
            this.menuViewLog});
            this.menuItem5.Text = "&View";
            // 
            // menuLastError
            // 
            this.menuLastError.Index = 0;
            this.menuLastError.Text = "Last &Error";
            this.menuLastError.Click += new System.EventHandler(this.menuLastError_Click);
            // 
            // menuStats
            // 
            this.menuStats.Index = 1;
            this.menuStats.Text = "&Stats";
            this.menuStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // menuViewLog
            // 
            this.menuViewLog.Index = 2;
            this.menuViewLog.Text = "&Log Entries";
            this.menuViewLog.Click += new System.EventHandler(this.menuViewLog_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3});
            this.menuItem2.Text = "&Help";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "&About";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // _statusTimer
            // 
            this._statusTimer.Tick += new System.EventHandler(this._statusTimer_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblLookup);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.lblUsers);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.lblStatus);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblErrors);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lblFinancials);
            this.groupBox3.Controls.Add(this.lblMatUpd);
            this.groupBox3.Controls.Add(this.lblClientUpd);
            this.groupBox3.Controls.Add(this.lblTime);
            this.groupBox3.Controls.Add(this.lblMatAdd);
            this.groupBox3.Controls.Add(this.lbClientAdd);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(80, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(448, 141);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Statistics";
            // 
            // lblLookup
            // 
            this.lblLookup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLookup.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLookup.Location = new System.Drawing.Point(369, 95);
            this.lblLookup.Name = "lblLookup";
            this.lblLookup.Size = new System.Drawing.Size(72, 16);
            this.lblLookup.TabIndex = 20;
            this.lblLookup.Text = "0";
            this.lblLookup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(232, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(128, 16);
            this.label13.TabIndex = 19;
            this.label13.Text = "Lookups Added:";
            // 
            // lblUsers
            // 
            this.lblUsers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUsers.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUsers.Location = new System.Drawing.Point(137, 95);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(72, 16);
            this.lblUsers.TabIndex = 18;
            this.lblUsers.Text = "0";
            this.lblUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(8, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 16);
            this.label10.TabIndex = 17;
            this.label10.Text = "Users Added:";
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStatus.Location = new System.Drawing.Point(65, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(144, 16);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(8, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 16);
            this.label12.TabIndex = 15;
            this.label12.Text = "Status:";
            // 
            // lblErrors
            // 
            this.lblErrors.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblErrors.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrors.Location = new System.Drawing.Point(372, 16);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(68, 16);
            this.lblErrors.TabIndex = 14;
            this.lblErrors.Text = "0";
            this.lblErrors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(232, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 16);
            this.label11.TabIndex = 13;
            this.label11.Text = "Errors:";
            // 
            // lblFinancials
            // 
            this.lblFinancials.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFinancials.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblFinancials.Location = new System.Drawing.Point(369, 76);
            this.lblFinancials.Name = "lblFinancials";
            this.lblFinancials.Size = new System.Drawing.Size(72, 16);
            this.lblFinancials.TabIndex = 12;
            this.lblFinancials.Text = "0";
            this.lblFinancials.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMatUpd
            // 
            this.lblMatUpd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMatUpd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMatUpd.Location = new System.Drawing.Point(369, 56);
            this.lblMatUpd.Name = "lblMatUpd";
            this.lblMatUpd.Size = new System.Drawing.Size(72, 16);
            this.lblMatUpd.TabIndex = 11;
            this.lblMatUpd.Text = "0";
            this.lblMatUpd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblClientUpd
            // 
            this.lblClientUpd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblClientUpd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblClientUpd.Location = new System.Drawing.Point(369, 36);
            this.lblClientUpd.Name = "lblClientUpd";
            this.lblClientUpd.Size = new System.Drawing.Size(72, 16);
            this.lblClientUpd.TabIndex = 10;
            this.lblClientUpd.Text = "0";
            this.lblClientUpd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTime
            // 
            this.lblTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTime.Location = new System.Drawing.Point(137, 75);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(72, 16);
            this.lblTime.TabIndex = 9;
            this.lblTime.Text = "0";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMatAdd
            // 
            this.lblMatAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMatAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMatAdd.Location = new System.Drawing.Point(137, 55);
            this.lblMatAdd.Name = "lblMatAdd";
            this.lblMatAdd.Size = new System.Drawing.Size(72, 16);
            this.lblMatAdd.TabIndex = 8;
            this.lblMatAdd.Text = "0";
            this.lblMatAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbClientAdd
            // 
            this.lbClientAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbClientAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbClientAdd.Location = new System.Drawing.Point(137, 35);
            this.lbClientAdd.Name = "lbClientAdd";
            this.lbClientAdd.Size = new System.Drawing.Size(72, 16);
            this.lbClientAdd.TabIndex = 7;
            this.lbClientAdd.Text = "0";
            this.lbClientAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(231, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 16);
            this.label9.TabIndex = 6;
            this.label9.Text = "Financials Added:";
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(231, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "Matters Updated:";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(231, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Clients Updated:";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(8, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "Time Entries Added:";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(8, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Matters Added:";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(8, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Clients Added:";
            // 
            // lblLastError
            // 
            this.lblLastError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLastError.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLastError.Location = new System.Drawing.Point(8, 3);
            this.lblLastError.Name = "lblLastError";
            this.lblLastError.Size = new System.Drawing.Size(31, 10);
            this.lblLastError.TabIndex = 18;
            this.lblLastError.Visible = false;
            // 
            // animate_timer
            // 
            this.animate_timer.Interval = 500;
            this.animate_timer.Tick += new System.EventHandler(this.animate_timer_Tick);
            // 
            // frmSvcController
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(530, 147);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblLastError);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "frmSvcController";
            this.Text = "3E MatterSphere OMS Export Service Manager";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmManager_Closing);
            this.Load += new System.EventHandler(this.frmSvcController_Load);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		
		static void Main() 
		{
			//check if it is already running and do n ot do anything if it is
			Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
			if (processes.Length < 2)
			{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmSvcController());
			}
			else
			{
				MessageBox.Show("Application already running check task manager.");
			}
		}
		
		#endregion

		#region Event Handlers
		
		/// <summary>
		/// Show the settings dialog form if on local machine
		/// </summary>
		private void mmenuItemSettings_Click(object sender, System.EventArgs e)
		{
			//show the settings form
			frmSettings frm = new frmSettings();
			frm.ShowDialog();
		}
		
		/// <summary>
		/// resets all counters back to 0
		/// </summary>
		private void mmenuItemRefresh_Click(object sender, System.EventArgs e)
		{
			string strWrite = @"0,0,0,0,0,0,0,0,0, ";

			try
			{
				string strFile = FWBS.OMS.OMSEXPORT.StaticLibrary.LogFileName;

                try
                {
                    System.IO.File.Delete(strFile);
                }
                catch { }
						
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(strFile))
				{
					sw.Write(strWrite);
				}

				lblErrors.Text = "0";
				lbClientAdd.Text = "0";
				lblClientUpd.Text = "0";
				lblMatAdd.Text = "0";
				lblMatUpd.Text = "0";
				lblTime.Text = "0";
				lblFinancials.Text = "0";
				lblLastError.Text = "";
                lblUsers.Text = "0";
				
			}
			catch {}

			SetLabels();
		}


		
		/// <summary>
		/// connects to OMSExchange service spcified on the server within the text box
		/// </summary>
		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			//change cursor to hourglass as it may take a while
			HourglassCursor();
			ConnectToService();
			NormalCursor();
		}



		/// <summary>
		/// Handler fo double click of tray icon
		/// </summary>
		private void _notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			// Set the WindowState to normal if the form is minimized.
			if (this.WindowState == FormWindowState.Minimized)
			{
				// Activate the form.
				this.Activate();	
				this.Visible = true;
				this.WindowState = FormWindowState.Normal;
				this.ShowInTaskbar = true;
			}
		}

		
		/// <summary>
		/// event handler for the exit menu item
		/// </summary>
		private void menuItemExit_Click(object sender,EventArgs e)
		{	
			//force icon to disappear
			try
			{
				_notifyIcon1.Visible = false;
			
				if(_svcController !=null)
				{
					_svcController.Dispose();
					_svcController = null;
				}
			}
			catch{}

			Application.DoEvents();
			Application.Exit();
		}
		
		/// <summary>
		/// Handlws the about menu item being clicked
		/// </summary>
		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			frmAbout frm = new frmAbout();
			frm.ShowDialog();
		}
		
		
		/// <summary>
		/// starts or resumes the processing
		/// </summary>
		private void btnGo_Click(object sender, System.EventArgs e)
		{
			SetCaption("Service Starting...");
			HourglassCursor();
			_statusTimer.Stop();
			
			// implemetented error handling here for instances where service cannot start
			// may need mnore refining possible one option is to kick of a seperate thread 
			// to query the service status while we await for it to start
			// may need to also strengthen up pause and stop buttons
			try
			{
				CreateServiceController();

				//start the service or continue if paused
				if (_svcController.Status == ServiceControllerStatus.Paused)
				{
					_svcController.Continue();
					_svcController.Close();
				}
				else
				{
					if(_svcController.Status != ServiceControllerStatus.Running)
					{
						//attempt to start service
						Thread thread = new Thread(new ThreadStart(StartService)); 
						thread.Start();
							
						//wait for 1 minute for service to be running 
						_svcController.WaitForStatus(ServiceControllerStatus.Running,new System.TimeSpan(0,1,0));
						
						//set buttons for running state
						SetStatus();
						_svcController.Close();	
												
					}
					else
					{
						//set buttons for running state
						SetStatus();
						_svcController.Close();		
					}
				}
				
			}
			catch(Exception ex)
			{
				lblStatus.Text = ex.Message;
				Application.DoEvents();
				SetStatus();
					
			}
			finally
			{
				try
				{
					if(_svcController != null)
					{
						_svcController.Dispose();
						_svcController = null;	
					}
				}
				catch{}
				
				_statusTimer.Start();
			}

		}
		
		
		/// <summary>
		/// stops the processing
		/// </summary>
		private void btnStop_Click(object sender, System.EventArgs e)
		{
			
			SetCaption("Service Stopping...");	
			HourglassCursor();
			
			_statusTimer.Stop();
			try
			{
				CreateServiceController();

				if((_svcController.Status == ServiceControllerStatus.Running)
					||_svcController.Status == ServiceControllerStatus.Paused)
				{
					_svcController.Stop();
	
					//wait for up to a minute
					_svcController.WaitForStatus(ServiceControllerStatus.Stopped,new System.TimeSpan(0,1,0));
				}

				_svcController.Close();

				SetStatus();
			
			}
			catch
			{}
			finally
			{
				_svcController.Dispose();
				_svcController = null;
				_statusTimer.Start();
			}
		}
		
		
		

		
		/// <summary>
		/// pause processing
		/// </summary>
		private void btnPause_Click(object sender, System.EventArgs e)
		{
			SetCaption("Service pausing...");
			HourglassCursor();
			_statusTimer.Stop();
			try
			{
				CreateServiceController();

				if(_svcController.Status == ServiceControllerStatus.Running)
				{
					_svcController.Pause();
					
					//wait for up to a minute
					_svcController.WaitForStatus(ServiceControllerStatus.Paused,new System.TimeSpan(0,1,0));
				}
			
				SetStatus();

				_svcController.Close();

			}
			catch
			{}
			finally
			{
				_svcController.Dispose();
				_svcController = null;
				_statusTimer.Start();
			}
		}
		
		
		/// <summary>
		/// as form is closing check is made to see if process is running
		/// </summary>
		private void frmManager_Closing(object sender,CancelEventArgs e)
		{
			e.Cancel = true;
			this.WindowState = FormWindowState.Minimized;
			this.ShowInTaskbar = false;
			this.Visible = false;
		}


		#endregion

		#region Methods
		
		/// <summary>
		/// sets up inital form values called from a seperate thread in the forms load event
		/// </summary>
		private void SetUpForm()
		{
			_caption = this.Text;
			try
			{
				//create the service controller component
				CreateServiceController();
			
				//this method has the logic to handle the error when service does not exist
				SetStatus();
			}
			catch
			{}
			finally
			{
				if(_svcController != null)
				{
					_svcController.Close();
					_svcController.Dispose();
					_svcController = null;
				}
				
				_statusTimer.Start();
			}
		}

		
		/// <summary>
		/// sets the labels and controld depending on the current state of processing
		/// </summary>
		/// <param name="status">1=start 2=paused 3=stopped</param>
		private void SetStatus()
		{
						
			try
			{
				//when creating ervice controller on local machine it doesn't error
				//until we query it's status so error handling will catch it.
				switch(_svcController.Status.ToString())
				{
					case "Running": // running
						btnGo.Enabled = false;
						btnPause.Enabled = true;
						btnStop.Enabled = true;
						menuItemExit.Enabled = true;
						mmenuItemExit.Enabled = true;
						menuItemPause.Enabled = true;
						menuItemStart.Enabled = false;
						menuItemStop.Enabled = true;
						SetCaption("Running");
						_blnRunning = true;
						animate_timer.Start();
						_notifyIcon1.Text = "OMS Export Service Running";
						break;
					case "Paused": //paused
						btnGo.Enabled = true;
						btnPause.Enabled = false;
						btnStop.Enabled = true;
						menuItemExit.Enabled = true;
						mmenuItemExit.Enabled = true;
						menuItemPause.Enabled = false;
						menuItemStart.Enabled = true;
						menuItemStop.Enabled = true;
						SetCaption("Paused");
						_blnRunning = false;
						StopAnimation();
						_notifyIcon1.Text = "OMS Export Service Paused";
						break;
					case "Stopped": //stopped
						btnGo.Enabled = true;
						btnPause.Enabled = false;
						btnStop.Enabled = false;
						menuItemExit.Enabled = true;
						mmenuItemExit.Enabled = true;
						menuItemPause.Enabled = false;
						menuItemStart.Enabled = true;
						menuItemStop.Enabled = false;
						_blnRunning = false;
						SetCaption("Stopped");
						StopAnimation();
						_notifyIcon1.Text = "OMS Export Service Stopped";
						break;
					case "Stopping":  //Stopping need to test these
						btnGo.Enabled = false;
						btnPause.Enabled = false;
						btnStop.Enabled = false;
						menuItemExit.Enabled = true;
						mmenuItemExit.Enabled = true;
						menuItemPause.Enabled = false;
						menuItemStart.Enabled = false;
						menuItemStop.Enabled = false;
						_blnRunning = false;
						SetCaption("Stopping");
						StopAnimation();
						_notifyIcon1.Text = "OMS Export Service Stopping";
						break;
					case "Starting":
						btnGo.Enabled = false;
						btnPause.Enabled = false;
						btnStop.Enabled = false;
						menuItemExit.Enabled = true;
						mmenuItemExit.Enabled = true;
						menuItemPause.Enabled = false;
						menuItemStart.Enabled = false;
						menuItemStop.Enabled = false;
						_blnRunning = false;
						SetCaption("Starting");
						StopAnimation();
						_notifyIcon1.Text = "OMS Export Service Starting";
						break;

				}
				SetLabels();
				NormalCursor();
			}
			catch
			{
				//error querying service status
				btnGo.Enabled = false;
				btnPause.Enabled = false;
				btnStop.Enabled = false;
				menuItemExit.Enabled = true;
				mmenuItemExit.Enabled = true;
				menuItemPause.Enabled = false;
				menuItemStart.Enabled = false;
				menuItemStop.Enabled = false;
				this.Text = _caption + "-" + "Unavailable";
				SetCaption("Service Unavailable....");
			}
			finally
			{}

		}
		
		
		/// <summary>
		/// updates controls with status event information
		/// </summary>
		/// <param name="message">message to display</param>
		private void SetCaption(string message)
		{
			lblStatus.Text = message;
			Application.DoEvents();
		}
		
		/// <summary>
		/// sets the screen cursor to an hourglass
		/// </summary>
		private void HourglassCursor()
		{
			Cursor cursor = Cursors.WaitCursor;
			this.Cursor = cursor;
		}
		
		
		/// <summary>
		/// sets the screen cursor to the normal arrow
		/// </summary>
		private void NormalCursor()
		{
			Cursor cursor = Cursors.Default;
			this.Cursor = cursor;
		}
		
		/// <summary>
		/// stops the timer and sets icon to default icon
		/// </summary>
		private void StopAnimation()
		{
			try
			{
				animate_timer.Stop();
			}
			catch{}
			SetDefaultIcon();
		}

		

		/// <summary>
		/// animates the systray icon DMB 16/6/2004 now called from timer instead of seperate thread
		/// </summary>
		private void AnimateIcon()
		{
			try
			{

				if (_iIndex > 6)// if we are off the scale of iteartion has finished set to standard icon
					_iIndex = 2;

				switch(_iIndex)
				{
					case 1:
						_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync1.ico"));
						break;
					case 2:
						_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync2.ico"));
						break;
					case 3:
						_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync3.ico"));
						break;
					case 4:
						_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync4.ico"));
						break;
					case 5:
						_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync5.ico"));
						break;
					case 6:
						_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync6.ico"));
						break;
				}
				_notifyIcon1.Visible = true;
				Application.DoEvents();
				
				_iIndex++;
				
			}
			catch
			{}
			
		}

		
		/// <summary>
		/// This is called when the sevice is not running
		/// </summary>
		private void SetDefaultIcon()
		{
			try
			{
				_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync1.ico"));
				_notifyIcon1.Visible = true;
				Application.DoEvents();
			}
			catch{}
		}
		

		/// <summary>
		/// sets ups the shortcut menu
		/// </summary>
		private void SetUpMenu()
		{
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			
			this.menuItemRestore = new System.Windows.Forms.MenuItem();
			this.menuItemStart = new System.Windows.Forms.MenuItem();
			this.menuItemPause = new System.Windows.Forms.MenuItem();
			this.menuItemStop = new System.Windows.Forms.MenuItem();
			this.menuSpacer1 = new System.Windows.Forms.MenuItem();
			this.menuSpacer2 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
						
			// Initialize contextMenu1
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {	this.menuItemRestore,
																						 this.menuSpacer1,
																						 this.menuItemStart,
																						 this.menuItemPause,
																						 this.menuItemStop,
																						 this.menuSpacer2,
																						 this.menuItemExit});
			
			// Initialize menuItemStop
			this.menuItemRestore.Index = 0;
			this.menuItemRestore.Text = "O&pen OMS Export Manager";
			this.menuItemRestore.Click += new System.EventHandler(this._notifyIcon1_DoubleClick);

			//insert first spacer
			this.menuSpacer1.Index = 1;
			this.menuSpacer1.Text = "-";

			// Initialize menuItemStart
			this.menuItemStart.Index = 2;
			this.menuItemStart.Text = "S&tart";
			this.menuItemStart.Click += new System.EventHandler(this.btnGo_Click);
			
			// Initialize menuItemPause
			this.menuItemPause.Index = 3;
			this.menuItemPause.Text = "P&ause";
			this.menuItemPause.Click += new System.EventHandler(this.btnPause_Click);
			
			// Initialize menuItemStop
			this.menuItemStop.Index = 4;
			this.menuItemStop.Text = "Sto&p";
			this.menuItemStop.Click += new System.EventHandler(this.btnStop_Click);
			
			//insert second spacer
			this.menuSpacer2.Index = 5;
			this.menuSpacer2.Text = "-";

			// Initialize menuItemExit
			this.menuItemExit.Index = 6;
			this.menuItemExit.Text = "E&xit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);

			//disable some tof the items so we can wait until the application starts
			this.menuItemStart.Enabled = false;
			this.menuItemStop.Enabled = false;
			this.menuItemPause.Enabled = false;
			
		}

		
		/// <summary>
		/// Set up the sys tray icon
		/// </summary>
		private void SetUpTrayIcon()
		{
			//stuff for tray icon 
			this.components = new System.ComponentModel.Container();
			
			// Create the NotifyIcon.
			this._notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			
			_notifyIcon1.Icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("FWBS.OMS.OMSEXPORT.sync1.ico"));

			// The ContextMenu property sets the menu that will appear when the systray icon is right clicked.
			_notifyIcon1.ContextMenu = this.contextMenu1;

			// The Text property sets the text that will be displayed,
			// in a tooltip, when the mouse hovers over the systray icon.
			_notifyIcon1.Text = "OMS Export Service";
			_notifyIcon1.Visible = true;

			// Handle the DoubleClick event to activate the form.
			_notifyIcon1.DoubleClick += new System.EventHandler(this._notifyIcon1_DoubleClick);
		}




		/// <summary>
		/// Connect to the service on the machine listed within the text box
		/// </summary>
		private void ConnectToService()
		{
			try
			{
				//hook onto the service
				_svcController = new ServiceController(_servicename);
				SetStatus();
				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			finally
			{
				if(_svcController !=null)
				{
					_svcController.Close();
					_svcController.Dispose();
					_svcController = null;
				}
			}
		}
		


		/// <summary>
		/// Creates an instance of the service controller against the FWBS.OMS.ExchangeService on the local machine 
		/// </summary>
		private void CreateServiceController()
		{
			try
			{
				//create the serrvice controller against the local machine
				_svcController = new ServiceController(_servicename);
			}
			catch
			{
				_svcController = null;
			}
			finally
			{}
	
		}
		
		
		/// <summary>
		/// method to start the service and capture failure so it can be called on seperate thread 
		/// </summary>
		private void StartService()
		{	
			try
			{
				_svcController.Start();
			}
			catch
			{}
		}

		
		/// <summary>
		/// Checks the status of the service
		/// </summary>
		private void _statusTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				CreateServiceController();
				SetStatus();
                Application.DoEvents();
			}
			catch{}
			finally
			{
				if(_svcController !=null)
				{
					_svcController.Close();
					_svcController.Dispose();
					_svcController = null;
				}
			}
		}

        /// <summary>
        /// Origional Method now overloaded to support initial form load and force read of stats
        /// </summary>
        private void SetLabels()
        {
            SetLabels(false);
        }
        
        /// <summary>
		/// Updates the totals reading from the text file
		/// </summary>
		private void SetLabels(bool force)
		{
			if(! _blnRunning && ! force)
				return;
						
			System.IO.StreamReader sr = null;
			
			try
			{

				LogCounter counter = FWBS.OMS.OMSEXPORT.StaticLibrary.GetCounter();
				
				lblErrors.Text = Convert.ToString(counter.Errors);
				lbClientAdd.Text = Convert.ToString(counter.ClientsAdded);
				lblClientUpd.Text = Convert.ToString(counter.ClientsUpdated);
				lblMatAdd.Text = Convert.ToString(counter.MattersAdded);
				lblMatUpd.Text = Convert.ToString(counter.MattersUpdated);
				lblTime.Text = Convert.ToString(counter.TimeAdded);
				lblFinancials.Text = Convert.ToString(counter.FinancialsAdded);
				lblLastError.Text = counter.LastError;
                lblLookup.Text = Convert.ToString(counter.LookupsAdded);
                lblUsers.Text = Convert.ToString(counter.UsersAdded);
                
			}
			catch
			{
				lblErrors.Text = "?";
				lbClientAdd.Text = "?";
				lblClientUpd.Text = "?";
				lblMatAdd.Text = "?";
				lblMatUpd.Text = "?";
				lblTime.Text = "?";
				lblFinancials.Text = "?";
				lblLastError.Text = "";
                lblLookup.Text = "?";
                lblUsers.Text = "?";

			}
			finally
			{
				try
				{
					if(sr!=null)
                        sr.Close();
				}
				catch{}

			}

		}
	
		#endregion

		private void animate_timer_Tick(object sender, System.EventArgs e)
		{
			AnimateIcon();
		}

		private void frmSvcController_Load(object sender, EventArgs e)
		{
			SetUpForm();
			SetLabels(true);
            _statusTimer.Interval = 5000;
		}

        private void menuLastError_Click(object sender, EventArgs e)
        {
            lblLastError.Visible = true;
            if (lblLastError.Text == "")
                lblLastError.Text = "No Error Recorded";

            lblLastError.Location = new Point(80, 3);
            lblLastError.Width = groupBox3.Width;
            lblLastError.Height = groupBox3.Height;
            lblLastError.BringToFront();
            menuLastError.Checked = true;
            menuStats.Checked = false;
            Application.DoEvents();
            
        }

        private void menuStats_Click(object sender, EventArgs e)
        {
            lblLastError.Visible = false;
            menuStats.Checked = true;
            menuLastError.Checked = false;
        }

        private void menuViewLog_Click(object sender, EventArgs e)
        {
            if (StaticLibrary.GetBoolSetting("LogToDB", "", false))
            {
                frmViewLog frm = new frmViewLog();
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("This service is not configured to log to the database", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


	}
}

