#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using ActiproSoftware.SyntaxEditor;
using ActiproSoftware.SyntaxEditor.Commands;
using ActiproSoftware.SyntaxEditor.Addons.Dynamic;
using ActiproSoftware.Win32;
using ActiproSoftware.WinUICore;
using ActiproSoftware.ComponentModel;
#endregion

namespace FWBS.Sharepoint
{
	internal partial class SaveDlg : Window
	{
		#region Constructor
		internal SaveDlg()
		{
			InitializeComponent();
		}
		#endregion

		#region Window_Loaded event
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.listBoxActivityType.Items.Add(typeof(System.Activities.Activity).ToString());
			this.listBoxActivityType.Items.Add(typeof(System.Activities.CodeActivity).ToString());
			this.listBoxActivityType.Items.Add(typeof(System.Activities.NativeActivity).ToString());
			// select the 1st item
			this.listBoxActivityType.SelectedIndex = 0;
		}
		#endregion

		#region Properties
		internal string ScriptCode { get; set; }
		internal string ActivityTypeName { get; set; }
		internal string NamespaceName { get; set; }
		internal string ClassName { get; set; }
		internal string ReturnTypeName { get; set; }
		#endregion

		#region Handle control events
		#region OK button clicked
		private void buttonOK_Click(object sender, RoutedEventArgs e)
		{
			// Validate controls anyway - should not be able to click OK button unless there is some text in the boxes...
			if (string.IsNullOrWhiteSpace(this.textBoxNamespaceName.Text) ||
				string.IsNullOrWhiteSpace(this.textBoxClassName.Text) ||
				string.IsNullOrWhiteSpace(this.textBoxScriptCode.Text) ||
				(this.listBoxActivityType.SelectedItem == null))
			{
				;	// do nothing
			}
			else
			{
				// TODO: Check script code does not already exist in the database
				this.ScriptCode = this.textBoxScriptCode.Text;
				this.NamespaceName = this.textBoxNamespaceName.Text;
				this.ClassName = this.textBoxClassName.Text;
				this.ReturnTypeName = this.textBoxReturnTypeName.Text.Trim();
				this.ActivityTypeName = this.listBoxActivityType.SelectedItem as string;

				// exit dialog
				this.DialogResult = true;
			}
		}
		#endregion

		#region namespace text changed
		private void textBoxNamespaceName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			// enable/disable OK button
			if ((this.textBoxNamespaceName.Text.Length > 0) && (this.textBoxNamespaceName.Text.Trim().Length == 0))
			{
				this.textBoxNamespaceName.Text = string.Empty;
			}
			this.buttonOK.IsEnabled = (this.textBoxNamespaceName.Text.Length > 0 && this.textBoxClassName.Text.Length > 0 && this.textBoxScriptCode.Text.Length > 0);
		}
		#endregion

		#region class name changed
		private void textBoxClassName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			// enable/disable OK button
			if ((this.textBoxClassName.Text.Length > 0) && (this.textBoxClassName.Text.Trim().Length == 0))
			{
				this.textBoxClassName.Text = string.Empty;
			}
			this.buttonOK.IsEnabled = (this.textBoxNamespaceName.Text.Length > 0 && this.textBoxClassName.Text.Length > 0 && this.textBoxScriptCode.Text.Length > 0);
		}
		#endregion

		#region script code changed
		private void textBoxScriptCode_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			// validate if the string has changed
			if (e.Changes.Count > 0)
			{
				string str;
				if (!this.ValidateString(this.textBoxScriptCode.Text, out str))
				{
					// assign new string if validation fails
					this.textBoxScriptCode.Text = str;
					// set caret position to end
					this.textBoxScriptCode.CaretIndex = str.Length;
				}
			}

			// enable/disable OK button
			this.buttonOK.IsEnabled = (this.textBoxNamespaceName.Text.Length > 0 && this.textBoxClassName.Text.Length > 0 && this.textBoxScriptCode.Text.Length > 0);
		}
		#endregion
		#endregion

		#region ValidateString
		// Strips invalid characters from the input string
		//	Retuns true if there was no stripping, false otherwise
		private bool ValidateString(string inStr, out string outStr)
		{
			bool retValue = true;

			StringBuilder sb = new StringBuilder(inStr.Length);
			for (int i = 0; i < inStr.Length; i++)
			{
				// only accept letter, digit and '_' characters
				if (char.IsLetterOrDigit(inStr[i]) || inStr[i] == '_')
				{
					sb.Append(inStr[i]);
				}
				else
				{
					// string contains invalid char, strip the char
					retValue = false;
				}
			}

			outStr = sb.ToString();

			return retValue;
		}
		#endregion
	}
}
