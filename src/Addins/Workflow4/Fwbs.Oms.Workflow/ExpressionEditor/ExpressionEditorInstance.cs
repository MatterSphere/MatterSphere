#region References
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
    public class ExpressionEditorInstance : IExpressionEditorInstance
	{
		#region Fields
		private ExpressionEditorInstanceControl control = null;
		private int minLines = 1;
		private int maxLines = 5;
		#endregion

		#region Constructors
		private ExpressionEditorInstance() { }

		public ExpressionEditorInstance(
			AssemblyContextControlItem assemblies,
			ImportedNamespaceContextItem importedNamespaces,
			List<ModelItem> variables,
			string text,
			Type expressionType,
			System.Windows.Size initialSize,
			ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom.DotNetProjectResolver dotNetProjectResolver
			)
		{
			this.control = new ExpressionEditorInstanceControl(assemblies, importedNamespaces, variables, text, dotNetProjectResolver);
			// set control size
			if (!initialSize.IsEmpty)
			{
				this.control.Width = initialSize.Width;
				this.control.Height = initialSize.Height;
			}

			this.control.Loaded += new RoutedEventHandler(control_Loaded);
		}
		#endregion

		#region Loaded event
		void control_Loaded(object sender, RoutedEventArgs e)
		{
			this.control.editor.GotFocus += new EventHandler(editor_GotFocus);
			this.control.editor.LostFocus += new EventHandler(editor_LostFocus);
			this.control.editor.TextChanged += new EventHandler(editor_TextChanged);
		}
		#endregion

		#region Properties
		#region AcceptsReturn
		public bool AcceptsReturn
		{
			get
			{
				return this.control.editor.Document.Multiline;
			}
			set
			{
				
				this.control.editor.Document.Multiline = value;
			}
		}
		#endregion

		#region AcceptsTab
		public bool AcceptsTab
		{
			get
			{
				return this.control.editor.AcceptsTab;
			}
			set
			{
				this.control.editor.AcceptsTab = value;
			}
		}
		#endregion

		#region HasAggregateFocus
		public bool HasAggregateFocus
		{
			get
			{
				return (this.control.editor != null) ? this.control.editor.ContainsFocus : false;
			}
		}
		#endregion

		#region HostControl
		public System.Windows.Controls.Control HostControl
		{
			get
			{
				return this.control;
			}
		}
		#endregion

		#region HorizontalScrollBarVisibility
		public System.Windows.Controls.ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return ScrollBarVisibility.Hidden;
			}

			set
			{
			}
		}
		#endregion

		#region VerticalScrollBarVisibility
		public System.Windows.Controls.ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return ScrollBarVisibility.Hidden;
			}

			set
			{
			}
		}
		#endregion

		#region MaxLines
		public int MaxLines
		{
			get
			{
				return this.maxLines;
			}

			set
			{
				this.maxLines = value;
				this.control.editor.Document.Multiline = (this.maxLines > 1);
			}
		}
		#endregion

		#region MinLines
		public int MinLines
		{
			get
			{
				return this.minLines;
			}
			set
			{
				this.minLines = value;
				if (this.minLines > 1)
				{
					this.control.editor.Document.Multiline = true;
				}
			}
		}
		#endregion

		#region Text
		public string Text
		{
			get
			{
				return (this.control.editor != null) ? this.control.editor.Document.Text : string.Empty;
			}

			set
			{
				this.control.editor.Document.Text = value;
			}
		}
		#endregion
		#endregion

		#region More IExpressionEditorInstance interface
		public event EventHandler Closing;
		public event EventHandler GotAggregateFocus;
		public event EventHandler LostAggregateFocus;
		public event EventHandler TextChanged;

		public bool CanCompleteWord()
		{
			return true;
		}
		public bool CanCopy()
		{
			return true;
		}
		public bool CanCut()
		{
			return true;
		}
		public bool CanDecreaseFilterLevel()
		{
			return false;
		}
		public bool CanGlobalIntellisense()
		{
			return true;
		}
		public bool CanIncreaseFilterLevel()
		{
			return false;
		}
		public bool CanParameterInfo()
		{
			return true;
		}
		public bool CanPaste()
		{
			return true;
		}
		public bool CanQuickInfo()
		{
			return true;
		}
		public bool CanRedo()
		{
			return true;
		}
		public bool CanUndo()
		{
			return true;
		}

		public void ClearSelection()
		{

		}
		public void Close()
		{
			if (this.control.editor != null)
			{
				this.control.Visibility = System.Windows.Visibility.Hidden;	// .editor.Visible = false;
			}
		}
		public bool CompleteWord()
		{
			return false;
		}
		public bool Copy()
		{
			this.control.editor.SelectedView.Selection.SelectAll();
			this.control.editor.SelectedView.CopyToClipboard();
			return true;
		}
		public bool Cut()
		{
			this.control.editor.SelectedView.Selection.SelectAll();
			this.control.editor.SelectedView.CutToClipboard();
			return true;
		}
		public bool DecreaseFilterLevel()
		{
			return false;
		}
		public void Focus()
		{
		}
		public string GetCommittedText()
		{
			return this.control.editor.Document.Text;
		}
		public bool GlobalIntellisense()
		{
			return true;
		}
		public bool IncreaseFilterLevel()
		{
			return false;
		}
		public bool ParameterInfo()
		{
			return false;
		}
		public bool Paste()
		{
			this.control.editor.SelectedView.PasteFromClipboard();
			return true;
		}
		public bool QuickInfo()
		{
			return false;
		}
		public bool Redo()
		{
			return false;
		}
		public bool Undo()
		{
			return false;
		}
		#endregion

		#region Editor events to pass on
		#region Handle Focus events
		void editor_GotFocus(object sender, EventArgs e)
		{
			if (this.GotAggregateFocus != null)
			{
				this.GotAggregateFocus.Invoke(this.control.windowsFormsHost1, e);
			}
		}

		void editor_LostFocus(object sender, EventArgs e)
		{
			if (this.LostAggregateFocus != null)
			{
				this.LostAggregateFocus.Invoke(this.control.windowsFormsHost1, e);
			}
		}
		#endregion

		#region handle text changed event
		void editor_TextChanged(object sender, EventArgs e)
		{
			if (this.TextChanged != null)
			{
				this.TextChanged.Invoke(this.control.windowsFormsHost1, e);
			}
		}
		#endregion
		#endregion
	}
}
