Imports System.Linq
Imports System.Runtime.CompilerServices
Imports FWBS.Office.Outlook

Friend Module InspectorExtensions

    <Extension()> _
   Friend Sub RemoveCommandBars(ByVal insp As OutlookInspector, ByVal appcontroller As OutlookOMS)

        Dim bar As Microsoft.Office.Core.CommandBar = Nothing

        For Each cb As Microsoft.Office.Core.CommandBar In insp.CommandBars
            If cb.Name.StartsWith("Outlook Item") Then
                bar = cb
                Exit For
            End If
        Next cb

        Try
            If (Not bar Is Nothing) Then
                For Each t As Microsoft.Office.Core.CommandBarControl In bar.Controls.Cast(Of Microsoft.Office.Core.CommandBarControl).ToArray()
                    t.Delete(True)
                Next
                bar.Visible = False
            End If
        Catch
        End Try


    End Sub

    <Extension()> _
    Friend Sub BuildCommandBars(ByVal insp As OutlookInspector, ByVal appcontroller As OutlookOMS, Optional ByVal useCommandBarSetting As Boolean = True)

        If (appcontroller.IsAddinInstance = False) Then
            Exit Sub
        End If

        If (Not appcontroller.UseCommandBars) Then
            Return
        End If

        If Not Session.CurrentSession.IsLoggedIn Then
            Return
        End If

        If Val(insp.Application.Version) >= 12 Then
            Return
        End If


        Dim bar As Microsoft.Office.Core.CommandBar = Nothing

        For Each cb As Microsoft.Office.Core.CommandBar In insp.CommandBars
            If cb.Name.StartsWith("Outlook Item") Then
                bar = cb
                Exit For
            End If
        Next cb

        If bar Is Nothing Then
            bar = insp.CommandBars.Add("Outlook Item Office Manager", , , True)
        End If


        Call InternalBuildCommandBars(appcontroller, insp.CommandBars, bar, insp.CurrentItem, AddressOf appcontroller.CommandBarButtonClick, useCommandBarSetting)

    End Sub



    Friend Sub BuildCommandBars(ByVal appcontroller As OutlookOMS, ByVal bar As Microsoft.Office.Core.CommandBar, ByVal obj As OutlookItem, ByVal handler As Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler, Optional ByVal useCommandBarSetting As Boolean = True)
        If (appcontroller.IsAddinInstance = False) Then
            Exit Sub
        End If


        If Not Session.CurrentSession.IsLoggedIn Then
            Return
        End If

        Call InternalBuildCommandBars(appcontroller, Nothing, bar, obj, handler, useCommandBarSetting)
    End Sub

    Private Sub InternalBuildCommandBars(ByVal appcontroller As OutlookOMS, ByVal bars As Microsoft.Office.Core.CommandBars, ByVal bar As Microsoft.Office.Core.CommandBar, ByVal obj As OutlookItem, ByVal handler As Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler, ByVal useCommandBarsSetting As Boolean)

        Dim filter As String = "ctrlfilter = '*'"

        Dim filteringVersion As Common.ApplicationSetting = New Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "UI\CommandBars", "FilteringVersion", "1")

        Dim isprec As Boolean = appcontroller.IsPrecedent(obj)
        Dim isdoc As Boolean = appcontroller.IsCompanyDocument(obj)
        Dim docid As Long = appcontroller.GetDocVariable(obj, OMSApp.DOCUMENT, 0)


        Dim ctrls As Microsoft.Office.Core.CommandBarControl() = Nothing


        If isprec Then
            If (filteringVersion.ToString() = "1") Then
                filter &= " or ctrlfilter like '%PRECEDENT%'"
            Else
                filter &= " or ctrlfilter like '%PREC;%'"
            End If
            appcontroller.BuildCommandBar(obj, False, bar, ctrls, "OUTLOOKITEM", filter, , , useCommandBarsSetting)

        Else
            If (filteringVersion.ToString() = "1") Then
                If isdoc Then
                    If appcontroller.GetDocVariable(obj, OMSApp.CLIENT, 0) <> 0 Then filter &= " or ctrlfilter like '%CLIENTID%'"
                    If appcontroller.GetDocVariable(obj, OMSApp.FILE, 0) <> 0 Then filter &= " or ctrlfilter like '%FILEID%'"
                    If appcontroller.GetDocVariable(obj, OMSApp.ASSOCIATE, 0) <> 0 Then filter &= " or ctrlfilter like '%ASSOCID%'"
                    filter &= " or ctrlfilter like '%SAVED%'"
                    If appcontroller.GetDocDirection(obj, Nothing) = DocumentDirection.In Then
                        filter &= " or ctrlfilter like '%INWARD%'"
                    Else
                        If appcontroller.GetDocVariable(obj, OMSApp.DOCUMENT, 0) = 0 Then
                            filter &= " or ctrlfilter like '%NEW%'"
                        Else
                            filter &= " or ctrlfilter like '%SAVED%'"
                        End If
                    End If

                Else

                    appcontroller.RemoveDocVariable(obj, OutlookOMS.SENTON)
                    If appcontroller.GetDocDirection(obj, Nothing) = DocumentDirection.In Then
                        filter &= " or ctrlfilter like '%INWARD%'"
                    Else
                        filter &= " or ctrlfilter like '%NEW%'"
                    End If
                End If



            Else
                filter &= " or ctrlfilter like '%DOC;%'"
                If isdoc Then
                    If appcontroller.GetDocVariable(obj, OMSApp.CLIENT, 0) <> 0 Then filter &= " or ctrlfilter like '%DOC+CLIENTID;%'"
                    If appcontroller.GetDocVariable(obj, OMSApp.FILE, 0) <> 0 Then filter &= " or ctrlfilter like '%DOC+FILEID;%'"
                    If appcontroller.GetDocVariable(obj, OMSApp.ASSOCIATE, 0) <> 0 Then filter &= " or ctrlfilter like '%DOC+ASSOCID;%'"

                    If appcontroller.GetDocDirection(obj, Nothing) = DocumentDirection.In Then
                        filter &= " or ctrlfilter like '%DOC+IN+" & IIf(docid = 0, "NEW", "SAVED") & ";%'"
                    Else
                        filter &= " or ctrlfilter like '%DOC+OUT+" & IIf(docid = 0, "NEW", "SAVED") & ";%'"
                    End If


                Else
                    appcontroller.RemoveDocVariable(obj, OutlookOMS.SENTON)
                    If appcontroller.GetDocDirection(obj, Nothing) = DocumentDirection.In Then
                        filter &= " or ctrlfilter like '%DOC+IN+" & IIf(docid = 0, "NEW", "SAVED") & ";%'"
                    Else
                        filter &= " or ctrlfilter like '%DOC+OUT+" & IIf(docid = 0, "NEW", "SAVED") & ";%'"
                    End If
                End If
            End If


            RemoveCustomButton(bar, "SYSTEM;VIEWCURRENTFILE")
            RemoveCustomButton(bar, "SYSTEM;AUTHORISE")
            RemoveCustomButton(bar, "SYSTEM;AUTHORISE;CONFIRM")
            RemoveCustomButton(bar, "SYSTEM;AUTHORISE;REJECT")

            appcontroller.BuildCommandBar(obj, False, bar, ctrls, "OUTLOOKITEM", filter, , handler, useCommandBarsSetting)


            If isdoc Then
                Dim assoc As Associate = appcontroller.GetCurrentAssociate(obj)
                If Not assoc Is Nothing Then
                    Dim filebtn As Microsoft.Office.Core.CommandBarButton = bar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, , , 1, True)
                    filebtn.Caption = assoc.OMSFile.ToString()
                    filebtn.Tag = "SYSTEM;VIEWCURRENTFILE"
                    AddHandler filebtn.Click, handler
                End If
            End If
        End If

        'Add the authorise button
        Dim item As FWBS.OMS.DocumentManagement.Storage.IStorageItem = appcontroller.GetDocumentToAuthorise(obj)
        If (Not item Is Nothing) Then

            If (appcontroller.OutwardAuthorisation(obj)) Then

                Dim authRejectButton As Microsoft.Office.Core.CommandBarButton = bar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, , , 1, True)
                authRejectButton.Caption = Session.CurrentSession.Resources.GetResource("REFAUTHORISE", "Reject", "").Text
                authRejectButton.Tag = "SYSTEM;AUTHORISE;REJECT"
                AddHandler authRejectButton.Click, handler

                Dim authAcceptButton As Microsoft.Office.Core.CommandBarButton = bar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, , , 1, True)
                authAcceptButton.Caption = Session.CurrentSession.Resources.GetResource("CONFAUTHORISE", "Authorise", "").Text
                authAcceptButton.Tag = "SYSTEM;AUTHORISE;CONFIRM"
                AddHandler authAcceptButton.Click, handler

            End If

            Dim authbtn As Microsoft.Office.Core.CommandBarButton = bar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, , , 1, True)
            authbtn.Caption = Session.CurrentSession.Resources.GetResource("AUTHORISEDOC2", "Check Document", "").Text
            authbtn.Tag = "SYSTEM;AUTHORISE"
            AddHandler authbtn.Click, handler
        End If




        If (bars Is Nothing) Then
            Return
        End If

        Dim editctrls As Microsoft.Office.Core.CommandBarControls
        editctrls = bars.FindControls(Microsoft.Office.Core.MsoControlType.msoControlButton, 5604)
        If Not editctrls Is Nothing Then
            For Each ctrl As Microsoft.Office.Core.CommandBarControl In editctrls
                ctrl.Visible = appcontroller.CheckEmailProfileOption(EmailProfileOption.epoAllowEdit)
            Next
        End If



    End Sub

    Private Sub RemoveCustomButton(ByVal bar As Microsoft.Office.Core.CommandBar, ByVal tag As String)

        'Remove the custom buttons and event handlers
        Dim btn As Microsoft.Office.Core.CommandBarButton = bar.FindControl(, , tag)
        If Not btn Is Nothing Then
            Try
                btn.Delete()
            Catch ex As Exception
            End Try
        End If
    End Sub

End Module
