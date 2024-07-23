using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;

namespace FWBS.OMS.Design
{

    /// <summary>
    /// Represents a form for a selection a line to go to.
    /// </summary>
    internal class GoToLineForm : FWBS.OMS.UI.Windows.BaseForm
    {

		private SyntaxEditor	syntaxEditor;

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label lineNumberLabel;
		private System.Windows.Forms.TextBox lineNumberTextBox;
        private System.ComponentModel.IContainer components;

		public GoToLineForm(SyntaxEditor syntaxEditor) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Add any constructor code after InitializeComponent call
			//
			this.syntaxEditor = syntaxEditor;
			// Update the line textbox and select the text
			lineNumberLabel.Text = Session.CurrentSession.Resources.GetResource("GOTOLINENUMBER", "Line number  (1 - %1%):", "", false, syntaxEditor.Document.Lines.Count.ToString()).Text;
			lineNumberTextBox.Text = (syntaxEditor.Document.Lines.IndexOf(syntaxEditor.Caret.Offset) + 1).ToString();
			lineNumberTextBox.SelectAll();
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) 
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.lineNumberLabel = new System.Windows.Forms.Label();
            this.lineNumberTextBox = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(146, 60);
            this.resourceLookup.SetLookup(this.cancelButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cance&l";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(65, 60);
            this.resourceLookup.SetLookup(this.okButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 25);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // lineNumberLabel
            // 
            this.lineNumberLabel.AutoSize = true;
            this.lineNumberLabel.Location = new System.Drawing.Point(11, 9);
            this.lineNumberLabel.Name = "lineNumberLabel";
            this.lineNumberLabel.Size = new System.Drawing.Size(117, 15);
            this.lineNumberLabel.TabIndex = 2;
            this.lineNumberLabel.Text = "Line number  (1 - %1%):";
            // 
            // lineNumberTextBox
            // 
            this.lineNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lineNumberTextBox.Location = new System.Drawing.Point(11, 27);
            this.lineNumberTextBox.Name = "lineNumberTextBox";
            this.lineNumberTextBox.Size = new System.Drawing.Size(210, 23);
            this.lineNumberTextBox.TabIndex = 0;
            this.lineNumberTextBox.Text = "1";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.lineNumberLabel);
            this.panel.Controls.Add(this.lineNumberTextBox);
            this.panel.Controls.Add(this.okButton);
            this.panel.Controls.Add(this.cancelButton);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(8);
            this.panel.Size = new System.Drawing.Size(232, 96);
            this.panel.TabIndex = 3;
            // 
            // GoToLineForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(232, 96);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("GoToLineForm", "Go To Line", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GoToLineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Go To Line";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Occurs when the button is clicked.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void cancelButton_Click(object sender, System.EventArgs e) {
			// Close the form
			this.Close();
		}

		/// <summary>
		/// Occurs when the button is clicked.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		private void okButton_Click(object sender, System.EventArgs e) {
			// Get the document line index
			int documentLineIndex;
			try {
				documentLineIndex = int.Parse(lineNumberTextBox.Text.Trim());
			}
			catch {
				MessageBox.Show(this, "Invalid Number" , Application.ProductName);
				return;
			}

			// Valid line number check
			if ((documentLineIndex < 1) || (documentLineIndex > syntaxEditor.Document.Lines.Count)) {
                MessageBox.Show(this, "Invalid Number", Application.ProductName);
				return;
			}

			// Move to the specified position
			syntaxEditor.SelectedView.GoToLine(documentLineIndex - 1);

			// Close the form
			this.Close();
		}

	}
}
