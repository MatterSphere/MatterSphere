Imports System.Windows.Forms
Imports FWBS.Office.Outlook
Imports Outlook = Microsoft.Office.Interop.Outlook

Public Class AppointmentProfileItem
    Implements IProfileItem

    Sub Refresh(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) Implements IProfileItem.Refresh
        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return
        End If
        If (Not appcontroller.UseCommandBars) Then
            Return
        End If
        Dim insp As OutlookInspector = item.Inspector
        If insp Is Nothing Then
            Return
        End If
        If FWBS.OMS.Session.CurrentSession.IsPackageInstalled("APPOINTMENTS") Then
            insp.RemoveCommandBars(appcontroller)
            insp.BuildCommandBars(appcontroller)
        End If
    End Sub


    Sub BeforeActivate(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeActivate
        Dim insp As OutlookInspector = args.Item.Inspector
        If insp Is Nothing Then
            Return
        End If

        If (insp.GetState(Of Boolean)(OutlookOMS.INSP_ACTIVATE_REFREH, False, True)) Then
            Refresh(appcontroller, args.Item)
        End If
    End Sub

    Sub BeforeSend(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeSend

    End Sub

    Sub BeforeDelete(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeDelete

    End Sub
    Sub BeforeReply(ByVal appcontroller As OutlookOMS, ByVal args As BeforeReplyItemEventArgs) Implements IProfileItem.BeforeReply

    End Sub
    Sub BeforeForward(ByVal appcontroller As OutlookOMS, ByVal args As BeforeForwardItemEventArgs) Implements IProfileItem.BeforeForward

    End Sub
    Sub BeforeClose(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeClose

    End Sub
    Sub BeforeOpen(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeOpen
        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return
        End If

        Dim appointment As OutlookAppointment = args.Item


        If appointment.RecurrenceState = Outlook.OlRecurrenceState.olApptNotRecurring Then
            Refresh(appcontroller, appointment)
            If appointment.Saved = False Then appointment.Save()
        End If

    End Sub

    Function GetDocumentDirection(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal prec As Precedent) As DocumentDirection Implements IProfileItem.GetDocumentDirection
        Return DocumentDirection.In
    End Function

    Function GetDisplayText(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String Implements IProfileItem.GetDisplayText
        Return item.Subject
    End Function


    Function BeforeDocumentSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal doc As OMSDocument, ByVal version As FWBS.OMS.DocumentManagement.DocumentVersion) As Boolean Implements IProfileItem.BeforeDocumentSave
        Return False
    End Function

    Function BeginSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal settings As SaveSettings, ByRef status As DocSaveStatus) As Boolean Implements IProfileItem.BeginSave

        OutlookInspector.ApplyActiveControlData()

        Dim apnt As OutlookAppointment = DirectCast(item, OutlookAppointment)
        Dim win As IWin32Window = appcontroller.App.GetWindow(apnt)

        Dim assoc As Associate = Nothing
        Dim associd As Long

        Dim file As OMSFile = Nothing
        Dim fileid As Long

        fileid = appcontroller.GetDocVariable(apnt, OMSApp.FILE, -1)
        associd = appcontroller.GetDocVariable(apnt, OMSApp.ASSOCIATE, -1)

        If fileid <> -1 And fileid <> 0 Then file = FWBS.OMS.OMSFile.GetFile(fileid)
        If associd <> -1 And associd <> 0 Then assoc = FWBS.OMS.Associate.GetAssociate(associd)

        If file Is Nothing Then
            assoc = appcontroller.SelectAssociate(appcontroller.CheckEmailOption(EmailOption.optUseDefAssoc), assoc)
            If assoc Is Nothing Then
                status = DocSaveStatus.Cancel
                Return True
            End If
        Else
            If assoc Is Nothing Then
                assoc = file.DefaultAssociate
            End If
        End If


        Dim a As Appointment

        If appcontroller.IsCompanyDocument(apnt) = False Or appcontroller.GetDocVariable(apnt, OMSApp.APPID, -1) = -1 Then
            a = New Appointment(assoc.OMSFile, apnt.Subject)

            Try
                a.SetExtraInfo("appExternal", True)
            Catch
            End Try

            CopyAppointment(apnt, a)

            Dim save As Boolean = True

            If (settings.Mode = PrecSaveMode.Save) Then
                save = Services.Wizards.SaveAppointment(a)
            Else
                If (save) Then
                    a.Update()
                End If
            End If

            If (Not save) Then
                status = DocSaveStatus.Cancel
                Return True
            Else
                CopyAppointment(appcontroller, a, apnt)
            End If

            GoTo SaveItem

        Else

            a = Appointment.GetAppointment(appcontroller.GetDocVariable(apnt, OMSApp.APPID, -1))

            CopyAppointment(apnt, a)
            a.Update()

        End If

CloseItem:


        apnt.Close(Outlook.OlInspectorClose.olSave)

        status = DocSaveStatus.Success

        Return True

SaveItem:

        appcontroller.AttachDocumentVars(apnt, appcontroller.CheckEmailOption(EmailOption.optUseDefAssoc), assoc)
        apnt.Save()

        If (apnt.Recipients.Count > 0) Then

            Dim inviteresult As DialogResult = DialogResult.Yes

            If (settings.Mode = PrecSaveMode.Save) Then
                inviteresult = MessageBox.ShowYesNoQuestion("QSENDINVITENOW", "Would you like to send the invitation request now?")
            End If
            If (inviteresult = DialogResult.Yes) Then
                apnt.Send()
            End If
        End If

        GoTo CloseItem
    End Function

    Function CanSaveAsDocument(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As Boolean Implements IProfileItem.CanSaveAsDocument
        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return False
        End If

        If Session.CurrentSession.IsPackageInstalled("APPOINTMENTS") AndAlso DirectCast(item, OutlookAppointment).IsRecurring = False Then
            Dim messagetype As String = item.MessageClass.ToUpper()
            Dim messagetypes() As String = Session.CurrentSession.EmailMessageTypes.Split(";"c)

            For Each mt As String In messagetypes
                If messagetype Like mt Then
                    Return True
                End If
            Next
        End If

        Return False
    End Function

    Private Function CompareAppointments(ByVal appcontroller As OutlookOMS, ByVal app1 As Outlook.AppointmentItem, ByVal app2 As Appointment) As Boolean
        If appcontroller.GetDocVariable(app1, OMSApp.APPID, -1) <> app2.ID Then
            Return False
        End If

        If appcontroller.GetDocVariable(app1, OMSApp.FILE, -1) <> app2.FileID Then
            Return False
        End If
        If app1.Subject <> app2.Description Then
            Return False
        End If
        If app1.Location <> app2.Location Then
            Return False
        End If
        If app1.Body <> app2.Notes Then
            Return False
        End If

        Dim enddate As DateTime
        If app1.AllDayEvent Then
            If (app2.AllDay) Then
                enddate = app2.EndDate.AddDays(1).Date
            Else
                enddate = app2.EndDate.Date
            End If

        Else
            If (app2.AllDay) Then
                enddate = app2.EndDate.AddDays(1)
            Else
                enddate = app2.EndDate
            End If
        End If

        If app1.End <> enddate Then
            Return False
        End If

        If app1.AllDayEvent Then
            If app1.Start <> app2.StartDate.Date Then
                Return False
            End If
        Else
            If app1.Start <> app2.StartDate Then
                Return False
            End If
        End If
        If app1.ReminderSet <> app2.HasReminder Then
            Return False
        Else
            If app2.HasReminder Then
                If app1.ReminderMinutesBeforeStart <> app2.Reminder Then
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Private Overloads Sub CopyAppointment(ByVal fromApp As Outlook.AppointmentItem, ByVal toApp As Appointment)
        'Get the due date.
        Dim parent As Object = fromApp

       
        'Get the reminder date and time, if set.
        toApp.Reminder = fromApp.ReminderMinutesBeforeStart
        toApp.HasReminder = fromApp.ReminderSet

        toApp.Description = fromApp.Subject
        toApp.Location = fromApp.Location
        Try
            toApp.Notes = fromApp.Body
        Catch ex As Exception
        End Try
        toApp.AllDay = fromApp.AllDayEvent
        toApp.StartDate = fromApp.Start
        If fromApp.AllDayEvent Then
            toApp.EndDate = fromApp.End.AddDays(-1)
        Else
            toApp.EndDate = fromApp.End
        End If

        toApp.InSync = True


    End Sub

    Private Overloads Sub CopyAppointment(ByVal appcontroller As OutlookOMS, ByVal fromApp As Appointment, ByVal toApp As Outlook.AppointmentItem)

        appcontroller.SetDocVariable(toApp, OMSApp.APPID, fromApp.ID)
        toApp.Subject = fromApp.Description
        toApp.Location = fromApp.Location
        Try
            toApp.Body = fromApp.Notes

        Catch ex As Exception

        End Try

        If (toApp.IsRecurring) Then

            Dim rt As Outlook.RecurrencePattern = toApp.GetRecurrencePattern()

            If fromApp.AllDay Then
                rt.PatternStartDate = fromApp.StartDate.Date
                rt.PatternEndDate = fromApp.EndDate.Date.AddDays(1)
            Else
                rt.PatternStartDate = fromApp.StartDate
                rt.PatternEndDate = fromApp.EndDate
            End If


            toApp.ReminderMinutesBeforeStart = fromApp.Reminder
            toApp.ReminderSet = fromApp.HasReminder

        Else

            If fromApp.AllDay Then
                toApp.Start = fromApp.StartDate.Date
                toApp.End = fromApp.EndDate.Date.AddDays(1)
            Else
                toApp.Start = fromApp.StartDate
                toApp.End = fromApp.EndDate
            End If

            Dim reminder As Integer
            If fromApp.HasReminder Then
                reminder = fromApp.Reminder
            Else
                reminder = 0
            End If

            toApp.ReminderMinutesBeforeStart = reminder
            toApp.ReminderSet = fromApp.HasReminder


            'Make sure that this gets set after the start and end dates are set.
            toApp.AllDayEvent = fromApp.AllDay
        End If
    End Sub


    Function GetDocKey(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByRef key() As String) As Boolean Implements IProfileItem.GetDocKey
        Return False
    End Function
    Function SetDocKey(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal key As String) As Boolean Implements IProfileItem.SetDocKey
        Return False
    End Function
    Function GetDefaultDocType(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String Implements IProfileItem.GetDefaultDocType
        Return String.Empty
    End Function
    Function GenerateChecksum(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String Implements IProfileItem.GenerateChecksum
        Return String.Empty
    End Function

End Class
