Imports System.Windows.Forms
Imports FWBS.Office.Outlook
Imports Outlook = Microsoft.Office.Interop.Outlook

Public Class TaskProfileItem
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
        If Session.CurrentSession.IsPackageInstalled("TASKS") Then
            insp.BuildCommandBars(appcontroller)
        End If
    End Sub

    Sub BeforeActivate(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeActivate
        Dim insp As OutlookInspector = args.Item.Inspector
        If insp Is Nothing Then
            Return
        End If

        If (insp.GetState(Of Boolean)(OutlookOMS.INSP_ACTIVATE_REFREH, False, True)) Then
            insp.RemoveCommandBars(appcontroller)
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


        Dim fileid As Long = Convert.ToInt64(appcontroller.GetDocVariable(args.Item, OMSApp.FILE, 0))
        Dim isOMSDoc As Boolean = IIf(fileid = 0, False, True)
        If isOMSDoc = False And Len(args.Item.BillingInformation) > 0 And IsNumeric(args.Item.BillingInformation) Then
            If IsNumeric(args.Item.BillingInformation) Then
                appcontroller.SetDocVariable(args.Item, OMSApp.FILE, CLng(Val(args.Item.BillingInformation)))
                args.Item.Save()
                fileid = Convert.ToInt64(appcontroller.GetDocVariable(args.Item, OMSApp.FILE, 0))
                isOMSDoc = True
            End If
        End If

        Refresh(appcontroller, args.Item)

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

    Function CanSaveAsDocument(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As Boolean Implements IProfileItem.CanSaveAsDocument
        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return False
        End If

        If (Session.CurrentSession.IsPackageInstalled("TASKS")) Then
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

    Function BeginSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal settings As SaveSettings, ByRef status As DocSaveStatus) As Boolean Implements IProfileItem.BeginSave

        OutlookInspector.ApplyActiveControlData()

        Dim tsk As OutlookTask = DirectCast(item, OutlookTask)
        Dim win As IWin32Window = appcontroller.App.GetWindow(tsk)

        Dim assoc As Associate = appcontroller.GetCurrentAssociate(tsk)

        assoc = appcontroller.SelectAssociate(appcontroller.CheckEmailOption(EmailOption.optUseDefAssoc), assoc)

        If assoc Is Nothing Then
            status = DocSaveStatus.Cancel
            Return True
        End If

        Dim t As Task

        If appcontroller.IsCompanyDocument(tsk) = False Or appcontroller.GetDocVariable(tsk, OMSApp.TASKID, -1) = -1 Then
            t = New Task(assoc.OMSFile, tsk.Subject)

            Try
                t.SetExtraInfo("tskExternal", True)
            Catch
            End Try

            CopyTask(appcontroller, tsk, t)

            Dim save As Boolean = True

            If (settings.Mode = PrecSaveMode.Save) Then
                save = Services.Wizards.SaveTask(t)
            Else
                If (save) Then
                    t.Update()
                End If
            End If

            If (Not save) Then
                status = DocSaveStatus.Cancel
                Return True
            Else
                CopyTask(appcontroller, t, tsk)
            End If

            GoTo SaveItem

        Else

            t = Task.GetTask(appcontroller.GetDocVariable(tsk, OMSApp.TASKID, -1))

            If CompareTasks(appcontroller, tsk, t) Then
                GoTo CloseItem
            End If

            Dim differsresult As DialogResult = DialogResult.Yes

            If (settings.Mode = PrecSaveMode.Save) Then
                differsresult = MessageBox.ShowYesNoQuestion(win, "TASKDIFFERS", "The task differs from the one in the database, would you like to keep them in synch?")
            End If

            If (differsresult <> DialogResult.Yes) Then
                GoTo CloseItem
            End If

            Dim dres As DialogResult = IIf(tsk.Saved, DialogResult.Yes, DialogResult.No)

            If (settings.Mode = PrecSaveMode.Save) Then
                dres = MessageBox.ShowYesNoCancel(win, "TASKREFRESH", "Would you like to refresh from the database?, clicking No will update the database.")
            End If

            Select Case dres
                Case DialogResult.Yes
                    CopyTask(appcontroller, t, tsk)
                    GoTo SaveItem
                Case DialogResult.No
                    CopyTask(appcontroller, tsk, t)
                    t.Update()
                Case Else
                    status = DocSaveStatus.Cancel
                    Return True
            End Select
        End If

CloseItem:

        If (Not settings.ContinueEditing) Then
            'Added this as task window is sometime disabled!!!

            Try
                tsk.Close(Outlook.OlInspectorClose.olSave)
            Catch ex As Exception

            End Try

        End If

        status = DocSaveStatus.Success

        Return True

SaveItem:

        appcontroller.AttachDocumentVars(tsk, appcontroller.CheckEmailOption(EmailOption.optUseDefAssoc), assoc)
        tsk.Save()

        GoTo CloseItem

    End Function

    Private Function CompareTasks(ByVal appcontroller As OutlookOMS, ByVal tsk1 As Outlook.TaskItem, ByVal tsk2 As Task) As Boolean
        If appcontroller.GetDocVariable(tsk1, OMSApp.TASKID, -1) <> tsk2.ID Then
            Return False
        End If
        If appcontroller.GetDocVariable(tsk1, OMSApp.FILE, -1) <> tsk2.FileID Then
            Return False
        End If
        If tsk1.Subject <> tsk2.Description Then
            Return False
        End If
        If tsk1.Body <> tsk2.Notes Then
            Return False
        End If
        'UTCFIX: DM - 04/12/06 - Consider time value in task.
        If tsk2.Due.IsNull Then
            If tsk1.DueDate <> OutlookOMS.MAX_DATE Then
                Return False
            End If
        Else
            If tsk1.DueDate <> DirectCast(tsk2.Due.ToObject(), DateTime) Then
                Return False
            End If
        End If
        If tsk1.Complete <> tsk2.IsCompleted Then
            Return False
        Else
            If (tsk2.IsCompleted) Then
                If tsk1.DateCompleted <> DirectCast(tsk2.Completed.ToObject(), DateTime) Then
                    Return False
                End If
            Else
                If tsk1.DateCompleted <> OutlookOMS.MAX_DATE Then
                    Return False
                End If
            End If
        End If
        If tsk1.ReminderSet <> tsk2.HasReminder Then
            Return False
        Else
            If tsk2.HasReminder Then
                If tsk1.ReminderTime <> DirectCast(tsk2.Reminder.ToObject(), DateTime) Then
                    Return False
                End If
            Else

            End If
        End If
        Return True
    End Function

    Private Overloads Sub CopyTask(ByVal appcontroller As OutlookOMS, ByVal fromTask As Outlook.TaskItem, ByVal toTask As Task)
        'Get the due date.
        Dim due As Common.DateTimeNULL


        If (fromTask.DueDate < OutlookOMS.MAX_DATE) Then
            due = Common.ConvertDef.ToDateTimeNULL(fromTask.DueDate, New Common.DateTimeNULL)
        Else
            due = Common.ConvertDef.ToDateTimeNULL(DBNull.Value, New Common.DateTimeNULL)
        End If

        'Get the completed date , if completed.
        Dim completed As Common.DateTimeNULL
        If (fromTask.Complete) Then
            completed = Common.ConvertDef.ToDateTimeNULL(fromTask.DateCompleted, New Common.DateTimeNULL)
        Else
            completed = Common.ConvertDef.ToDateTimeNULL(DBNull.Value, New Common.DateTimeNULL)
        End If


        'Get the reminder date and time, if set.
        Dim reminder As Common.DateTimeNULL
        If (fromTask.ReminderSet) Then
            reminder = Common.ConvertDef.ToDateTimeNULL(fromTask.ReminderTime, New Common.DateTimeNULL)
        Else
            reminder = Common.ConvertDef.ToDateTimeNULL(DBNull.Value, New Common.DateTimeNULL)
        End If

        toTask.Description = fromTask.Subject
        Try
            toTask.Notes = fromTask.Body
        Catch ex As Exception
        End Try
        toTask.Due = due
        toTask.Reminder = reminder
        toTask.Completed = completed
        toTask.InSync = True
    End Sub

    Private Overloads Sub CopyTask(ByVal appcontroller As OutlookOMS, ByVal fromTask As Task, ByVal toTask As Outlook.TaskItem)

        appcontroller.SetDocVariable(toTask, OMSApp.TASKID, fromTask.ID)
        toTask.Subject = fromTask.Description
        Try
            toTask.Body = fromTask.Notes
        Catch ex As Exception
        End Try

        If fromTask.Due.IsNull Then
            toTask.DueDate = OutlookOMS.MAX_DATE
        Else
            toTask.DueDate = fromTask.Due.ToObject()
        End If

        If fromTask.Reminder.IsNull Then
            toTask.ReminderSet = False
        Else
            toTask.ReminderSet = True
            toTask.ReminderTime = fromTask.Reminder.ToObject()
        End If

        If fromTask.Completed.IsNull Then
            toTask.Complete = False
        Else
            toTask.DateCompleted = fromTask.Completed.ToObject()
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
