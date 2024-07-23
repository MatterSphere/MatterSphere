Imports System.Windows.Forms
Imports FWBS.Office.Outlook
Imports FWBS.OMS.DocumentManagement.Storage
Imports Outlook = Microsoft.Office.Interop.Outlook

Public Class GenericProfileItem
    Implements IProfileItem

    Overridable Sub Refresh(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) Implements IProfileItem.Refresh
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
        If FWBS.OMS.Session.CurrentSession.IsPackageInstalled("CLMCONTLEGAL") Or FWBS.OMS.Session.CurrentSession.IsPackageInstalled("EMAILPIECE") Then
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

    Overridable Sub BeforeSend(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeSend

        OutlookInspector.ApplyActiveControlData()

        If Not Session.CurrentSession.IsLoggedIn Then
            args.Handled = False
            Return
        End If

        If (Not CanSaveAsDocument(appcontroller, args.Item)) Then
            args.Handled = False
            Return
        End If

        If appcontroller.HasDocVariable(args.Item, OutlookOMS.SENTON) Then

            Dim disableSaveForwardedItem As Fwbs.Common.ApplicationSetting = New Fwbs.Common.ApplicationSetting(Fwbs.OMS.Global.ApplicationKey, Fwbs.OMS.Global.VersionKey, "Outlook", "DisableSaveDraftOptions", "false")
            Dim disableBoolean As Boolean = disableSaveForwardedItem.ToBoolean()
            If disableBoolean Then
                Diagnostics.Debug.WriteLine("DisableSaveDraftOptions is turned on")
                args.Handled = False
                Return
            Else
                Diagnostics.Debug.WriteLine("Outlook BeforeSend event - checking SENTON")
                If Not appcontroller.GetDocVariable(args.Item, OutlookOMS.SENTON, DateTime.Now) = Nothing Then
                    args.Handled = False
                    Return
                End If
            End If
        End If

        Dim assoc As FWBS.OMS.Associate = appcontroller.GetCurrentAssociate(args.Item)
        If Not appcontroller.CheckEmailProfileOption(EmailProfileOption.epoNew) And assoc Is Nothing Then
            args.Handled = False
            Return
        End If

        If GetDocumentDirection(appcontroller, args.Item, Nothing) = DocumentDirection.Out OrElse assoc Is Nothing Then
            If assoc Is Nothing Then
                Try
                    Services.AllowPrivateAssociate = True
                    assoc = appcontroller.AttachDocumentVars(args.Item, appcontroller.CheckEmailOption(EmailOption.optUseDefAssoc), assoc)
                Finally
                    Services.AllowPrivateAssociate = False
                End Try

                If assoc Is Nothing Then
                    GoTo Cancel
                    Return
                ElseIf assoc Is FWBS.OMS.Associate.Private Then
                    args.Handled = False
                    Return
                End If
            End If
        End If

        Dim docModifyActivity As FWBS.OMS.StatusManagement.FileActivity = New FWBS.OMS.StatusManagement.FileActivity(assoc.OMSFile, FWBS.OMS.StatusManagement.Activities.FileStatusActivityType.DocumentModification)
        docModifyActivity.Check()


        Dim strEmail As String = assoc.DefaultEmail
        Dim count As Integer = args.Item.Recipients.Count


        For ctr As Integer = count To 1 Step -1
            Dim recip As Outlook.Recipient = args.Item.Recipients(ctr)
            If (recip.Resolved = False) Then recip.Resolve()
        Next ctr

        count = args.Item.Recipients.Count

        If count = 1 Then
            Dim onlyrecip As Outlook.Recipient = args.Item.Recipients(1)
            Dim displaytype As Outlook.OlDisplayType = Outlook.OlDisplayType.olUser

            'Get a comexception randomly from Outlook 2010 RC
            Try
                displaytype = onlyrecip.AddressEntry.DisplayType
            Catch comex As System.Runtime.InteropServices.COMException
                'Above DisplayType now fixed using AddressEntry.  Keep this capture just incase.
            End Try

            If (displaytype = Outlook.OlDisplayType.olUser) Then

                Dim strRecName As String = GetRecipientEmailAddress(onlyrecip)

                If (strRecName = "") Then
                    GoTo Cancel
                    Return
                End If

                'If the sending email address differs with the one given from the database then ask if
                'they would like to save the new one over the existing one.
                If appcontroller.CheckEmailOption(EmailOption.optResolveEmail) Then
                    If Trim(UCase(strEmail)) <> Trim(UCase(strRecName)) Or assoc.Contact.HasEmail(strRecName, "") = False Then
                        If MessageBox.Show(appcontroller.ActiveWindow, Session.CurrentSession.Resources.GetMessage("STOREEMAIL", "The recipient's email differs to the one in the database. Would you like to store this item for future use?", ""), "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                            assoc.DefaultEmail = strRecName
                            assoc.Update()
                        End If
                    End If
                End If

            End If
        End If

        Dim nCount As Integer = args.Item.Recipients.Count
        For ctr As Integer = 1 To nCount
            Dim recip As Outlook.Recipient = args.Item.Recipients(ctr)
            If (recip.Resolved = False) Then recip.Resolve()
        Next ctr

        appcontroller.SetDocVariable(args.Item, OutlookOMS.SENTON, DateTime.Now)


        'Add spelling check option as it is not checked when programmtically sent
        If (args.Item.CheckSpellingOnSend) Then
            appcontroller.ExecuteSpellCheck(args.Item)
        End If

        Dim existingdoc As FWBS.OMS.OMSDocument = appcontroller.GetCurrentDocument(args.Item)
        If (Not existingdoc Is Nothing Or appcontroller.GetDocVariable(args.Item, OMSApp.PROCESSING, False)) Then
            Return
        End If


        Dim ret As Boolean = False
        Dim objectDisposed As Boolean = False

        Try

            If (appcontroller.CheckEmailOption(EmailOption.optQuickSave)) Then
                ret = appcontroller.SaveQuick(args.Item, AddressOf GenericProfileItem.KeepEditingSaveSettingsCallback)
            Else
                ret = appcontroller.Save(args.Item, AddressOf GenericProfileItem.KeepEditingSaveSettingsCallback)
            End If
        Catch ObjectDisposedException As Exception
            objectDisposed = True
        Finally
            If (Not ret) Then
                If Not objectDisposed Then
                    appcontroller.RemoveDocVariable(args.Item, OutlookOMS.SENTON)
                End If
            End If

        End Try

        If (Not ret) Then
            GoTo Cancel
            Return
        End If

        args.Item.Save()

        Exit Sub
Cancel:
        appcontroller.RemoveDocVariable(args.Item, OutlookOMS.SENTON)
        args.Cancel = True
        Exit Sub

    End Sub

    Private Function GetRecipientEmailAddress(ByVal recipient As Outlook.Recipient) As String
        Dim address As String = String.Empty
        Try
            Dim addressEntry As Outlook.AddressEntry = recipient.AddressEntry
            Dim entryType As Outlook.OlAddressEntryUserType = addressEntry.AddressEntryUserType
            If entryType = Outlook.OlAddressEntryUserType.olSmtpAddressEntry Then
                address = addressEntry.Address
            ElseIf entryType = Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry Or entryType = Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry Then
                Dim exchangeUser As Outlook.ExchangeUser = addressEntry.GetExchangeUser()
                If Not exchangeUser Is Nothing Then
                    address = exchangeUser.PrimarySmtpAddress
                    Runtime.InteropServices.Marshal.ReleaseComObject(exchangeUser) : exchangeUser = Nothing
                Else
                    address = addressEntry.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x39FE001E")
                End If
            Else
                address = addressEntry.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x39FE001E") 'PR_SMTP_ADDRESS
            End If
            Runtime.InteropServices.Marshal.ReleaseComObject(addressEntry) : addressEntry = Nothing
        Catch
            address = recipient.Address
        End Try
        GetRecipientEmailAddress = address
    End Function

    Overridable Sub BeforeDelete(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeDelete
        Try
            Dim res As DialogResult = DialogResult.Yes

            'PROFILE IF
            '**********
            '1. Is logged in
            '2. If OnDelete Profile option is On
            '3. Is not already been saved
            '4. Is an Incoming Email
            '5. It flagged for profiling
            '6. Agreed by MessageBox prompt

            If (Not Session.CurrentSession.IsLoggedIn) Then
                args.Handled = False
                Return
            End If
            If (Not CanSaveAsDocument(appcontroller, args.Item)) Then
                args.Handled = False
                Return
            End If
            If (Not appcontroller.CheckEmailProfileOption(EmailProfileOption.epoDelete)) Then
                args.Handled = False
                Return
            End If

            If (appcontroller.IsCompanyDocument(args.Item)) Then
                args.Handled = False
                Return
            End If
            If (Not GetDocumentDirection(appcontroller, args.Item, Nothing) = DocumentDirection.In) Then
                args.Handled = False
                Return
            End If

            If (Not appcontroller.GetDocVariable(args.Item, OutlookOMS.PROFILE, True) = True) Then
                args.Handled = False
                Return
            End If


            Try
                res = MessageBox.Show(appcontroller.ActiveWindow, Session.CurrentSession.Resources.GetMessage("SAVEDELITEM", "Would you like to save the following item to the system before it is deleted?" & Environment.NewLine & Environment.NewLine & "%1%", "", GetDisplayText(appcontroller, args.Item)), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            Catch ex As System.Runtime.InteropServices.COMException
                args.Cancel = True
                Return
            End Try

            Select Case res
                Case DialogResult.Yes
                    Dim saved As Boolean
                    args.Cancel = True


                    If appcontroller.CheckEmailOption(EmailOption.optQuickSave) Then
                        saved = appcontroller.SaveQuick(args.Item, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
                    Else
                        saved = appcontroller.Save(args.Item, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
                    End If
                    If saved Then
                        Try
                            OutlookOMS.DeleteItem(args.Item)
                        Catch
                        End Try
                    End If
                Case DialogResult.No
                    args.Cancel = False
                    args.Handled = False
                Case DialogResult.Cancel
                    args.Cancel = True
            End Select

            If (appcontroller.HasDocVariable(args.Item, OutlookOMS.PROFILE)) Then
                appcontroller.RemoveDocVariable(args.Item, OutlookOMS.PROFILE)
            End If

            If (args.Cancel = False) Then
                args.Item.SetState(Of Boolean)("RUNCLOSERULES", False)
            End If

        Catch secex As FWBS.OMS.Security.SecurityException _
            When secex.HelpID = HelpIndexes.PasswordRequestCancelled
            args.Cancel = True
        End Try
    End Sub

    Overridable Sub BeforeReply(ByVal appcontroller As OutlookOMS, ByVal args As BeforeReplyItemEventArgs) Implements IProfileItem.BeforeReply
        Try
            If (Not Session.CurrentSession.IsLoggedIn) Then
                args.Handled = False
                Return
            End If
            If (Not CanSaveAsDocument(appcontroller, args.Item)) Then
                args.Handled = False
                Return
            End If

            Call appcontroller.RemoveDocVariable(args.ReplyItem, OutlookOMS.SENTON)
            Call appcontroller.DettachDocumentVars(args.ReplyItem)

            If (Not appcontroller.CheckEmailProfileOption(EmailProfileOption.epoReply)) Then
                args.Handled = False
                Return
            End If

            Dim assoc As Associate = Nothing
            If (appcontroller.IsCompanyDocument(args.Item)) Then
                assoc = appcontroller.GetCurrentAssociate(args.Item)
            Else
                'GM - 28/06/2005 - removed [MessageBox.Show(_oms.ActiveWindow....] as this caused issues with cursor focus on reply of emails.
                Dim res As DialogResult = DialogResult.Yes

                Try
                    res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("SAVEREPITEM", "Would you like to save the item that is being replied to?" & Environment.NewLine & Environment.NewLine & "%1%", "", GetDisplayText(appcontroller, args.Item)), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                Catch ex As System.Runtime.InteropServices.COMException
                    args.Cancel = True
                    Return
                End Try

                Dim ret As Boolean = False

                Select Case res
                    Case DialogResult.Yes

                        If appcontroller.CheckEmailOption(EmailOption.optQuickSave) Then
                            ret = appcontroller.SaveQuick(args.Item, AddressOf DisallowMoveAndKeepEditingSaveSettingsCallback)
                        Else
                            ret = appcontroller.Save(args.Item, AddressOf DisallowMoveAndKeepEditingSaveSettingsCallback)
                        End If
                        If ret Then
                            assoc = appcontroller.GetCurrentAssociate(args.Item)
                        Else
                            args.Cancel = True
                            Return
                        End If

                        appcontroller.MoveItem(args.Item)
                        appcontroller.Close(args.Item)
                    Case DialogResult.No
                        'Carry on
                    Case DialogResult.Cancel
                        args.Cancel = True
                        Return
                End Select

            End If

            Call appcontroller.DettachDocumentVars(args.ReplyItem)

            If (Not assoc Is Nothing) Then
                Dim precobj As Precedent = FWBS.OMS.Precedent.GetDefaultPrecedent("EMAIL", assoc)
                Dim preclink As PrecedentLink = New PrecedentLink(precobj, assoc)
                Try
                    appcontroller.SetDocVariable(args.ReplyItem, OutlookOMS.ISREPLY, True)
                    appcontroller.LinkDocument(args.ReplyItem, preclink, False)

                Finally
                    appcontroller.RemoveDocVariable(args.ReplyItem, OutlookOMS.ISREPLY)
                End Try
                args.Cancel = False
            End If

        Catch secex As FWBS.OMS.Security.SecurityException _
            When secex.HelpID = HelpIndexes.PasswordRequestCancelled
            args.Cancel = True
        End Try
    End Sub



    Overridable Sub BeforeForward(ByVal appcontroller As OutlookOMS, ByVal args As BeforeForwardItemEventArgs) Implements IProfileItem.BeforeForward

        Try
            If (Not Session.CurrentSession.IsLoggedIn) Then
                args.Handled = False
                Return
            End If
            If (Not CanSaveAsDocument(appcontroller, args.Item)) Then
                args.Handled = False
                Return
            End If

            Dim disableSaveForwardedItem As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Outlook", "DisableSaveDraftOptions", "false")
            Dim disableBoolean As Boolean = disableSaveForwardedItem.ToBoolean()
            If disableBoolean Then
                Diagnostics.Debug.WriteLine("DisableSaveDraftOptions is turned on")
            Else
                Diagnostics.Debug.WriteLine("Outlook BeforeForward event - Saving forwarded item")
                Dim blnStatus As Boolean = args.Item.Saved()

                'Fix put in to resolve forwarding issue in Outlook, using a different method to the above as this did not work in all areas. CM 17/02/14
                Dim forwardMaxDelayCount As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Outlook", "ForwardMaxDelayCount", 10)
                Dim forwardMaxDelay As Integer = Convert.ToInt32(forwardMaxDelayCount.ToString())

                Dim ctr As Integer = 0
                While ctr < forwardMaxDelay
                    Threading.Thread.Sleep(100)
                    Try
                        args.ForwardItem.Save()
                    Catch
                        Debug.WriteLine("Outlook BeforeForward event - Could not save Forward Item")
                    End Try

                    If args.ForwardItem.Saved() Then
                        Debug.WriteLine("Outlook BeforeForward event - Forward Item saved")
                        Exit While
                    End If

                    ctr += ctr
                End While

                If blnStatus Then args.Item.Save()
            End If

            Call appcontroller.RemoveDocVariable(args.ForwardItem, OutlookOMS.SENTON)
            Call appcontroller.DettachDocumentVars(args.ForwardItem)

            If (Not appcontroller.CheckEmailProfileOption(EmailProfileOption.epoForward)) Then
                args.Handled = False
                Return
            End If

            Dim assoc As Associate = Nothing

            If (appcontroller.IsCompanyDocument(args.Item)) Then
                assoc = appcontroller.GetCurrentAssociate(args.Item)
            Else
                Dim res As DialogResult = DialogResult.Yes

                Try
                    res = MessageBox.Show(appcontroller.ActiveWindow, Session.CurrentSession.Resources.GetMessage("SAVEFWDITEM", "Would you like to save the item that is being forward?" & Environment.NewLine & Environment.NewLine & "%1%", "", GetDisplayText(appcontroller, args.Item)), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                Catch ex As System.Runtime.InteropServices.COMException
                    args.Cancel = True
                    Return
                End Try


                Dim ret As Boolean = False

                Select Case res
                    Case DialogResult.Yes
                        Try

                            If appcontroller.CheckEmailOption(EmailOption.optQuickSave) Then
                                ret = appcontroller.SaveQuick(args.Item, AddressOf DisallowMoveAndKeepEditingSaveSettingsCallback)
                            Else
                                ret = appcontroller.Save(args.Item, AddressOf DisallowMoveAndKeepEditingSaveSettingsCallback)
                            End If
                            If ret Then
                                assoc = appcontroller.GetCurrentAssociate(args.Item)
                            Else
                                args.Cancel = True
                                Return
                            End If
                        Finally

                        End Try
                        appcontroller.MoveItem(args.Item)
                        appcontroller.Close(args.Item)
                    Case DialogResult.No

                    Case DialogResult.Cancel
                        args.Cancel = True
                        Return
                End Select
            End If

            Call appcontroller.DettachDocumentVars(args.ForwardItem)

            If Not assoc Is Nothing Then
                Dim precobj As Precedent = FWBS.OMS.Precedent.GetDefaultPrecedent("EMAIL", assoc)
                Dim preclink As PrecedentLink = New PrecedentLink(precobj, assoc)
                appcontroller.LinkDocument(args.ForwardItem, preclink, False)

                args.Cancel = False
            End If


        Catch secex As FWBS.OMS.Security.SecurityException _
            When secex.HelpID = HelpIndexes.PasswordRequestCancelled
            args.Cancel = True
        End Try
    End Sub

    Overridable Sub BeforeClose(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeClose

        If appcontroller.ShuttingDown Then
            args.Handled = False
            Return
        End If

        If (Not CanSaveAsDocument(appcontroller, args.Item)) Then
            args.Handled = False
            Return
        End If

        If (Not args.Item.GetState(Of Boolean)("RUNCLOSERULES", True, True)) Then
            args.Handled = False
            Return
        End If
        Try
            'PROFILE IF
            '**********
            '1. Is not flagged to skip profile on close by BeforeDelete (_runClose)
            '2. If logged in
            '3. If OnClose Profile option is On
            '4. Is not already been saved
            '5. Is an Incoming Email
            '6. It flagged for profiling
            '7. Agreed by MessageBox prompt

            Dim res As DialogResult = DialogResult.Yes

            If (Not Session.CurrentSession.IsLoggedIn) Then
                args.Handled = False
                Return
            End If
            If (Not appcontroller.CheckEmailProfileOption(EmailProfileOption.epoClose)) Then
                args.Handled = False
                Return
            End If
            If appcontroller.IsCompanyDocument(args.Item) Then
                args.Handled = False
                Return
            End If

            If (Not GetDocumentDirection(appcontroller, args.Item, Nothing) = DocumentDirection.In) Then
                args.Handled = False
                Return
            End If

            If (Not appcontroller.GetDocVariable(args.Item, OutlookOMS.PROFILE, True) = True) Then
                args.Handled = False
                Return
            End If

            Try
                res = MessageBox.Show(appcontroller.ActiveWindow, Session.CurrentSession.Resources.GetMessage("SAVECLOSEDITEM", "Would you like to save the following item to the system before it is closed?" & Environment.NewLine & Environment.NewLine & "%1%", "", GetDisplayText(appcontroller, args.Item)), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            Catch ex As System.Runtime.InteropServices.COMException
                args.Cancel = True
                Return
            End Try

            Select Case res
                Case DialogResult.Yes
                    If appcontroller.CheckEmailOption(EmailOption.optQuickSave) Then
                        args.Cancel = Not appcontroller.SaveQuick(args.Item)
                    Else
                        args.Cancel = Not appcontroller.Save(args.Item)
                    End If
                Case DialogResult.No
                    args.Cancel = False
                Case DialogResult.Cancel
                    args.Cancel = True
            End Select

            If (appcontroller.HasDocVariable(args.Item, OutlookOMS.PROFILE)) Then
                appcontroller.RemoveDocVariable(args.Item, OutlookOMS.PROFILE)
            End If

            If (args.Cancel = False) Then
                args.Item.Save()
            End If
        Catch secex As FWBS.OMS.Security.SecurityException _
            When secex.HelpID = HelpIndexes.PasswordRequestCancelled
            args.Cancel = True
        End Try
    End Sub
    Overridable Sub BeforeOpen(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs) Implements IProfileItem.BeforeOpen
        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return
        End If
        Refresh(appcontroller, args.Item)
    End Sub

    Overridable Function GetDocumentDirection(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal prec As Precedent) As DocumentDirection Implements IProfileItem.GetDocumentDirection
        If item.SentOn = OutlookOMS.MAX_DATE Or item.Sent = False Then
            Return DocumentDirection.Out
        Else
            Return DocumentDirection.In
        End If
    End Function

    Overridable Function GetDisplayText(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String Implements IProfileItem.GetDisplayText
        If Not String.IsNullOrEmpty(item.SenderName) Then
            Return item.SenderName & " - " & item.Subject
        Else
            Return item.Subject
        End If
    End Function

    Overridable Function BeginSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal settings As SaveSettings, ByRef status As DocSaveStatus) As Boolean Implements IProfileItem.BeginSave

        OutlookInspector.ApplyActiveControlData()

        ValidateCanSave(appcontroller, item)

        Return False
    End Function

    Protected Sub ValidateCanSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem)
        If Not CanSaveAsDocument(appcontroller, item) Then
            Throw New NotSupportedException(String.Format("Cannot save item of class type '{0}'", item.Class))
        End If
    End Sub

    Overridable Function BeforeDocumentSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal doc As OMSDocument, ByVal version As FWBS.OMS.DocumentManagement.DocumentVersion) As Boolean Implements IProfileItem.BeforeDocumentSave

        If (doc.IsNew) Then

            Dim si As IStorageItem = version
            Dim duplication As IStorageItemDuplication
            If si Is Nothing Then
                si = doc
            End If

            duplication = si

            'DM - 23/04/04
            'Had to make sure that the authored date was not set to the received date on an outgoing message.  
            'Make sure a MAX_DATE / Null value is not set either.
            If (doc.Direction = DocumentDirection.In) Then

                If (item.ReceivedTime <> OutlookOMS.MAX_DATE) Then
                    doc.AuthoredDate = Common.ConvertDef.ToDateTimeNULL(item.ReceivedTime, New Common.DateTimeNULL)
                End If

                'DM - 27/05/04
                'Set the checksum information on the incoming email.  
                'Make sure it is the same from OWA with Martin Davies from Microsoft.

                Dim checksum As String = GenerateChecksum(appcontroller, item)

                duplication.GenerateChecksum(checksum.ToUpper())

            Else
                'DM - 24/07/06 - Make sure that any outgoing emails blank out the checksum
                duplication.GenerateChecksum(String.Empty)
            End If
        End If

    End Function


    Overridable Function GetDocKey(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByRef key() As String) As Boolean Implements IProfileItem.GetDocKey
        key = New String() {}

        If Not GetDocumentDirection(appcontroller, item, Nothing) = DocumentDirection.In Then
            Return True
        End If

        Dim body As String
        Select Case (item.BodyFormat)
            Case Outlook.OlBodyFormat.olFormatHTML
                body = item.HTMLBody
            Case Else
                Try
                    body = item.Body
                Catch ex As Exception
                    body = ""
                End Try
        End Select

        If body Is Nothing Then body = String.Empty

        Const fieldregx As String = "-{-(?<DocKeys>.*?)-}-"
        Dim matches As System.Text.RegularExpressions.MatchCollection = System.Text.RegularExpressions.Regex.Matches(body, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline)

        ReDim key(matches.Count)

        Dim ctr As Integer = 0
        For Each m As System.Text.RegularExpressions.Match In matches
            Dim k As String = m.Value
            k = k.Replace("-{-", "")
            k = k.Replace("-}-", "")
            key(ctr) = k
            ctr = ctr + 1
        Next

        Return True
    End Function

    Overridable Function SetDocKey(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal key As String) As Boolean Implements IProfileItem.SetDocKey
        If Not GetDocumentDirection(appcontroller, item, Nothing) = DocumentDirection.Out Then
            Return True
        End If

        Try
            Dim res As String
            Select Case (item.BodyFormat)
                Case Outlook.OlBodyFormat.olFormatHTML
                    res = Session.CurrentSession.Resources.GetResource("RESDOCKKEYHTM", "<font color=#ffffff>%1%</font>", "", False, "-{-" & Trim(key) & "-}-").Text()
                    If (res <> String.Empty) Then
                        item.HTMLBody &= String.Format("{0}{0}{1}{0}{0}", Environment.NewLine, res)
                    Else
                        res = Session.CurrentSession.Resources.GetResource("RESDOCKKEY", "Document Key (Internal Use Only) %1% ", "", False, "-{-" & Trim(key) & "-}-").Text
                        If (res <> String.Empty) Then
                            item.HTMLBody &= String.Format("{0}{0}{1}{0}{0}", Environment.NewLine, res)
                        End If
                    End If
                Case Else
                    res = Session.CurrentSession.Resources.GetResource("RESDOCKKEY", "Document Key (Internal Use Only) %1% ", "", False, "-{-" & Trim(key) & "-}-").Text
                    If (res <> String.Empty) Then
                        Try
                            item.Body &= String.Format("{0}{0}{1}{0}{0}", Environment.NewLine, res)
                        Catch ex As Exception
                        End Try
                    End If
            End Select
        Catch
        End Try

        Return True

    End Function

    Overridable Function GetDefaultDocType(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String Implements IProfileItem.GetDefaultDocType
        Return "EMAIL"
    End Function

    Overridable Function CanSaveAsDocument(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As Boolean Implements IProfileItem.CanSaveAsDocument
        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return False
        End If

        If (Not Session.CurrentSession.IsLicensedFor("EMAIL")) Then
            Return False
        End If

        If Session.CurrentSession.IsPackageInstalled("CLMCONTLEGAL") OrElse Session.CurrentSession.IsPackageInstalled("EMAILPIECE") Then
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

    Overridable Function GenerateChecksum(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String Implements IProfileItem.GenerateChecksum
        Dim checksum As String = String.Empty

        Dim recip As Outlook.Recipients = item.Recipients

        If (Not recip Is Nothing) Then
            For Each r As Outlook.Recipient In recip
                checksum &= r.Address
                checksum &= ";"
            Next
        End If

        checksum &= item.SenderName
        checksum &= ";"
        'UTCFIX: DM - 04/12/06 - Use UTC in duplicate keychecker
        checksum &= DateTime.SpecifyKind(item.SentOn, DateTimeKind.Local).ToFileTimeUtc.ToString()
        checksum &= ";"
        checksum &= item.Subject

        Return checksum
    End Function




    Friend Shared Sub DisallowMoveAndKeepEditingSaveSettingsCallback(ByVal settings As SaveSettings)
        DisallowMoveSaveSettingsCallback(settings)
        KeepEditingSaveSettingsCallback(settings)
    End Sub


    Friend Shared Sub DisallowMoveSaveSettingsCallback(ByVal settings As SaveSettings)
        settings.Move = False
    End Sub


    Friend Shared Sub KeepEditingSaveSettingsCallback(ByVal settings As SaveSettings)
        settings.ContinueEditing = True
    End Sub

End Class
