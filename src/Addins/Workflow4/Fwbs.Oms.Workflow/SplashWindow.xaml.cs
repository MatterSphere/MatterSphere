#region References
using System;
using System.Windows;
#endregion

namespace FWBS.OMS.Workflow
{
	internal partial class SplashWindow : Window
	{
		#region Constructor
		internal SplashWindow()
		{
			InitializeComponent();
		}

		internal SplashWindow(string text)
			: this()
		{
			this.textBlockMessage.Text = text;
		}
		#endregion

		#region Properties
		internal string Text
		{
			set { this.textBlockMessage.Text = value;  }
		}
		#endregion
	}
}
