Imports Word = Microsoft.Office.Interop.Word


Friend Class frmPrint
    Inherits BaseForm

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
    Private Const WM_CLOSE As System.Int32 = &H10

#Region "Fields"

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblCurrentPrinter As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlSelPrinter As System.Windows.Forms.Panel
    Friend WithEvents lblCurPrinter As System.Windows.Forms.Label
    Friend WithEvents gbSelPrinter As System.Windows.Forms.GroupBox
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnFax As System.Windows.Forms.Button
    Friend WithEvents btnFaxOptions As System.Windows.Forms.Button
    Friend WithEvents btnPrintFAX As System.Windows.Forms.Button
    Friend WithEvents btnPPreview As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gbPrintOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblNoCopies As System.Windows.Forms.Label
    Friend WithEvents txtLetterhead As System.Windows.Forms.TextBox
    Friend WithEvents lblLetterhead As System.Windows.Forms.Label
    Friend WithEvents lblCopies As System.Windows.Forms.Label
    Friend WithEvents txtCopies As System.Windows.Forms.TextBox
    Friend WithEvents lblEngrossment As System.Windows.Forms.Label
    Friend WithEvents txtEngrossment As System.Windows.Forms.TextBox
    Friend WithEvents lblBillPaper As System.Windows.Forms.Label
    Friend WithEvents txtbillpaper As System.Windows.Forms.TextBox
    Friend WithEvents lblColoured As System.Windows.Forms.Label
    Friend WithEvents txtcoloured As System.Windows.Forms.TextBox
    Friend WithEvents chkDuplexed As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnvelope As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents cbPrinter As System.Windows.Forms.ComboBox
    Friend WithEvents lblPages As System.Windows.Forms.Label
    Friend WithEvents txtNoOfPages As System.Windows.Forms.TextBox
    Friend WithEvents pnlNotice As System.Windows.Forms.Panel
    Friend WithEvents lblnotice As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents picPrint As System.Windows.Forms.PictureBox

    Private _doc As Word.Document
    Private _app As Word.Application
    Private _appcontroller As FWBS.OMS.UI.Windows.OMSApp
    Private _firsttimeran As Boolean = False
    Private _oldprint As String
    Friend WithEvents ResourceLookup1 As FWBS.OMS.UI.Windows.ResourceLookup
    Friend WithEvents cboOverrideTrays As System.Windows.Forms.ComboBox
    Friend WithEvents lblPageTrayOption As System.Windows.Forms.Label
    Friend WithEvents btnEmailPDF As System.Windows.Forms.Button
    Private _printsettings As FWBS.OMS.PrecPrintMode = PrecPrintMode.Dialog
    Private _printdialog As FWBS.OMS.UI.Windows.PrintSettings
    Private bulkprint As Boolean = False

#End Region

#Region "Constructors"

    Private Sub New()
    End Sub

    Public Sub New(ByVal obj As Object, ByRef appcontroller As FWBS.OMS.UI.Windows.OMSApp, ByVal printSettings As PrecPrintMode)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        FWBS.OMS.UI.Windows.[Global].RightToLeftFormConverter(Me)

        _appcontroller = appcontroller
        _doc = obj
        _app = _doc.Application
        _printsettings = printSettings
    End Sub

    Public Sub New(ByVal obj As Object, ByRef appcontroller As FWBS.OMS.UI.Windows.OMSApp, ByVal printdialogSettings As PrintSettings)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        FWBS.OMS.UI.Windows.[Global].RightToLeftFormConverter(Me)
        _appcontroller = appcontroller
        _doc = obj
        _app = _doc.Application
        _printsettings = printdialogSettings.Mode
        _printdialog = printdialogSettings
        bulkprint = printdialogSettings.BulkPrintMode
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

#End Region

#Region " Windows Form Designer generated code "
    Friend WithEvents chkTrackChanges As System.Windows.Forms.CheckBox
    Friend WithEvents btnSignDocument As System.Windows.Forms.Button
    Friend WithEvents btnAuth As System.Windows.Forms.Button
    Friend WithEvents btnEmail As System.Windows.Forms.Button



    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrint))
        Me.lblCurrentPrinter = New System.Windows.Forms.Label()
        Me.gbSelPrinter = New System.Windows.Forms.GroupBox()
        Me.cbPrinter = New System.Windows.Forms.ComboBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnFax = New System.Windows.Forms.Button()
        Me.btnFaxOptions = New System.Windows.Forms.Button()
        Me.btnPrintFAX = New System.Windows.Forms.Button()
        Me.btnPPreview = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.gbPrintOptions = New System.Windows.Forms.GroupBox()
        Me.lblPageTrayOption = New System.Windows.Forms.Label()
        Me.cboOverrideTrays = New System.Windows.Forms.ComboBox()
        Me.txtNoOfPages = New System.Windows.Forms.TextBox()
        Me.lblPages = New System.Windows.Forms.Label()
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
        Me.chkTrackChanges = New System.Windows.Forms.CheckBox()
        Me.chkEnvelope = New System.Windows.Forms.CheckBox()
        Me.chkDuplexed = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblCurPrinter = New System.Windows.Forms.Label()
        Me.pnlSelPrinter = New System.Windows.Forms.Panel()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnEmailPDF = New System.Windows.Forms.Button()
        Me.btnAuth = New System.Windows.Forms.Button()
        Me.btnEmail = New System.Windows.Forms.Button()
        Me.btnSignDocument = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlNotice = New System.Windows.Forms.Panel()
        Me.lblnotice = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.picPrint = New System.Windows.Forms.PictureBox()
        Me.ResourceLookup1 = New FWBS.OMS.UI.Windows.ResourceLookup(Me.components)
        Me.gbSelPrinter.SuspendLayout()
        Me.gbPrintOptions.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlSelPrinter.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlNotice.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.picPrint, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCurrentPrinter
        '
        Me.lblCurrentPrinter.Location = New System.Drawing.Point(2, 2)
        Me.ResourceLookup1.SetLookup(Me.lblCurrentPrinter, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblCurrPrinter", "Currently Selected Printer", ""))
        Me.lblCurrentPrinter.Name = "lblCurrentPrinter"
        Me.lblCurrentPrinter.Size = New System.Drawing.Size(189, 23)
        Me.lblCurrentPrinter.TabIndex = 2
        Me.lblCurrentPrinter.Text = "Currently Selected  Printer"
        '
        'gbSelPrinter
        '
        Me.gbSelPrinter.Controls.Add(Me.cbPrinter)
        Me.gbSelPrinter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSelPrinter.Location = New System.Drawing.Point(3, 3)
        Me.ResourceLookup1.SetLookup(Me.gbSelPrinter, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblSelPrinter", "Select Printer", ""))
        Me.gbSelPrinter.Name = "gbSelPrinter"
        Me.gbSelPrinter.Size = New System.Drawing.Size(267, 52)
        Me.gbSelPrinter.TabIndex = 0
        Me.gbSelPrinter.TabStop = False
        '
        'cbPrinter
        '
        Me.cbPrinter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrinter.Location = New System.Drawing.Point(3, 19)
        Me.cbPrinter.Name = "cbPrinter"
        Me.cbPrinter.Size = New System.Drawing.Size(261, 23)
        Me.cbPrinter.TabIndex = 0
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPrint.Location = New System.Drawing.Point(5, 9)
        Me.ResourceLookup1.SetLookup(Me.btnPrint, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnPrint", "&Print", ""))
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(139, 27)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        '
        'btnFax
        '
        Me.btnFax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFax.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFax.Location = New System.Drawing.Point(5, 112)
        Me.ResourceLookup1.SetLookup(Me.btnFax, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnFax", "Just &Fax", ""))
        Me.btnFax.Name = "btnFax"
        Me.btnFax.Size = New System.Drawing.Size(139, 27)
        Me.btnFax.TabIndex = 3
        Me.btnFax.Text = "Just &Fax"
        '
        'btnFaxOptions
        '
        Me.btnFaxOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFaxOptions.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFaxOptions.Location = New System.Drawing.Point(5, 148)
        Me.ResourceLookup1.SetLookup(Me.btnFaxOptions, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnFaxOptions", "&Fax Options", ""))
        Me.btnFaxOptions.Name = "btnFaxOptions"
        Me.btnFaxOptions.Size = New System.Drawing.Size(139, 27)
        Me.btnFaxOptions.TabIndex = 4
        Me.btnFaxOptions.Text = "&Fax Options"
        '
        'btnPrintFAX
        '
        Me.btnPrintFAX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrintFAX.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPrintFAX.Location = New System.Drawing.Point(5, 77)
        Me.ResourceLookup1.SetLookup(Me.btnPrintFAX, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnPrintFAX", "Print && Fa&x", ""))
        Me.btnPrintFAX.Name = "btnPrintFAX"
        Me.btnPrintFAX.Size = New System.Drawing.Size(139, 27)
        Me.btnPrintFAX.TabIndex = 2
        Me.btnPrintFAX.Text = "Print && Fa&x"
        '
        'btnPPreview
        '
        Me.btnPPreview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPPreview.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPPreview.Location = New System.Drawing.Point(5, 43)
        Me.ResourceLookup1.SetLookup(Me.btnPPreview, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnPPreview", "Print Preview", ""))
        Me.btnPPreview.Name = "btnPPreview"
        Me.btnPPreview.Size = New System.Drawing.Size(139, 27)
        Me.btnPPreview.TabIndex = 1
        Me.btnPPreview.Text = "Print Preview"
        Me.btnPPreview.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnCancel.Location = New System.Drawing.Point(5, 349)
        Me.ResourceLookup1.SetLookup(Me.btnCancel, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""))
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(139, 27)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cance&l"
        '
        'gbPrintOptions
        '
        Me.gbPrintOptions.Controls.Add(Me.lblPageTrayOption)
        Me.gbPrintOptions.Controls.Add(Me.cboOverrideTrays)
        Me.gbPrintOptions.Controls.Add(Me.txtNoOfPages)
        Me.gbPrintOptions.Controls.Add(Me.lblPages)
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
        Me.gbPrintOptions.Controls.Add(Me.chkTrackChanges)
        Me.gbPrintOptions.Controls.Add(Me.chkEnvelope)
        Me.gbPrintOptions.Controls.Add(Me.chkDuplexed)
        Me.gbPrintOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbPrintOptions.Location = New System.Drawing.Point(3, 3)
        Me.ResourceLookup1.SetLookup(Me.gbPrintOptions, New FWBS.OMS.UI.Windows.ResourceLookupItem("gbPrintOptions", "Print Options", ""))
        Me.gbPrintOptions.Name = "gbPrintOptions"
        Me.gbPrintOptions.Size = New System.Drawing.Size(267, 325)
        Me.gbPrintOptions.TabIndex = 0
        Me.gbPrintOptions.TabStop = False
        Me.gbPrintOptions.Text = "Print Options"
        '
        'lblPageTrayOption
        '
        Me.lblPageTrayOption.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblPageTrayOption.Location = New System.Drawing.Point(8, 227)
        Me.ResourceLookup1.SetLookup(Me.lblPageTrayOption, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblPageTOpt", "Page Tray Option", ""))
        Me.lblPageTrayOption.Name = "lblPageTrayOption"
        Me.lblPageTrayOption.Size = New System.Drawing.Size(155, 27)
        Me.lblPageTrayOption.TabIndex = 12
        Me.lblPageTrayOption.Text = "Page Tray Option"
        Me.lblPageTrayOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboOverrideTrays
        '
        Me.cboOverrideTrays.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOverrideTrays.DisplayMember = "Value"
        Me.cboOverrideTrays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOverrideTrays.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboOverrideTrays.Location = New System.Drawing.Point(172, 230)
        Me.cboOverrideTrays.Name = "cboOverrideTrays"
        Me.cboOverrideTrays.Size = New System.Drawing.Size(83, 23)
        Me.cboOverrideTrays.TabIndex = 6
        Me.cboOverrideTrays.ValueMember = "Key"
        '
        'txtNoOfPages
        '
        Me.txtNoOfPages.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNoOfPages.Location = New System.Drawing.Point(172, 200)
        Me.txtNoOfPages.Name = "txtNoOfPages"
        Me.txtNoOfPages.Size = New System.Drawing.Size(83, 23)
        Me.txtNoOfPages.TabIndex = 5
        '
        'lblPages
        '
        Me.lblPages.Location = New System.Drawing.Point(8, 197)
        Me.ResourceLookup1.SetLookup(Me.lblPages, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblPages", "Page(s)", ""))
        Me.lblPages.Name = "lblPages"
        Me.lblPages.Size = New System.Drawing.Size(155, 27)
        Me.lblPages.TabIndex = 13
        Me.lblPages.Text = "Page(s)"
        Me.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtcoloured
        '
        Me.txtcoloured.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtcoloured.Location = New System.Drawing.Point(172, 171)
        Me.txtcoloured.MaxLength = 4
        Me.txtcoloured.Name = "txtcoloured"
        Me.txtcoloured.Size = New System.Drawing.Size(83, 23)
        Me.txtcoloured.TabIndex = 4
        Me.txtcoloured.Text = "0"
        Me.txtcoloured.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtbillpaper
        '
        Me.txtbillpaper.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbillpaper.Location = New System.Drawing.Point(172, 141)
        Me.txtbillpaper.MaxLength = 4
        Me.txtbillpaper.Name = "txtbillpaper"
        Me.txtbillpaper.Size = New System.Drawing.Size(83, 23)
        Me.txtbillpaper.TabIndex = 3
        Me.txtbillpaper.Text = "0"
        Me.txtbillpaper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEngrossment
        '
        Me.txtEngrossment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEngrossment.Location = New System.Drawing.Point(172, 111)
        Me.txtEngrossment.MaxLength = 4
        Me.txtEngrossment.Name = "txtEngrossment"
        Me.txtEngrossment.Size = New System.Drawing.Size(83, 23)
        Me.txtEngrossment.TabIndex = 2
        Me.txtEngrossment.Text = "0"
        Me.txtEngrossment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCopies
        '
        Me.txtCopies.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopies.Location = New System.Drawing.Point(172, 81)
        Me.txtCopies.MaxLength = 4
        Me.txtCopies.Name = "txtCopies"
        Me.txtCopies.Size = New System.Drawing.Size(83, 23)
        Me.txtCopies.TabIndex = 1
        Me.txtCopies.Text = "0"
        Me.txtCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLetterhead
        '
        Me.txtLetterhead.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLetterhead.Location = New System.Drawing.Point(172, 52)
        Me.txtLetterhead.MaxLength = 4
        Me.txtLetterhead.Name = "txtLetterhead"
        Me.txtLetterhead.Size = New System.Drawing.Size(83, 23)
        Me.txtLetterhead.TabIndex = 0
        Me.txtLetterhead.Text = "0"
        Me.txtLetterhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNoCopies
        '
        Me.lblNoCopies.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNoCopies.Location = New System.Drawing.Point(5, 25)
        Me.ResourceLookup1.SetLookup(Me.lblNoCopies, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblNoCopies", "Number of Copies:", ""))
        Me.lblNoCopies.Name = "lblNoCopies"
        Me.lblNoCopies.Size = New System.Drawing.Size(249, 27)
        Me.lblNoCopies.TabIndex = 0
        Me.lblNoCopies.Text = "Number of Copies:"
        '
        'lblColoured
        '
        Me.lblColoured.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblColoured.Location = New System.Drawing.Point(8, 168)
        Me.ResourceLookup1.SetLookup(Me.lblColoured, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblColoured", "Coloured", ""))
        Me.lblColoured.Name = "lblColoured"
        Me.lblColoured.Size = New System.Drawing.Size(155, 27)
        Me.lblColoured.TabIndex = 10
        Me.lblColoured.Text = "Coloured"
        Me.lblColoured.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBillPaper
        '
        Me.lblBillPaper.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblBillPaper.Location = New System.Drawing.Point(8, 138)
        Me.ResourceLookup1.SetLookup(Me.lblBillPaper, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblBillPaper", "Bill Paper", ""))
        Me.lblBillPaper.Name = "lblBillPaper"
        Me.lblBillPaper.Size = New System.Drawing.Size(155, 27)
        Me.lblBillPaper.TabIndex = 8
        Me.lblBillPaper.Text = "Bill Paper"
        Me.lblBillPaper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEngrossment
        '
        Me.lblEngrossment.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblEngrossment.Location = New System.Drawing.Point(8, 108)
        Me.ResourceLookup1.SetLookup(Me.lblEngrossment, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblEngrossment", "Engrossment", ""))
        Me.lblEngrossment.Name = "lblEngrossment"
        Me.lblEngrossment.Size = New System.Drawing.Size(155, 27)
        Me.lblEngrossment.TabIndex = 6
        Me.lblEngrossment.Text = "Engrossment"
        Me.lblEngrossment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCopies
        '
        Me.lblCopies.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCopies.Location = New System.Drawing.Point(8, 78)
        Me.ResourceLookup1.SetLookup(Me.lblCopies, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblCopies", "Copies", ""))
        Me.lblCopies.Name = "lblCopies"
        Me.lblCopies.Size = New System.Drawing.Size(155, 27)
        Me.lblCopies.TabIndex = 4
        Me.lblCopies.Text = "Copies"
        Me.lblCopies.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLetterhead
        '
        Me.lblLetterhead.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblLetterhead.Location = New System.Drawing.Point(8, 50)
        Me.ResourceLookup1.SetLookup(Me.lblLetterhead, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblLetterhead", "Letterhead", ""))
        Me.lblLetterhead.Name = "lblLetterhead"
        Me.lblLetterhead.Size = New System.Drawing.Size(155, 27)
        Me.lblLetterhead.TabIndex = 2
        Me.lblLetterhead.Text = "Letterhead"
        Me.lblLetterhead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkTrackChanges
        '
        Me.chkTrackChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkTrackChanges.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkTrackChanges.Location = New System.Drawing.Point(12, 295)
        Me.ResourceLookup1.SetLookup(Me.chkTrackChanges, New FWBS.OMS.UI.Windows.ResourceLookupItem("chkTrackChanges", "Print &Track Changes", ""))
        Me.chkTrackChanges.Name = "chkTrackChanges"
        Me.chkTrackChanges.Size = New System.Drawing.Size(234, 24)
        Me.chkTrackChanges.TabIndex = 9
        Me.chkTrackChanges.Text = "Print &Track Changes"
        '
        'chkEnvelope
        '
        Me.chkEnvelope.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkEnvelope.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkEnvelope.Location = New System.Drawing.Point(172, 270)
        Me.ResourceLookup1.SetLookup(Me.chkEnvelope, New FWBS.OMS.UI.Windows.ResourceLookupItem("chkEnvelope", "&Envelope", ""))
        Me.chkEnvelope.Name = "chkEnvelope"
        Me.chkEnvelope.Size = New System.Drawing.Size(88, 24)
        Me.chkEnvelope.TabIndex = 8
        Me.chkEnvelope.Text = "&Envelope"
        '
        'chkDuplexed
        '
        Me.chkDuplexed.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkDuplexed.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkDuplexed.Location = New System.Drawing.Point(12, 270)
        Me.ResourceLookup1.SetLookup(Me.chkDuplexed, New FWBS.OMS.UI.Windows.ResourceLookupItem("chkDuplexed", "&Duplexed", ""))
        Me.chkDuplexed.Name = "chkDuplexed"
        Me.chkDuplexed.Size = New System.Drawing.Size(135, 24)
        Me.chkDuplexed.TabIndex = 7
        Me.chkDuplexed.Text = "&Duplexed"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblCurPrinter)
        Me.Panel1.Controls.Add(Me.lblCurrentPrinter)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Panel1.Location = New System.Drawing.Point(69, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(420, 65)
        Me.Panel1.TabIndex = 3
        '
        'lblCurPrinter
        '
        Me.lblCurPrinter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCurPrinter.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblCurPrinter.Location = New System.Drawing.Point(14, 25)
        Me.lblCurPrinter.Name = "lblCurPrinter"
        Me.lblCurPrinter.Size = New System.Drawing.Size(394, 29)
        Me.lblCurPrinter.TabIndex = 3
        '
        'pnlSelPrinter
        '
        Me.pnlSelPrinter.Controls.Add(Me.gbSelPrinter)
        Me.pnlSelPrinter.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSelPrinter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.pnlSelPrinter.Location = New System.Drawing.Point(69, 70)
        Me.pnlSelPrinter.Name = "pnlSelPrinter"
        Me.pnlSelPrinter.Padding = New System.Windows.Forms.Padding(3)
        Me.pnlSelPrinter.Size = New System.Drawing.Size(273, 58)
        Me.pnlSelPrinter.TabIndex = 4
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnEmailPDF)
        Me.pnlButtons.Controls.Add(Me.btnAuth)
        Me.pnlButtons.Controls.Add(Me.btnEmail)
        Me.pnlButtons.Controls.Add(Me.btnSignDocument)
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnPPreview)
        Me.pnlButtons.Controls.Add(Me.btnPrintFAX)
        Me.pnlButtons.Controls.Add(Me.btnFaxOptions)
        Me.pnlButtons.Controls.Add(Me.btnFax)
        Me.pnlButtons.Controls.Add(Me.btnPrint)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlButtons.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.pnlButtons.Location = New System.Drawing.Point(342, 70)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Padding = New System.Windows.Forms.Padding(2, 6, 2, 1)
        Me.pnlButtons.Size = New System.Drawing.Size(147, 389)
        Me.pnlButtons.TabIndex = 2
        '
        'btnEmailPDF
        '
        Me.btnEmailPDF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmailPDF.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnEmailPDF.Location = New System.Drawing.Point(5, 248)
        Me.ResourceLookup1.SetLookup(Me.btnEmailPDF, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnEmailPDF", "Email As PDF", ""))
        Me.btnEmailPDF.Name = "btnEmailPDF"
        Me.btnEmailPDF.Size = New System.Drawing.Size(139, 27)
        Me.btnEmailPDF.TabIndex = 9
        Me.btnEmailPDF.Text = "Email As PDF"
        '
        'btnAuth
        '
        Me.btnAuth.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuth.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnAuth.Location = New System.Drawing.Point(5, 291)
        Me.ResourceLookup1.SetLookup(Me.btnAuth, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnAuth", "Send To Authorise", ""))
        Me.btnAuth.Name = "btnAuth"
        Me.btnAuth.Size = New System.Drawing.Size(139, 27)
        Me.btnAuth.TabIndex = 7
        Me.btnAuth.Text = "Send To Authorise"
        '
        'btnEmail
        '
        Me.btnEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmail.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnEmail.Location = New System.Drawing.Point(5, 215)
        Me.ResourceLookup1.SetLookup(Me.btnEmail, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnEmail", "Email", ""))
        Me.btnEmail.Name = "btnEmail"
        Me.btnEmail.Size = New System.Drawing.Size(139, 27)
        Me.btnEmail.TabIndex = 6
        Me.btnEmail.Text = "Email"
        '
        'btnSignDocument
        '
        Me.btnSignDocument.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSignDocument.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSignDocument.Location = New System.Drawing.Point(5, 182)
        Me.ResourceLookup1.SetLookup(Me.btnSignDocument, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnSignDocument", "Si&gn Document", ""))
        Me.btnSignDocument.Name = "btnSignDocument"
        Me.btnSignDocument.Size = New System.Drawing.Size(139, 27)
        Me.btnSignDocument.TabIndex = 5
        Me.btnSignDocument.Text = "Si&gn Document"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gbPrintOptions)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Panel2.Location = New System.Drawing.Point(69, 128)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(3)
        Me.Panel2.Size = New System.Drawing.Size(273, 331)
        Me.Panel2.TabIndex = 1
        '
        'pnlNotice
        '
        Me.pnlNotice.Controls.Add(Me.lblnotice)
        Me.pnlNotice.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlNotice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.pnlNotice.Location = New System.Drawing.Point(69, 459)
        Me.pnlNotice.Name = "pnlNotice"
        Me.pnlNotice.Padding = New System.Windows.Forms.Padding(2)
        Me.pnlNotice.Size = New System.Drawing.Size(420, 39)
        Me.pnlNotice.TabIndex = 7
        Me.pnlNotice.Visible = False
        '
        'lblnotice
        '
        Me.lblnotice.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblnotice.Location = New System.Drawing.Point(2, 2)
        Me.lblnotice.Name = "lblnotice"
        Me.lblnotice.Size = New System.Drawing.Size(416, 35)
        Me.lblnotice.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.picPrint)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel3.Location = New System.Drawing.Point(5, 5)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(64, 493)
        Me.Panel3.TabIndex = 8
        '
        'picPrint
        '
        Me.picPrint.Location = New System.Drawing.Point(8, 8)
        Me.picPrint.Name = "picPrint"
        Me.picPrint.Size = New System.Drawing.Size(48, 48)
        Me.picPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picPrint.TabIndex = 46
        Me.picPrint.TabStop = False
        '
        'frmPrint
        '
        Me.AcceptButton = Me.btnPrint
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(494, 503)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.pnlSelPrinter)
        Me.Controls.Add(Me.pnlButtons)
        Me.Controls.Add(Me.pnlNotice)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.ResourceLookup1.SetLookup(Me, New FWBS.OMS.UI.Windows.ResourceLookupItem("frmPrint", "Print Options...", ""))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPrint"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Print Options..."
        Me.gbSelPrinter.ResumeLayout(False)
        Me.gbPrintOptions.ResumeLayout(False)
        Me.gbPrintOptions.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.pnlSelPrinter.ResumeLayout(False)
        Me.pnlButtons.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.pnlNotice.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.picPrint, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        LoadDefaultPrintSettings()
    End Sub
    Public Sub LoadDefaultPrintSettings()
        If _doc.Saved = False And _doc.ReadOnly = False Then
            If MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4010", "Document not Saved do you wish to Save?", ""), Nothing, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1) = vbYes Then
                Me.Close()
                _appcontroller.Save(_doc)
                Exit Sub
            End If
        End If

        'Document Save on Print - this routine is now optional via a registry key [WI.12874]
        Dim disableSaveOnPrint As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "DisableSaveOnPrint", "false")
        If Not FWBS.Common.ConvDef.ToBoolean(disableSaveOnPrint.GetSetting(False), False) Then
            'Make sure that all fields are merged.
            If _doc.Saved And _doc.ReadOnly = False Then
                _appcontroller.CheckFields(_doc)
                If _doc.Saved = False And _doc.Path <> "" Then
                    _doc.Save()
                End If
            End If
        End If

        'The document can only be authorised if the document is saved.
        btnAuth.Enabled = (_appcontroller.IsCompanyDocument(_doc) And _appcontroller.GetDocVariable(_doc, OMSApp.DOCUMENT, 0) <> 0 And _doc.Saved = True)


        picPrint.Image = Images.GetCoolButton(11, Images.IconSize.Size48).ToBitmap()

        'Check to see if there is already fax information stored within the document.
        CheckFaxInfo()

        'The document can only be emailed if the document is saved.
        btnEmail.Enabled = (_appcontroller.IsCompanyDocument(_doc) And _appcontroller.GetDocVariable(_doc, OMSApp.DOCUMENT, 0) <> 0 And _doc.Saved = True)
        btnEmailPDF.Enabled = (_appcontroller.IsCompanyDocument(_doc) And _appcontroller.GetDocVariable(_doc, OMSApp.DOCUMENT, 0) <> 0 And _doc.Saved = True)

        'Get the default copy count for each tray.
        txtLetterhead.Text = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("printer/prtNoLetterhead", 0))
        txtCopies.Text = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("printer/prtNoCopies", 2))
        txtbillpaper.Text = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("printer/prtNoBillPaper", 0))
        txtEngrossment.Text = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("printer/prtNoEngrossment", 0))
        txtcoloured.Text = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("printer/prtNoColoured", 0))
        chkDuplexed.Checked = False


        'Get the printer list from the database.
        On Error Resume Next
        cbPrinter.BeginUpdate()
        cbPrinter.DataSource = FWBS.OMS.Printer.GetPrinterList(True)
        cbPrinter.DisplayMember = "printName"
        cbPrinter.ValueMember = "printID"
        cbPrinter.EndUpdate()

        ListTrayOptions()

        UpdatePrintInfo()


        If _appcontroller.IsPrecedent(_doc) Then
            btnEmail.Visible = False
            btnEmailPDF.Visible = False
            btnAuth.Visible = False
            btnFax.Visible = False
            btnFaxOptions.Visible = False
            btnPrintFAX.Visible = False
            btnSignDocument.Visible = False
        End If
        If bulkprint = True Then
            LoadBulkPrintValues()
        End If

        txtCopies.Select()
    End Sub

    Private Sub LoadBulkPrintValues()
        txtCopies.Text = _printdialog.CopiesToPrint
    End Sub


    Private Sub txtLetterhead_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLetterhead.KeyPress, txtbillpaper.KeyPress, txtCopies.KeyPress, txtcoloured.KeyPress, txtEngrossment.KeyPress
        Select Case e.KeyChar
            Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", vbBack, Chr(9)

            Case Else
                e.Handled = True
        End Select

    End Sub


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        btnPrint.Enabled = False
        Try
            If bulkprint = True Then
                PrintNow()
            Else
                If PrintNow() Then Me.DialogResult = System.Windows.Forms.DialogResult.OK
            End If

        Catch ex As Exception
            MessageBox.Show(ex)
            btnPrint.Enabled = True
        End Try
    End Sub

    Public Function PrintNow() As Boolean

        PrintNow = True

        'Fetch the selected printer.
        Call CheckPrinterSettings()

        If Val(txtLetterhead.Text) > 0 And txtLetterhead.Enabled Then
            SetPaperTray(FWBS.OMS.Session.CurrentSession.CurrentPrinter.LetterheadTray, FWBS.OMS.Session.CurrentSession.CurrentPrinter.ContinuationTray)
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("PRINTONLH", "Printing On LetterHead", "").Text
            NormalPrint(txtLetterhead.Text)
        End If

        If Val(txtCopies.Text) > 0 And txtCopies.Enabled Then
            SetPaperTray(FWBS.OMS.Session.CurrentSession.CurrentPrinter.CopyTray, FWBS.OMS.Session.CurrentSession.CurrentPrinter.CopyTray)
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("PRINTONCOPY", "Printing On Copy Paper", "").Text
            NormalPrint(txtCopies.Text)
        End If

        If Val(txtEngrossment.Text) > 0 And txtEngrossment.Enabled Then
            SetPaperTray(FWBS.OMS.Session.CurrentSession.CurrentPrinter.EngrossmentTray, FWBS.OMS.Session.CurrentSession.CurrentPrinter.EngrossmentTray)
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("PRINTONEP", "Printing On Engrossment Paper", "").Text
            NormalPrint(txtEngrossment.Text)
        End If

        If Val(txtbillpaper.Text) > 0 And txtbillpaper.Enabled Then
            SetPaperTray(FWBS.OMS.Session.CurrentSession.CurrentPrinter.BillPaperTray, FWBS.OMS.Session.CurrentSession.CurrentPrinter.BillPaperTray)
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("PRINTONBP", "Printing On Bill Paper", "").Text
            NormalPrint(txtbillpaper.Text)
        End If

        If Val(txtcoloured.Text) > 0 And txtcoloured.Enabled Then
            SetPaperTray(FWBS.OMS.Session.CurrentSession.CurrentPrinter.ColouredTray, FWBS.OMS.Session.CurrentSession.CurrentPrinter.ColouredTray)
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("PRINTONCP", "Printing On Coloured Paper", "").Text
            NormalPrint(txtcoloured.Text)
        End If


        ' Print Envelope Routine
        If chkEnvelope.Checked = True Then
            On Error Resume Next
            _doc.Envelope.PrintOut(ExtractAddress:=False, OmitReturnAddress _
            :=False, PrintBarCode:=False, PrintFIMA:=False, Height:= _
            _app.CentimetersToPoints(11), Width:=_app.CentimetersToPoints(22), Address:= _
            _appcontroller.GetDocVariable(_doc, "#ADDRESSEE", ""), ReturnAddress:="", AddressFromLeft:=Word.WdConstants.wdAutoPosition, AddressFromTop:=Word.WdConstants.wdAutoPosition, _
            ReturnAddressFromLeft:=Word.WdConstants.wdAutoPosition, ReturnAddressFromTop:= _
            Word.WdConstants.wdAutoPosition)
        End If

        If _oldprint <> "" Then
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("RESELECTPRINT", "ReSelecting the Printer...", "").Text
            SetPrinterByName(_oldprint)
        End If


    End Function

    Private Sub UpdatePrintInfo()
        Dim prop As Object

        'Get the default template print information that was saved with
        'the precedent / template.
        For Each prop In _doc.CustomDocumentProperties
            If Not prop Is Nothing Then
                Dim localname As String = Session.CurrentSession.GetInternalDocumentPropertyName(prop.Name)

                Select Case UCase$(localname)
                    Case "PRINTERNAME"
                        Try
                            cbPrinter.SelectedValue = Convert.ToInt32(prop.Value)
                        Catch ex As Exception
                            cbPrinter.Text = prop.Value
                        End Try
                    Case "PRINTNOW", "PRINTITNOW"
                        If prop.Value = True Then
                            _printsettings = PrecPrintMode.Print
                        End If
                    Case "NOLETTERHEAD", "NOOFLETTERHEAD", "TRAYONE"
                        txtLetterhead.Text = Trim(Str(prop.Value))
                    Case "NOCOPIES", "NOOFCOPIES", "TRAYTWO"
                        txtCopies.Text = Trim(Str(prop.Value))
                    Case "NOENGROSSMENT", "TRAYTHREE"
                        txtEngrossment.Text = Trim(Str(prop.Value))
                    Case "NOBILLPAPER", "TRAYFOUR"
                        txtbillpaper.Text = Trim(Str(prop.Value))
                    Case "NOCOLOURED", "TRAYFIVE"
                        txtcoloured.Text = Trim(Str(prop.Value))
                    Case "DUPLEX"
                        chkDuplexed.Checked = prop.Value
                    Case "PAGETRAYOPTION"
                        cboOverrideTrays.SelectedValue = prop.Value
                End Select
            End If
        Next

        If cbPrinter.SelectedValue Is DBNull.Value Then
            If Not FWBS.OMS.Session.CurrentSession.CurrentPrinter Is Nothing Then
                cbPrinter.SelectedValue = FWBS.OMS.Session.CurrentSession.CurrentPrinter.ID
            End If
        End If

        If FWBS.OMS.Session.CurrentSession.IsLicensedFor("FAX") = False Then
            btnPrintFAX.Visible = False
            btnFax.Visible = False
            btnFaxOptions.Visible = False
            Me.AcceptButton = btnPrint
        Else
            btnPrintFAX.Visible = True
            btnFax.Visible = True
            btnFaxOptions.Visible = True
            If _appcontroller.HasDocVariable(_doc, "FAXONLY") Then
                If Me.AcceptButton Is btnFax Then
                    If txtLetterhead.Text <> "0" Then
                        txtLetterhead.Text = Trim(Str(Val(txtLetterhead.Text) - 1))
                    End If
                End If
            End If
        End If
    End Sub

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

    Private Sub CheckPrinterSettings()
        Dim duplexopt As String = ""
        'Set the duplexing option
        If chkDuplexed.Checked = True Then
            duplexopt = "_DUPLEX"
            lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("SETTINGDUPLEX", "Setting Duplex Printer Please Wait...", "").Text
        End If

        If Not cbPrinter.SelectedValue Is System.DBNull.Value Then ' Printer has been changed
            _oldprint = _app.ActivePrinter
            On Error Resume Next

            'Set the duplexing option
            If chkDuplexed.Checked = False Then
                lblCurPrinter.Text = Session.CurrentSession.Resources.GetResource("SELECTINGPRINT", "Selecting the Printer Please Wait...", "").Text
            End If

            FWBS.OMS.Session.CurrentSession.CurrentPrinter = FWBS.OMS.Printer.GetPrinter(cbPrinter.SelectedValue)
            If Err.Number() <> 0 Then
                Throw New OMSException2("4011", "Error Selecting the Printer Object Please contact a supervisor.", "")
            Else
                SetPrinterByName(FWBS.OMS.Session.CurrentSession.CurrentPrinter.PrinterName & duplexopt)
                lblCurPrinter.Text = _app.ActivePrinter & duplexopt
                If Err.Number() <> 0 Then
                    MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4012", "Printer not Installed.", ""), Nothing, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
                    If FWBS.OMS.Session.CurrentSession.CurrentPrinter.InstallCommand <> "" Then
                        If MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4013", "Would you Like to install this Printer? You may need an Administrator to install this printer.", ""), Nothing, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                            FWBS.OMS.Session.CurrentSession.CurrentPrinter.Install()
                        End If
                    End If
                End If
            End If
        Else
            If FWBS.OMS.Session.CurrentSession.CurrentPrinter Is Nothing Then
                Throw New OMSException2("NOPRINTERSET", "There is no current printer set.")
            Else
                SetPrinterByName(FWBS.OMS.Session.CurrentSession.CurrentPrinter.PrinterName & duplexopt)
            End If
        End If
    End Sub

    Private Sub SetPaperTray(ByVal InFirstTrayText As String, ByVal InSecondTrayText As String)

        If cboOverrideTrays.SelectedValue = "OVERRIDE" Then
            Return
        End If

        Using pd As WordOMS2.UnProtect = New WordOMS2.UnProtect(_doc)

            'Capture the Argument out of Range COMException when sections have a different margin value
            'When this error occurs set each section first and secondary pages seperately.
            Try
                If (cboOverrideTrays.SelectedValue = "NEW") Then
                    For ctr As Integer = 1 To _doc.Sections.Count
                        Dim section As Word.Section = _doc.Sections(ctr)
                        If (ctr = 1) Then
                            SetPaperTray(section.PageSetup, InFirstTrayText, InSecondTrayText)
                        Else
                            SetPaperTray(section.PageSetup, InSecondTrayText, InSecondTrayText)
                        End If
                    Next ctr
                Else
                    SetPaperTray(_doc.PageSetup, InFirstTrayText, InSecondTrayText)
                End If
            Catch comex As System.Runtime.InteropServices.COMException When comex.ErrorCode = -2146823680

                If (cboOverrideTrays.SelectedValue = "NEW") Then
                    For ctr As Integer = 1 To _doc.Sections.Count
                        Dim section As Word.Section = _doc.Sections(ctr)
                        If (ctr = 1) Then
                            SetPaperTray(section.PageSetup, InFirstTrayText, InSecondTrayText)
                        Else
                            SetPaperTray(section.PageSetup, InSecondTrayText, InSecondTrayText)
                        End If
                    Next ctr
                Else
                    'Old original style for backward compatibility
                    For ctr As Integer = 1 To _doc.Sections.Count
                        Dim section As Word.Section = _doc.Sections(ctr)
                        If (ctr = 1) Then
                            SetPaperTray(section.PageSetup, InFirstTrayText, InSecondTrayText)
                        Else
                            SetPaperTray(section.PageSetup, InFirstTrayText, InSecondTrayText)
                        End If
                    Next ctr
                End If
            End Try

        End Using
    End Sub


    Private Sub SetPaperTray(ByVal pagesetup As Word.PageSetup, ByVal InFirstTrayText As String, ByVal InSecondTrayText As String)

        If _doc.ProtectionType <> Word.WdProtectionType.wdNoProtection Then
            ' Code to check if the current document is protected still and if so jump from this routine.
            Exit Sub
        End If



        With pagesetup

            Select Case InFirstTrayText
                Case "Upper Paper Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterUpperBin
                Case "Automatic Sheet Feed"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterAutomaticSheetFeed
                Case "Default Tray", "Default"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterDefaultBin
                Case "Envelope Feed"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterEnvelopeFeed
                Case "Form Source"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterFormSource
                Case "Large Capacity Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterLargeCapacityBin
                Case "Lower Paper Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterLowerBin
                Case "Large Format Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterLargeFormatBin
                Case "Manual Envelope Feed"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterManualEnvelopeFeed
                Case "Manual Feed"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterManualFeed
                Case "Middle Paper Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterMiddleBin
                Case "Only Paper Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterOnlyBin
                Case "Paper Cassette"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterPaperCassette
                Case "Small Format Tray"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterSmallFormatBin
                Case "Tractor Feed"
                    .FirstPageTray = Word.WdPaperTray.wdPrinterTractorFeed
                Case Else
                    If Val(InFirstTrayText) > 0 Then
                        .FirstPageTray = Val(InFirstTrayText)
                    Else
                        .FirstPageTray = Word.WdPaperTray.wdPrinterDefaultBin
                    End If
            End Select

            Select Case InSecondTrayText
                Case "Upper Paper Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterUpperBin
                Case "Automatic Sheet Feed"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterAutomaticSheetFeed
                Case "Default Tray", "Default"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterDefaultBin
                Case "Envelope Feed"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterEnvelopeFeed
                Case "Form Source"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterFormSource
                Case "Large Capacity Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterLargeCapacityBin
                Case "Lower Paper Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterLowerBin
                Case "Large Format Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterLargeFormatBin
                Case "Manual Envelope Feed"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterManualEnvelopeFeed
                Case "Manual Feed"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterManualFeed
                Case "Middle Paper Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterMiddleBin
                Case "Only Paper Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterOnlyBin
                Case "Paper Cassette"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterPaperCassette
                Case "Small Format Tray"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterSmallFormatBin
                Case "Tractor Feed"
                    .OtherPagesTray = Word.WdPaperTray.wdPrinterTractorFeed
                Case Else
                    If Val(InSecondTrayText) > 0 Then
                        .OtherPagesTray = Val(InSecondTrayText)
                    Else
                        .OtherPagesTray = Word.WdPaperTray.wdPrinterDefaultBin
                    End If
            End Select
        End With

    End Sub

    Private Sub NormalPrint(ByVal toprint As String)
        With _app.Options
            .UpdateFieldsAtPrint = False
            .UpdateLinksAtPrint = False
            .DefaultTray = "Use Printer Settings"
            .PrintBackground = False
            .PrintProperties = False
            .PrintFieldCodes = False
            .PrintComments = False
            .PrintHiddenText = False
            .PrintDrawingObjects = True
            .PrintDraft = False
            .PrintReverse = False
            .MapPaperSize = True
        End With
        With _doc
            .PrintPostScriptOverText = False
            .PrintFormsData = False
        End With
        Debug.Assert(_app.ActiveDocument Is _doc, "Application ActiveDocument is not equal to the Document")
        If txtNoOfPages.Text = "ALL" Or txtNoOfPages.Text = "" Then
            _doc.PrintOut(Item:=IIf(chkTrackChanges.Checked,
                                    Word.WdPrintOutItem.wdPrintDocumentWithMarkup,
                                    Word.WdPrintOutItem.wdPrintDocumentContent),
                          Copies:=Val(toprint),
                          Pages:="1",
                          PageType:=Word.WdPrintOutPages.wdPrintAllPages,
                          Collate:=True,
                          Background:=False,
                          PrintToFile:=False)
        Else
            Dim pages As Object = txtNoOfPages.Text
            _doc.PrintOut(Range:=Word.WdPrintOutRange.wdPrintRangeOfPages,
                          Item:=IIf(chkTrackChanges.Checked,
                                    Word.WdPrintOutItem.wdPrintDocumentWithMarkup,
                                    Word.WdPrintOutItem.wdPrintDocumentContent),
                          Copies:=Val(toprint),
                          Pages:=pages,
                          PageType:=Word.WdPrintOutPages.wdPrintAllPages,
                          Collate:=True,
                          Background:=False,
                          PrintToFile:=False)
        End If

    End Sub

    Private Sub SetPrinterByName(ByVal instring As String)
        On Error Resume Next

        'A regisrty key to allow alternate ways of setting the default printer.  This is dor performance reasons
        'as Application.ActivePrinter seems to be very slow in certain scenarios.
        Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "SetPrinterFunction", "ACTIVEPRINTER")

        Select Case reg.ToString().ToUpper()
            Case "ACTIVEPRINTER"
                If Not _app.ActivePrinter.ToUpper().StartsWith(instring.ToUpper()) Then
                    _app.ActivePrinter = instring
                End If
            Case Else
                _app.WordBasic.FilePrintSetup(Printer:=instring, DoNotSetAsSysDefault:=1)
        End Select

    End Sub


    Private Sub btnFaxOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFaxOptions.Click
        Try
            Call _appcontroller.ShowFaxOptions(Me, _doc)
            Call CheckFaxInfo()
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
    End Sub



    Private Sub CheckFaxInfo()
        ' Check the current status for the fax if the fax is enabled in the
        ' system then enable the buttons accordingly.
        ' If FAXTIME is not present then default to send immediately.
        ' Enable the Print and fax nuttons only if the system is going
        ' to allow a fax and default the button if set accordingly.

        On Error Resume Next
        If _appcontroller.HasDocVariable(_doc, "FAXTIME") Then
            If _appcontroller.GetDocVariable(_doc, "FAXTIME", "DONTFAX") = "DONTFAX" Then
                ' Manual Fax
                ' Dont Enable the buttons
                btnPrintFAX.Enabled = False
                btnFax.Enabled = False
                Me.AcceptButton = btnPrint
            Else
                btnPrintFAX.Enabled = True
                btnFax.Enabled = True
                Me.AcceptButton = btnPrintFAX
            End If
            lblnotice.Text = ResourceLookup.GetLookupText("FAXTO") & " " & _appcontroller.GetDocVariable(_doc, "FAXTO", "")
            lblnotice.Text = lblnotice.Text & ", " & _appcontroller.GetDocVariable(_doc, "FAXCOMPNAME", "")
            lblnotice.Text = lblnotice.Text & Environment.NewLine & ResourceLookup.GetLookupText("FAXNO") & " " & _appcontroller.GetDocVariable(_doc, "FAXNUMBER", "")

            If _appcontroller.GetDocVariable(_doc, "FAXTIME", "") = "DONTFAX" Then
                lblnotice.Text = lblnotice.Text & " " & ResourceLookup.GetLookupText("FAXMANUAL")
            End If
            If _appcontroller.GetDocVariable(_doc, "FAXTIME", "") = "NOW" Then
                lblnotice.Text = lblnotice.Text & " " & ResourceLookup.GetLookupText("FAXNOW")
            End If
            If _appcontroller.GetDocVariable(_doc, "FAXTIME", "") = "OFFPEAK" Then
                lblnotice.Text = lblnotice.Text & " " & ResourceLookup.GetLookupText("FAXOFFPEAK")
            End If
            On Error Resume Next
            If _appcontroller.HasDocVariable(_doc, "FAXONLY") Then
                If _appcontroller.GetDocVariable(_doc, "FAXONLY", False) Then
                    Me.AcceptButton = btnFax
                End If
            End If
            pnlNotice.Visible = True
        Else
            pnlNotice.Visible = False
        End If

    End Sub

    Private Sub btnPPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPPreview.Click
        _doc.PrintPreview()
        _doc.Activate()
        System.Windows.Forms.Application.DoEvents()
        Me.Close()

    End Sub

    Public Property Setting() As FWBS.OMS.PrecPrintMode
        Get
            Return _printsettings
        End Get
        Set(ByVal Value As FWBS.OMS.PrecPrintMode)
            _printsettings = Value
        End Set
    End Property

    Private Sub frmPrint_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        If _firsttimeran = False Then
            _firsttimeran = True
            If (_printsettings Or PrecPrintMode.Dialog) <> _printsettings Then
                Try
                    If ((_printsettings Or PrecPrintMode.Email) = _printsettings) Then
                        SendDocViaEmail()
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex)
                End Try
                If ((_printsettings Or PrecPrintMode.Fax) = _printsettings) Then
                    _appcontroller.SetDocVariable(_doc, "FAXTIME", "NOW")
                    FaxIt()
                End If
                Try
                Catch ex As Exception
                    MessageBox.Show(ex)
                End Try
                Try
                    If ((_printsettings Or PrecPrintMode.Print) = _printsettings) Then
                        PrintNow()
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex)
                End Try
                DialogResult = System.Windows.Forms.DialogResult.OK
            End If
        End If
    End Sub


    Private Sub btnSignDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSignDocument.Click
        Try
            _appcontroller.RunCommand(_doc, "INTERNAL;SIGNDOCUMENT")
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
    End Sub

    Private Sub SendForAuthorize()
        _appcontroller.SendDocForAuthorisation(_doc)
    End Sub

    Private Function SendDocViaEmail() As Boolean

        If btnEmail.Enabled = False Then
            Return False
        End If

        Return DirectCast(_appcontroller, WordOMS2).SendDocViaEmail(_doc, Me)

    End Function
    Private Function SendPDFDocViaEmail() As Boolean

        If btnEmailPDF.Enabled = False Then
            Return False
        End If

        Return DirectCast(_appcontroller, WordOMS2).SendDocViaEmail(_doc, Me, True)

    End Function



    Private Function FaxIt() As Boolean

        FWBS.OMS.Session.CurrentSession.ValidateLicensedFor("FAX")

        Dim doc As Word.Document = _doc

        If _appcontroller.GetActiveDocType(doc) = "LETTERHEAD" Then
            Try
                Dim n As String = doc.Shapes("SIGNATURE").Name
            Catch ex As Exception
                If MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ADDSIGTOFAX", "You have not added a signature to this document, would you like to do this now?", ""), "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                    _appcontroller.RunCommand(doc, "INTERNAL;SIGNDOCUMENT")
                End If
            End Try

            Try
                Dim n As String = doc.Shapes("SIGNATURE").Name
            Catch ex As Exception
                If MessageBox.Show(Session.CurrentSession.Resources.GetMessage("FAXWITHOUTSIG", "Are you sure you wish to continue Faxing this document with no signature?", ""), "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                    Return False
                End If
            End Try

        End If

        ' Store the Old Printer then relink the ZEFAX Printer
        Dim faxto As String = _appcontroller.GetDocVariable(doc, "FAXTO", "")
        If faxto = "" Then
            If _appcontroller.ShowFaxOptions(Me, _doc) = False Then
                Return False
            End If
            CheckFaxInfo()
            faxto = _appcontroller.GetDocVariable(doc, "FAXTO", "")
            If faxto = "" Then
                Throw New OMSException2("NOFAXINFO", "No fax information found!")
                Exit Function
            End If
        End If
        If _appcontroller.GetDocVariable(_doc, "FAXTIME", "DONTFAX") = "DONTFAX" Then Return False

        Dim oldpr As String
        oldpr = doc.Application.ActivePrinter
        Dim zfaxpr As String

        Dim reg1 As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "FAX", "FaxPrinterName", "Zetafax Printer")
        zfaxpr = Convert.ToString(reg1.GetSetting())
        SetPrinterByName(zfaxpr)

        Dim t As Integer = 1
        doc.PrintOut(Background:=False)

tryAgain:
        t = t + 1
        Dim hwnd As Integer = Common.Functions.FindWindow(vbNullString, "Zetafax - Addressing")
        System.Windows.Forms.Application.DoEvents()
        If hwnd = 0 And t < 12001 Then
            System.Windows.Forms.Application.DoEvents()
            GoTo tryAgain
        End If

        System.Windows.Forms.Application.DoEvents()
        If t = 12000 Then
            SetPrinterByName(oldpr)
            Throw New OMSException2("ZFAXHUNG", "Zetafax Not Responding Can't Fax!", "")
            Exit Function
        End If

        't = Timer: While Timer - t < 1.5: DoEvents: Wend
        Dim conv1 As Integer = doc.Application.DDEInitiate("Zetafax", "Addressing")
        doc.Application.DDEExecute(conv1, "[DDEControl]")

        If _appcontroller.GetDocVariable(doc, "FAXTIME", "") = "OFFPEAK" Then
            doc.Application.DDEPoke(conv1, "After", "18:00:00")
        End If
        doc.Application.DDEPoke(conv1, "To", _appcontroller.GetDocVariable(doc, "FAXNUMBER", "") & "," & _appcontroller.GetDocVariable(doc, "FAXTO", "") & "," & _appcontroller.GetDocVariable(doc, "FAXCOMPNAME", ""))

        Dim tmpget As String
        Dim tmpgetfaxtemplate As String
        If _appcontroller.GetActiveDocType(doc) = "LETTERHEAD" Then
            Dim reg2 As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "FAX", "LETTHEAD", "LETTHEAD")
            tmpget = Convert.ToString(reg2.GetSetting())
            If tmpget = "TEMPLATE" Then
                If UCase(doc.AttachedTemplate.name) = "LETTERHEAD.DOT" Then
                    tmpgetfaxtemplate = "LETTHEAD"
                Else
                    tmpgetfaxtemplate = UCase(Microsoft.VisualBasic.Left(Microsoft.VisualBasic.Left(doc.AttachedTemplate.Name, InStr(doc.AttachedTemplate.Name, ".") - 1), 8))
                End If
                doc.Application.DDEPoke(conv1, "Letterhead", tmpgetfaxtemplate)
            Else
                doc.Application.DDEPoke(conv1, "Letterhead", tmpget)
            End If
        End If

        doc.Application.DDEPoke(conv1, "Quality", "High")
        doc.Application.DDEExecute(conv1, "[Send]")
        doc.Application.DDEExecute(conv1, "[DDERelease]")
        doc.Application.DDETerminate(conv1)

        SendMessage(hwnd, WM_CLOSE, 0, 0)

        SetPrinterByName(oldpr)
        System.Windows.Forms.Application.DoEvents()

        Return True

    End Function

    Private Sub btnFax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFax.Click
        Try
            If FaxIt() Then DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
    End Sub

    Private Sub btnPrintFAX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintFAX.Click
        Dim fax As Boolean = False
        Dim print As Boolean = False
        Try
            fax = FaxIt()
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
        Try
            print = PrintNow()
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
        If fax And print Then DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub cbPrinter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPrinter.SelectedIndexChanged
        lblCurPrinter.Text = Convert.ToString(cbPrinter.Text)

        Dim pr As FWBS.OMS.Printer = Nothing
        If Not cbPrinter.SelectedValue Is DBNull.Value Then
            Try
                If Not Session.CurrentSession.CurrentPrinter Is Nothing Then
                    If Convert.ToInt32(cbPrinter.SelectedValue) = Session.CurrentSession.CurrentPrinter.ID Then
                        pr = Session.CurrentSession.CurrentPrinter
                    End If
                End If
                If pr Is Nothing Then
                    pr = FWBS.OMS.Printer.GetPrinter(Convert.ToInt32(cbPrinter.SelectedValue))
                End If
            Catch
            End Try

            'Now Turn off the Options if they are set to NA
            If Not pr Is Nothing Then

                If pr.LetterheadTray = "NA" Then
                    txtLetterhead.Enabled = False
                    txtLetterhead.Text = "NA"
                Else
                    txtLetterhead.Enabled = True
                End If
                If pr.CopyTray = "NA" Then
                    txtCopies.Enabled = False
                    txtCopies.Text = "NA"
                Else
                    txtCopies.Enabled = True
                End If
                If pr.BillPaperTray = "NA" Then
                    txtbillpaper.Enabled = False
                    txtbillpaper.Text = "NA"
                Else
                    txtbillpaper.Enabled = True
                End If
                If pr.EngrossmentTray = "NA" Then
                    txtEngrossment.Enabled = False
                    txtEngrossment.Text = "NA"
                Else
                    txtEngrossment.Enabled = True
                End If
                If pr.ColouredTray = "NA" Then
                    txtcoloured.Enabled = False
                    txtcoloured.Text = "NA"
                Else
                    txtcoloured.Enabled = True
                End If
            End If
        End If
    End Sub


    Private Sub btnEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmail.Click
        Try
            Dim m As System.Windows.Forms.DialogResult
            m = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("WYOULPEMAIL", "Would you like to Print before emailing this document?")
            If m = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
            If m = System.Windows.Forms.DialogResult.Yes Then
                btnPrint.Enabled = False
                Call SendDocViaEmail()
                Try
                    If PrintNow() Then Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Catch ex As Exception
                    MessageBox.Show(ex)
                    btnPrint.Enabled = True
                End Try
            Else
                Call SendDocViaEmail()
            End If
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
    End Sub

    Private Sub btnEmailPDF_Click(sender As System.Object, e As System.EventArgs) Handles btnEmailPDF.Click
        Try

            Dim m As System.Windows.Forms.DialogResult
            m = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("WYOULPEMAIL", "Would you like to Print before emailing this document?")
            If m = System.Windows.Forms.DialogResult.Cancel Then Exit Sub

            If m = System.Windows.Forms.DialogResult.Yes Then
                btnPrint.Enabled = False
                Try
                    If PrintNow() Then Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Catch ex As Exception
                    MessageBox.Show(ex)
                    btnPrint.Enabled = True
                End Try
            End If

            Call SendPDFDocViaEmail()
            'make main code call to actual email as pdf stuff

        Catch ex As Exception
            MessageBox.Show(ex)
        End Try

    End Sub


    Private Sub btnAuth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuth.Click
        Try
            Call SendForAuthorize()
        Catch ex As Exception
            MessageBox.Show(ex)
        End Try
    End Sub


    Private Sub frmPrint_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        txtCopies.Select()
    End Sub


End Class


