using System;
using System.Drawing;
using System.Windows.Forms;
using ActiproSoftware.ComponentModel;
using ActiproSoftware.SyntaxEditor;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Design
{

    /// <summary>
    /// Represents a form for selection a line to go to.
    /// </summary>
    public class StyleEditorForm : BaseForm
    {

        private SyntaxEditor editor;
        private HighlightingStyle[][] highlightingStyles;
        private bool ignoreUpdateRequest;
        private SyntaxLanguageCollection languages;
        private HighlightingStyle selectedHighlightingStyle;
        //
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Label backColorLabel;
        private System.Windows.Forms.Label foreColorLabel;
        private System.Windows.Forms.CheckBox boldCheckBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListBox styleListBox;
        private System.Windows.Forms.Label styleLabel;
        private System.Windows.Forms.ComboBox languageDropDownList;
        private System.Windows.Forms.CheckBox italicCheckBox;
        private ColorButton backColorButton;
        private ColorButton foreColorButton;
        private System.Windows.Forms.Label languagesLabel;
        private ActiproSoftware.SyntaxEditor.TextStylePreview samplePreview;
        private ComboBox defaultFontSizeComboBox;
        private Label fontFamilyLabel;
        private FontDropDownList defaultFontFaceComboBox;
        private Label fontSizeLabel;
        private Label sampleLabel;
        private Panel panel;
        private System.ComponentModel.IContainer components;

        public StyleEditorForm(SyntaxEditor editor)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.editor = editor;

            // Get the default font 
            defaultFontFaceComboBox.SelectedIndex = defaultFontFaceComboBox.FindStringExact(editor.Font.Name);
            defaultFontSizeComboBox.SelectedIndex = defaultFontSizeComboBox.FindStringExact(Math.Round(editor.Font.SizeInPoints).ToString());

            // Get a collection of all the language that are currently loaded
            languages = SyntaxLanguage.GetLanguageCollection(editor.Document.Language);

            // Create an array to hold highlighting styles
            highlightingStyles = new HighlightingStyle[languages.Count][];

            // Loop through each language
            for (int languageIndex = 0; languageIndex < languages.Count; languageIndex++)
            {
                // Load the language drop down
                SyntaxLanguage language = languages[languageIndex];
                languageDropDownList.Items.Add(language.Key);

                // Update the highlighting style array and create clones of each higlighting style
                highlightingStyles[languageIndex] = new HighlightingStyle[language.HighlightingStyles.Count];
                for (int highlightingStyleIndex = 0; highlightingStyleIndex < language.HighlightingStyles.Count; highlightingStyleIndex++)
                {
                    highlightingStyles[languageIndex][highlightingStyleIndex] = language.HighlightingStyles[highlightingStyleIndex].Clone();
                }
            }
            languageDropDownList.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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
            this.backColorButton = new FWBS.OMS.Design.ColorButton();
            this.foreColorButton = new FWBS.OMS.Design.ColorButton();
            this.backColorLabel = new System.Windows.Forms.Label();
            this.foreColorLabel = new System.Windows.Forms.Label();
            this.boldCheckBox = new System.Windows.Forms.CheckBox();
            this.italicCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.samplePreview = new ActiproSoftware.SyntaxEditor.TextStylePreview();
            this.styleListBox = new System.Windows.Forms.ListBox();
            this.styleLabel = new System.Windows.Forms.Label();
            this.languageDropDownList = new System.Windows.Forms.ComboBox();
            this.languagesLabel = new System.Windows.Forms.Label();
            this.defaultFontSizeComboBox = new System.Windows.Forms.ComboBox();
            this.fontFamilyLabel = new System.Windows.Forms.Label();
            this.defaultFontFaceComboBox = new ActiproSoftware.SyntaxEditor.FontDropDownList();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            this.sampleLabel = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // backColorButton
            // 
            this.backColorButton.BackColor = System.Drawing.SystemColors.Window;
            this.backColorButton.Color = System.Drawing.Color.Brown;
            this.backColorButton.Location = new System.Drawing.Point(188, 174);
            this.backColorButton.Name = "backColorButton";
            this.backColorButton.Size = new System.Drawing.Size(200, 22);
            this.backColorButton.TabIndex = 12;
            this.backColorButton.UseVisualStyleBackColor = false;
            this.backColorButton.ColorChanged += new System.EventHandler(this.backColorButton_ColorChanged);
            // 
            // foreColorButton
            // 
            this.foreColorButton.BackColor = System.Drawing.SystemColors.Window;
            this.foreColorButton.Color = System.Drawing.Color.IndianRed;
            this.foreColorButton.Location = new System.Drawing.Point(188, 119);
            this.foreColorButton.Name = "foreColorButton";
            this.foreColorButton.Size = new System.Drawing.Size(200, 22);
            this.foreColorButton.TabIndex = 10;
            this.foreColorButton.UseVisualStyleBackColor = false;
            this.foreColorButton.ColorChanged += new System.EventHandler(this.foreColorButton_ColorChanged);
            // 
            // backColorLabel
            // 
            this.backColorLabel.AutoSize = true;
            this.backColorLabel.Location = new System.Drawing.Point(186, 156);
            this.resourceLookup.SetLookup(this.backColorLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labItmBackColor", "Item background:", ""));
            this.backColorLabel.Name = "backColorLabel";
            this.backColorLabel.Size = new System.Drawing.Size(101, 15);
            this.backColorLabel.TabIndex = 11;
            this.backColorLabel.Text = "Item background:";
            // 
            // foreColorLabel
            // 
            this.foreColorLabel.AutoSize = true;
            this.foreColorLabel.Location = new System.Drawing.Point(186, 100);
            this.resourceLookup.SetLookup(this.foreColorLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labItmForeColor", "Item foreground:", ""));
            this.foreColorLabel.Name = "foreColorLabel";
            this.foreColorLabel.Size = new System.Drawing.Size(97, 15);
            this.foreColorLabel.TabIndex = 9;
            this.foreColorLabel.Text = "Item foreground:";
            // 
            // boldCheckBox
            // 
            this.boldCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.boldCheckBox.Location = new System.Drawing.Point(188, 211);
            this.resourceLookup.SetLookup(this.boldCheckBox, new FWBS.OMS.UI.Windows.ResourceLookupItem("BOLD", "Bold", ""));
            this.boldCheckBox.Name = "boldCheckBox";
            this.boldCheckBox.Size = new System.Drawing.Size(52, 28);
            this.boldCheckBox.TabIndex = 13;
            this.boldCheckBox.Text = "Bold";
            this.boldCheckBox.CheckedChanged += new System.EventHandler(this.boldCheckBox_CheckedChanged);
            // 
            // italicCheckBox
            // 
            this.italicCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.italicCheckBox.Location = new System.Drawing.Point(246, 211);
            this.resourceLookup.SetLookup(this.italicCheckBox, new FWBS.OMS.UI.Windows.ResourceLookupItem("ITALIC", "Italic", ""));
            this.italicCheckBox.Name = "italicCheckBox";
            this.italicCheckBox.Size = new System.Drawing.Size(54, 28);
            this.italicCheckBox.TabIndex = 14;
            this.italicCheckBox.Text = "Italic";
            this.italicCheckBox.CheckedChanged += new System.EventHandler(this.italicCheckBox_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(232, 341);
            this.resourceLookup.SetLookup(this.okButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 25);
            this.okButton.TabIndex = 17;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(313, 341);
            this.resourceLookup.SetLookup(this.cancelButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cance&l";
            // 
            // samplePreview
            // 
            this.samplePreview.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.samplePreview.Location = new System.Drawing.Point(188, 269);
            this.samplePreview.Name = "samplePreview";
            this.samplePreview.Size = new System.Drawing.Size(200, 64);
            this.samplePreview.TabIndex = 16;
            // 
            // styleListBox
            // 
            this.styleListBox.DisplayMember = "Name";
            this.styleListBox.IntegralHeight = false;
            this.styleListBox.ItemHeight = 15;
            this.styleListBox.Location = new System.Drawing.Point(9, 119);
            this.styleListBox.Name = "styleListBox";
            this.styleListBox.Size = new System.Drawing.Size(170, 214);
            this.styleListBox.TabIndex = 8;
            this.styleListBox.SelectedIndexChanged += new System.EventHandler(this.styleListBox_SelectedIndexChanged);
            // 
            // styleLabel
            // 
            this.styleLabel.AutoSize = true;
            this.styleLabel.Location = new System.Drawing.Point(6, 100);
            this.resourceLookup.SetLookup(this.styleLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labDisplayItems", "Display items:", ""));
            this.styleLabel.Name = "styleLabel";
            this.styleLabel.Size = new System.Drawing.Size(80, 15);
            this.styleLabel.TabIndex = 7;
            this.styleLabel.Text = "Display items:";
            // 
            // languageDropDownList
            // 
            this.languageDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageDropDownList.Location = new System.Drawing.Point(77, 12);
            this.languageDropDownList.Name = "languageDropDownList";
            this.languageDropDownList.Size = new System.Drawing.Size(311, 23);
            this.languageDropDownList.TabIndex = 2;
            this.languageDropDownList.SelectedIndexChanged += new System.EventHandler(this.languageDropDownList_SelectedIndexChanged);
            // 
            // languagesLabel
            // 
            this.languagesLabel.AutoSize = true;
            this.languagesLabel.Location = new System.Drawing.Point(6, 15);
            this.resourceLookup.SetLookup(this.languagesLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labLanguages", "Languages:", ""));
            this.languagesLabel.Name = "languagesLabel";
            this.languagesLabel.Size = new System.Drawing.Size(67, 15);
            this.languagesLabel.TabIndex = 1;
            this.languagesLabel.Text = "Languages:";
            // 
            // defaultFontSizeComboBox
            // 
            this.defaultFontSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultFontSizeComboBox.Items.AddRange(new object[] {
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26"});
            this.defaultFontSizeComboBox.Location = new System.Drawing.Point(298, 65);
            this.defaultFontSizeComboBox.Name = "defaultFontSizeComboBox";
            this.defaultFontSizeComboBox.Size = new System.Drawing.Size(90, 23);
            this.defaultFontSizeComboBox.TabIndex = 6;
            this.defaultFontSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.fontComboBox_SelectedIndexChanged);
            // 
            // fontFamilyLabel
            // 
            this.fontFamilyLabel.AutoSize = true;
            this.fontFamilyLabel.Location = new System.Drawing.Point(6, 46);
            this.resourceLookup.SetLookup(this.fontFamilyLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labFontFamily", "Font (bold type indicates fixed-width fonts):", ""));
            this.fontFamilyLabel.Name = "fontFamilyLabel";
            this.fontFamilyLabel.Size = new System.Drawing.Size(239, 15);
            this.fontFamilyLabel.TabIndex = 3;
            this.fontFamilyLabel.Text = "Font (bold type indicates fixed-width fonts):";
            // 
            // defaultFontFaceComboBox
            // 
            this.defaultFontFaceComboBox.Location = new System.Drawing.Point(9, 65);
            this.defaultFontFaceComboBox.Name = "defaultFontFaceComboBox";
            this.defaultFontFaceComboBox.Size = new System.Drawing.Size(278, 24);
            this.defaultFontFaceComboBox.TabIndex = 4;
            this.defaultFontFaceComboBox.SelectedIndexChanged += new System.EventHandler(this.fontComboBox_SelectedIndexChanged);
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.AutoSize = true;
            this.fontSizeLabel.Location = new System.Drawing.Point(297, 46);
            this.resourceLookup.SetLookup(this.fontSizeLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labSize", "Size:", ""));
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(30, 15);
            this.fontSizeLabel.TabIndex = 5;
            this.fontSizeLabel.Text = "Size:";
            // 
            // sampleLabel
            // 
            this.sampleLabel.AutoSize = true;
            this.sampleLabel.Location = new System.Drawing.Point(186, 250);
            this.resourceLookup.SetLookup(this.sampleLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("labSample", "Sample:", ""));
            this.sampleLabel.Name = "sampleLabel";
            this.sampleLabel.Size = new System.Drawing.Size(49, 15);
            this.sampleLabel.TabIndex = 15;
            this.sampleLabel.Text = "Sample:";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.languagesLabel);
            this.panel.Controls.Add(this.languageDropDownList);
            this.panel.Controls.Add(this.fontFamilyLabel);
            this.panel.Controls.Add(this.defaultFontFaceComboBox);
            this.panel.Controls.Add(this.fontSizeLabel);
            this.panel.Controls.Add(this.defaultFontSizeComboBox);
            this.panel.Controls.Add(this.styleLabel);
            this.panel.Controls.Add(this.styleListBox);
            this.panel.Controls.Add(this.foreColorLabel);
            this.panel.Controls.Add(this.foreColorButton);
            this.panel.Controls.Add(this.backColorLabel);
            this.panel.Controls.Add(this.backColorButton);
            this.panel.Controls.Add(this.boldCheckBox);
            this.panel.Controls.Add(this.italicCheckBox);
            this.panel.Controls.Add(this.sampleLabel);
            this.panel.Controls.Add(this.samplePreview);
            this.panel.Controls.Add(this.okButton);
            this.panel.Controls.Add(this.cancelButton);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(399, 376);
            this.panel.TabIndex = 0;
            // 
            // StyleEditorForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(399, 376);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("StyleEditorForm", "Highlighting Style Editor", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StyleEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Highlighting Style Editor";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // EVENT HANDLERS
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Occurs when the color of the <see cref="ColorButton"/> is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void backColorButton_ColorChanged(object sender, EventArgs e)
        {
            this.UpdateHighlightingStyle();
        }

        /// <summary>
        /// Occurs when the checked state of the <see cref="CheckBox"/> is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void boldCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            this.UpdateHighlightingStyle();
        }

        /// <summary>
        /// Occurs when the selected index is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void fontComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((defaultFontFaceComboBox.SelectedItem != null) && (defaultFontSizeComboBox.SelectedItem != null))
                samplePreview.Font = new Font(((FontDropDownList.FontFamilyData)defaultFontFaceComboBox.SelectedItem).Name, Convert.ToSingle(defaultFontSizeComboBox.SelectedItem));
        }

        /// <summary>
        /// Occurs when the color of the <see cref="ColorButton"/> is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void foreColorButton_ColorChanged(object sender, EventArgs e)
        {
            this.UpdateHighlightingStyle();
        }

        /// <summary>
        /// Occurs when the checked state of the <see cref="CheckBox"/> is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void italicCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            this.UpdateHighlightingStyle();
        }

        /// <summary>
        /// Occurs when the checked state of the <see cref="CheckBox"/> is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void underlineCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            this.UpdateHighlightingStyle();
        }

        /// <summary>
        /// Occurs when the selected index is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void languageDropDownList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.LoadLanguage(languageDropDownList.SelectedIndex);
        }

        /// <summary>
        /// Occurs when the selected index is changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void okButton_Click(object sender, System.EventArgs e)
        {
            // Mark for batch update
            editor.Document.Language.IsUpdating = true;

            // Loop through each language and update the highlighting styles
            for (int languageIndex = 0; languageIndex < languages.Count; languageIndex++)
            {
                foreach (HighlightingStyle highlightingStyle in highlightingStyles[languageIndex])
                {
                    languages[languageIndex].HighlightingStyles.Add(highlightingStyle);
                }
            }

            // Mark batch update complete
            editor.Document.Language.IsUpdating = false;

            // Update the font
            editor.Font = samplePreview.Font;
        }

        /// <summary>
        /// Occurs when the button is pressed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void styleListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.UpdateHighlightingStyle();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // NON-PUBLIC PROCEDURES
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Loads the highlighting styles from the specified language into the editor.
        /// </summary>
        /// <param name="index">The index of the language to load.</param>
        private void LoadLanguage(int index)
        {
            selectedHighlightingStyle = null;

            styleListBox.Items.Clear();
            foreach (HighlightingStyle highlightingStyle in highlightingStyles[index])
            {
                styleListBox.Items.Add(highlightingStyle);
            }

            styleListBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the selected style with the current settings and updates the controls.
        /// </summary>
        private void UpdateHighlightingStyle()
        {
            if (ignoreUpdateRequest)
                return;

            // Update old selected style
            if (selectedHighlightingStyle != null)
            {
                selectedHighlightingStyle.ForeColor = foreColorButton.Color;
                selectedHighlightingStyle.BackColor = backColorButton.Color;
                selectedHighlightingStyle.Bold = (boldCheckBox.Checked ? DefaultableBoolean.True : DefaultableBoolean.Default);
                selectedHighlightingStyle.Italic = (italicCheckBox.Checked ? DefaultableBoolean.True : DefaultableBoolean.Default);
            }

            if (styleListBox.SelectedIndex == -1)
                return;

            ignoreUpdateRequest = true;

            // Update controls
            selectedHighlightingStyle = (HighlightingStyle)styleListBox.SelectedItem;
            foreColorButton.Color = selectedHighlightingStyle.ForeColor;
            backColorButton.Color = selectedHighlightingStyle.BackColor;
            boldCheckBox.Checked = (selectedHighlightingStyle.Bold == DefaultableBoolean.True);
            italicCheckBox.Checked = (selectedHighlightingStyle.Italic == DefaultableBoolean.True);

            // Update sample label
            samplePreview.HighlightingStyle = selectedHighlightingStyle;

            ignoreUpdateRequest = false;
        }

    }
}
