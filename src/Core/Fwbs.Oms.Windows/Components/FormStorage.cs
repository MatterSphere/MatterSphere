using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;

namespace FWBS.OMS.UI.Windows
{
    using Screen = System.Windows.Forms.Screen;

    /// <summary>
    /// Summary description for ucFormStorage.
    /// </summary>
    public enum SaveLocation {svXML,svRegistry,svINI}
	public class ucFormStorage : System.ComponentModel.Component
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Form _form;
		public bool Override = false;
		private long _version = 0;
		private int _defpcgwidth = 0;
		private int _defpcgheight = 0;

		[Category("Design")]
        [DefaultValue(null)]
		public Form FormToStore
		{
			get
			{
				return _form;
			}
			set
			{
				if (_form != null)
				{
					_form.Load -= new System.EventHandler(this.Form_Load);
                    _form.FormClosed -= new FormClosedEventHandler(this.Form_Closed);
                }
				_form = value;
				if (_form != null)
				{
					_form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                    _form.Load -= new System.EventHandler(this.Form_Load);
                    _form.FormClosed -= new FormClosedEventHandler(this.Form_Closed);
                    _form.Load += new System.EventHandler(this.Form_Load);
                    _form.FormClosed += new FormClosedEventHandler(this.Form_Closed);
				}
			}
		}

		private SaveLocation _svloc = SaveLocation.svXML;
		[Browsable(false)]
		[Category("Storage")]
		[DefaultValue(SaveLocation.svXML)]
		public SaveLocation StoreWhere
		{
			get
			{
				return _svloc;
			}
			set
			{
				_svloc = value;
			}
		}

		[Category("Options")]
		[DefaultValue(0)]
		public long Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}
		
		private bool _position = true;
		[Category("Options")]
		[DefaultValue(true)]
		public bool Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		private bool _size = true;
		[Category("Options")]
		[DefaultValue(true)]
		public bool Size
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
			}
		}

		private bool _state = true;
		[Category("Options")]
		[DefaultValue(true)]
		public bool State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

		private bool _activecontrol = false;
		[Browsable(false)]
		[Category("Options")]
		[DefaultValue(false)]
		public bool ActiveControl
		{
			get
			{
				return _activecontrol;
			}
			set
			{
				_activecontrol = value;
			}
		}

		private System.Windows.Forms.FormStartPosition _startpos = System.Windows.Forms.FormStartPosition.CenterScreen;
		[Category("Options")]
		public System.Windows.Forms.FormStartPosition StartPosition
		{
			get
			{
				return _startpos;
			}
			set
			{
				_startpos = value;
				if (_form != null)
					_form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			}
		}

		[Category("Design")]
		private string _UniqueID = "";
		public string UniqueID
		{
			get
			{
				return _UniqueID;
			}
			set
			{
				_UniqueID = value;
			}
		}

		[Category("Defaults")]
		[DefaultValue(0)]
		[Description("0 = Not Set")]
		public int DefaultPercentageWidth
		{
			get
			{
				return _defpcgwidth;
			}
			set
			{
				_defpcgwidth = value;
			}
		}
		
		[Category("Defaults")]
		[Description("0 = Not Set")]
		[DefaultValue(0)]
		public int DefaultPercentageHeight
		{
			get
			{
				return _defpcgheight;	
			}
			set
			{
				_defpcgheight = value;
			}
		}

		private string GetSetting(string Group, string Value)
		{
			string uid = _UniqueID;
			if (!uid.StartsWith(@"\") && uid != "") uid = @"\" + uid;
			return Convert.ToString(RegistryAccess.GetSetting(Microsoft.Win32.RegistryHive.CurrentUser,@"\SOFTWARE\FWBS\OMS\2.0" + uid + @"\" + Group,Value));
		}

		private void SetSetting(string Group, string Key, string Value)
		{
			string uid = _UniqueID;
			if (!uid.StartsWith(@"\") && uid != "") uid = @"\" + uid;
            RegistryAccess.SetSetting(Microsoft.Win32.RegistryHive.CurrentUser,@"\SOFTWARE\FWBS\OMS\2.0" + uid + @"\" + Group,Key,Value);
		}
		
		private void LoadFromXML()
		{
			_form.Visible=false;
			string screens = "";
			for (int t=0; t < Screen.AllScreens.Length; t++)
			{
				screens = screens + Screen.AllScreens[t].WorkingArea.ToString();
			}
			try
			{
				if (GetSetting("FS","Screens") == screens && GetSetting("FS","Version") == _version.ToString())
				{
					if (_position)
					{
						string _top = GetSetting("FS" + GetSetting("FS","State"),"Top");
						string _left = GetSetting("FS" + GetSetting("FS","State"),"Left");
						SetFormPosition(_left,_top);
					}
					else
					{
						string _top = GetSetting("FS" ,"ScreenY");
						string _left = GetSetting("FS" ,"ScreenX");
						SetFormPosition(_left,_top);
					}
					if (_state)
					{
						try{_form.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState),GetSetting("FS","State"),true);}
						catch{}

						if (_position && _form.WindowState == FormWindowState.Maximized)
						{
							string _top = GetSetting("FS" + "Normal","Top");
							string _left = GetSetting("FS" + "Normal","Left");
							SetFormPosition(_left,_top);
							if ( _size)
							{
								string _width = GetSetting("FS" + "Normal","Width");
								string _height = GetSetting("FS" + "Normal","Height");
								if (_width != null) _form.Width = Convert.ToInt32(_width);
								if (_height != null) _form.Height = Convert.ToInt32(_height);
							}
						}
						else
						{
							if (_position && _form.StartPosition == FormStartPosition.CenterParent)
							{
								Screen sc = Screen.FromHandle(_form.Handle);
								int x = sc.WorkingArea.Width - _form.Width / 2;
								int y = sc.WorkingArea.Height - _form.Height / 2;
								_form.Location = new Point(x,y);
							}
						}

					}
					if ( _size && _form.WindowState != FormWindowState.Maximized)
					{
						string _width = GetSetting("FS" + _form.WindowState.ToString(),"Width");
						string _height = GetSetting("FS" + _form.WindowState.ToString(),"Height");
						if (_width != null) _form.Width = Convert.ToInt32(_width);
						if (_height != null) _form.Height = Convert.ToInt32(_height);
					}

					if (!_position && (_startpos == System.Windows.Forms.FormStartPosition.CenterScreen || _startpos == System.Windows.Forms.FormStartPosition.CenterParent))
					{
						Screen sc = Screen.FromHandle(_form.Handle);
						int x = (sc.WorkingArea.Width - _form.Width) / 2;
						int y = (sc.WorkingArea.Height - _form.Height) / 2;
						_form.Location = new Point(x + sc.WorkingArea.X,y + sc.WorkingArea.Y); 
					}

				}
				else
				{
					if ( _size && _form.WindowState != FormWindowState.Maximized)
					{
						if (_defpcgheight > 0 && _defpcgwidth > 0)
						{
							_form.Width = (Screen.FromHandle(_form.Handle).WorkingArea.Width * _defpcgwidth) / 100;
							_form.Height = (Screen.FromHandle(_form.Handle).WorkingArea.Height * _defpcgheight) / 100;
						}
					}
					SetFormPosition(null,null);
				}
			}
			catch
			{}
			_form.Visible=true;
		}

		private void SetFormPosition(string _left, string _top)
		{
			Rectangle workingArea = Screen.FromHandle(_form.Handle).WorkingArea;
			if (_top != null || _left != null)
			{
				if (_top != null) _form.Top = System.Math.Max(Convert.ToInt32(_top), 0);
				if (_left != null) _form.Left = System.Math.Max(Convert.ToInt32(_left), 0);
				// move the form if the right bottom corner is outside of the working area
				if (_form.Top + _form.Height > workingArea.Height)
					_form.Top = workingArea.Height - _form.Height;
				if (_form.Left + _form.Width > workingArea.Width)
					_form.Left = workingArea.Width - _form.Width;
			}
			else
			{
				if (_startpos == FormStartPosition.CenterScreen || _startpos == FormStartPosition.CenterParent)
				{
					int x = (workingArea.Width - _form.Width) / 2;
					int y = (workingArea.Height - _form.Height) / 2;
					_form.Location = new Point(x + workingArea.X, y + workingArea.Y);
				}
			}
		}

		public void SaveNow()
		{
			SaveToXML();
		}

		public void LoadNow()
		{
			LoadFromXML();
		}


		private void SaveToXML()
		{
			try
			{
                if (_form != null && _form.WindowState != FormWindowState.Minimized)
				{
					SetSetting("FS" + _form.WindowState.ToString(),"Width",_form.Width.ToString());
					SetSetting("FS" + _form.WindowState.ToString(),"Height",_form.Height.ToString());
					SetSetting("FS" + _form.WindowState.ToString(),"Top",_form.Top.ToString());
					SetSetting("FS" + _form.WindowState.ToString(),"Left",_form.Left.ToString());
					string screens = "";
					for (int t=0; t < Screen.AllScreens.Length; t++)
					{
						screens = screens + Screen.AllScreens[t].WorkingArea.ToString();
					}
					SetSetting("FS","Screens",screens);
					SetSetting("FS","Version",_version.ToString());
					Screen sc = Screen.FromHandle(_form.Handle);
					SetSetting("FS","ScreenX",sc.WorkingArea.X.ToString());
					SetSetting("FS","ScreenY",sc.WorkingArea.Y.ToString());
					SetSetting("FS","State",_form.WindowState.ToString());
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Error Saving the Form please use the manual Save command (" + ex.Message + ")");
			}
		}


        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            if (_svloc == SaveLocation.svXML)
                SaveToXML();
        }

		private void Form_Load(object sender, System.EventArgs e)
		{
			if ((_svloc == SaveLocation.svXML) || Override)
				LoadFromXML();
			Override = false;
		}

        public ucFormStorage(IContainer container)
        {
            container.Add(this);
        }
		
        [Obsolete("Please use ucFormStorage(IContainer container) contructor to avoid memory leaks")]
        public ucFormStorage()
		{
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _form = null;
            }
            base.Dispose(disposing);
        }

	}
}
