using System;
using System.Collections;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for Accelerators.
    /// </summary>
    public class Accelerators : System.ComponentModel.Component
	{
		#region Fields
		/// <summary>
		/// The Form to Process
		/// </summary>
		private System.Windows.Forms.Form _form;
		/// <summary>
		/// The Wait Timer
		/// </summary>
		private System.Windows.Forms.Timer _timer;
		/// <summary>
		/// The String of already collected Accelerators Keys
		/// </summary>
		private string accelerators = "";
        /// <summary>
        /// Required field asterisk should be skipped in accelerator
        /// </summary>
        private const string requiredAsterisk = "*";
		/// <summary>
		/// The Controls that need to be processed
		/// </summary>
		private ArrayList _toprocess;
		/// <summary>
		/// If the Process should be automatic
		/// </summary>
		private bool _active = true;
		#endregion

		#region Contructors
        public Accelerators(IContainer container) : this()
        {
            container.Add(this);
        }
		/// <summary>
		/// Contructor the Component
		/// </summary>
        [Obsolete("Please use Accelerators(IContainer container) contructor to avoid memory leaks")]
        public Accelerators()
		{
			_timer = new System.Windows.Forms.Timer();
			_timer.Enabled = false;
			_timer.Interval = 1000;
			_timer.Tick +=new EventHandler(Tick);
			_toprocess = new ArrayList();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets & Sets the Form to Process
		/// </summary>
		[Category("Settings")]		
		public System.Windows.Forms.Form Form
		{
			get
			{
				return _form;
			}
			set
			{
				_form = value;
				_form.Load +=new EventHandler(Load);
			}
		}

		/// <summary>
		/// Gets & Sets the Automatic State
		/// </summary>
		[DefaultValue(true)]
		[Category("Settings")]		
		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				_active = value;
			}
		}

		/// <summary>
		/// Gets or Set the Wait Interval before commencement
		/// </summary>
		[DefaultValue(1000)]
		[Category("Settings")]		
		public int WaitInterval
		{
			get
			{
				return _timer.Interval;
			}
			set
			{
				_timer.Interval = value;

			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Process the Form
		/// </summary>
		public void Execute()
		{
            ControlAcceleratorParser(_form);
		}

		/// <summary>
		/// Executes after a wait
		/// </summary>
		public void WaitExecute()
		{
			_timer.Enabled = true;
		}
		#endregion
		
		#region Private
		/// <summary>
		/// On Form Load Active the Wait Timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Load(object sender, EventArgs e)
		{
			if (_form.Visible)
				_timer.Enabled = _active;
		}

		/// <summary>
		/// Tick Tick Tick Boom
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Tick(object sender, EventArgs e)
		{
			_timer.Enabled =false;
			Execute();
		}

		/// <summary>
		/// Resets the Accelerators String
		/// </summary>
		private void ResetAccelerators()
		{
			accelerators = ": ";
		}

		/// <summary>
		/// Uses the Accelerator parser to set the Key Board Mnonics (Sorry can't spell it)
		/// </summary>
		/// <param name="frm">Form to be parsed.</param>
		private void ControlAcceleratorParser(System.Windows.Forms.Form frm)
		{
            if (frm == null) return;

			accelerators = ": ";
			if (Session.OMS != null)
			{
				foreach (System.Windows.Forms.Control ctrl in frm.Controls)
				{
					ControlAcceleratorParser(ctrl);
				}

				// Process any Visible Controls
				for (int ii = _toprocess.Count -1; ii > -1; ii--)
				{
					System.Windows.Forms.Control ctrl = _toprocess[ii] as System.Windows.Forms.Control;
					if (ctrl != null)
					{
						string[] words = ctrl.Text.Split(' ');
						bool flag = false;
						string newvalue = "";
						if (words.Length > 1)
						{
							for(int i = 0; i < words.Length; i++)
							{
								if (words[i] != "" && words[i] != requiredAsterisk 
                                    && accelerators.IndexOf(words[i].ToUpper().Substring(0,1)) == -1 
                                    && flag == false)
								{
									accelerators +=	words[i].ToUpper().Substring(0,1);
									words[i] = "&" + words[i];
									flag = true;
								}
								newvalue += words[i] + " ";
							}
						}
							
						if (!flag)
						{
							for(int i = 0; i < ctrl.Text.Length; i++)
							{
                                var letter = ctrl.Text.ToUpper().Substring(i, 1);
                                if (letter != requiredAsterisk && accelerators.IndexOf(letter) == -1)
								{
									accelerators += letter;
									ctrl.Text = ctrl.Text.Insert(i,"&");
									break;
								}
							}
						}
						else
						{
							ctrl.Text = newvalue.Trim();
						}
					}
				}
				_toprocess.Clear();
			}
		}

		/// <summary>
		/// Loops round all potential containers and parses the Accelerator of the child controls.
		/// </summary>
		/// <param name="ctrl">Control to be checked for child controls.</param>
		private void ControlAcceleratorParser(System.Windows.Forms.Control ctrl)
		{
            if (ctrl == null) return;

            try
			{
				if (Session.OMS != null)
				{
					if (ctrl.Text != "" && ctrl.Visible)
					{
						if (AcceptableControls(ctrl))
						{
							int pos = ctrl.Text.IndexOf("&");
							if (pos > -1)
							{
								accelerators +=	ctrl.Text.ToUpper().Substring(pos+1,1);
							}
							else
							{
								_toprocess.Add(ctrl);
							}
						}
					}
					if ((ctrl is FWBS.OMS.UI.Windows.eAddress || ctrl is FWBS.Common.UI.IBasicEnquiryControl2 == false) && ctrl.Visible)
					{
						foreach (System.Windows.Forms.Control child in ctrl.Controls)
						{
							ControlAcceleratorParser(child);
						}
					}
				}
			}
			catch
			{

			}
		}

		/// <summary>
		/// Checks the Type of the Control
		/// </summary>
		/// <param name="ctrl">The Control to Check</param>
		/// <returns>Returns true if acceptable</returns>
		private bool AcceptableControls(System.Windows.Forms.Control ctrl)
		{
            if (ctrl is FWBS.Common.UI.Windows.eLabel2)
				return false;
			else if (ctrl is FWBS.Common.UI.IBasicEnquiryControl2)
				return true;
			else if (ctrl is System.Windows.Forms.Button)
				return true;
			else return false;
		}
		#endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_form != null)
                {
                    if (_timer != null)
                    {
                        _timer.Enabled = false;
                        _timer.Dispose();
                    }


                    _form.Load -= new EventHandler(Load);
                    _form = null;
                }
            }
            base.Dispose(disposing);
        }

	}
}
