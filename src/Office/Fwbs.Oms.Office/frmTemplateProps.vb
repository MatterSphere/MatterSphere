Imports Word = Microsoft.Office.Interop.Word

Friend Class frmTemplateProps
    Inherits BaseForm

#Region "Fields"

    Dim _app As WordOMS2
    Friend WithEvents lblPageTrayOption As System.Windows.Forms.Label
    Dim _doc As Word.Document

#End Region

#Region " Windows Form Designer generated code "

    Private Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub New(ByVal app As WordOMS2, ByVal doc As Word.Document)
        Me.New()

        _app = app
        _doc = doc
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents gbPrintOptions As System.Windows.Forms.GroupBox
    Friend WithEvents chkAutoPrintTemp As System.Windows.Forms.CheckBox
    Friend WithEvents chkDuplexed As System.Windows.Forms.CheckBox
    Friend WithEvents txtcoloured As System.Windows.Forms.TextBox
    Friend WithEvents txtbillpaper As System.Windows.Forms.TextBox
    Friend WithEvents txtEngrossment As System.Windows.Forms.TextBox
    Friend WithEvents txtCopies As System.Windows.Forms.TextBox
    Friend WithEvents txtLetterhead As System.Windows.Forms.TextBox
    Friend WithEvents lblNoCopies As System.Windows.Forms.Label
    Friend WithEvents lblColoured As System.Windows.Forms.Label
    Friend WithEvents lblBillPaper As System.Windows.Forms.Label
    Friend WithEvents lblEngrossment As System.Windows.Forms.Label
    Friend WithEvents lblCopies As System.Windows.Forms.Label
    Friend WithEvents lblLetterhead As System.Windows.Forms.Label
    Friend WithEvents gbSelPrinter As System.Windows.Forms.GroupBox
    Friend WithEvents cbPrinter As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents res As FWBS.OMS.UI.Windows.ResourceLookup
    Friend WithEvents cboOverrideTrays As System.Windows.Forms.ComboBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTemplateProps))
        Me.gbPrintOptions = New System.Windows.Forms.GroupBox()
        Me.lblPageTrayOption = New System.Windows.Forms.Label()
        Me.cboOverrideTrays = New System.Windows.Forms.ComboBox()
        Me.chkDuplexed = New System.Windows.Forms.CheckBox()
        Me.txtcoloured = New System.Windows.Forms.TextBox()
        Me.txtbillpaper = New System.Windows.Forms.TextBox()
        Me.txtEngrossment = New System.Windows.Forms.TextBox()
        Me.txtCopies = New System.Windows.Forms.TextBox()
        Me.txtLetterhead = New System.Windows.Forms.TextBox()
        Me.lblNoCopies = New System.Windows.Forms.Label()
        Me.lblColoured = New System.Windows.Forms.Label()
        Me.lblBillPaper = New System.Windows.Forms.Label()
        Me.lblEngrossment = New System.Windows.Forms.Label()
        Me.lblCopies = New System.Windows.Forms.Label()
        Me.lblLetterhead = New System.Windows.Forms.Label()
        Me.gbSelPrinter = New System.Windows.Forms.GroupBox()
        Me.cbPrinter = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkAutoPrintTemp = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.res = New FWBS.OMS.UI.Windows.ResourceLookup(Me.components)
        Me.gbPrintOptions.SuspendLayout
        Me.gbSelPrinter.SuspendLayout
        Me.Panel1.SuspendLayout
        Me.Panel2.SuspendLayout
        Me.SuspendLayout
        '
        'gbPrintOptions
        '
        Me.gbPrintOptions.Controls.Add(Me.lblPageTrayOption)
        Me.gbPrintOptions.Controls.Add(Me.cboOverrideTrays)
        Me.gbPrintOptions.Controls.Add(Me.chkDuplexed)
        Me.gbPrintOptions.Controls.Add(Me.txtcoloured)
        Me.gbPrintOptions.Controls.Add(Me.txtbillpaper)
        Me.gbPrintOptions.Controls.Add(Me.txtEngrossment)
        Me.gbPrintOptions.Controls.Add(Me.txtCopies)
        Me.gbPrintOptions.Controls.Add(Me.txtLetterhead)
        Me.gbPrintOptions.Controls.Add(Me.lblNoCopies)
        Me.gbPrintOptions.Controls.Add(Me.lblColoured)
        Me.gbPrintOptions.Controls.Add(Me.lblBillPaper)
        Me.gbPrintOptions.Controls.Add(Me.lblEngrossment)
        Me.gbPrintOptions.Controls.Add(Me.lblCopies)
        Me.gbPrintOptions.Controls.Add(Me.lblLetterhead)
        Me.gbPrintOptions.Controls.Add(Me.gbSelPrinter)
        Me.gbPrintOptions.Controls.Add(Me.Panel1)
        Me.gbPrintOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbPrintOptions.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.gbPrintOptions.Location = New System.Drawing.Point(5, 5)
        Me.res.SetLookup(Me.gbPrintOptions, New FWBS.OMS.UI.Windows.ResourceLookupItem("PRINTOPTIONS", "Print Options", ""))
        Me.gbPrintOptions.Name = "gbPrintOptions"
        Me.gbPrintOptions.Size = New System.Drawing.Size(259, 384)
        Me.gbPrintOptions.TabIndex = 0
        Me.gbPrintOptions.TabStop = false
        Me.gbPrintOptions.Text = "Print Options"
        '
        'lblPageTrayOption
        '
        Me.lblPageTrayOption.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblPageTrayOption.Location = New System.Drawing.Point(12, 300)
        Me.res.SetLookup(Me.lblPageTrayOption, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblPageTOpt", "Page Tray Option", ""))
        Me.lblPageTrayOption.Name = "lblPageTrayOption"
        Me.lblPageTrayOption.Size = New System.Drawing.Size(137, 27)
        Me.lblPageTrayOption.TabIndex = 11
        Me.lblPageTrayOption.Text = "Page Tray Option"
        Me.lblPageTrayOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboOverrideTrays
        '
        Me.cboOverrideTrays.DisplayMember = "Value"
        Me.cboOverrideTrays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOverrideTrays.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboOverrideTrays.Location = New System.Drawing.Point(169, 302)
        Me.cboOverrideTrays.Name = "cboOverrideTrays"
        Me.cboOverrideTrays.Size = New System.Drawing.Size(72, 23)
        Me.cboOverrideTrays.TabIndex = 7
        Me.cboOverrideTrays.ValueMember = "Key"
        '
        'chkDuplexed
        '
        Me.chkDuplexed.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkDuplexed.Location = New System.Drawing.Point(15, 330)
        Me.res.SetLookup(Me.chkDuplexed, New FWBS.OMS.UI.Windows.ResourceLookupItem("DUPLEXED", "&Duplexed", ""))
        Me.chkDuplexed.Name = "chkDuplexed"
        Me.chkDuplexed.Size = New System.Drawing.Size(116, 28)
        Me.chkDuplexed.TabIndex = 8
        Me.chkDuplexed.Text = "&Duplexed"
        '
        'txtcoloured
        '
        Me.txtcoloured.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtcoloured.Location = New System.Drawing.Point(169, 272)
        Me.txtcoloured.MaxLength = 4
        Me.txtcoloured.Name = "txtcoloured"
        Me.txtcoloured.Size = New System.Drawing.Size(72, 23)
        Me.txtcoloured.TabIndex = 6
        Me.txtcoloured.Text = "0"
        Me.txtcoloured.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtbillpaper
        '
        Me.txtbillpaper.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtbillpaper.Location = New System.Drawing.Point(169, 242)
        Me.txtbillpaper.MaxLength = 4
        Me.txtbillpaper.Name = "txtbillpaper"
        Me.txtbillpaper.Size = New System.Drawing.Size(72, 23)
        Me.txtbillpaper.TabIndex = 5
        Me.txtbillpaper.Text = "0"
        Me.txtbillpaper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEngrossment
        '
        Me.txtEngrossment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtEngrossment.Location = New System.Drawing.Point(169, 212)
        Me.txtEngrossment.MaxLength = 4
        Me.txtEngrossment.Name = "txtEngrossment"
        Me.txtEngrossment.Size = New System.Drawing.Size(72, 23)
        Me.txtEngrossment.TabIndex = 4
        Me.txtEngrossment.Text = "0"
        Me.txtEngrossment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCopies
        '
        Me.txtCopies.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtCopies.Location = New System.Drawing.Point(169, 182)
        Me.txtCopies.MaxLength = 4
        Me.txtCopies.Name = "txtCopies"
        Me.txtCopies.Size = New System.Drawing.Size(72, 23)
        Me.txtCopies.TabIndex = 3
        Me.txtCopies.Text = "0"
        Me.txtCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLetterhead
        '
        Me.txtLetterhead.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtLetterhead.Location = New System.Drawing.Point(169, 152)
        Me.txtLetterhead.MaxLength = 4
        Me.txtLetterhead.Name = "txtLetterhead"
        Me.txtLetterhead.Size = New System.Drawing.Size(72, 23)
        Me.txtLetterhead.TabIndex = 2
        Me.txtLetterhead.Text = "0"
        Me.txtLetterhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNoCopies
        '
        Me.lblNoCopies.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblNoCopies.Location = New System.Drawing.Point(8, 127)
        Me.res.SetLookup(Me.lblNoCopies, New FWBS.OMS.UI.Windows.ResourceLookupItem("NOCOPIESON", "Number of Copies On", ""))
        Me.lblNoCopies.Name = "lblNoCopies"
        Me.lblNoCopies.Size = New System.Drawing.Size(244, 26)
        Me.lblNoCopies.TabIndex = 0
        Me.lblNoCopies.Text = "Number of Copies On"
        '
        'lblColoured
        '
        Me.lblColoured.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblColoured.Location = New System.Drawing.Point(12, 270)
        Me.res.SetLookup(Me.lblColoured, New FWBS.OMS.UI.Windows.ResourceLookupItem("COLOURED", "Coloured", ""))
        Me.lblColoured.Name = "lblColoured"
        Me.lblColoured.Size = New System.Drawing.Size(137, 27)
        Me.lblColoured.TabIndex = 10
        Me.lblColoured.Text = "Coloured"
        Me.lblColoured.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBillPaper
        '
        Me.lblBillPaper.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblBillPaper.Location = New System.Drawing.Point(12, 240)
        Me.res.SetLookup(Me.lblBillPaper, New FWBS.OMS.UI.Windows.ResourceLookupItem("BILLPAPER", "Bill Paper", ""))
        Me.lblBillPaper.Name = "lblBillPaper"
        Me.lblBillPaper.Size = New System.Drawing.Size(137, 27)
        Me.lblBillPaper.TabIndex = 8
        Me.lblBillPaper.Text = "Bill Paper"
        Me.lblBillPaper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEngrossment
        '
        Me.lblEngrossment.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblEngrossment.Location = New System.Drawing.Point(12, 210)
        Me.res.SetLookup(Me.lblEngrossment, New FWBS.OMS.UI.Windows.ResourceLookupItem("ENGROSSMENT", "Engrossment", ""))
        Me.lblEngrossment.Name = "lblEngrossment"
        Me.lblEngrossment.Size = New System.Drawing.Size(137, 27)
        Me.lblEngrossment.TabIndex = 6
        Me.lblEngrossment.Text = "Engrossment"
        Me.lblEngrossment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCopies
        '
        Me.lblCopies.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCopies.Location = New System.Drawing.Point(12, 180)
        Me.res.SetLookup(Me.lblCopies, New FWBS.OMS.UI.Windows.ResourceLookupItem("COPIES", "Copies", ""))
        Me.lblCopies.Name = "lblCopies"
        Me.lblCopies.Size = New System.Drawing.Size(137, 27)
        Me.lblCopies.TabIndex = 4
        Me.lblCopies.Text = "Copies"
        Me.lblCopies.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLetterhead
        '
        Me.lblLetterhead.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblLetterhead.Location = New System.Drawing.Point(12, 150)
        Me.res.SetLookup(Me.lblLetterhead, New FWBS.OMS.UI.Windows.ResourceLookupItem("LETTERHEAD", "Letterhead", ""))
        Me.lblLetterhead.Name = "lblLetterhead"
        Me.lblLetterhead.Size = New System.Drawing.Size(137, 27)
        Me.lblLetterhead.TabIndex = 2
        Me.lblLetterhead.Text = "Letterhead"
        Me.lblLetterhead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbSelPrinter
        '
        Me.gbSelPrinter.Controls.Add(Me.cbPrinter)
        Me.gbSelPrinter.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbSelPrinter.Location = New System.Drawing.Point(3, 65)
        Me.res.SetLookup(Me.gbSelPrinter, New FWBS.OMS.UI.Windows.ResourceLookupItem("SELECTPRINTER", "Select Printer", ""))
        Me.gbSelPrinter.Name = "gbSelPrinter"
        Me.gbSelPrinter.Padding = New System.Windows.Forms.Padding(9, 3, 9, 3)
        Me.gbSelPrinter.Size = New System.Drawing.Size(253, 51)
        Me.gbSelPrinter.TabIndex = 1
        Me.gbSelPrinter.TabStop = false
        Me.gbSelPrinter.Text = "Select Printer"
        '
        'cbPrinter
        '
        Me.cbPrinter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrinter.Location = New System.Drawing.Point(9, 19)
        Me.cbPrinter.Name = "cbPrinter"
        Me.cbPrinter.Size = New System.Drawing.Size(235, 23)
        Me.cbPrinter.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkAutoPrintTemp)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 19)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Padding = New System.Windows.Forms.Padding(9, 3, 9, 3)
        Me.Panel1.Size = New System.Drawing.Size(253, 46)
        Me.Panel1.TabIndex = 0
        '
        'chkAutoPrintTemp
        '
        Me.chkAutoPrintTemp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkAutoPrintTemp.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkAutoPrintTemp.Location = New System.Drawing.Point(9, 3)
        Me.res.SetLookup(Me.chkAutoPrintTemp, New FWBS.OMS.UI.Windows.ResourceLookupItem("AUTOPRINTTEMP", "Auto Print this Template", ""))
        Me.chkAutoPrintTemp.Name = "chkAutoPrintTemp"
        Me.chkAutoPrintTemp.Size = New System.Drawing.Size(235, 40)
        Me.chkAutoPrintTemp.TabIndex = 0
        Me.chkAutoPrintTemp.Text = "Auto Print this Template"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnCancel)
        Me.Panel2.Controls.Add(Me.btnSave)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Panel2.Location = New System.Drawing.Point(264, 5)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(87, 384)
        Me.Panel2.TabIndex = 8
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(6, 37)
        Me.res.SetLookup(Me.btnCancel, New FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""))
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cance&l"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(6, 7)
        Me.res.SetLookup(Me.btnSave, New FWBS.OMS.UI.Windows.ResourceLookupItem("SAVE", "&Save", ""))
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 24)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "&Save"
        '
        'frmTemplateProps
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96!, 96!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(356, 394)
        Me.Controls.Add(Me.gbPrintOptions)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.res.SetLookup(Me, New FWBS.OMS.UI.Windows.ResourceLookupItem("TEMPLATEPROPS", "Template Properties", ""))
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "frmTemplateProps"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Template Properties"
        Me.gbPrintOptions.ResumeLayout(false)
        Me.gbPrintOptions.PerformLayout
        Me.gbSelPrinter.ResumeLayout(false)
        Me.Panel1.ResumeLayout(false)
        Me.Panel2.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub

#End Region

    Private Sub ListTrayOptions()
        Me.cboOverrideTrays.Items.Clear()
        Dim dt As DataTable = New DataTable
        dt.Columns.Add("Key")
        dt.Columns.Add("Value")
        dt.Rows.Add("ORIGINAL", Session.CurrentSession.Resources.GetResource("ORIGINAL", "Original", "").Text)
        dt.Rows.Add("NEW", Session.CurrentSession.Resources.GetResource("NEW", "New", "").Text)
        dt.Rows.Add("OVERRIDE", Session.CurrentSession.Resources.GetResource("OVERRIDE", "Override", "").Text)
        dt.AcceptChanges()
        cboOverrideTrays.DataSource = dt
        cboOverrideTrays.SelectedValue = "ORIGINAL"
    End Sub

    Private Sub ListPrinters()
        'Get the printer list from the database.
        On Error Resume Next
        cbPrinter.BeginUpdate()
        cbPrinter.DataSource = FWBS.OMS.Printer.GetPrinterList(True)
        cbPrinter.DisplayMember = "printName"
        cbPrinter.ValueMember = "printID"
        cbPrinter.EndUpdate()
    End Sub

    Private Sub SaveProperties()
        _app.SetDocProperty(_doc, "PRINTERNAME", Convert.ToString(cbPrinter.SelectedValue))
        _app.SetDocProperty(_doc, "PRINTITNOW", chkAutoPrintTemp.Checked)
        _app.SetDocProperty(_doc, "NOLETTERHEAD", Common.ConvertDef.ToInt16(txtLetterhead.Text, 0))
        _app.SetDocProperty(_doc, "NOCOPIES", Common.ConvertDef.ToInt16(txtCopies.Text, 0))
        _app.SetDocProperty(_doc, "NOENGROSSMENT", Common.ConvertDef.ToInt16(txtEngrossment.Text, 0))
        _app.SetDocProperty(_doc, "NOBILLPAPER", Common.ConvertDef.ToInt16(txtbillpaper.Text, 0))
        _app.SetDocProperty(_doc, "NOCOLOURED", Common.ConvertDef.ToInt16(txtcoloured.Text, 0))
        _app.SetDocProperty(_doc, "DUPLEX", chkDuplexed.Checked)
        _app.SetDocProperty(_doc, "PAGETRAYOPTION", Convert.ToString(cboOverrideTrays.SelectedValue))
    End Sub

    Private Sub GetProperties()

        'Get the default template print information that was saved with
        'the precedent / template.
        Dim printname As String = Convert.ToString(_app.GetDocProperty(_doc, "PRINTERNAME", ""))
        Try
            cbPrinter.SelectedValue = Convert.ToInt32(printname)
        Catch ex As Exception
            cbPrinter.Text = printname
        End Try

        chkAutoPrintTemp.Checked = Common.ConvertDef.ToBoolean(_app.GetDocProperty(_doc, "PRINTNOW", False), False)
        If (chkAutoPrintTemp.Checked = False) Then chkAutoPrintTemp.Checked = Common.ConvertDef.ToBoolean(_app.GetDocProperty(_doc, "PRINTITNOW", False), False)

        txtLetterhead.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOLETTERHEAD", 0))
        If (txtLetterhead.Text = "0") Then txtLetterhead.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOOFLETTERHEAD", 0))
        If (txtLetterhead.Text = "0") Then txtLetterhead.Text = Convert.ToString(_app.GetDocProperty(_doc, "TRAYONE", 0))

        txtCopies.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOCOPIES", 0))
        If (txtCopies.Text = "0") Then txtCopies.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOOFCOPIES", 0))
        If (txtCopies.Text = "0") Then txtCopies.Text = Convert.ToString(_app.GetDocProperty(_doc, "TRAYTWO", 0))

        txtEngrossment.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOENGROSSMENT", 0))
        If (txtEngrossment.Text = "0") Then txtEngrossment.Text = Convert.ToString(_app.GetDocProperty(_doc, "TRAYTHREE", 0))

        txtbillpaper.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOBILLPAPER", 0))
        If (txtbillpaper.Text = "0") Then txtbillpaper.Text = Convert.ToString(_app.GetDocProperty(_doc, "TRAYFOUR", 0))

        txtcoloured.Text = Convert.ToString(_app.GetDocProperty(_doc, "NOCOLOURED", 0))
        If (txtcoloured.Text = "0") Then txtcoloured.Text = Convert.ToString(_app.GetDocProperty(_doc, "TRAYFIVE", 0))

        chkDuplexed.Checked = Common.ConvertDef.ToBoolean(_app.GetDocProperty(_doc, "DUPLEX", False), False)
        cboOverrideTrays.SelectedValue = _app.GetDocProperty(_doc, "PAGETRAYOPTION", "ORIGINAL")

    End Sub

    Private Sub frmTemplateProps_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ListPrinters()
            ListTrayOptions()
            GetProperties()
        Catch ex As Exception
            ErrorBox.Show(Me, ex)
        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            SaveProperties()
        Catch ex As Exception
            ErrorBox.Show(Me, ex)
        End Try
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class
