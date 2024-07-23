Imports Outlook = Microsoft.Office.Interop.Outlook

Friend Class DelayItemActionList
    Private ns As Outlook.NameSpace
    Private timer_interval As Integer

    Public Sub New(ByVal interval As Integer)
        If interval < 100 Then
            interval = 100
        End If
        timer_interval = interval
    End Sub

    Public Enum ItemAction
        Move
        Delete
        Edit
    End Enum

    Public Class Item
        Public Sub New(ByVal itemid As String, ByVal folderid As String, ByVal action As ItemAction)
            Me.ItemId = itemid
            Me.FolderId = folderid
            Me.Action = action
        End Sub

        Public ReadOnly ItemId As String
        Public ReadOnly FolderId As String
        Public ReadOnly Action As ItemAction
    End Class

    Private queue As New System.Collections.Generic.Queue(Of Item)
    Private WithEvents timer As New System.Windows.Forms.Timer

    Public Sub Add(ByVal item As Item)
        Try
            queue.Enqueue(item)
        Finally
        End Try
    End Sub

    Public Sub Process(ByVal ns As Outlook.NameSpace)
        If (Not timer.Enabled) Then
            timer.Interval = timer_interval
            Me.ns = ns
            timer.Enabled = True
            timer.Start()
        End If
    End Sub

    Private Sub ThreadedProcess(ByVal sender As Object, ByVal e As EventArgs) Handles timer.Tick
        Try
            If queue.Count > 0 Then
                While queue.Count > 0
                    Dim item As Item = queue.Dequeue()
                    Try
                        Dim m As FWBS.Office.Outlook.OutlookItem = ns.GetItemFromID(item.ItemId)

                        Try

                        
                            Select Case item.Action
                                Case ItemAction.Delete
                                    If Not m Is Nothing Then
                                        m.Attach()
                                        m.Delete()
                                    End If
                                Case ItemAction.Move
                                    Dim f As Outlook.MAPIFolder = ns.GetFolderFromID(item.FolderId)
                                    If Not m Is Nothing And Not f Is Nothing Then
                                        m.Attach()
                                        m.Move(f)
                                    End If
                                Case ItemAction.Edit
                                    Call ExecuteEditMessage(m)
                            End Select
                        Finally
                            If Not m Is Nothing Then
                                m.Detach()
                            End If
                        End Try
                    Catch ex As System.Runtime.InteropServices.COMException
                    End Try
                End While
            End If
        Finally
            timer.Stop()
            timer.Enabled = False
        End Try
    End Sub

    Private Sub ExecuteEditMessage(ByVal obj As Object)
        Dim insp As Outlook.Inspector
        Dim version As Double = Val(obj.Application.Version)
        If TypeOf obj Is Outlook.Inspector Then
            insp = obj
        Else
            insp = obj.GetInspector()
        End If

        If (version > 11) Then
            Try
                insp.CommandBars.ExecuteMso("EditMessage")
            Catch ex As Exception
            End Try
        Else
            Dim btn As Microsoft.Office.Core.CommandBarButton = insp.CommandBars.FindControl(Microsoft.Office.Core.MsoControlType.msoControlButton, 5604, , False)
            If (Not btn Is Nothing) Then
                Try
                    btn.Execute()
                Catch ex As System.Runtime.InteropServices.COMException
                End Try
            End If
        End If

    End Sub
End Class
