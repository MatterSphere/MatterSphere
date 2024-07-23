using System;

namespace FWBS.Common
{
	/// <summary>
	/// A common FWBS event arguments.
	/// </summary>
	public class FWBSEventArgs : EventArgs
	{
	}

	/// <summary>
	/// A delegate used to prompt a normal string, resource or exception to a calling client application.
	/// </summary>
	public delegate void MessageEventHandler(object sender, MessageEventArgs e);

	public class MessageEventArgs : FWBSEventArgs
	{
		private string _message;
		private Resources.ResourceItemAttribute _res;
		private Exception _exception;

		private MessageEventArgs(){}

		public MessageEventArgs (string message)
		{
			_message = message;
			_exception = null;
			_res = null;
		}

		public MessageEventArgs (Resources.ResourceItemAttribute res, string [] param)
		{
			_res = Resources.ResourceLookup.GetMessage(res, param);
			_exception = null;
			_message = _res.Text;
		}

		public MessageEventArgs (Exception exception)
		{
			_message = exception.Message;
			_exception = exception;
			_res = null;
		}

		public string Message
		{
			get
			{
				return _message;
			}
		}


	}

	
	/// <summary>
	/// A delegate used to prompt a progress bar to a calling application.
	/// </summary>
	public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

	public class ProgressEventArgs : System.ComponentModel.CancelEventArgs
	{
		private string _message = "";
		private int _current = 0;
		private int _total = 1;
		private bool _canCancel = true;

		public ProgressEventArgs(){}

		public ProgressEventArgs (int total) : base ()
		{
			_total = total;
		}


		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
			}
		}

		public int Current
		{
			get
			{
				return _current;
			}
			set
			{
				if (value < Total)
					_current = value;
			}
		}

		public bool CanCancel
		{
			get
			{
				return _canCancel;
			}
			set
			{
				_canCancel = value;
			}
		}

		public int Total
		{
			get
			{
				return _total;
			}
			set
			{
				_total = value;
			}
		}


		public void SetResource(Resources.ResourceItemAttribute res, params string [] param)
		{
			res = Resources.ResourceLookup.GetMessage(res, param);
			_message = res.Text;
		}

		public float ToPercentageCompleted()
		{
			return (float)((float)Current / (float)Total) * 100f;
		}
	}



	

}
