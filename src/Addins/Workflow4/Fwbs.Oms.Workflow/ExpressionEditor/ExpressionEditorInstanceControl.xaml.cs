#region References
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using ActiproSoftware.SyntaxEditor;
#endregion

namespace FWBS.OMS.Workflow
{
    /// <summary>
    /// Interaction logic for ExpressionEditorInstanceControl.xaml
    /// </summary>
    public partial class ExpressionEditorInstanceControl : UserControl
	{
		#region Fields
		public ActiproSoftware.SyntaxEditor.SyntaxEditor editor = null;
		#endregion

		#region Constructors
		public ExpressionEditorInstanceControl()
		{
		}

		public ExpressionEditorInstanceControl(
			AssemblyContextControlItem assemblies,
			ImportedNamespaceContextItem importedNamespaces,
			List<ModelItem> variables,
			string text,
			ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.DotNetProjectResolver dotNetProjectResolver
			)
		{
			InitializeComponent();
			System.Windows.Forms.Integration.WindowsFormsHost.EnableWindowsFormsInterop();

			// Start the parser service (only call this once at the start of your application)
			if (!SemanticParserService.IsRunning)
			{
				SemanticParserService.Start();
			}

			// setup the panel - need the panel for some events to work properly!
			System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
			panel.Dock = System.Windows.Forms.DockStyle.Fill;
			panel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
			panel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			panel.Location = new System.Drawing.Point(0, 0);
			this.windowsFormsHost1.Child = panel;

			// add the editor to panel
			this.editor = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
			panel.Controls.Add(this.editor);

			// setup editor properties
			this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editor.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.editor.Location = new System.Drawing.Point(0, 0);
			this.editor.Name = "editor";
			this.editor.Size = new System.Drawing.Size(50, 50);
			this.editor.TabIndex = 0;
			this.editor.ScrollBarType = ScrollBarType.None;			// no scroll bars displayed
			this.editor.SplitType = SyntaxEditorSplitType.None;		// no splitting of windows
			this.editor.AcceptsTab = false;							// does not accept TAB char
			this.editor.HideSelection = true;
			this.editor.IndicatorMarginVisible = false;				// hide left hand side bits
			this.editor.LineNumberMarginVisible = false;
			this.editor.UserMarginVisible = false;
			this.editor.SelectionMarginWidth = 0;
			this.editor.AllowDrop = false;							// no drap and drop
			this.editor.BracketHighlightingVisible = true;

			this.editor.IntelliPrompt.DropShadowEnabled = true;

			// setup editor document
			ActiproSoftware.SyntaxEditor.Document document = new ActiproSoftware.SyntaxEditor.Document();
			this.editor.Document = document;
			this.editor.Document.Filename = DateTime.Now.Ticks.ToString() + ".vb";
			this.editor.Document.SemanticParsingEnabled = true;
			this.editor.Document.LexicalParsingEnabled = true;
			this.editor.Document.AutoCaseCorrectEnabled = true;
			this.editor.Document.LineModificationMarkingEnabled = false;
			this.editor.Document.Multiline = true;
			this.editor.Document.Outlining.Mode = OutliningMode.None;
			this.editor.Document.Language = new ActiproSoftware.SyntaxEditor.Addons.VB.VBSyntaxLanguage();
			this.editor.Document.LanguageData = dotNetProjectResolver;

			//
			// Set the header and footer text to get proper intellisense in the 'Text' - handle both C# and VB for future proof
			//
			// set header and footer
			StringBuilder headerText = new StringBuilder();
			StringBuilder footerText = new StringBuilder();
			if (this.editor.Document.Language.Key == "C#")
			{
				#region C#
				// add using stmts
				foreach (string nm in importedNamespaces.ImportedNamespaces)
				{
					headerText.AppendLine("using " + nm + ";");
				}
				headerText.AppendLine("namespace myNamespace {");
				headerText.AppendLine("public class MySub {");
				foreach (ModelItem mi in variables)
				{
					ModelProperty mp = mi.Properties.Find("Name");
					if (mp != null)
					{
						ModelProperty mpType = mi.Properties.Find("Type");

						if (mpType != null)
						{
							headerText.AppendLine(mpType.ComputedValue + " " + mp.ComputedValue + ";");
						}
					}
				}
				headerText.AppendLine("public void MySub() {");
				footerText.Append("} } }");
				#endregion
			}
			else
			{
				#region VB.NET
				// add using stmts
				foreach (string nm in importedNamespaces.ImportedNamespaces)
				{
					headerText.AppendLine("Imports " + nm);
				}
				headerText.AppendLine("Namespace MyNamespace ");
				headerText.AppendLine("Public Class [MyClass]");
				foreach (ModelItem mi in variables)
				{
					ModelProperty mp = mi.Properties.Find("Name");
					if (mp != null)
					{
						ModelProperty mpType = mi.Properties.Find("Type");

						if (mpType != null)
						{
							headerText.AppendLine("Dim " + mp.ComputedValue + " as " + mpType.ComputedValue);
						}
					}
				}
				headerText.AppendLine("Public Sub MySub() ");
				footerText.AppendLine("End Sub");
				footerText.AppendLine("End Class");
				footerText.AppendLine("End Namespace");
				#endregion
			}
			this.editor.Document.HeaderText = headerText.ToString();
			this.editor.Document.FooterText = footerText.ToString();
			// set text
			if (!string.IsNullOrWhiteSpace(text))
			{
				this.editor.Document.Text = text;
			}

			// handle loaded event to set various other things like font...
			this.Loaded += new System.Windows.RoutedEventHandler(ExpressionEditorInstanceControl_Loaded);
		}
		#endregion

		#region Loaded event
		void ExpressionEditorInstanceControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			// Set the Font - convert from WPF to WinForms
			string fontFamily = this.FontFamily.Source;
			float fontSize = (float)(this.FontSize * 72.0 / 96.0);
			System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
			if (this.FontStyle == FontStyles.Italic || this.FontStyle == FontStyles.Oblique)
			{
				fontStyle = System.Drawing.FontStyle.Italic;
			}
			this.editor.Font = new System.Drawing.Font(fontFamily, fontSize, fontStyle);

			// NOTE: You must set the focus to both the windowsformhost and the editor,
			//	otherwise you will get weird behaviour with the mouse and goodness knows what else!
			this.windowsFormsHost1.Focus();
			this.editor.Focus();
			this.editor.Caret.Offset = this.editor.Document.Length;	// set caret to end of line
		}
		#endregion
	}
}
