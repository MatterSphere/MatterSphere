namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    partial class ucCalendarPickerPopup
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            btnFirstItem.ItemClicked -= OnItemClicked;
            btnSecondItem.ItemClicked -= OnItemClicked;
            btnThirdItem.ItemClicked -= OnItemClicked;
            btnFourthItem.ItemClicked -= OnItemClicked;
            btnFifthItem.ItemClicked -= OnItemClicked;
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpContainer = new System.Windows.Forms.TableLayoutPanel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnFifthItem = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPickerPopupItem();
            this.btnFourthItem = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPickerPopupItem();
            this.btnThirdItem = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPickerPopupItem();
            this.btnSecondItem = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPickerPopupItem();
            this.btnFirstItem = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.ucCalendarPickerPopupItem();
            this.tlpContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpContainer
            // 
            this.tlpContainer.ColumnCount = 1;
            this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContainer.Controls.Add(this.btnDown, 0, 6);
            this.tlpContainer.Controls.Add(this.btnFifthItem, 0, 5);
            this.tlpContainer.Controls.Add(this.btnFourthItem, 0, 4);
            this.tlpContainer.Controls.Add(this.btnThirdItem, 0, 3);
            this.tlpContainer.Controls.Add(this.btnSecondItem, 0, 2);
            this.tlpContainer.Controls.Add(this.btnFirstItem, 0, 1);
            this.tlpContainer.Controls.Add(this.btnUp, 0, 0);
            this.tlpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContainer.Location = new System.Drawing.Point(0, 0);
            this.tlpContainer.Name = "tlpContainer";
            this.tlpContainer.RowCount = 7;
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpContainer.Size = new System.Drawing.Size(150, 155);
            this.tlpContainer.TabIndex = 0;
            // 
            // btnDown
            // 
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDown.FlatAppearance.BorderSize = 0;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.btnDown.Location = new System.Drawing.Point(0, 140);
            this.btnDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(150, 15);
            this.btnDown.TabIndex = 6;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.btnUp.Location = new System.Drawing.Point(0, 0);
            this.btnUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(150, 15);
            this.btnUp.TabIndex = 0;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnFifthItem
            // 
            this.btnFifthItem.BackColor = System.Drawing.Color.White;
            this.btnFifthItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFifthItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFifthItem.Location = new System.Drawing.Point(0, 115);
            this.btnFifthItem.Margin = new System.Windows.Forms.Padding(0);
            this.btnFifthItem.Name = "btnFifthItem";
            this.btnFifthItem.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnFifthItem.Size = new System.Drawing.Size(150, 25);
            this.btnFifthItem.TabIndex = 5;
            this.btnFifthItem.Title = "Title";
            // 
            // btnFourthItem
            // 
            this.btnFourthItem.BackColor = System.Drawing.Color.White;
            this.btnFourthItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFourthItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFourthItem.Location = new System.Drawing.Point(0, 90);
            this.btnFourthItem.Margin = new System.Windows.Forms.Padding(0);
            this.btnFourthItem.Name = "btnFourthItem";
            this.btnFourthItem.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnFourthItem.Size = new System.Drawing.Size(150, 25);
            this.btnFourthItem.TabIndex = 4;
            this.btnFourthItem.Title = "Title";
            // 
            // btnThirdItem
            // 
            this.btnThirdItem.BackColor = System.Drawing.Color.White;
            this.btnThirdItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnThirdItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnThirdItem.Location = new System.Drawing.Point(0, 65);
            this.btnThirdItem.Margin = new System.Windows.Forms.Padding(0);
            this.btnThirdItem.Name = "btnThirdItem";
            this.btnThirdItem.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnThirdItem.Size = new System.Drawing.Size(150, 25);
            this.btnThirdItem.TabIndex = 3;
            this.btnThirdItem.Title = "Title";
            // 
            // btnSecondItem
            // 
            this.btnSecondItem.BackColor = System.Drawing.Color.White;
            this.btnSecondItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSecondItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSecondItem.Location = new System.Drawing.Point(0, 40);
            this.btnSecondItem.Margin = new System.Windows.Forms.Padding(0);
            this.btnSecondItem.Name = "btnSecondItem";
            this.btnSecondItem.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnSecondItem.Size = new System.Drawing.Size(150, 25);
            this.btnSecondItem.TabIndex = 2;
            this.btnSecondItem.Title = "Title";
            // 
            // btnFirstItem
            // 
            this.btnFirstItem.BackColor = System.Drawing.Color.White;
            this.btnFirstItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirstItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFirstItem.Location = new System.Drawing.Point(0, 15);
            this.btnFirstItem.Margin = new System.Windows.Forms.Padding(0);
            this.btnFirstItem.Name = "btnFirstItem";
            this.btnFirstItem.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnFirstItem.Size = new System.Drawing.Size(150, 25);
            this.btnFirstItem.TabIndex = 1;
            this.btnFirstItem.Title = "Title";
            // 
            // ucCalendarPickerPopup
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.MinimumSize = new System.Drawing.Size(150, 155);
            this.Name = "ucCalendarPickerPopup";
            this.Size = new System.Drawing.Size(150, 155);
            this.tlpContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpContainer;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        protected ucCalendarPickerPopupItem btnFifthItem;
        protected ucCalendarPickerPopupItem btnFourthItem;
        protected ucCalendarPickerPopupItem btnThirdItem;
        protected ucCalendarPickerPopupItem btnSecondItem;
        protected ucCalendarPickerPopupItem btnFirstItem;
    }
}
