Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Outlook = Microsoft.Office.Interop.Outlook

Friend Class frmEmailSelector
    Inherits BaseForm

#Region "Enums"

    Public Enum EmailSelectionType
        Delete
        Move
        Save
    End Enum

    Public Enum Icons
        None = -1
        Email = 0
        Saving = 1
        Deleted = 2
        Success = 3
        Err = 4
        Processing = 5
        Cancelled = 6
        Moved = 7
    End Enum

    Public Enum Columns
        Icons = 0
        From = 1
        Subject = 2
        TargetAssocInfo = 3
        TargetAssocID = 5
        SourceAssocInfo = 4
        SourceAssocID = 6
    End Enum

#End Region

#Region "Fields"

    Private _oms As OutlookOMS
    Private _sel As IEnumerable(Of OutlookItem)
    Private _seltype As EmailSelectionType
    Private _assoc As Associate
    Private _folder As Outlook.MAPIFolder
    Private _manual As List(Of OutlookItem) = New List(Of OutlookItem)
    Private _cancelled As List(Of OutlookItem) = New List(Of OutlookItem)
    Private cancel As Boolean = False
    Private processing As Boolean = False
    Private checkSelectAll As Boolean = False

#End Region

#Region "Constructors"

#Region " Windows Form Designer generated code "

    Private Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        lvwList.RightToLeft = Me.RightToLeft
        lvwList.RightToLeftLayout = (Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes)

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            FWBS.OMS.UI.Windows.Global.RemoveAndDisposeControls(Me)

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
    Private formStorage As FWBS.OMS.UI.Windows.ucFormStorage
    Private WithEvents ResourceLookup1 As FWBS.OMS.UI.Windows.ResourceLookup
    Private WithEvents colAttachedTo As System.Windows.Forms.ColumnHeader
    Private WithEvents lblRedDraft As System.Windows.Forms.Label
    Private WithEvents Splitter1 As System.Windows.Forms.Splitter
    Private WithEvents chkQuickSave As System.Windows.Forms.CheckBox
    Private WithEvents chkSkipTime As System.Windows.Forms.CheckBox
    Private WithEvents lvwList As FWBS.OMS.UI.ListView
    Private WithEvents colSaveTo As System.Windows.Forms.ColumnHeader
    Private WithEvents colSubject As System.Windows.Forms.ColumnHeader
    Private WithEvents colFrom As System.Windows.Forms.ColumnHeader
    Private WithEvents txtPreview As System.Windows.Forms.RichTextBox
    Private WithEvents pnlButtons As System.Windows.Forms.Panel
    Private WithEvents ImageList1 As System.Windows.Forms.ImageList
    Private WithEvents colStatus As System.Windows.Forms.ColumnHeader
    Private WithEvents btnProcess As System.Windows.Forms.Button
    Private WithEvents btnCancel As System.Windows.Forms.Button
    Private WithEvents chkPreview As System.Windows.Forms.CheckBox
    Private WithEvents btnChange As System.Windows.Forms.Button
    Private WithEvents picSave As System.Windows.Forms.PictureBox
    Private WithEvents lblSave As System.Windows.Forms.Label
    Private WithEvents lblDel As System.Windows.Forms.Label
    Private WithEvents picDel As System.Windows.Forms.PictureBox
    Private WithEvents lblError As System.Windows.Forms.Label
    Private WithEvents picError As System.Windows.Forms.PictureBox
    Private WithEvents lblCancel As System.Windows.Forms.Label
    Private WithEvents picCancel As System.Windows.Forms.PictureBox
    Private WithEvents lblBoldUnsaved As System.Windows.Forms.Label
    Private WithEvents lblNormalSaved As System.Windows.Forms.Label
    Private WithEvents lblMove As System.Windows.Forms.Label
    Private WithEvents picMove As System.Windows.Forms.PictureBox
    Private WithEvents lblMoving As System.Windows.Forms.Label
    Private WithEvents picMoving As System.Windows.Forms.PictureBox
    Private WithEvents lblDeleting As System.Windows.Forms.Label
    Private WithEvents picDeleting As System.Windows.Forms.PictureBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEmailSelector))
        Me.lvwList = New FWBS.OMS.UI.ListView()
        Me.colStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colFrom = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colSubject = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colSaveTo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colAttachedTo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.formStorage = New FWBS.OMS.UI.Windows.ucFormStorage(Me.components)
        Me.txtPreview = New System.Windows.Forms.RichTextBox()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.chkSkipTime = New System.Windows.Forms.CheckBox()
        Me.chkQuickSave = New System.Windows.Forms.CheckBox()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnChange = New System.Windows.Forms.Button()
        Me.chkPreview = New System.Windows.Forms.CheckBox()
        Me.lblRedDraft = New System.Windows.Forms.Label()
        Me.lblDeleting = New System.Windows.Forms.Label()
        Me.picDeleting = New System.Windows.Forms.PictureBox()
        Me.lblNormalSaved = New System.Windows.Forms.Label()
        Me.lblBoldUnsaved = New System.Windows.Forms.Label()
        Me.lblCancel = New System.Windows.Forms.Label()
        Me.picCancel = New System.Windows.Forms.PictureBox()
        Me.lblError = New System.Windows.Forms.Label()
        Me.picError = New System.Windows.Forms.PictureBox()
        Me.lblMoving = New System.Windows.Forms.Label()
        Me.picMoving = New System.Windows.Forms.PictureBox()
        Me.lblDel = New System.Windows.Forms.Label()
        Me.picDel = New System.Windows.Forms.PictureBox()
        Me.lblSave = New System.Windows.Forms.Label()
        Me.picSave = New System.Windows.Forms.PictureBox()
        Me.lblMove = New System.Windows.Forms.Label()
        Me.picMove = New System.Windows.Forms.PictureBox()
        Me.ResourceLookup1 = New FWBS.OMS.UI.Windows.ResourceLookup(Me.components)
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.pnlButtons.SuspendLayout()
        CType(Me.picDeleting, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picError, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picMoving, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picDel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSave, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picMove, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvwList
        '
        Me.lvwList.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lvwList.CheckBoxes = True
        Me.lvwList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colStatus, Me.colFrom, Me.colSubject, Me.colSaveTo, Me.colAttachedTo})
        Me.lvwList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwList.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lvwList.FullRowSelect = True
        Me.lvwList.HideSelection = False
        Me.lvwList.Location = New System.Drawing.Point(0, 0)
        Me.lvwList.Name = "lvwList"
        Me.lvwList.OwnerDraw = True
        Me.lvwList.ShowItemToolTips = True
        Me.lvwList.Size = New System.Drawing.Size(822, 561)
        Me.lvwList.SmallImageList = Me.ImageList1
        Me.lvwList.TabIndex = 0
        Me.lvwList.UseCompatibleStateImageBehavior = False
        Me.lvwList.View = System.Windows.Forms.View.Details
        '
        'colStatus
        '
        Me.colStatus.Text = "Save"
        Me.colStatus.Width = 56
        '
        'colFrom
        '
        Me.colFrom.Text = "From"
        Me.colFrom.Width = 176
        '
        'colSubject
        '
        Me.colSubject.Text = "Subject"
        Me.colSubject.Width = 300
        '
        'colSaveTo
        '
        Me.colSaveTo.Text = "Save To"
        Me.colSaveTo.Width = 140
        '
        'colAttachedTo
        '
        Me.colAttachedTo.Text = "Attached To"
        Me.colAttachedTo.Width = 140
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        '
        'formStorage
        '
        Me.formStorage.FormToStore = Me
        Me.formStorage.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.formStorage.State = False
        Me.formStorage.UniqueID = "Forms\\MultipleEmailSaving"
        Me.formStorage.Version = CType(0, Long)
        '
        'txtPreview
        '
        Me.txtPreview.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtPreview.Location = New System.Drawing.Point(0, 381)
        Me.txtPreview.Name = "txtPreview"
        Me.txtPreview.ReadOnly = True
        Me.txtPreview.Size = New System.Drawing.Size(822, 180)
        Me.txtPreview.TabIndex = 2
        Me.txtPreview.Text = ""
        Me.txtPreview.Visible = False
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.chkSkipTime)
        Me.pnlButtons.Controls.Add(Me.chkQuickSave)
        Me.pnlButtons.Controls.Add(Me.btnProcess)
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnChange)
        Me.pnlButtons.Controls.Add(Me.chkPreview)
        Me.pnlButtons.Controls.Add(Me.lblRedDraft)
        Me.pnlButtons.Controls.Add(Me.lblDeleting)
        Me.pnlButtons.Controls.Add(Me.picDeleting)
        Me.pnlButtons.Controls.Add(Me.lblNormalSaved)
        Me.pnlButtons.Controls.Add(Me.lblBoldUnsaved)
        Me.pnlButtons.Controls.Add(Me.lblCancel)
        Me.pnlButtons.Controls.Add(Me.picCancel)
        Me.pnlButtons.Controls.Add(Me.lblError)
        Me.pnlButtons.Controls.Add(Me.picError)
        Me.pnlButtons.Controls.Add(Me.lblMoving)
        Me.pnlButtons.Controls.Add(Me.picMoving)
        Me.pnlButtons.Controls.Add(Me.lblDel)
        Me.pnlButtons.Controls.Add(Me.picDel)
        Me.pnlButtons.Controls.Add(Me.lblSave)
        Me.pnlButtons.Controls.Add(Me.picSave)
        Me.pnlButtons.Controls.Add(Me.lblMove)
        Me.pnlButtons.Controls.Add(Me.picMove)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlButtons.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.pnlButtons.Location = New System.Drawing.Point(822, 0)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(112, 561)
        Me.pnlButtons.TabIndex = 3
        '
        'chkSkipTime
        '
        Me.chkSkipTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSkipTime.Location = New System.Drawing.Point(10, 440)
        Me.ResourceLookup1.SetLookup(Me.chkSkipTime, New FWBS.OMS.UI.Windows.ResourceLookupItem("SKIPTIME", "S&kip Time", ""))
        Me.chkSkipTime.Name = "chkSkipTime"
        Me.chkSkipTime.Size = New System.Drawing.Size(96, 20)
        Me.chkSkipTime.TabIndex = 22
        Me.chkSkipTime.Text = "S&kip Time"
        Me.chkSkipTime.UseVisualStyleBackColor = True
        '
        'chkQuickSave
        '
        Me.chkQuickSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkQuickSave.Location = New System.Drawing.Point(10, 465)
        Me.ResourceLookup1.SetLookup(Me.chkQuickSave, New FWBS.OMS.UI.Windows.ResourceLookupItem("QUICKSAVE", "Q&uick Save", ""))
        Me.chkQuickSave.Name = "chkQuickSave"
        Me.chkQuickSave.Size = New System.Drawing.Size(96, 20)
        Me.chkQuickSave.TabIndex = 21
        Me.chkQuickSave.Text = "Q&uick Save"
        Me.chkQuickSave.UseVisualStyleBackColor = True
        '
        'btnProcess
        '
        Me.btnProcess.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnProcess.Location = New System.Drawing.Point(8, 8)
        Me.ResourceLookup1.SetLookup(Me.btnProcess, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnProcess", "&Process", ""))
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(96, 25)
        Me.btnProcess.TabIndex = 0
        Me.btnProcess.Text = "&Process"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnCancel.Location = New System.Drawing.Point(8, 39)
        Me.ResourceLookup1.SetLookup(Me.btnCancel, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "&Cancel", ""))
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(96, 25)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "&Cancel"
        '
        'btnChange
        '
        Me.btnChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnChange.Enabled = False
        Me.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnChange.Location = New System.Drawing.Point(8, 497)
        Me.ResourceLookup1.SetLookup(Me.btnChange, New FWBS.OMS.UI.Windows.ResourceLookupItem("btnChange", "C&hange...", ""))
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(96, 25)
        Me.btnChange.TabIndex = 3
        Me.btnChange.Text = "C&hange..."
        '
        'chkPreview
        '
        Me.chkPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkPreview.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkPreview.Enabled = False
        Me.chkPreview.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkPreview.Location = New System.Drawing.Point(8, 528)
        Me.ResourceLookup1.SetLookup(Me.chkPreview, New FWBS.OMS.UI.Windows.ResourceLookupItem("chkPreview", "P&review", ""))
        Me.chkPreview.Name = "chkPreview"
        Me.chkPreview.Size = New System.Drawing.Size(96, 25)
        Me.chkPreview.TabIndex = 2
        Me.chkPreview.Text = "P&review"
        Me.chkPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRedDraft
        '
        Me.lblRedDraft.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
        Me.lblRedDraft.ForeColor = System.Drawing.Color.Red
        Me.lblRedDraft.Location = New System.Drawing.Point(8, 343)
        Me.ResourceLookup1.SetLookup(Me.lblRedDraft, New FWBS.OMS.UI.Windows.ResourceLookupItem("LBLREDDRAFT", "Red = Draft", ""))
        Me.lblRedDraft.Name = "lblRedDraft"
        Me.lblRedDraft.Size = New System.Drawing.Size(96, 24)
        Me.lblRedDraft.TabIndex = 20
        Me.lblRedDraft.Text = "Red = Draft"
        '
        'lblDeleting
        '
        Me.lblDeleting.Location = New System.Drawing.Point(30, 145)
        Me.ResourceLookup1.SetLookup(Me.lblDeleting, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblDeleting", "Deleting", ""))
        Me.lblDeleting.Name = "lblDeleting"
        Me.lblDeleting.Size = New System.Drawing.Size(74, 18)
        Me.lblDeleting.TabIndex = 19
        Me.lblDeleting.Text = "Deleting"
        '
        'picDeleting
        '
        Me.picDeleting.Location = New System.Drawing.Point(10, 146)
        Me.picDeleting.Name = "picDeleting"
        Me.picDeleting.Size = New System.Drawing.Size(16, 16)
        Me.picDeleting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picDeleting.TabIndex = 18
        Me.picDeleting.TabStop = False
        '
        'lblNormalSaved
        '
        Me.lblNormalSaved.Location = New System.Drawing.Point(8, 318)
        Me.ResourceLookup1.SetLookup(Me.lblNormalSaved, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblNormalSaved", "Normal = Saved", ""))
        Me.lblNormalSaved.Name = "lblNormalSaved"
        Me.lblNormalSaved.Size = New System.Drawing.Size(96, 24)
        Me.lblNormalSaved.TabIndex = 15
        Me.lblNormalSaved.Text = "Normal = Saved"
        '
        'lblBoldUnsaved
        '
        Me.lblBoldUnsaved.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblBoldUnsaved.Location = New System.Drawing.Point(8, 293)
        Me.ResourceLookup1.SetLookup(Me.lblBoldUnsaved, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblBoldUnsaved", "Bold = Unsaved", ""))
        Me.lblBoldUnsaved.Name = "lblBoldUnsaved"
        Me.lblBoldUnsaved.Size = New System.Drawing.Size(96, 24)
        Me.lblBoldUnsaved.TabIndex = 14
        Me.lblBoldUnsaved.Text = "Bold = Unsaved"
        '
        'lblCancel
        '
        Me.lblCancel.Location = New System.Drawing.Point(30, 245)
        Me.ResourceLookup1.SetLookup(Me.lblCancel, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblCancel", "Cancel", ""))
        Me.lblCancel.Name = "lblCancel"
        Me.lblCancel.Size = New System.Drawing.Size(74, 18)
        Me.lblCancel.TabIndex = 13
        Me.lblCancel.Text = "Cancel"
        '
        'picCancel
        '
        Me.picCancel.Location = New System.Drawing.Point(10, 246)
        Me.picCancel.Name = "picCancel"
        Me.picCancel.Size = New System.Drawing.Size(16, 16)
        Me.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picCancel.TabIndex = 12
        Me.picCancel.TabStop = False
        '
        'lblError
        '
        Me.lblError.Location = New System.Drawing.Point(30, 220)
        Me.ResourceLookup1.SetLookup(Me.lblError, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblError", "Error", ""))
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(74, 18)
        Me.lblError.TabIndex = 11
        Me.lblError.Text = "Error"
        '
        'picError
        '
        Me.picError.Location = New System.Drawing.Point(10, 221)
        Me.picError.Name = "picError"
        Me.picError.Size = New System.Drawing.Size(16, 16)
        Me.picError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picError.TabIndex = 10
        Me.picError.TabStop = False
        '
        'lblMoving
        '
        Me.lblMoving.Location = New System.Drawing.Point(30, 145)
        Me.ResourceLookup1.SetLookup(Me.lblMoving, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblMoving", "Moving", ""))
        Me.lblMoving.Name = "lblMoving"
        Me.lblMoving.Size = New System.Drawing.Size(74, 18)
        Me.lblMoving.TabIndex = 9
        Me.lblMoving.Text = "Moving"
        '
        'picMoving
        '
        Me.picMoving.Location = New System.Drawing.Point(10, 146)
        Me.picMoving.Name = "picMoving"
        Me.picMoving.Size = New System.Drawing.Size(16, 16)
        Me.picMoving.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picMoving.TabIndex = 8
        Me.picMoving.TabStop = False
        '
        'lblDel
        '
        Me.lblDel.Location = New System.Drawing.Point(30, 170)
        Me.ResourceLookup1.SetLookup(Me.lblDel, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblDel", "Deleted", ""))
        Me.lblDel.Name = "lblDel"
        Me.lblDel.Size = New System.Drawing.Size(74, 18)
        Me.lblDel.TabIndex = 7
        Me.lblDel.Text = "Deleted"
        '
        'picDel
        '
        Me.picDel.Location = New System.Drawing.Point(10, 171)
        Me.picDel.Name = "picDel"
        Me.picDel.Size = New System.Drawing.Size(16, 16)
        Me.picDel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picDel.TabIndex = 6
        Me.picDel.TabStop = False
        '
        'lblSave
        '
        Me.lblSave.Location = New System.Drawing.Point(30, 120)
        Me.ResourceLookup1.SetLookup(Me.lblSave, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblSave", "Saving", ""))
        Me.lblSave.Name = "lblSave"
        Me.lblSave.Size = New System.Drawing.Size(74, 18)
        Me.lblSave.TabIndex = 5
        Me.lblSave.Text = "Saving"
        '
        'picSave
        '
        Me.picSave.Location = New System.Drawing.Point(10, 121)
        Me.picSave.Name = "picSave"
        Me.picSave.Size = New System.Drawing.Size(16, 16)
        Me.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picSave.TabIndex = 4
        Me.picSave.TabStop = False
        '
        'lblMove
        '
        Me.lblMove.Location = New System.Drawing.Point(30, 170)
        Me.ResourceLookup1.SetLookup(Me.lblMove, New FWBS.OMS.UI.Windows.ResourceLookupItem("lblMove", "Moved", ""))
        Me.lblMove.Name = "lblMove"
        Me.lblMove.Size = New System.Drawing.Size(74, 18)
        Me.lblMove.TabIndex = 17
        Me.lblMove.Text = "Moved"
        '
        'picMove
        '
        Me.picMove.Location = New System.Drawing.Point(10, 171)
        Me.picMove.Name = "picMove"
        Me.picMove.Size = New System.Drawing.Size(16, 16)
        Me.picMove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picMove.TabIndex = 16
        Me.picMove.TabStop = False
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Splitter1.Location = New System.Drawing.Point(0, 378)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(822, 3)
        Me.Splitter1.TabIndex = 4
        Me.Splitter1.TabStop = False
        '
        'frmEmailSelector
        '
        Me.AcceptButton = Me.btnProcess
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(934, 561)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.txtPreview)
        Me.Controls.Add(Me.lvwList)
        Me.Controls.Add(Me.pnlButtons)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.ResourceLookup1.SetLookup(Me, New FWBS.OMS.UI.Windows.ResourceLookupItem("frmEmailSel", "Multiple Email Saving", ""))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(800, 550)
        Me.Name = "frmEmailSelector"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Multiple Email Saving"
        Me.pnlButtons.ResumeLayout(False)
        CType(Me.picDeleting, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picError, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picMoving, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picDel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSave, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picMove, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region




    Public Sub New(ByVal sel As IEnumerable(Of OutlookItem), ByVal oms As OutlookOMS, ByVal associate As Associate, ByVal selectionType As EmailSelectionType, Optional ByVal folder As Outlook.MAPIFolder = Nothing)
        Me.New()

        _sel = sel
        _oms = oms
        _seltype = selectionType
        _assoc = associate
        _folder = folder

        chkSkipTime.Visible = Session.CurrentSession.IsPackageInstalled("TIMERECORDING") AndAlso Session.CurrentSession.IsLicensedFor("TIMEREC")
        chkQuickSave.Checked = oms.CheckEmailOption(EmailOption.optQuickSave)

        Dim file_info As String = GetAssocInfo(_assoc)
        Dim associd As Long = _assoc.ID
        Dim progress As Boolean = False

        Try
            Using (oms.App.BeginProcess())

                'Client and Matter Status check
                Dim targetAssoc As FWBS.OMS.Associate = FWBS.OMS.Associate.GetAssociate(associd)
                Dim docModifyActivity As FWBS.OMS.StatusManagement.FileActivity = New FWBS.OMS.StatusManagement.FileActivity(targetAssoc.OMSFile, FWBS.OMS.StatusManagement.Activities.FileStatusActivityType.DocumentModification)
                docModifyActivity.Check()

                Using items As DetachableItems = New DetachableItems(oms.App, _sel)

                    Dim count As Integer = items.Count
                    If count > OutlookOMS.PROGRESS_AMOUNT Then progress = True

                    If (progress) Then _oms.OnProgressStart(Session.CurrentSession.Resources.GetResource("CALCULATING", "Calculating", "").Text,
                                                            Session.CurrentSession.Resources.GetResource("CALCTOTALSAVE", "Calculating the number of items to save.", "").Text,
                                                            count)

                    Dim ctr As Integer = 0
                    For Each itm As OutlookItem In items
                        ctr += 1

                        If (progress) Then _oms.OnProgress(Session.CurrentSession.Resources.GetResource("CALCITEMS", "Calculating %1%/%2% Items.", "", ctr.ToString(), count.ToString()).Text, ctr)

                        If Not _oms.CanSaveItemAsDocument(itm) Then
                            _manual.Add(itm)
                            Continue For
                        End If


                        Dim litem As System.Windows.Forms.ListViewItem = lvwList.Items.Add("", Icons.Email)
                        litem.SubItems.Add(itm.SenderName)


                        litem.SubItems.Add(itm.Subject)
                        litem.Tag = itm

                        Dim targetinfoitem As System.Windows.Forms.ListViewItem.ListViewSubItem = litem.SubItems.Add(file_info)
                        Dim sourceinfoitem As System.Windows.Forms.ListViewItem.ListViewSubItem = litem.SubItems.Add("")
                        Dim targetiditem As System.Windows.Forms.ListViewItem.ListViewSubItem = litem.SubItems.Add(associd.ToString())
                        Dim sourceiditem As System.Windows.Forms.ListViewItem.ListViewSubItem = litem.SubItems.Add("")

                        Dim assoc As Associate = _oms.GetCurrentAssociate(itm)
                        litem.Checked = False


                        If Not assoc Is Nothing Then
                            sourceinfoitem.Text = GetAssocInfo(assoc)
                            sourceiditem.Text = assoc.ID.ToString()
                            litem.ToolTipText = Session.CurrentSession.Resources.GetResource("ALREADYSAVEDTO", "Already saved to %1%", "", sourceinfoitem.Text).Text
                        End If

                        If Not _oms.IsStoredDocument(itm) Then
                            If itm.IsDraft = True Then
                                litem.Checked = False
                                litem.Font = New System.Drawing.Font(litem.Font, litem.Font.Style Or Drawing.FontStyle.Italic)
                                litem.ForeColor = Drawing.Color.Red
                            Else
                                litem.Checked = True
                            End If

                            litem.Font = New System.Drawing.Font(litem.Font, litem.Font.Style Or Drawing.FontStyle.Bold)

                            Continue For
                        Else
                            targetinfoitem.Text = ""
                            targetiditem.Text = ""
                        End If



                    Next
                End Using
                checkSelectAll = (lvwList.Items.Count = lvwList.CheckedItems.Count)
            End Using
        Finally
            If (progress) Then _oms.OnProgressFinished()
        End Try

    End Sub

#End Region

#Region "Events"

    Private Sub frmEmailSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If DeviceDpi <> 96 Then
            lvwList.SmallImageList = Images.ScaleList(Me.ImageList1, LogicalToDeviceUnits(Me.ImageList1.ImageSize))
        End If
        lvwList.Columns(0).Text = " " & Session.CurrentSession.Resources.GetResource("Save", "Save", "").Text
        lvwList.Columns(1).Text = Session.CurrentSession.Resources.GetResource("From", "From", "").Text
        lvwList.Columns(2).Text = Session.CurrentSession.Resources.GetResource("Subject", "Subject", "").Text
        lvwList.Columns(3).Text = Session.CurrentSession.Resources.GetResource("SaveTo", "Save To", "").Text
        lvwList.Columns(4).Text = Session.CurrentSession.Resources.GetResource("AttachedTo", "Attached To", "").Text

        picCancel.Image = ImageList1.Images(Icons.Cancelled)
        picDel.Image = ImageList1.Images(Icons.Deleted)
        picError.Image = ImageList1.Images(Icons.Err)
        picDeleting.Image = ImageList1.Images(Icons.Processing)
        picSave.Image = ImageList1.Images(Icons.Saving)
        picMove.Image = ImageList1.Images(Icons.Moved)
        picMoving.Image = ImageList1.Images(Icons.Processing)

        If _seltype = EmailSelectionType.Delete Then
            picMove.Visible = False
            lblMove.Visible = False
            picMoving.Visible = False
            lblMoving.Visible = False
        ElseIf _seltype = EmailSelectionType.Move Then
            picDel.Visible = False
            lblDel.Visible = False
            picDeleting.Visible = False
            lblDeleting.Visible = False
        ElseIf _seltype = EmailSelectionType.Save Then
            Select Case _oms.CheckEmailSaveLocationOption(Nothing)
                Case EmailSaveLocation.Delete
                    picMove.Visible = False
                    lblMove.Visible = False
                    picMoving.Visible = False
                    lblMoving.Visible = False
                Case EmailSaveLocation.Leave
                    picDel.Visible = False
                    lblDel.Visible = False
                    picDeleting.Visible = False
                    lblDeleting.Visible = False
                    picMove.Visible = False
                    lblMove.Visible = False
                    picMoving.Visible = False
                    lblMoving.Visible = False
                Case EmailSaveLocation.Move
                    picDel.Visible = False
                    lblDel.Visible = False
                    picDeleting.Visible = False
                    lblDeleting.Visible = False
            End Select
        End If

        If String.IsNullOrEmpty(FWBS.OMS.Session.CurrentSession.GetHelpPath("frmEmailSelector")) Then
            Me.HelpButton = False
        End If

    End Sub

    Private Sub lvwList_DrawColumnHeader(sender As Object, e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvwList.DrawColumnHeader
        If e.ColumnIndex = colStatus.Index Then
            Dim checkBoxSize As Integer = LogicalToDeviceUnits(13)
            e.DrawBackground()
            ControlPaint.DrawCheckBox(e.Graphics, LogicalToDeviceUnits(3), (e.Bounds.Height - checkBoxSize) / 2, checkBoxSize, checkBoxSize, IIf(checkSelectAll, ButtonState.Checked, ButtonState.Normal))
            e.DrawText(TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or TextFormatFlags.LeftAndRightPadding)
        Else
            e.DrawDefault = True
        End If
    End Sub

    Private Sub lvwList_DrawItem(sender As Object, e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvwList.DrawItem
        e.DrawDefault = True
    End Sub

    Private Sub lvwList_DrawSubItem(sender As Object, e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvwList.DrawSubItem
        e.DrawDefault = True
    End Sub

    Private Sub lvwList_DpiChangedAfterParent(sender As Object, e As EventArgs) Handles lvwList.DpiChangedAfterParent
        lvwList.SmallImageList = Nothing
        lvwList.SmallImageList = If(lvwList.DeviceDpi = 96, Me.ImageList1, Images.ScaleList(Me.ImageList1, lvwList.LogicalToDeviceUnits(Me.ImageList1.ImageSize)))
    End Sub

    Private Sub lvwList_FontChanged(sender As Object, e As EventArgs) Handles lvwList.FontChanged
        For Each litem As ListViewItem In lvwList.Items
            litem.Font = New Drawing.Font(lvwList.Font, litem.Font.Style)
        Next
    End Sub

    Private Sub lvwList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwList.SelectedIndexChanged

        Try
            Dim selCount As Integer = lvwList.SelectedItems.Count
            btnChange.Enabled = selCount > 0
            chkPreview.Enabled = selCount = 1
            If (selCount > 1) Then
                chkPreview.Checked = False
            End If

            If selCount = 0 OrElse Not txtPreview.Visible Then
                txtPreview.Text = String.Empty
                Return
            End If
            Dim itm As OutlookItem = lvwList.SelectedItems(0).Tag
            DisplayPreview(itm)

        Catch
        End Try
    End Sub

    Private Sub DisplayPreview(ByVal itm As OutlookItem)

        Dim wasAttached As Boolean

        If (itm.IsDetached) Then
            itm.Attach()
            wasAttached = False
        End If

        If (Not itm Is Nothing And itm.BodyFormat = Outlook.OlBodyFormat.olFormatRichText) Then
            Try
                txtPreview.Rtf = itm.HTMLBody
            Catch ex As Exception
                txtPreview.Text = itm.Body
            End Try
        Else
            txtPreview.Text = itm.Body
        End If

        If (wasAttached = False) Then
            If Not itm Is Nothing Then
                itm.Detach()
            End If
        End If


    End Sub

    Private Sub chkPreview_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPreview.CheckedChanged
        If Not chkPreview.Checked Then
            txtPreview.Visible = False
            Return
        End If


        If (lvwList.SelectedItems.Count <= 0) Then
            Return
        End If
        txtPreview.Visible = True
        DisplayPreview(lvwList.SelectedItems(0).Tag)


    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Try
            Cursor = System.Windows.Forms.Cursors.WaitCursor
            If lvwList.SelectedItems.Count <= 0 Then
                Return
            End If
            Dim assoc As Associate = _oms.SelectAssociate(Me)
            If assoc Is Nothing Then
                Return
            End If

            Dim docModifyActivity As FWBS.OMS.StatusManagement.FileActivity = New FWBS.OMS.StatusManagement.FileActivity(assoc.OMSFile, FWBS.OMS.StatusManagement.Activities.FileStatusActivityType.DocumentModification)
            docModifyActivity.Check()

            Dim info As String = GetAssocInfo(assoc)
            Dim associd As String = assoc.ID.ToString()
            For Each litem As System.Windows.Forms.ListViewItem In lvwList.SelectedItems
                litem.Checked = True
                litem.SubItems(Columns.TargetAssocInfo).Text = info
                litem.SubItems(Columns.TargetAssocID).Text = associd
            Next

        Finally
            Cursor = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Function GetAssocInfo(ByVal assoc As Associate) As String
        Return assoc.OMSFile.ToString() & " (" & assoc.ToString() & ")"
    End Function

    Private Sub Item_CheckedChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwList.ItemChecked

        If (e.Item.Checked) Then
            e.Item.SubItems(Columns.TargetAssocID).Text = _assoc.ID.ToString()
            e.Item.SubItems(Columns.TargetAssocInfo).Text = GetAssocInfo(_assoc)
        Else
            e.Item.SubItems(Columns.TargetAssocID).Text = ""
            e.Item.SubItems(Columns.TargetAssocInfo).Text = ""
        End If

    End Sub

    Private Sub lvwList_ColumnClick(sender As Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwList.ColumnClick
        If e.Column = colStatus.Index Then
            checkSelectAll = Not checkSelectAll

            For Each litem As System.Windows.Forms.ListViewItem In lvwList.Items
                litem.Checked = checkSelectAll
            Next
        End If
    End Sub

    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Dim ctr As Integer = 0
        Dim skipTime As Boolean = chkSkipTime.Checked
        Dim quick As Boolean = chkQuickSave.Checked

        Try

            Using (_oms.App.BeginProcess())

                Cursor = System.Windows.Forms.Cursors.WaitCursor
                processing = True
                btnProcess.Enabled = False
                btnChange.Enabled = False
                chkPreview.Enabled = False
                chkSkipTime.Enabled = False
                chkQuickSave.Enabled = False

                If (quick) Then
                    _oms.OnProgressStart(Me.Text, Session.CurrentSession.Resources.GetResource("SHELL_6", "Saving selected documents...", "").Text, ItemsCount)
                    System.Windows.Forms.Application.DoEvents()
                End If

                For Each litem As System.Windows.Forms.ListViewItem In lvwList.Items

                    If cancel Then
                        _manual.Clear()
                        _cancelled.Clear()
                        If (quick) Then _oms.OnProgressFinished()
                        processing = False
                        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                        Close()
                        Return
                    End If

                    If (quick) Then _oms.OnProgress(litem.SubItems(Columns.Subject).Text, ctr)
                    litem.ImageIndex = Icons.Processing
                    System.Windows.Forms.Application.DoEvents()

                    Try

                        Dim itm As OutlookItem = litem.Tag

                        Try

                            itm.Attach()

                            System.Windows.Forms.Application.DoEvents()

                            Dim success As Boolean = False

                            litem.EnsureVisible()

                            If litem.Checked Then

                                Dim targetAssocID As Long = FWBS.Common.ConvertDef.ToInt64(litem.SubItems(Columns.TargetAssocID).Text, -1)
                                Dim sourceAssocID As Long = FWBS.Common.ConvertDef.ToInt64(litem.SubItems(Columns.SourceAssocID).Text, -1)

                                litem.ImageIndex = Icons.Saving

                                System.Windows.Forms.Application.DoEvents()

                                Dim saveSettings As SaveSettings = SaveSettings.Default
                                saveSettings.UseDefaultAssociate = _oms.CheckEmailOption(EmailOption.optUseDefAssoc)
                                saveSettings.TargetAssociate = Associate.GetAssociate(targetAssocID)
                                saveSettings.AllowRelink = False
                                saveSettings.SkipTimeRecords = skipTime
                                If quick Then saveSettings.Mode = PrecSaveMode.Quick

                                If _seltype = EmailSelectionType.Save AndAlso sourceAssocID <> -1 AndAlso sourceAssocID <> targetAssocID Then
                                    success = _oms.SaveAs(itm, False, saveSettings, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
                                Else
                                    _oms.AttachDocumentVars(itm, saveSettings.UseDefaultAssociate, saveSettings.TargetAssociate)
                                    success = _oms.Save(itm, saveSettings, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
                                End If

                                If success Then
                                    litem.ImageIndex = Icons.Processing
                                Else
                                    litem.ImageIndex = Icons.Cancelled
                                    _cancelled.Add(itm)
                                End If
                                If success Then
                                    Try
                                        If _seltype = EmailSelectionType.Delete Then
                                            _oms.SetDocVariable(itm, OutlookOMS.PROFILE, False)
                                            OutlookOMS.DeleteItem(itm)
                                        ElseIf _seltype = EmailSelectionType.Move Then
                                            If targetAssocID = _assoc.ID Then
                                                OutlookOMS.MoveItem(itm, _folder)
                                            Else
                                                _oms.RunCommand(itm, "OMS;FILEIT")
                                            End If
                                        ElseIf _seltype = EmailSelectionType.Save Then
                                            _oms.MoveItem(itm)
                                        End If
                                        SetSuccessIcon(litem)
                                    Catch ex As Exception
                                        _oms.WriteLog("Error Moving Item", " There was an error moving the item after saving during the the bulk profiling process", "Refer to the exception information below", ex)
                                        litem.ImageIndex = Icons.Err
                                    End Try
                                End If
                            Else

                                litem.ImageIndex = Icons.Processing

                                System.Windows.Forms.Application.DoEvents()

                                Try
                                    If _seltype = EmailSelectionType.Delete Then
                                        _oms.SetDocVariable(itm, OutlookOMS.PROFILE, False)
                                        OutlookOMS.DeleteItem(itm)
                                    ElseIf _seltype = EmailSelectionType.Move Then
                                        OutlookOMS.MoveItem(itm, _folder)
                                    ElseIf _seltype = EmailSelectionType.Save Then
                                        'Do nothing
                                    End If
                                    SetSuccessIcon(litem)
                                Catch ex As Exception
                                    _oms.WriteLog("Error Moving Item", " There was an error moving the item after saving during the the bulk profiling process", "Refer to the exception information below", ex)
                                    litem.ImageIndex = Icons.Err
                                End Try
                            End If
                        Finally
                            itm.Detach()
                        End Try

                    Catch ex As Exception
                        _oms.WriteLog("Bulk Profiling Error", " There was an error when bulk profiling", "Refer to the exception information below", ex)
                        litem.ImageIndex = Icons.Err
                    End Try

                    ctr += 1
                    If (quick) Then _oms.OnProgress(litem.SubItems(Columns.Subject).Text, ctr)
                    System.Windows.Forms.Application.DoEvents()
                Next

                If (quick) Then _oms.OnProgressFinished()
            End Using

            processing = False
            System.Windows.Forms.Application.DoEvents()
            DialogResult = System.Windows.Forms.DialogResult.OK
            Close()

        Finally
            processing = False
            Cursor = System.Windows.Forms.Cursors.Default
            btnProcess.Enabled = True
            btnChange.Enabled = True
            chkPreview.Enabled = True
            chkSkipTime.Enabled = True
            chkQuickSave.Enabled = True
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'CM: This will call the OnFormClosed method as linked to DialogResult.Cancel
    End Sub

    Private Sub frmEmailSelector_HelpButtonClicked(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.HelpButtonClicked
        Dim startInfo As ProcessStartInfo = New System.Diagnostics.ProcessStartInfo(FWBS.OMS.Session.CurrentSession.GetHelpPath("frmEmailSelector"))
        If String.IsNullOrEmpty(startInfo.Arguments) Then
            Dim process As System.Diagnostics.Process = New System.Diagnostics.Process()
            process.StartInfo = startInfo
            process.Start()
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub SetSuccessIcon(ByVal litem As System.Windows.Forms.ListViewItem)
        If _seltype = EmailSelectionType.Delete Then
            litem.ImageIndex = Icons.Deleted
            litem.Font = New System.Drawing.Font(litem.Font, litem.Font.Style Or Drawing.FontStyle.Strikeout)
        ElseIf _seltype = EmailSelectionType.Move Then
            litem.ImageIndex = Icons.Moved
        ElseIf _seltype = EmailSelectionType.Save Then
            If Not litem.Checked Then
                litem.ImageIndex = Icons.Email
                Return
            End If
            Select Case _oms.CheckEmailSaveLocationOption(litem.Tag)
                Case EmailSaveLocation.Delete
                    litem.ImageIndex = Icons.Deleted
                    litem.Font = New System.Drawing.Font(litem.Font, litem.Font.Style Or Drawing.FontStyle.Strikeout)
                Case EmailSaveLocation.Leave
                    litem.ImageIndex = Icons.Success
                Case EmailSaveLocation.Move
                    litem.ImageIndex = Icons.Moved
            End Select
        End If
    End Sub

    Private Sub CancelMultipleSaveWizard()
        If processing Then
            If MessageBox.ShowYesNoQuestion("RUSURECANCEL", "Are you sure that you wish to cancel?") = System.Windows.Forms.DialogResult.Yes Then
                cancel = True
            End If
        End If
    End Sub

    Protected Overrides Sub OnFormClosing(e As System.Windows.Forms.FormClosingEventArgs)
        CancelMultipleSaveWizard()
        e.Cancel = processing
        MyBase.OnFormClosing(e)
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property ItemsCount() As Integer
        Get
            Return lvwList.Items.Count
        End Get
    End Property

    Public ReadOnly Property CancelledItems() As IEnumerable(Of OutlookItem)
        Get
            Return _cancelled
        End Get
    End Property

    Public ReadOnly Property ManualItems() As IEnumerable(Of OutlookItem)
        Get
            Return _manual
        End Get
    End Property

#End Region



End Class
