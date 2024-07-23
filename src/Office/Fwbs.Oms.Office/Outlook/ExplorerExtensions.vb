Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports FWBS.Office.Outlook

Friend Module ExplorerExtensions

    <Extension()> _
    Friend Sub RemoveCommandBars(ByVal exp As OutlookExplorer, ByVal appcontroller As OutlookOMS)

        If (Not appcontroller.UseCommandBars OrElse Not appcontroller.IsAddinInstance) Then
            Return
        End If

        Dim merge As Boolean = appcontroller.MergeMenus

        Dim _omsbar As Microsoft.Office.Core.CommandBar = Nothing
        Dim _precbar As Microsoft.Office.Core.CommandBar = Nothing
        Dim _timebar As Microsoft.Office.Core.CommandBar = Nothing

        If merge Then
            Dim menu As Microsoft.Office.Core.CommandBar = Nothing
            For Each cb As Microsoft.Office.Core.CommandBar In exp.CommandBars
                If cb.Type = Microsoft.Office.Core.MsoBarType.msoBarTypeMenuBar Then
                    menu = cb
                    Exit For
                End If
            Next

            If menu Is Nothing Then
                Return
            End If
            Try
                _omsbar = menu.Controls(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME))
            Catch
            End Try
            Try
                _timebar = menu.Controls.Item(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME))
            Catch
            End Try
            Try
                _precbar = menu.Controls.Item(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME))
            Catch
            End Try

        Else
            Try
                _omsbar = exp.CommandBars.Item(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME))
            Catch
            End Try
            Try
                _timebar = exp.CommandBars.Item(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME))
            Catch
            End Try
            Try
                _precbar = exp.CommandBars.Item(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME))
            Catch
            End Try
        End If


        Try
            For Each t As Microsoft.Office.Core.CommandBarControl In _omsbar.Controls.Cast(Of Microsoft.Office.Core.CommandBarControl).ToArray()
                If ((t.Tag = "SYSTEM;CONNECT") Or (t.Tag = "SYSTEM;DISCONNECT")) Then
                Else
                    t.Delete(True)
                End If
            Next t
        Catch ex As Exception
        End Try

        Try
            If (Not _timebar Is Nothing) Then
                For Each t As Microsoft.Office.Core.CommandBarControl In _timebar.Controls.Cast(Of Microsoft.Office.Core.CommandBarControl).ToArray()
                    t.Delete(True)
                Next
                _timebar.Visible = False
            End If
        Catch
        End Try

        Try
            If (Not _precbar Is Nothing) Then
                For Each t As Microsoft.Office.Core.CommandBarControl In _precbar.Controls.Cast(Of Microsoft.Office.Core.CommandBarControl).ToArray()
                    t.Delete(True)
                Next
                _precbar.Visible = False
            End If
        Catch
        End Try


    End Sub


    <Extension()> _
    Friend Sub BuildCommandBars(ByVal exp As OutlookExplorer, ByVal appcontroller As OutlookOMS)

        If (Not appcontroller.UseCommandBars OrElse Not appcontroller.IsAddinInstance) Then
            Return
        End If

        Dim merge As Boolean = appcontroller.MergeMenus

        Dim bars As List(Of Microsoft.Office.Core.CommandBar) = New List(Of Microsoft.Office.Core.CommandBar)

        Dim _omsbar As Microsoft.Office.Core.CommandBar = Nothing
        Dim _precbar As Microsoft.Office.Core.CommandBar = Nothing
        Dim _timebar As Microsoft.Office.Core.CommandBar = Nothing

        Dim _omsbarctrls As Microsoft.Office.Core.CommandBarControl() = Nothing
        Dim _timebarctrls As Microsoft.Office.Core.CommandBarControl() = Nothing
        Dim _precbarctrls As Microsoft.Office.Core.CommandBarControl() = Nothing

        If merge Then
            Dim menu As Microsoft.Office.Core.CommandBar = Nothing
            For Each cb As Microsoft.Office.Core.CommandBar In exp.CommandBars
                If cb.Type = Microsoft.Office.Core.MsoBarType.msoBarTypeMenuBar Then
                    menu = cb
                    Exit For
                End If
            Next

            If menu Is Nothing Then
                Return
            End If



            Try
                _omsbar = menu.Controls(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME))
            Catch
            End Try
            If (_omsbar Is Nothing) Then
                _omsbar = menu.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlPopup, , , , True)
            End If

            Try
                _timebar = menu.Controls.Item(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME))
            Catch
            End Try
            If (_timebar Is Nothing) Then
                _timebar = menu.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlPopup, , , , True)

            End If

            Try
                _precbar = menu.Controls.Item(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME))
            Catch
            End Try
            If (_precbar Is Nothing) Then
                _precbar = menu.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlPopup, , , , True)
            End If

            _omsbar.Caption = CBM.MAIN_COMMAND_BAR_NAME
            _timebar.Caption = CBM.TIME_COMMAND_BAR_NAME
            _precbar.Caption = CBM.PRECEDENT_COMMAND_BAR_NAME

        Else
            Try
                _omsbar = exp.CommandBars.Item(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME))
            Catch
            End Try
            If (_omsbar Is Nothing) Then
                _omsbar = exp.CommandBars.Add(CBM.GetName(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME), , , True)
            End If

            Try
                _timebar = exp.CommandBars.Item(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME))
            Catch
            End Try
            If (_timebar Is Nothing) Then
                _timebar = exp.CommandBars.Add(CBM.GetName(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME), , , True)
            End If

            Try
                _precbar = exp.CommandBars.Item(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME))
            Catch
            End Try
            If (_precbar Is Nothing) Then
                _precbar = exp.CommandBars.Add(CBM.GetName(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME), , , True)
            End If
        End If


        _omsbar.Position = Microsoft.Office.Core.MsoBarPosition.msoBarTop

        If (Session.CurrentSession.IsLoggedIn = False) Then
            ReDim _omsbarctrls(0)
            ReDim _timebarctrls(0)
            ReDim _precbarctrls(0)
        End If

        If (Session.CurrentSession.IsLoggedIn) Then

            CBM.Add(appcontroller.BuildCommandBar(appcontroller, False, _omsbar, _omsbarctrls, CBM.MAIN_COMMAND_BAR_CODE, "", appcontroller.MainMenuVisible))
            CBM.Add(appcontroller.BuildCommandBar(appcontroller, False, _timebar, _timebarctrls, CBM.TIME_COMMAND_BAR_CODE, "", appcontroller.TimeRecMenuVisible))

            If (Session.CurrentSession.IsLoggedIn) Then
                If Session.CurrentSession.CurrentUser.IsInRoles("PRECEDIT") Then
                    CBM.Add(appcontroller.BuildCommandBar(appcontroller, False, _precbar, _precbarctrls, CBM.PRECEDENT_COMMAND_BAR_CODE, "", appcontroller.PrecedentMenuVisible))
                End If
            End If


        Else

            ReDim _omsbarctrls(2)

            Dim ctrlConnect As Microsoft.Office.Core.CommandBarButton = _omsbar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, , , , True)
            RemoveHandler ctrlConnect.Click, AddressOf appcontroller.CommandBarButtonClick
            AddHandler ctrlConnect.Click, AddressOf appcontroller.CommandBarButtonClick
            ctrlConnect.Tag = "SYSTEM;CONNECT"
            ctrlConnect.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonIconAndCaption
            ctrlConnect.Caption = Session.CurrentSession.RegistryRes("Connect", "Connect")
            ctrlConnect.TooltipText = Session.CurrentSession.RegistryRes("Connect_Tip", String.Format("Connects or Logs into an {0} session.", FWBS.OMS.Global.ApplicationName))
            ctrlConnect.FaceId = 279
            ctrlConnect.Visible = True
            _omsbarctrls(0) = ctrlConnect

            Dim ctrlDisconnect As Microsoft.Office.Core.CommandBarButton = _omsbar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, , , , True)
            RemoveHandler ctrlDisconnect.Click, AddressOf appcontroller.CommandBarButtonClick
            AddHandler ctrlDisconnect.Click, AddressOf appcontroller.CommandBarButtonClick
            ctrlDisconnect.Tag = "SYSTEM;DISCONNECT"
            ctrlDisconnect.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonIconAndCaption
            ctrlDisconnect.Caption = Session.CurrentSession.RegistryRes("Disconnect", "Disconnect")
            ctrlDisconnect.TooltipText = Session.CurrentSession.RegistryRes("Disconnect_Tip", String.Format("Disconnects or logs out of an existing {0} session.", FWBS.OMS.Global.ApplicationName))
            ctrlDisconnect.FaceId = 2151
            ctrlDisconnect.Visible = True
            _omsbarctrls(1) = ctrlDisconnect

            CBM.Add(CBM.MAIN_COMMAND_BAR_CODE, _omsbar.Name)

        End If

        _omsbar.Visible = True

    End Sub

   

End Module
