Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports FWBS.OMS.Apps
Imports Microsoft.Office.Core

<System.Runtime.InteropServices.ComVisible(False)> _
Public MustInherit Class OfficeOMSApp
    Inherits OMSApp
    Implements IDisposable


#Region "Fields"

    Protected Friend _omsbar As Object
    Protected Friend Shared _omsbarctrls As CommandBarControl()

    Protected Friend _timebar As Object
    Protected Friend Shared _timebarctrls As CommandBarControl()

    Protected Friend _precbar As Object
    Protected Friend Shared _precbarctrls As CommandBarControl()

    Protected menuScripts As OMS.Script.MenuScriptAggregator

    Private _addinInst As Boolean = False
    Private _useCommandBars As Boolean = True
    Private _code As String
    Public ReadOnly _dpiAwareness As DPIAwareness.DPI_AWARENESS = DPIAwareness.DPI_AWARENESS.UNAWARE


#End Region

#Region "Constructors"
    Public Sub New()
    End Sub

    Public Sub New(ByVal code As String, ByVal useCommandBars As Boolean, Optional ByVal addinInstance As Boolean = True)
        _useCommandBars = useCommandBars
        _addinInst = addinInstance
        _code = code

        If addinInstance Then
            If DPIAwareness.IsSupported Then
                _dpiAwareness = DPIAwareness.FromWindow(Process.GetCurrentProcess().MainWindowHandle)
            End If
            AppDomain.CurrentDomain.SetData("DPI_AWARENESS", _dpiAwareness)
        End If
    End Sub

    Protected Sub AddinInitialisation()

        CheckVersionCompatibility()

        AddHandler Session.CurrentSession.Connected, AddressOf _Connected
        AddHandler Session.CurrentSession.Disconnected, AddressOf Disconnected

        'Build the default bars and command bars if needed.
        BuildDefaultCommandBar()

        BuildCommandBars(True)
    End Sub

    Private Sub CheckVersionCompatibility()
        Dim version As Integer = ApplicationVersion

        If (version < 12) Then
            Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "Data", "DisableOfficeVersionCheck", "False")
            If reg.ToBoolean() = False Then
                MsgBox(Session.CurrentSession.Resources.GetMessage("ADDFR07USD", "You are using a %1% Office Addin that was designed for Microsoft Office 2007.  You may continue using the addin but some problems may occur.", "", FWBS.OMS.Global.ApplicationName).Text, MsgBoxStyle.Exclamation, FWBS.OMS.Global.ApplicationName)
            End If
        End If
    End Sub
#End Region

#Region "Command Bar Building"

    Public Sub RefreshRibbon()
        Dim ribbonx As Object

        Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "OMSAddinCOM", "OMSOFFICE2007")

        Try
            ribbonx = Application.ComAddins.Item(reg.ToString()).Object

            If Not ribbonx Is Nothing Then
                If (TypeOf Application Is FWBS.Office.OfficeObject) Then
                    ribbonx.RefreshUI(Application.Unwrap())
                Else
                    ribbonx.RefreshUI(Application)
                End If
            End If
        Catch comex As System.Runtime.InteropServices.COMException
        End Try
    End Sub

    Public ReadOnly Property UseCommandBars() As Boolean
        Get
            Return _useCommandBars
        End Get
    End Property

    Public ReadOnly Property MergeMenus() As Boolean
        Get
            Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "UI\Tweaks", "MergeMenus", "False")
            Return reg.ToBoolean()
        End Get
    End Property


    Public ReadOnly Property PrecedentMenuVisible() As Boolean
        Get
            Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "UI\CommandBars", CBM.PRECEDENT_COMMAND_BAR_CODE, "False")
            Return reg.ToBoolean()
        End Get
    End Property

    Public ReadOnly Property TimeRecMenuVisible() As Boolean
        Get
            Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "UI\CommandBars", CBM.TIME_COMMAND_BAR_CODE, "True")
            Return reg.ToBoolean()
        End Get
    End Property

    Public ReadOnly Property MainMenuVisible() As Boolean
        Get
            Dim reg As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "UI\CommandBars", CBM.MAIN_COMMAND_BAR_CODE, "True")
            Return reg.ToBoolean()
        End Get
    End Property


    Protected MustOverride ReadOnly Property CommandBarContainer() As CommandBars

    Protected MustOverride ReadOnly Property Application() As Object

	Public MustOverride ReadOnly Property ApplicationName() As String

	Public MustOverride ReadOnly Property ApplicationVersion() As Integer

	Protected Friend Sub BuildDefaultCommandBar()
        BuildDefaultCommandBar(CommandBarContainer)
    End Sub

    Protected Friend Sub BuildDefaultCommandBar(ByVal cmdBars As CommandBars)

        If (cmdBars Is Nothing) Then Return
        If UseCommandBars Then


            Try
                Dim merge As Boolean = MergeMenus

                If merge Then
                    Dim menu As Microsoft.Office.Core.CommandBar = Nothing
                    For Each cb As Microsoft.Office.Core.CommandBar In cmdBars
                        If cb.Type = MsoBarType.msoBarTypeMenuBar Then
                            menu = cb
                            Exit For
                        End If
                    Next
                    If Not menu Is Nothing Then
                        Try
                            _omsbar = menu.Controls(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME))
                        Catch
                        End Try
                        If (_omsbar Is Nothing) Then
                            _omsbar = menu.Controls.Add(MsoControlType.msoControlPopup, , , , True)
                            _omsbar.Caption = CBM.MAIN_COMMAND_BAR_NAME
                            _omsbar.Visible = True
                        End If

                        Try
                            _timebar = cmdBars.Item(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME))
                        Catch
                        End Try
                        If (_timebar Is Nothing) Then
                            _timebar = menu.Controls.Add(MsoControlType.msoControlPopup, , , , True)
                            _timebar.Caption = CBM.TIME_COMMAND_BAR_NAME
                            ReDim _timebarctrls(0)
                            _timebar.Visible = False
                        End If

                        Try
                            _precbar = cmdBars.Item(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME))
                        Catch
                        End Try
                        If (_precbar Is Nothing) Then
                            _precbar = menu.Controls.Add(MsoControlType.msoControlPopup, , , , True)
                            _precbar.Caption = CBM.PRECEDENT_COMMAND_BAR_NAME
                            ReDim _precbarctrls(0)
                            _precbar.Visible = False
                        End If

                    End If
                Else
                    Try
                        _omsbar = cmdBars.Item(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME))
                    Catch
                    End Try
                    If (_omsbar Is Nothing) Then
                        _omsbar = cmdBars.Add(CBM.MAIN_COMMAND_BAR_NAME, , , True)
                        _omsbar.Position = MsoBarPosition.msoBarTop
                        _omsbar.Visible = True
                    End If
                    CBM.Add(CBM.MAIN_COMMAND_BAR_CODE, _omsbar.Name)

                    Try
                        _timebar = cmdBars.Item(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME))
                    Catch
                    End Try
                    If (_timebar Is Nothing) Then
                        _timebar = cmdBars.Add(CBM.TIME_COMMAND_BAR_NAME, , , True)
                        ReDim _timebarctrls(0)
                        _timebar.Visible = False
                    End If
                    CBM.Add(CBM.TIME_COMMAND_BAR_CODE, _timebar.Name)

                    Try
                        _precbar = cmdBars.Item(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME))
                    Catch
                    End Try
                    If (_precbar Is Nothing) Then
                        _precbar = cmdBars.Add(CBM.PRECEDENT_COMMAND_BAR_NAME, , , True)
                        ReDim _precbarctrls(0)
                        _precbar.Visible = False
                    End If
                    CBM.Add(CBM.PRECEDENT_COMMAND_BAR_CODE, _precbar.Name)

                End If

                ReDim _omsbarctrls(2)


                If Not _omsbar Is Nothing Then
                    Dim ctrlConnect As CommandBarButton = _omsbar.Controls.Add(MsoControlType.msoControlButton, , , , True)
                    AddHandler ctrlConnect.Click, AddressOf CommandBarButtonClick
                    ctrlConnect.Tag = "SYSTEM;CONNECT"
                    ctrlConnect.Style = MsoButtonStyle.msoButtonIconAndCaption
                    ctrlConnect.Caption = Session.CurrentSession.RegistryRes("Connect", "Connect")
                    ctrlConnect.TooltipText = Session.CurrentSession.RegistryRes("Connect_Tip", String.Format("Connects or Logs into an {0} session.", FWBS.OMS.Global.ApplicationName))
                    ctrlConnect.FaceId = 279
                    ctrlConnect.Visible = True
                    _omsbarctrls(0) = ctrlConnect

                    Dim ctrlDisconnect As CommandBarButton = _omsbar.Controls.Add(MsoControlType.msoControlButton, , , , True)
                    AddHandler ctrlDisconnect.Click, AddressOf CommandBarButtonClick
                    ctrlDisconnect.Tag = "SYSTEM;DISCONNECT"
                    ctrlDisconnect.Style = MsoButtonStyle.msoButtonIconAndCaption
                    ctrlDisconnect.Caption = Session.CurrentSession.RegistryRes("Disconnect", "Disconnect")
                    ctrlDisconnect.TooltipText = Session.CurrentSession.RegistryRes("Disconnect_Tip", String.Format("Disconnects or logs out of an existing {0} session.", FWBS.OMS.Global.ApplicationName))
                    ctrlDisconnect.FaceId = 2151
                    ctrlDisconnect.Visible = True
                    _omsbarctrls(1) = ctrlDisconnect
                End If

            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show(ex.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            End Try


        End If
    End Sub

    Protected Friend Sub BuildCommandBars(ByVal local As Boolean)


        If (CommandBarContainer Is Nothing) Then Return
        If UseCommandBars Then

            CBM.Add(BuildCommandBar(Me, local, _omsbar, _omsbarctrls, CBM.MAIN_COMMAND_BAR_CODE, "", MainMenuVisible))
            CBM.Add(BuildCommandBar(Me, local, _timebar, _timebarctrls, CBM.TIME_COMMAND_BAR_CODE, "", TimeRecMenuVisible))

            If (Session.CurrentSession.IsLoggedIn) Then
                If Session.CurrentSession.CurrentUser.IsInRoles("PRECEDIT") Then
                    CBM.Add(BuildCommandBar(Me, local, _precbar, _precbarctrls, CBM.PRECEDENT_COMMAND_BAR_CODE, "", PrecedentMenuVisible))
                End If
            End If

            _omsbar.Visible = True

            RunCommand(Application, "SYSTEM;TOOLBARSCHANGED")

        End If

    End Sub


    Protected Friend Function BuildCommandBar(ByVal doc As Object, ByVal local As Boolean, ByVal bar As Object, ByRef ctrls As CommandBarControl(), ByVal commandBar As String, ByVal filter As String, Optional ByVal visible As Boolean = True, Optional ByVal handler As Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler = Nothing, Optional ByVal UseCommandBarSetting As Boolean = True) As System.Collections.Generic.KeyValuePair(Of String, String)


        If handler Is Nothing Then handler = AddressOf CommandBarButtonClick

        If (UseCommandBarSetting = False Or UseCommandBars) Then

            Try

                'Loop counter for index accessing.
                Dim ctr As Integer = 0

                'Get the data set that holds the rendering values.
                Dim cb As System.Data.DataSet = Nothing

                'Parent command bar for using FindControl
                Dim parent As CommandBar

                If (local) Then

                    Try

                        cb = FWBS.OMS.Session.GetLocalCommandBar(commandBar)

                    Catch

                        Return Nothing
                    End Try

                Else
                    cb = FWBS.OMS.Session.CurrentSession.GetCommandBar(commandBar)
                End If

                'Place the bar that to the position specified in the database.
                'Create it if it has not been already.
                If TypeOf bar Is CommandBar Then
                    parent = bar
                    Try
                        bar.Name = cb.Tables("COMMANDBAR").Rows(0)("cbdesc").ToString()
                        bar.Position = DirectCast(System.Enum.Parse(GetType(MsoBarPosition), cb.Tables("COMMANDBAR").Rows(0)("cbposition").ToString(), True), MsoBarPosition)
                    Catch nameex As System.Runtime.InteropServices.COMException When nameex.ErrorCode = -2147467259
                        'Cannot name the command bar as it may be the Context Menu
                    End Try

                Else
                    parent = bar.Parent
                End If

                BuildCommandBar = New System.Collections.Generic.KeyValuePair(Of String, String)(commandBar, parent.Name)

                'Reinitialise the controls array list with the number of controls in the database.
                'But first, reduce the existing controls click delegate counter.
                If (Not ctrls Is Nothing) Then
                    Dim cbc As CommandBarControl
                    For Each cbc In ctrls
                        If (TypeOf cbc Is CommandBarButton) Then
                            Try
                                Dim btn As CommandBarButton = cbc
                                RemoveHandler btn.Click, handler
                            Catch
                            End Try
                        End If
                    Next
                End If

                Dim vw As System.Data.DataView = New System.Data.DataView(cb.Tables("CONTROLS"))
                Dim vis_count As Integer = 0
                ReDim ctrls(cb.Tables("CONTROLS").DefaultView.Count)

                'Loop through all the data rows and start building each control that
                'is to exist on the 
                Dim row As System.Data.DataRowView
                For Each row In cb.Tables("CONTROLS").DefaultView
                    'Current control to be added.
                    Dim ctrl As CommandBarControl = Nothing

                    'Control type parsed into the office command bar control type from a string in the database.
                    Dim tpe As MsoControlType = DirectCast(System.Enum.Parse(GetType(MsoControlType), row("ctrltype").ToString(), True), MsoControlType)

                    '***************************
                    'Control Specific Properties
                    '***************************
                    Dim tag As String = ""
                    Select Case (Convert.ToString(row("ctrlcode")))

                        Case "CONNECT"
                            tag = "SYSTEM;CONNECT"
                        Case "DISCONNECT"
                            tag = "SYSTEM;DISCONNECT"
                        Case Else
                            tag = Convert.ToString(row("ctrlID")) + ";" + Convert.ToString(row("ctrlrunCommand"))
                    End Select

                    Dim mi As FWBS.OMS.Script.MenuItem = New FWBS.OMS.Script.MenuItem()
                    mi.Id = Convert.ToString(row("ctrlid"))
                    mi.Command = Convert.ToString(row("ctrlrunCommand"))
                    mi.Label = Convert.ToString(row("ctrldesc"))
                    mi.Tooltip = Convert.ToString(row("ctrlhelp"))
                    mi.Filter = Convert.ToString(row("ctrlfilter"))

                    'If the control has no parent value then add it to the parent bar.
                    'Otherwise, add it to the control if it is a popup style control
                    'that is.
                    If (tpe = MsoControlType.msoControlPopup) Then
                        tag = Convert.ToString(row("ctrlcode"))
                        ctrl = parent.FindControl(, , tag, , True)
                    Else
                        ctrl = parent.FindControl(, , tag, , True)
                    End If

                    If (ctrl Is Nothing) Then
                        If (row("ctrlparent") Is DBNull.Value) Then

                            ctrl = bar.Controls.Add(tpe, , , , True)

                        Else
                            Dim p As String = Convert.ToString(row("ctrlparent"))
                            ctrl = parent.FindControl(, , p, , True)
                            If (TypeOf ctrl Is CommandBarPopup) Then
                                ctrl = DirectCast(ctrl, CommandBarPopup).Controls.Add(tpe, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, True)
                            End If
                        End If
                    End If


                    'If the control has been successfully created then set the control properties.
                    If (ctrl Is Nothing) Then
                        Continue For
                    End If

                    '**********************
                    'Button Type Properties
                    '**********************
                    ctrl.Tag = tag

                    If (TypeOf ctrl Is CommandBarButton) Then

                        Dim btn As CommandBarButton = DirectCast(ctrl, CommandBarButton)
                        If (row("ctrlicon") Is DBNull.Value) Then
                            btn.Style = MsoButtonStyle.msoButtonCaption
                        Else
                            btn.Style = MsoButtonStyle.msoButtonIconAndCaption
                            btn.FaceId = Convert.ToInt32(row("ctrlicon"))
                        End If


                        Try
                            RemoveHandler btn.Click, handler
                        Catch
                        End Try

                        AddHandler btn.Click, handler

                    End If


                    ctrl.Caption = mi.Label
                    ctrl.TooltipText = mi.Tooltip
                    ctrl.BeginGroup = Convert.ToBoolean(row("ctrlbegingroup"))

                    If (filter = String.Empty) Then
                        Dim f As String = "(ctrlfilter = '*' or ctrlfilter = '{0}') and ctrlid = '{1}'"
                        f = String.Format(f, ApplicationName, Convert.ToString(row("ctrlid")))
                        vw.RowFilter = f
                        If (vw.Count = 0) Then
                            f = "((ctrlfilter like '%[*]%' or ctrlfilter like '%{0}%') and (ctrlfilter like '%[*]%' and ctrlfilter not like '%!{0}%')) and ctrlid = '{1}'"
                            f = String.Format(f, ApplicationName, Convert.ToString(row("ctrlid")))
                            vw.RowFilter = f
                        End If
                    Else
                        vw.RowFilter = "(" + filter + ") and ctrlid = '" + Convert.ToString(row("ctrlid")) + "'"
                    End If

                    If (vw.Count > 0) Then
                        mi.Visible = True
                        mi.Enabled = True
                    Else
                        mi.Enabled = True
                        mi.Visible = False
                    End If

                    If Not menuScripts Is Nothing Then
                        Dim handled As Boolean = False

                        menuScripts.Validate(mi, doc)

                        ctrl.Caption = mi.Label
                        ctrl.TooltipText = mi.Tooltip
                        ctrl.Visible = mi.Visible
                        ctrl.Enabled = mi.Enabled

                    Else
                        ctrl.Visible = mi.Visible
                        ctrl.Enabled = mi.Enabled
                    End If

                    If ctrl.Visible Then
                        vis_count = vis_count + 1
                    End If

                    'Store the control reference.
                    ctrls(ctr) = ctrl

                    'Up the counter.
                    ctr = ctr + 1

                Next row

                Try
                    If vis_count = 0 Then
                        bar.Visible = False
                    Else
                        bar.Visible = visible
                    End If
                Catch visex As System.Runtime.InteropServices.COMException When visex.ErrorCode = -2147467259
                End Try

            Catch ex As Exception
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex)
                Return Nothing
            End Try

        End If
    End Function

#End Region

#Region "Session Captured Events"

    Private Sub _Connected(ByVal sender As Object, ByVal e As EventArgs)
        Apps.ApplicationManager.CurrentManager.InitialiseInstance(_code, Me)

        Connected(sender, e)
    End Sub
    Protected Overridable Sub Connected(ByVal sender As Object, ByVal e As EventArgs)

        If menuScripts Is Nothing Then
            menuScripts = New OMS.Script.MenuScriptAggregator(Me.Application, Me)
        End If

        If UseCommandBars Then
            BuildCommandBars(False)
        End If

        ChangeComAddinState("Connect")

    End Sub

    Protected Overridable Sub Disconnected(ByVal sender As Object, ByVal e As EventArgs)

        If Not menuScripts Is Nothing Then
            menuScripts.Dispose()
            menuScripts = Nothing
        End If

        If (UseCommandBars) Then

            Try
                For Each t As CommandBarControl In DirectCast(_omsbar.Controls, IEnumerable).Cast(Of Object).ToArray()
                    If ((t.Tag = "SYSTEM;CONNECT") Or (t.Tag = "SYSTEM;DISCONNECT")) Then
                    Else
                        t.Delete(True)
                    End If
                Next t
            Catch
            End Try
            Try
                If (Not _timebar Is Nothing) Then
                    For Each t As CommandBarControl In DirectCast(_timebar.Controls, IEnumerable).Cast(Of Object).ToArray()
                        t.Delete(True)
                    Next
                    _timebar.Visible = False
                End If
            Catch
            End Try

            Try
                If (Not _precbar Is Nothing) Then
                    For Each t As CommandBarControl In DirectCast(_precbar.Controls, IEnumerable).Cast(Of Object).ToArray()
                        t.Delete(True)
                    Next
                    _precbar.Visible = False
                End If
            Catch
            End Try

            ChangeComAddinState("Disconnect")
            RunCommand(Application, "SYSTEM;TOOLBARSCHANGED")

        End If
    End Sub

    Private Sub ChangeComAddinState(ByVal action As String)

        ChangeComAddinState(True, action)
        ChangeComAddinState(False, action)

    End Sub

    Private Sub ChangeCOMAddinState(ByVal enable As Boolean, ByVal action As String)

        Dim regName As String

        Dim process As String = "Disable"

        If enable Then
            process = "Enable"
        End If

        regName = action + process

        Dim folder As String = "Office\" + ApplicationName

        Dim settings As FWBS.Common.Reg.ApplicationSetting = New FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, folder, regName)

        Dim applications As String = Convert.ToString(settings.GetSetting("")).ToUpper

        Dim splitters() As Char = {"|"}

        Dim adjustAddins As String() = applications.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries)

        Dim addin As String

        For Each addin In adjustAddins
            Dim foundaddin As Microsoft.Office.Core.COMAddIn
            For Each foundaddin In Application.COMAddIns

                Dim description As String = foundaddin.Description.ToUpper
                If addin = description Then
                    foundaddin.Connect = enable
                End If

            Next
        Next

    End Sub

#End Region

#Region "Event Methods"

    Protected Friend Sub CommandBarButtonClick(ByVal button As CommandBarButton, ByRef cancelDefault As Boolean)
        Try
            RunCommand(Application, button.Tag)
        Catch ex As Exception
            FWBS.OMS.UI.Windows.ErrorBox.Show(ex)
        End Try
    End Sub

#End Region

#Region "Properties"
    Public ReadOnly Property IsAddinInstance() As Boolean
        Get
            Return _addinInst
        End Get
    End Property

#End Region

#Region "OMSApp"

    Public Overrides Function GetAddin(ByVal name As String) As Apps.IOMSAppAddin
        Try
            Dim addins As Microsoft.Office.Core.COMAddIns = Application.COMAddins
            Return COMAddinWrapper.GetAddin(addins, name)
        Catch comex As COMException
            Return MyBase.GetAddin(name)
        End Try
    End Function

    Friend Shadows Sub OnProgressFinished()
        MyBase.OnProgressFinished()
    End Sub

    Friend Shadows Sub OnProgressStart(ByVal caption As String, ByVal label As String, ByVal max As Integer)
        MyBase.OnProgressStart(caption, label, max)
    End Sub

    Friend Shadows Sub OnProgress(ByVal text As String, ByVal iteration As Single)
        MyBase.OnProgress(text, iteration)
    End Sub


    Public Overrides Sub RunCommand(ByVal obj As Object, ByVal cmd As String)

        Dim originalcommand As String = cmd

        Dim pars As String() = cmd.Split(";")

        cmd = String.Join(";", pars, 1, pars.Length - 1)

        If Not menuScripts Is Nothing Then
            menuScripts.ParseCommand(obj, cmd)
            If (menuScripts.Execute(obj, cmd)) Then
                Return
            End If
        End If


        pars = cmd.Split(";")



        Select Case (pars(0))
            Case "EMAILSUPPORT"
                FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRSYSEMAILSUPP", Nothing, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, New FWBS.Common.KeyValueCollection())
            Case "SCRIPT"
                menuScripts.Invoke(pars(1))
            Case "EXECUTE"
                If (pars.Length > 2) Then
                    Services.ProcessStart(pars(1), pars(2))
                Else
                    Services.ProcessStart(pars(1), "")
                End If
            Case "REFRESHRIBBON"
                RefreshRibbon()
            Case "SAVE"
                If (Not IsPrecedent(obj)) And (Me.DocumentManagementMode And OMS.DocumentManagement.DocumentManagementMode.Save) = OMS.DocumentManagement.DocumentManagementMode.None Then
                    'Throw New Exception("SAVE Not Licensed") 'TODO Check best practice
                    Exit Select
                End If
                MyBase.RunCommand(obj, originalcommand)
                RefreshRibbon()
            Case Else
                MyBase.RunCommand(obj, originalcommand)
        End Select
    End Sub

    Public Function RunScriptCommand(ByVal commandName As String, ByVal properties As FWBS.Common.KeyValueCollection) As String


        Dim returnValue As Object = Nothing
        If menuScripts.Invoke(commandName, returnValue, properties) Then

            Return Convert.ToString(returnValue)

        Else

            Return Nothing

        End If

    End Function

    Protected Overloads Overrides Function BeginSave(ByVal obj As Object, ByVal settings As SaveSettings) As DocSaveStatus
        Using contextBlock As DPIContextBlock = If(_dpiAwareness > 0, New DPIContextBlock(_dpiAwareness), Nothing)
            Return MyBase.BeginSave(obj, settings)
        End Using
    End Function

    Protected Friend Overloads Function GetDocVariable(ByVal obj As Object, ByVal varName As String, ByVal defVal As Object) As Object
        Try
            Return GetDocVariable(obj, varName)
        Catch ex As Exception
            Return defVal
        End Try
    End Function

    Protected Friend Shadows Sub DettachDocumentVars(ByVal obj As Object)
        MyBase.DettachDocumentVars(obj)
    End Sub

    Protected Friend Shadows Function AttachDocumentVars(ByVal obj As Object, ByVal useDefaultAssociate As Boolean, Optional ByVal assoc As Associate = Nothing) As Associate
        Return MyBase.AttachDocumentVars(obj, useDefaultAssociate, assoc)
    End Function

#End Region

#Region "IDisposable"

    Public Sub Dispose() Implements System.IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected MustOverride Sub Dispose(ByVal disposing As Boolean)

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
#End Region


End Class






Friend Class COMAddinWrapper
    Implements IOMSAppAddin

#Region "Fields"

    Private _addin As Object
    Private _app As Object

#End Region

#Region "Constructors"

    Public Sub New(ByVal application As Object, ByVal addin As Object)

        If (application Is Nothing) Then
            Throw New ArgumentNullException("application")
        End If
        If (addin Is Nothing) Then
            Throw New ArgumentNullException("addin")
        End If

        _app = application
        _addin = addin
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetAddin(ByVal addins As COMAddIns, ByVal name As String) As IOMSAppAddin

        Dim addin As COMAddIn = GetAddinInternal(addins, name)

        If (addin Is Nothing) Then

            Dim s As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, "", "Office\Addins", name, "")

            Dim regname As String = DirectCast(s.GetSetting(), String)

            addin = GetAddinInternal(addins, regname)
        End If

        If (addin Is Nothing) Then
            For Each a As COMAddIn In addins
                If a.ProgId.StartsWith(name) Then
                    addin = a
                    Exit For
                End If
            Next a
        End If

        If (addin Is Nothing) Then
            Return Nothing
        End If

        Return New COMAddinWrapper(addins.Application, addin.Object)

    End Function

    Private Shared Function GetAddinInternal(ByVal addins As COMAddIns, ByVal name As String) As COMAddIn
        Try
            Dim n As Object = name
            Return addins.Item(n)
        Catch comex As COMException
            Return Nothing
        End Try
    End Function

#End Region


    Public ReadOnly Property Application() As Object Implements Apps.IOMSAppAddin.Application
        Get
            Return _app
        End Get
    End Property

    Public ReadOnly Property Online() As Boolean Implements Apps.IOMSAppAddin.Online
        Get
            Return _addin.Online
        End Get
    End Property

    Public Sub RefreshUI(ByVal context As Object) Implements Apps.IOMSAppAddin.RefreshUI
        _addin.RefreshUI(context)
    End Sub

    Public Function RunCommand(ByVal command As String, ByVal context As Object) As Object Implements Apps.IOMSAppAddin.RunCommand
        Return _addin.Send(command, context)
    End Function
End Class


Public Class CBM

#Region "Constants"

    Public Const MAIN_COMMAND_BAR_CODE As String = "MAIN"
    Public Const TIME_COMMAND_BAR_CODE As String = "TIMEREC"
    Public Const PRECEDENT_COMMAND_BAR_CODE As String = "PRECEDENT"
    Public Const MAIN_COMMAND_BAR_NAME As String = "Office Manager"
    Public Const TIME_COMMAND_BAR_NAME As String = "Time Recording"
    Public Const PRECEDENT_COMMAND_BAR_NAME As String = "Precedent Manager"

#End Region

    Private Shared CommandBarNames As Dictionary(Of String, String) = New Dictionary(Of String, String)()


    Public Shared Sub Add(ByVal code As String, ByVal name As String)
        If Not code Is Nothing Then
            CommandBarNames(code) = name
        End If
    End Sub

    Public Shared Sub Add(ByVal value As KeyValuePair(Of String, String))
        If Not value.Key Is Nothing Then
            CommandBarNames(value.Key) = value.Value
        End If
    End Sub

    Public Shared Function GetName(ByVal name As String, ByVal defaultValue As String) As String
        If CommandBarNames.ContainsKey(name) Then
            Return CommandBarNames(name)
        End If
        Return defaultValue
    End Function

    Public Shared Function GetMainName() As String
        Return GetName(MAIN_COMMAND_BAR_CODE, MAIN_COMMAND_BAR_NAME)
    End Function

    Public Shared Function GetTimeName() As String
        Return GetName(TIME_COMMAND_BAR_CODE, TIME_COMMAND_BAR_NAME)
    End Function

    Public Shared Function GetPrecedentName() As String
        Return GetName(PRECEDENT_COMMAND_BAR_CODE, PRECEDENT_COMMAND_BAR_NAME)
    End Function

End Class