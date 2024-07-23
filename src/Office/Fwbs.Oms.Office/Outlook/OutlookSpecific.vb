Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports FWBS.OMS.DocumentManagement
Imports FWBS.OMS.DocumentManagement.Storage
Imports FWBS.OMS.PDFConversion
Imports FWBS.OMS.PDFConversion.Tools
Imports FWBS.OMS.UI.DialogWrappers
Imports Outlook = Microsoft.Office.Interop.Outlook

<System.Runtime.InteropServices.Guid("190DBAD8-5121-48EF-9AA1-2E19A044D9B6"), System.Runtime.InteropServices.ComVisible(False)>
Public Class OutlookOMS
    Inherits OfficeOMSApp

    Private Shared DelayMoveAction As Boolean = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "OutlookDelayMoveAction", True).ToBoolean()
    Private Shared DelayDeleteAction As Boolean = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "OutlookDelayDeleteAction", True).ToBoolean()
    Private Shared ItemsToAction As New DelayItemActionList(FWBS.Common.ConvertDef.ToInt32(New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "OutlookDelayItemActionList_Timer", 250).GetSetting(), 250))


    Friend Shared Sub MoveItem(ByVal itm As OutlookItem, ByVal f As Outlook.MAPIFolder)
        System.Windows.Forms.Application.DoEvents()

        If (DelayMoveAction) Then
            ItemsToAction.Add(New DelayItemActionList.Item(itm.EntryID, f.EntryID, DelayItemActionList.ItemAction.Move))
            ItemsToAction.Process(itm.Session)
        Else
            Try
                itm.Attach()
                itm.Move(f)
            Catch ex As System.Runtime.InteropServices.COMException
                ItemsToAction.Add(New DelayItemActionList.Item(itm.EntryID, f.EntryID, DelayItemActionList.ItemAction.Move))
                ItemsToAction.Process(itm.Session)
            Finally
                itm.Detach()
            End Try

        End If


    End Sub

    Friend Shared Sub DeleteItem(ByVal itm As OutlookItem, Optional ByVal permanently As Boolean = False)
        If (Not permanently) Then
            Dim f As Outlook.MAPIFolder = itm.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems)
            If (DelayDeleteAction) Then
                ItemsToAction.Add(New DelayItemActionList.Item(itm.EntryID, f.EntryID, DelayItemActionList.ItemAction.Move))
                ItemsToAction.Process(itm.Session)
            Else
                Try
                    itm.Attach()
                    itm.Move(f)
                Catch ex As System.Runtime.InteropServices.COMException
                    ItemsToAction.Add(New DelayItemActionList.Item(itm.EntryID, f.EntryID, DelayItemActionList.ItemAction.Move))
                    ItemsToAction.Process(f.Session)
                Finally
                    itm.Detach()
                End Try
            End If
            Return
        End If


        If (DelayDeleteAction) Then
            ItemsToAction.Add(New DelayItemActionList.Item(itm.EntryID, String.Empty, DelayItemActionList.ItemAction.Delete))
            ItemsToAction.Process(itm.Session)
        Else
            Try
                itm.Attach()
                itm.Delete()
            Catch ex As System.Runtime.InteropServices.COMException
                ItemsToAction.Add(New DelayItemActionList.Item(itm.EntryID, String.Empty, DelayItemActionList.ItemAction.Delete))
                ItemsToAction.Process(itm.Session)
            Finally
                itm.Detach()
            End Try
        End If
    End Sub

#Region "Fields"

    Friend Const INSP_ACTIVATE_REFREH As String = "INSP_ACTIVATE_REFRESH"
    'Outlook maximum date
    Friend Const MAX_DATE As DateTime = #1/1/4501#
    'Custom sent on field name
    Friend Const SENTON As String = "SENTON"
    'Custom email checksum field name to see if any changes have been made to the core body of the email
    Friend Const CHECKSUM As String = "CHECKSUM"

    'Custom email property to specifiy whether it is a replied email or not.
    Friend Const ISREPLY As String = "_ISREPLY"

    'Custom email property to specifiy whether it should be profiled or not.
    Friend Const PROFILE As String = "PROFILE"

    Private Const FIELD_NAME_PREFIX As String = "?????"
    Private Const FIELD_CODE_ATTACHDOCID As String = "ATTACHDOCID"

    Friend Const PROGRESS_AMOUNT As Integer = 20


    Friend ShuttingDown As Boolean = False

    Private Initialising As Boolean = False

    Private handle As OutlookActivation

    Private _ns As Outlook.NameSpace
    Private _activeForm As Form = Nothing

    Public Const PDF_CONVERTED As String = "PDF_CONVERTED"

    Private _disposed As Boolean = False

#End Region

#Region "Constructors"

    'This constructor will get called by the OfficeConnect COMAddin
    Public Sub New(ByVal _app As Outlook.Application, ByVal code As String)
        Me.New(_app, code, True)
    End Sub


    Private InterceptObjectModel As Boolean = False

    Private Sub ResolveInterceptObjectModel()
        If Not Session.CurrentSession.IsLoggedIn Then
            InterceptObjectModel = False
            Return
        End If

        If (DocumentManager.Mode = OMS.DocumentManagement.DocumentManagementMode.None) Then
            InterceptObjectModel = False
            Return
        End If

        InterceptObjectModel = True
    End Sub
    Public Sub New(ByVal _app As Outlook.Application, ByVal code As String, ByVal useCommandBars As Boolean)

        MyBase.New(code, useCommandBars)

        handle = New OutlookActivation()

        Try

            Initialising = True

            Me.App = FWBS.Office.Outlook.OutlookApplication.CreateApplication(_app, IsAddinInstance)

            Me.App.Settings.IsConnected = New Func(Of Boolean)(Function() (InterceptObjectModel))
            Me.App.DisableHooks()

            Call ActivateApplication()

            If (Not Me.App Is Nothing) Then
                Call AddinInitialisation()
            End If

        Catch ex As Exception
            Throw
        Finally
            Initialising = False
        End Try
    End Sub

    'This constructor will get called through Email job processing within the business layer.
    Public Sub New()
        MyBase.New("OUTLOOK", False, False)
        handle = New OutlookActivation()
        ActivateApplication()
    End Sub


    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        On Error Resume Next

        ' Check to see if Dispose has already been called.
        If (Not _disposed) Then

            ' If disposing equals true, dispose all managed 
            'and unmanaged resources.
            If (disposing) Then

                RemoveApplicationDelegates()
                RemoveInspectorsDelegates()
                RemoveExplorersDelegates()

                If (Not _app Is Nothing) Then
                    _app.Dispose()
                    _app = Nothing
                End If

                If (Not contextselection Is Nothing) Then
                    contextselection.Dispose()
                    contextselection = Nothing
                End If

                If (Not handle Is Nothing) Then
                    handle.Release()
                    handle = Nothing
                End If

                If (Not _ns Is Nothing) Then
                    _ns = Nothing
                End If

                If (Not _exps Is Nothing) Then
                    _exps = Nothing
                End If

                If (Not _insps Is Nothing) Then
                    _insps = Nothing
                End If

            End If

            ' Call the appropriate methods to clean up unmanaged resources here.
            ' If disposing is false, only the following code is executed.

            'There is nothing to do

            ' Note disposing has been done.
            _disposed = True

        End If

    End Sub

#End Region

#Region "OfficeOMSApp"

    Protected Overrides ReadOnly Property CommandBarContainer() As Microsoft.Office.Core.CommandBars
        Get
            Return App.ActiveExplorer().CommandBars
        End Get
    End Property

    Protected Overrides ReadOnly Property Application() As Object
        Get
            Return App
        End Get
    End Property

    Public Overrides ReadOnly Property ApplicationName() As String
        Get
            Return "Outlook"
        End Get
    End Property

    Public Overrides ReadOnly Property ApplicationVersion() As Integer
		Get
			Return Version.Parse(_app.Version).Major
		End Get
	End Property

    Public Overrides ReadOnly Property ModuleName() As String
        Get
            Return "OMS Outlook Integration"
        End Get
    End Property

    Public Overrides ReadOnly Property ActiveWindow() As IWin32Window
        Get
            Try
                If _activeForm Is Nothing Then
                    Return App.ActiveWindow
                Else
                    Return _activeForm
                End If
            Catch ex As System.Runtime.InteropServices.COMException
                Return Nothing
            End Try
        End Get
    End Property

#End Region

#Region "Session Captured Events"

    Protected Overrides Sub Connected(ByVal sender As Object, ByVal e As EventArgs)

        ResolveInterceptObjectModel()

        Me.App.EnableHooks()

        If menuScripts Is Nothing Then
            menuScripts = New OMS.Script.MenuScriptAggregator(App, Me)
        End If
        For Each exp As Outlook.Explorer In App.Explorers
            Dim olexp As FWBS.Office.Outlook.OutlookExplorer = TryCast(exp, FWBS.Office.Outlook.OutlookExplorer)
            If (Not olexp Is Nothing) Then
                olexp.BuildCommandBars(Me)
            End If
        Next

        For Each insp As OutlookInspector In App.Inspectors
            Dim olinsp As FWBS.Office.Outlook.OutlookInspector = TryCast(insp, FWBS.Office.Outlook.OutlookInspector)
            If (Not olinsp Is Nothing) Then
                olinsp.SetState(Of Boolean)(INSP_ACTIVATE_REFREH, True)
            End If
        Next

    End Sub

    Protected Overrides Sub Disconnected(ByVal sender As Object, ByVal e As EventArgs)

        ResolveInterceptObjectModel()

        If Not Me.App Is Nothing Then
            Me.App.DisableHooks()

            For Each exp As Outlook.Explorer In App.Explorers
                Dim olexp As FWBS.Office.Outlook.OutlookExplorer = TryCast(exp, FWBS.Office.Outlook.OutlookExplorer)
                If (Not olexp Is Nothing) Then
                    olexp.RemoveCommandBars(Me)
                End If
            Next
            For Each insp As OutlookInspector In App.Inspectors
                Dim olinsp As FWBS.Office.Outlook.OutlookInspector = TryCast(insp, FWBS.Office.Outlook.OutlookInspector)
                If (Not olinsp Is Nothing) Then
                    olinsp.SetState(Of Boolean)(INSP_ACTIVATE_REFREH, True)
                End If
            Next
        End If
        If (Not menuScripts Is Nothing) Then
            menuScripts.Dispose()
            menuScripts = Nothing
        End If
    End Sub

#End Region

#Region "Bookmarks"

    Protected Overrides Function GetBookmark(ByVal obj As Object, ByVal index As Integer) As String
        Call CheckObjectIsDoc(obj)
        Return GetBookmarkRegExMatches(obj).Item(index).Groups("BookmarkName").Value
    End Function

    Protected Overrides Function GetBookMarkCount(ByVal obj As Object) As Integer
        Try
            Call CheckObjectIsDoc(obj)
            Return GetBookmarkRegExMatches(obj).Count
        Catch
            Return 0
        End Try
    End Function

    Protected Overrides Function HasBookmark(ByVal obj As Object, ByVal name As String) As Boolean

        Try
            Call CheckObjectIsDoc(obj)
            Dim m As System.Text.RegularExpressions.Match
            For Each m In GetBookmarkRegExMatches(obj)
                If m.Groups.Item("BookmarkName").Value = name Then
                    Return True
                    Exit Function
                End If
            Next
            Return False
        Catch
            Return False
        End Try
    End Function

    Protected Overrides Function SetBookmark(ByVal obj As Object, ByVal name As String, ByVal val As String) As Boolean
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        Try
            Dim body As String = item.GetBodyText()
            body = Replace(body, "###" & name & "###", val)
            item.SetBodyText(body)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function GetBookmarkRegExMatches(ByVal obj As Object) As System.Text.RegularExpressions.MatchCollection
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        Dim text As String = String.Empty
        Try
            text = item.GetBodyText()
        Catch
        End Try
        Const fieldregx As String = "###(?<BookmarkName>.*?)###"
        Dim matches As System.Text.RegularExpressions.MatchCollection = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline)
        Return matches

    End Function

#End Region

#Region "Field Routines"

    Public Overrides ReadOnly Property FieldExclusionList() As String()
        Get
            Dim exc() As String = MyBase.FieldExclusionList
            Dim newexc(exc.Length + 4) As String
            exc.CopyTo(newexc, 0)
            newexc(exc.Length) = SENTON
            newexc(exc.Length + 1) = APPID
            newexc(exc.Length + 2) = TASKID
            newexc(exc.Length + 3) = CHECKSUM
            Return newexc
        End Get
    End Property

    Private Function GetLessThanFieldMaker(ByVal obj As Object) As String
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj

        Select Case (item.BodyFormat)
            Case Outlook.OlBodyFormat.olFormatHTML
                Return "&lt;&lt;"
        End Select

        Return "<<"
    End Function

    Private Function GetGreaterThanFieldMaker(ByVal obj As Object) As String
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        Select Case (item.BodyFormat)
            Case Outlook.OlBodyFormat.olFormatHTML
                Return "&gt;&gt;"
        End Select
        Return ">>"
    End Function

    Public Overrides Sub CheckFields(ByVal obj As Object)
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        Dim fieldMatches As System.Text.RegularExpressions.MatchCollection = GetFieldRegExMatches(obj)
        Dim match As System.Text.RegularExpressions.Match

        obj.Recipients.ResolveAll()

        Dim from As String = obj.SentOnBehalfOfName
        Dim [to] As String = obj.To
        Dim cc As String = obj.CC
        Dim bcc As String = obj.BCC

        Dim subject As String = obj.Subject
        Dim body As String
        Try
            body = item.GetBodyText()
        Catch ex As Exception
            Exit Sub
        End Try

        ' Optimisation Code by MNW
        If body = "" Then Exit Sub

        Dim lt As String = GetLessThanFieldMaker(obj)
        Dim gt As String = GetGreaterThanFieldMaker(obj)
        Dim fp As FieldParser = New FieldParser(GetCurrentAssociate(obj))
        For Each match In fieldMatches
            Dim rawname As String = match.Groups("FieldName").Value
            Dim field As String = FIELD_NAME_PREFIX & rawname
            Dim value As String

            ' Correct processing ATTACHDOCID field when word document is sent by clicking "Send Via Email" button
            If field.StartsWith(FIELD_NAME_PREFIX & "!") Or field.Equals(FIELD_NAME_PREFIX & FIELD_CODE_ATTACHDOCID) Then
                value = Convert.ToString(GetDocVariable(obj, field))
            Else
                value = Convert.ToString(fp.Parse(True, rawname))
            End If

            from = Replace([from], "<<" & rawname & ">>", value)
            [to] = Replace([to], "<<" & rawname & ">>", value)
            cc = Replace(cc, "<<" & rawname & ">>", value)
            bcc = Replace(bcc, "<<" & rawname & ">>", value)
            subject = Replace(subject, "<<" & rawname & ">>", value.Replace(vbCrLf, " "))
            body = Replace(body, lt & rawname & gt, value)

        Next

        If Not String.IsNullOrEmpty(from) Then
            obj.SentOnBehalfOfName = [from]
        End If
        If Not String.IsNullOrEmpty([to]) Then
            obj.To = [to]
        End If
        If Not String.IsNullOrEmpty(cc) Then
            obj.CC = cc
        End If
        If Not String.IsNullOrEmpty(bcc) Then
            obj.BCC = bcc
        End If
        obj.Subject = subject
        item.SetBodyText(body)
        fieldMatches = Nothing

        Dim ass As Associate = GetCurrentAssociate(obj)
        AddressItem(obj, ass)
    End Sub

    Public Overrides Sub InsertTextIntoDocument(obj As Object, text As String)
        'Add's the text string to the template on merge
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        item.AddBodyText(Environment.NewLine & text, True)
    End Sub

    Public Overrides Sub AddField(ByVal obj As Object, ByVal name As String)
        Dim realname As String = FIELD_NAME_PREFIX & name
        MyBase.AddField(obj, name)
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        name = GetLessThanFieldMaker(obj) & name & GetGreaterThanFieldMaker(obj)
        item.AddBodyText(name & Environment.NewLine, True)
    End Sub

    Public Overrides Sub DeleteField(ByVal obj As Object, ByVal name As String)
        Dim realname As String = FIELD_NAME_PREFIX & name
        Dim val As String = GetDocVariable(obj, realname, "")
        MyBase.DeleteField(obj, name)
        CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj

        Dim fld As String = "<<" & name & ">>"


        If (Not String.IsNullOrEmpty(obj.SentOnBehalfOfName)) Then
            Dim data As String = obj.SentOnBehalfOfName
            data = data.Replace(fld, "")
            If (val <> "") Then data = data.Replace(val, "")
            obj.SentOnBehalfOfName = data
        End If

        If (Not String.IsNullOrEmpty(obj.To)) Then
            Dim data As String = obj.To
            data = data.Replace(fld, "")
            If (val <> "") Then data = data.Replace(val, "")
            obj.To = data
        End If

        If (Not String.IsNullOrEmpty(obj.CC)) Then
            Dim data As String = obj.CC
            data = data.Replace(fld, "")
            If (val <> "") Then data = data.Replace(val, "")
            obj.CC = data
        End If

        If (Not String.IsNullOrEmpty(obj.BCC)) Then
            Dim data As String = obj.BCC
            data = data.Replace(fld, "")
            If (val <> "") Then data = data.Replace(val, "")
            obj.BCC = data
        End If


        Dim subject As String = obj.Subject
        subject = subject.Replace(fld, "")
        If (val <> "") Then subject = subject.Replace(val, "")
        obj.Subject = subject

        Dim body As String
        Try
            body = item.GetBodyText()

        Catch ex As Exception
            Exit Sub
        End Try

        If body = "" Then Exit Sub
        fld = GetLessThanFieldMaker(obj) & name & GetGreaterThanFieldMaker(obj)
        body = body.Replace(fld, "")
        If (val <> "") Then body = body.Replace(val, "")
        item.SetBodyText(body)
        ScreenRefresh()
    End Sub



    Public Overrides Function GetFieldCount(ByVal obj As Object) As Integer
        CheckObjectIsDoc(obj)
        Dim fieldMatches As System.Text.RegularExpressions.MatchCollection = GetFieldRegExMatches(obj)
        Dim match As System.Text.RegularExpressions.Match
        For Each match In fieldMatches
            Dim field As String = FIELD_NAME_PREFIX + match.Groups("FieldName").Value
            If HasDocVariable(obj, field) = False Then
                SetDocVariable(obj, field, "")
            End If
        Next

        'loop through each field and only count the relevant ones.  Ignore the long field
        'name properties which should staty hidden.
        Dim ctr As Integer = 0
        For i As Integer = 1 To obj.UserProperties.Count
            Dim name As String = GetInternalPropertyName(obj.UserProperties(i).Name)
            If name.StartsWith(FIELD_NAME_PREFIX) Then
                ctr = ctr + 1
                Continue For
            End If
        Next
        Return ctr
    End Function

    Private Function GetFieldByIndex(ByVal obj As Object, ByVal index As Integer) As Outlook.UserProperty
        Dim props As System.Collections.Generic.IEnumerable(Of Outlook.UserProperty) = From f In DirectCast(obj.UserProperties, Outlook.UserProperties).Cast(Of Outlook.UserProperty)() Where f.Name.StartsWith(FIELD_NAME_PREFIX) Select f
        Return props.ElementAt(index)
    End Function

    Private Function GetFieldByName(ByVal obj As Object, ByVal name As String) As Outlook.UserProperty
        Return (From f In DirectCast(obj.UserProperties, Outlook.UserProperties).Cast(Of Outlook.UserProperty)() Where f.Name = FIELD_NAME_PREFIX & name Select f).First()
    End Function

    Public Overrides Function GetFieldName(ByVal obj As Object, ByVal index As Integer) As String
        CheckObjectIsDoc(obj)
        Dim prop As Outlook.UserProperty = GetFieldByIndex(obj, index)
        Return ConvertFromLegalFieldName(prop.Name.Substring(FIELD_NAME_PREFIX.Length))
    End Function

    Public Overloads Overrides Function GetFieldValue(ByVal obj As Object, ByVal index As Integer) As Object
        Call CheckObjectIsDoc(obj)
        Dim prop As Outlook.UserProperty = GetFieldByIndex(obj, index)
        Return prop.Value
    End Function

    Public Overloads Overrides Function GetFieldValue(ByVal obj As Object, ByVal name As String) As Object
        Call CheckObjectIsDoc(obj)
        Dim prop As Outlook.UserProperty = GetFieldByName(obj, name)
        Return prop.Value
    End Function

    Public Overloads Overrides Sub SetFieldValue(ByVal obj As Object, ByVal index As Integer, ByVal val As Object)
        Call CheckObjectIsDoc(obj)
        Dim prop As Outlook.UserProperty = GetFieldByIndex(obj, index)
        SetDocVariable(obj, prop.Name, val)
    End Sub

    Public Overloads Overrides Sub SetFieldValue(ByVal obj As Object, ByVal name As String, ByVal val As Object)
        Call CheckObjectIsDoc(obj)
        SetDocVariable(obj, FIELD_NAME_PREFIX & name, val)
    End Sub

    Private Function GetFieldRegExMatches(ByVal obj As Object) As System.Text.RegularExpressions.MatchCollection
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        Dim lt As String = GetLessThanFieldMaker(obj)
        Dim gt As String = GetGreaterThanFieldMaker(obj)

        Dim text As System.Text.StringBuilder = New System.Text.StringBuilder
        If (Not String.IsNullOrEmpty(obj.SentOnBehalfOfName)) Then
            text.AppendLine(obj.SentOnBehalfOfName)
        End If
        If (Not String.IsNullOrEmpty(obj.To)) Then
            text.AppendLine(obj.To)
        End If
        If (Not String.IsNullOrEmpty(obj.CC)) Then
            text.AppendLine(obj.CC)
        End If
        If (Not String.IsNullOrEmpty(obj.BCC)) Then
            text.AppendLine(obj.BCC)
        End If


        text.AppendLine(obj.Subject)
        text = text.Replace("<<", lt)
        text = text.Replace(">>", gt)

        text.AppendLine()
        text.Append(item.GetBodyText())


        Dim fieldregx As String = lt & "(?<FieldName>.*?)" & gt
        Dim matches As System.Text.RegularExpressions.MatchCollection = System.Text.RegularExpressions.Regex.Matches(text.ToString(), fieldregx, System.Text.RegularExpressions.RegexOptions.Singleline)
        Return matches
    End Function

#End Region

#Region "Document Variable Methods"

    Private Function ConvertToLegacyVariableName(ByVal varName As String) As String
        Select Case varName
            Case OMSApp.CLIENT : varName = "OMSClient"
            Case OMSApp.FILE : varName = "OMSMatter"
            Case OMSApp.ASSOCIATE : varName = "OMSAssociate"
            Case OMSApp.DOCUMENT : varName = "OMSDocId"
            Case OMSApp.EDITION : varName = "OMSEdition"
        End Select
        Return varName
    End Function

    Private Function ConvertFromLegacyVariableName(ByVal varName As String) As String
        Select Case varName
            Case "OMSClient" : varName = OMSApp.CLIENT
            Case "OMSMatter" : varName = OMSApp.FILE
            Case "OMSAssociate" : varName = OMSApp.ASSOCIATE
            Case "OMSDocId" : varName = OMSApp.DOCUMENT
            Case "OMSEdition" : varName = OMSApp.EDITION
        End Select
        Return varName
    End Function

    Public Overrides Function ConvertToLegalFieldName(ByVal varName As String) As String
        varName = varName.Replace("[", " ") 'ALT 0160
        varName = varName.Replace("]", "¡") 'ALT 0161
        varName = varName.Replace("_", "¢") 'ALT 0162
        varName = varName.Replace("#", "¤") 'ALT 0163
        Return varName
    End Function

    Public Overrides Function ConvertFromLegalFieldName(ByVal varName As String) As String
        varName = varName.Replace(" ", "[") 'ALT 0160
        varName = varName.Replace("¡", "]") 'ALT 0161
        varName = varName.Replace("¢", "_") 'ALT 0162
        varName = varName.Replace("¤", "#") 'ALT 0163
        Return varName
    End Function

    Public Overrides Sub RemoveDocVariable(ByVal obj As Object, ByVal name As String)
        Call CheckObjectIsDoc(obj)

        Dim globalname As String = GetExternalPropertyName(name)
        Dim prop As Outlook.UserProperty = Nothing



        prop = GetProperty(obj, globalname)
        If Not prop Is Nothing Then
            Try
                prop.Printable(False)

                prop.Delete()
            Catch
            End Try
        Else
            prop = GetProperty(obj, ConvertToLegacyVariableName(name))
            If Not prop Is Nothing Then
                Try
                    prop.Delete()
                Catch
                End Try
            End If
        End If

    End Sub

    Public Overrides Function HasDocVariable(ByVal obj As Object, ByVal name As String) As Boolean
        Try

            Call CheckObjectIsDoc(obj)

            Dim globalname As String = GetExternalPropertyName(name)

            If obj Is Nothing Then
                Return False
            Else
                Return Not IsNothing(GetProperty(obj, globalname))
            End If
        Catch
            Return False
        End Try
    End Function

    Protected Overloads Overrides Function GetDocVariable(ByVal obj As Object, ByVal name As String) As Object
        Call CheckObjectIsDoc(obj)
        REM DM - I added this so that the relink question will never be asked, as once emails are merged the fields are lost to remerge.
        If (name = OMSApp.RELINK) Then Return False

        Dim globalname As String = GetExternalPropertyName(name)


        Dim prop As Outlook.UserProperty = GetProperty(obj, globalname)
        If prop Is Nothing Then
            prop = GetProperty(obj, ConvertToLegacyVariableName(name))
            If prop Is Nothing Then
                Return Nothing
            Else
                Return prop.Value
            End If
        Else
            Return prop.Value
        End If
    End Function

    Protected Overloads Overrides Function SetDocVariable(ByVal obj As Object, ByVal varName As String, ByVal value As Object) As Boolean
        Return SetDocVariable(obj, varName, value, False)
    End Function

    Protected Overloads Function SetDocVariable(ByVal obj As Object, ByVal name As String, ByVal value As Object, ByVal folder As Boolean) As Boolean
        Call CheckObjectIsDoc(obj)

        ' A little bodge job for re-opening items from the system.
        If TypeOf obj Is Outlook.DocumentItem Then
            Return False
        End If

        'If the field name is greater than the maximum length allowed for Outlook
        'properties then checksum the field name and add an ? proprty for the original name
        'so reverse engineering the field name can still occur.
        Dim globalname As String = GetExternalPropertyName(name)

        Return SetDocVariableInternal(obj, globalname, value, folder)

    End Function

    Private Overloads Function SetDocVariableInternal(ByVal obj As Object, ByVal name As String, ByVal value As Object, ByVal folder As Boolean) As Boolean
        Dim t As Outlook.OlUserPropertyType = Outlook.OlUserPropertyType.olText
        If TypeOf value Is String Then
            t = Outlook.OlUserPropertyType.olText
        ElseIf TypeOf value Is Boolean Then
            t = Outlook.OlUserPropertyType.olYesNo
        ElseIf IsNumeric(value) Then
            t = Outlook.OlUserPropertyType.olNumber
        End If


        Dim globalname As String = GetExternalPropertyName(name)

        Dim prop As Outlook.UserProperty = GetProperty(obj, name)


        If prop Is Nothing Then
            If (String.IsNullOrEmpty(Convert.ToString(value)) And t = Outlook.OlUserPropertyType.olText) Then
                prop = obj.UserProperties.Add(globalname, t, folder)
                'Return False
            Else
                prop = obj.UserProperties.Add(globalname, t, folder)
            End If
        Else
            If (prop.Type <> t Or folder) Then
                RemoveDocVariable(obj, name)
                prop = obj.UserProperties.Add(globalname, t, folder)
            End If
        End If

        Try
            prop.Value = value
            prop.Printable(name = OutlookOMS.SENTON Or name = OMSApp.DOCUMENT)
            Return True
        Catch
            Try
                prop.Value = Convert.ToString(value)
                Return True
            Catch
                Return False
            End Try

        End Try
    End Function


#End Region

#Region "Document Property"

    Private Function GetInternalPropertyName(ByVal name As String) As String
        Return Session.CurrentSession.GetInternalDocumentPropertyName(ConvertFromLegalFieldName(name))
    End Function

    Private Function GetExternalPropertyName(ByVal name As String) As String
        Return ConvertToLegalFieldName(Session.CurrentSession.GetExternalDocumentPropertyName(name))
    End Function

    Private Function GetProperty(ByRef obj As Object, ByVal name As String) As Outlook.UserProperty
        Call CheckObjectIsDoc(obj)
        name = ConvertToLegalFieldName(name)
        Try
            Return obj.UserProperties(name)
        Catch
            Return Nothing
        End Try

    End Function

#End Region

#Region "IOMSApp Implementation"

    Public Overrides Function GetOpenFileFilter() As String
        Return "Outlook Files|*.msg"
    End Function

    Public Overloads Function SelectAssociate(ByVal win As IWin32Window) As Associate
        If CheckEmailOption(EmailOption.optUseDefAssoc) Then
            Return Services.SelectDefaultAssociate(win)
        Else
            Return Services.SelectAssociate(win)
        End If
    End Function

    Public Overrides Function ExtractPreview(ByVal obj As Object) As String
        CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj
        Return item.ExtractTextPreview()
    End Function

    Protected Overrides Function GetDocKey(ByVal obj As Object) As String()
        CheckObjectIsDoc(obj)
        Dim pi As IProfileItem = GetProfileItem(obj)
        Dim key() As String = Nothing
        If pi Is Nothing OrElse Not pi.GetDocKey(Me, obj, key) Then
            Return MyBase.GetDocKey(obj)
        Else
            Return key
        End If

    End Function

    Protected Overrides Sub SetDocKey(ByVal obj As Object, ByVal key As String)
        CheckObjectIsDoc(obj)
        Dim pi As IProfileItem = GetProfileItem(obj)
        If pi Is Nothing OrElse Not pi.SetDocKey(Me, obj, key) Then
            MyBase.SetDocKey(obj, key)
        End If
    End Sub

    Private Function GetTempFolder() As OutlookFolder

        Dim Tempopendocs As String = ResourceLookup.GetLookupText("DEF_OUTOPENDOC", "MatterSphereTemp\Opened Documents", "Default opendocuments Folder name created by 3E MatterSphere", String.Empty)

        Dim f As OutlookFolder = CreateFolder(Nothing, Tempopendocs.Split("\"), True)
        For ctr As Integer = f.Items.Count To 1 Step -1
            Try
                f.Items.Remove(ctr)
            Catch
            End Try
        Next
        Return f
    End Function

    Protected Overloads Overrides Function InternalDocumentOpen(ByVal document As OMSDocument, ByVal fetchData As FetchResults, ByVal settings As OpenSettings) As Object

        ActivateApplication()

        Dim win As IWin32Window = App.GetWindow(App)

        Dim doc As OutlookItem = Nothing

        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = document.GetStorageProvider()

        Dim file As System.IO.FileInfo = fetchData.LocalFile


        Dim f As OutlookFolder = GetTempFolder()

        Try

            Dim em As EmailDocument = New EmailDocument(document)

            doc = f.Import(file.FullName, em.FromName, em.From, If(em.Sent = MAX_DATE, CType(Nothing, DateTime?), em.Sent), True)

        Catch
            doc = f.Import(file.FullName, "N/A", "N/A", Nothing, True)
        End Try


        Select Case settings.Mode
            Case DocOpenMode.Edit
                doc.Display()
            Case DocOpenMode.Print
                Dim docs(0) As Object
                docs(0) = doc

                BeginPrint(docs, settings.Printing)
                doc.Close(Outlook.OlInspectorClose.olDiscard)
            Case DocOpenMode.View
                doc.Display()
        End Select


        Return doc

    End Function

    Private Function ImportPrecedentFile(ByVal file As System.IO.FileInfo, ByVal emptyFolder As Boolean) As OutlookItem
        Dim Tempopenprec As String = ResourceLookup.GetLookupText("DEF_OUTOPENPRE", "MatterSphereTemp\Opened Precedents", "Default openprecedent Folder name created by 3E MatterSphere", String.Empty)
        Dim f As OutlookFolder = CreateFolder(Nothing, Tempopenprec.Split("\"), True)
        If (emptyFolder) Then
            For ctr As Integer = f.Items.Count To 1 Step -1
                Try
                    f.Items.Remove(ctr)
                Catch
                End Try
            Next
        End If
        Return f.Import(file.FullName, Nothing, Nothing, Nothing, False)
    End Function

    Protected Overloads Overrides Function InternalPrecedentOpen(ByVal precedent As Precedent, ByVal fetchData As FetchResults, ByVal settings As OpenSettings) As Object
        Dim win As IWin32Window = App.GetWindow(App)
        ActivateApplication()

        Dim doc As OutlookItem = ImportPrecedentFile(fetchData.LocalFile, True)

        Select Case settings.Mode
            Case DocOpenMode.Edit
                doc.Display()
                If IsPrecedent(doc) Then
                    ExecuteEditMessage(doc)
                End If
            Case DocOpenMode.Print
                Dim docs(0) As Object
                docs(0) = doc
                doc.Display()
                BeginPrint(docs, settings.Printing)
            Case DocOpenMode.View
                doc.Display()
        End Select

        Return doc

    End Function

    Public Overrides Sub Close(ByVal obj As Object)
        CheckObjectIsDoc(obj)

        Try
            obj.Close(Outlook.OlInspectorClose.olSave)
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try

    End Sub

    Public Overrides Sub GotoNextStopCode(ByVal startAtTop As Boolean)
        Return
    End Sub

    Protected Overrides Sub SetWindowCaption(ByVal obj As Object, ByVal caption As String)
        Call CheckObjectIsDoc(obj)
        Dim win As IWin32Window = App.GetWindow(obj)
        FWBS.Common.Functions.SetWindowText(win.Handle.ToInt32(), caption)
    End Sub

    Protected Overrides Sub InsertText(ByVal obj As Object, ByVal precLink As PrecedentLink)
        Call CheckObjectIsDoc(obj)
        Dim item As OutlookItem = DirectCast(obj, OutlookItem)
        Dim insert As String = ""
        Dim body As String = item.GetBodyText()
        Dim prec As OutlookItem = Nothing

        Dim fetch As FWBS.OMS.DocumentManagement.Storage.FetchResults = precLink.Merge()
        If (fetch Is Nothing) Then Return
        Dim file As System.IO.FileInfo = fetch.LocalFile

        If Not file Is Nothing Then
            Try
                prec = App.CreateItemFromTemplate(file.FullName)
                insert = prec.GetBodyText()
                OutlookOMS.DeleteItem(prec, True)
                prec = Nothing
            Catch
                Dim str As System.IO.StreamReader
                str = file.OpenText()
                insert = str.ReadToEnd() & Environment.NewLine
                str.Close()
            End Try

            item.AddBodyText(Environment.NewLine & insert, True)
        End If

    End Sub

    Protected Overrides Sub CheckObjectIsDoc(ByRef obj As Object)
        If obj Is Me Then
            obj = DirectCast(App, Outlook.Application).ActiveWindow() ' returns either Explorer or Inspector object
        End If
        If TypeOf obj Is Outlook.Explorer Then
            obj = App.GetExplorer(obj).Selection.Item(1)
        ElseIf obj Is App Then
            obj = App.ActiveInspector()
        ElseIf TypeOf obj Is Outlook.Application Then
            obj = App.ActiveInspector()
        ElseIf TypeOf obj Is Outlook.Inspector Then
            obj = App.GetInspector(obj)
        ElseIf TypeOf obj Is OutlookItem Then
            obj = obj
        ElseIf TypeOf obj Is Outlook.ItemEvents_10_Event Then
            obj = App.GetItem(obj)
        Else
            Throw New Exception(Session.CurrentSession.Resources.GetMessage("NOAPPPSDTCTR", "The passed parameter is not a Outlook.Inspector object.", "").Text)
        End If

        If TypeOf obj Is Outlook.Inspector Then
            Dim insp As Outlook.Inspector = DirectCast(obj, Outlook.Inspector)
            If Not IsNothing(insp.CurrentItem) Then
                obj = App.GetItem(insp.CurrentItem)
            Else
                Throw New Exception(Session.CurrentSession.Resources.GetMessage("NOAPPPSDTCTR", "The passed parameter is not a Outlook.Inspector object.", "").Text)
            End If
        End If
    End Sub


    Public Overrides Sub ActivateApplication()
        Dim ctr As Integer = 0
ReTry:
        If App Is Nothing Then

            Dim olapp As Outlook.Application = Nothing
            Try
                olapp = GetObject(, "Outlook.Application")
            Catch
                handle.SpawnAndWait()
                olapp = GetObject("", "Outlook.Application")
            End Try
            App = FWBS.Office.Outlook.OutlookApplication.CreateApplication(olapp, IsAddinInstance)
        End If

        Try
            If (App.IsDetached Or App.IsDeleted Or App.IsDisposed) Then
                If (IsAddinInstance = False AndAlso App.IsDisposed = False) Then
                    App.Dispose()
                End If
                App = Nothing
                If (ctr = 0) Then
                    ctr = 1
                    GoTo ReTry
                End If
            End If

            Inspectors = App.Inspectors

            Explorers = App.Explorers
            _ns = App.Session



            If IsAddinInstance Then

                Dim exp As OutlookExplorer = App.ActiveExplorer()

                If exp Is Nothing Then
                    exp = App.Explorers.Add(App.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox), Outlook.OlFolderDisplayMode.olFolderDisplayNormal)
                End If

                If (exp.WindowState = Outlook.OlWindowState.olNormalWindow) Then
                    If (exp.Width = 0 Or exp.Height = 0) Then
                        exp.Width = SystemInformation.VirtualScreen.Width
                        exp.Height = SystemInformation.VirtualScreen.Height
                        exp.Top = 0
                        exp.Left = 0
                    End If

                End If

                If (Inspectors.Count = 0) Then
                    exp.Activate(True)
                End If
            End If

            If (IsAddinInstance) Then
                handle.Release()
            End If
        Catch ex As System.Runtime.InteropServices.COMException
            App = Nothing
            If (ctr = 0) Then
                ctr = 1
                GoTo ReTry
            End If
        Catch cwex As System.Runtime.InteropServices.InvalidComObjectException
            App = Nothing
            If (ctr = 0) Then
                ctr = 1
                GoTo ReTry
            End If
        Catch ex As Exception
            App = Nothing
            If (ctr = 0) Then
                ctr = 1
                GoTo ReTry
            End If
        End Try
    End Sub


#Region "Commands"

    Private Sub ExecuteCommand(ByVal obj As Object, ByVal mso As String, ByVal id As Integer)
        Dim insp As Outlook.Inspector
        If TypeOf obj Is Outlook.Inspector Then
            insp = obj
        Else
            insp = obj.GetInspector()
        End If

        If (ApplicationVersion > 11) Then
            If (Not String.IsNullOrEmpty(mso)) Then
                insp.CommandBars.ExecuteMso(mso)
            End If
        Else
            Dim btn As Microsoft.Office.Core.CommandBarButton = insp.CommandBars.FindControl(Microsoft.Office.Core.MsoControlType.msoControlButton, id)
            If (Not btn Is Nothing) Then
                Try
                    btn.Execute()
                Catch ex As System.Runtime.InteropServices.COMException
                End Try
            End If
        End If

    End Sub

    Friend Sub ExecuteCheckNames(ByVal item As OutlookItem)
        ExecuteCommand(item, "CheckNames", 361)
    End Sub

    Friend Sub ExecuteSpellCheck(ByVal item As OutlookItem)
        Dim mailDocument As Microsoft.Office.Interop.Word.Document = item.GetInspector.WordEditor
        mailDocument.CheckSpelling()
    End Sub

    Private Sub ExecuteEditMessage(ByVal item As OutlookItem)
        ItemsToAction.Add(New DelayItemActionList.Item(item.EntryID, "", DelayItemActionList.ItemAction.Edit))
        ItemsToAction.Process(item.Session)
    End Sub
    Public Class NamedAttachment
        Public FileName As String
        Public DisplayName As String

    End Class
    Public Overrides Sub RunCommand(ByVal obj As Object, ByVal cmd As String)

        Dim pars As String() = cmd.Split(";")
        Dim quick As Boolean = (pars(pars.GetUpperBound(0)) = "QUICK")

        'Main command bar.
        Select Case pars(1)
            Case "SAVEALL"
                Call SaveAll(quick)
                Return
            Case "SAVEALLEX"
                Call SaveAllEx()
                Return
            Case "FILEALL"
                Call FileAll()
        End Select

        Dim isdetached As Boolean = False
        Dim oi As OutlookItem = TryCast(obj, OutlookItem)

        Try

            If (Not oi Is Nothing) Then
                isdetached = oi.IsDetached
                oi.Attach()
            End If

            If (pars(pars.GetUpperBound(0)) = "MODELESS") Then
                RunModelessWizard(pars(1))
                Return
            End If

            Select Case pars(1)
                Case "ATTACHRECIPIENT"
                    Call CheckObjectIsDoc(obj)
                    Dim assoc As FWBS.OMS.Associate = AttachDocumentVars(obj, CheckEmailOption(EmailOption.optUseDefAssoc), GetCurrentAssociate(obj))
                    If (Not assoc Is Nothing) Then
                        If (pars.Length > 2) Then
                            AttachRecipients(pars(2), obj, assoc.OMSFile, False)
                        Else
                            AttachRecipients("CC", obj, assoc.OMSFile, False)
                        End If
                    End If
                Case "FILEIT"
                    Call CheckObjectIsDoc(obj)
                    FileIt(obj)
                Case "DETACHDOCVARS"
                    Call CheckObjectIsDoc(obj)

                    'Office 2007 loses RCW when item is closed. Need to refind
                    obj.Save()
                    Dim entryid As String = obj.EntryId
                    Close(obj)
                    obj = _app.Session.GetItemFromID(entryid)

                    DettachDocumentVars(obj)
                    'Workaround: add and delete temporary user property which forces Outlook to sync and apply other document variable changes.
                    obj.RealUserProperties.Add("TempDetachProp", Outlook.OlUserPropertyType.olYesNo, False).Delete()
                    obj.Save()
                    System.Windows.Forms.Application.DoEvents()
                    obj.Display()

                Case "CLIENTINFOITEM"
                    CheckObjectIsDoc(obj)
                    Dim cl As FWBS.OMS.Client = FWBS.OMS.Client.GetClient(GetDocVariable(obj, OMSApp.CLIENT, 0))
                    Services.ShowClient(ActiveWindow, cl, String.Empty)
                Case "FILEINFOITEM"
                    CheckObjectIsDoc(obj)
                    Dim file As FWBS.OMS.OMSFile = FWBS.OMS.OMSFile.GetFile(GetDocVariable(obj, OMSApp.FILE, 0))
                    Services.ShowFile(ActiveWindow, file, String.Empty)
                Case "ATTACHDOC"
                    CheckObjectIsDoc(obj)
                    Dim askversions As Boolean = True
                    If (pars.Length > 2) Then
                        askversions = pars(2) <> "LATEST"
                    End If
                    CommandAttachDocument(obj, askversions)

                Case "ATTACHPDFDOC"
                    CheckObjectIsDoc(obj)
                    Dim askversions As Boolean = True
                    If (pars.Length > 2) Then
                        askversions = pars(2) <> "LATEST"
                    End If
                    CommandAttachDocument(obj, askversions, True)

                Case "CONVERTATTAPDF"
                    CheckObjectIsDoc(obj)
                    ConvertAttachmentToPDF(obj)

                Case "ATTACHDOCREF"
                    CheckObjectIsDoc(obj)
                    Dim askversions As Boolean = True
                    If (pars.Length > 2) Then
                        askversions = pars(2) <> "LATEST"
                    End If
                    CommandAttachDocumentByRef(obj, askversions)

                Case "SAVESENDITEM"
                    SaveAndSend(obj)
                Case "SAVE"
                    If IsStoredDocument(obj) Then
                        SaveProfiledEmail(obj)
                    Else
                        MyBase.RunCommand(obj, cmd)
                    End If
                Case "SAVEQUICK"
                    If IsStoredDocument(obj) Then
                        SaveProfiledEmail(obj)
                    Else
                        MyBase.RunCommand(obj, cmd)
                    End If
                Case "REPLY"
                    Call CheckObjectIsDoc(obj)
                    Dim assoc As FWBS.OMS.Associate
                    If pars(3) = "CLIENT" Then
                        Dim file As FWBS.OMS.OMSFile = FWBS.OMS.OMSFile.GetFile(GetDocVariable(obj, OMSApp.FILE, 0))
                        assoc = file.DefaultAssociate
                    Else
                        'Always come up with the associatepicker.
                        assoc = GetCurrentAssociate(obj)
                        If (Not assoc Is Nothing) Then
                            Dim picker As FWBS.OMS.UI.SelectAssociate = New FWBS.OMS.UI.SelectAssociate(assoc.OMSFile)
                            picker.AutoConfirm = True
                            assoc = picker.Show(ActiveWindow)
                            If (assoc Is Nothing) Then
                                Return
                            End If
                        End If
                    End If
                    FWBS.OMS.UI.Windows.Services.TemplateStart(Me, pars(2), assoc)
                Case "VIEWINBOX"
                    Dim usr As User = Session.CurrentSession.CurrentUser
                    Select Case pars(2)
                        Case "USER"
inboxuser:
                            Dim folder As Outlook.MAPIFolder = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox)
                            folder.Display()
                            folder.GetExplorer().Activate()
                        Case "FEEEARNER"
                            usr = Session.CurrentSession.CurrentFeeEarner
inboxfee:
                            Dim recip As Outlook.Recipient = _ns.CreateRecipient(usr.FullName)
                            Dim folder As Outlook.MAPIFolder = _ns.GetSharedDefaultFolder(recip, Outlook.OlDefaultFolders.olFolderInbox)
                            folder.Display()
                            folder.GetExplorer().Activate()
                        Case "OTHER"
                            If pars(3) = "USER" Then
                                usr = Services.Searches.FindUser(ActiveWindow)
                            Else
                                usr = Services.Searches.FindFeeEarner(ActiveWindow)
                            End If
                            If Not usr Is Nothing Then
                                If (usr.ID = Session.CurrentSession.CurrentUser.ID) Then
                                    GoTo inboxuser
                                Else
                                    GoTo inboxfee
                                End If
                            End If
                    End Select
                Case "VIEWTASKS"
                    Dim usr As User = Session.CurrentSession.CurrentUser
                    Select Case pars(2)
                        Case "USER"
taskuser:
                            Dim folder As Outlook.MAPIFolder = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks)
                            folder.Display()
                            folder.GetExplorer().Activate()
                        Case "FEEEARNER"
                            usr = Session.CurrentSession.CurrentFeeEarner
taskfee:
                            Dim recip As Outlook.Recipient = _ns.CreateRecipient(usr.FullName)
                            Dim folder As Outlook.MAPIFolder = _ns.GetSharedDefaultFolder(recip, Outlook.OlDefaultFolders.olFolderTasks)
                            folder.Display()
                            folder.GetExplorer().Activate()
                        Case "OTHER"
                            If pars(3) = "USER" Then
                                usr = Services.Searches.FindUser(ActiveWindow)
                            Else
                                usr = Services.Searches.FindFeeEarner(ActiveWindow)
                            End If
                            If Not usr Is Nothing Then
                                If (usr.ID = Session.CurrentSession.CurrentUser.ID) Then
                                    GoTo taskuser
                                Else
                                    GoTo taskfee
                                End If
                            End If
                    End Select
                Case "VIEWCALENDAR"
                    Dim usr As User = Session.CurrentSession.CurrentUser
                    Select Case pars(2)
                        Case "USER"
caluser:
                            Dim folder As Outlook.MAPIFolder = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar)
                            folder.Display()
                            folder.GetExplorer().Activate()
                        Case "FEEEARNER"
                            usr = Session.CurrentSession.CurrentFeeEarner
calfee:
                            Dim recip As Outlook.Recipient = _ns.CreateRecipient(usr.FullName)
                            Dim folder As Outlook.MAPIFolder
                            Dim path As String 'used to validate that the folder path exists

                            Try
                                folder = _ns.GetSharedDefaultFolder(recip, Outlook.OlDefaultFolders.olFolderCalendar)
                                path = folder.FolderPath
                            Catch ex As Exception
                                If (Not String.IsNullOrWhiteSpace(usr.Email)) Then
                                    recip = _ns.CreateRecipient(usr.Email)
                                    folder = _ns.GetSharedDefaultFolder(recip, Outlook.OlDefaultFolders.olFolderCalendar)
                                    path = folder.FolderPath
                                Else
                                    Throw
                                End If
                            End Try

                            folder.Display()
                            folder.GetExplorer().Activate()
                        Case "OTHER"
                            If pars(3) = "USER" Then
                                usr = Services.Searches.FindUser(ActiveWindow)
                            Else
                                usr = Services.Searches.FindFeeEarner(ActiveWindow)
                            End If
                            If Not usr Is Nothing Then
                                If (usr.ID = Session.CurrentSession.CurrentUser.ID) Then
                                    GoTo caluser
                                Else
                                    GoTo calfee
                                End If
                            End If
                    End Select
                Case "AUTHORISE"

                    Dim version As DocumentVersion = GetDocumentToAuthorise(obj)
                    If (pars.Length = 3) Then

                        CheckObjectIsDoc(obj)

                        Select Case pars(2).ToUpper
                            Case "CONFIRM"

                                ReplyToAuthorisation(obj, version, True)

                            Case "REJECT"
                                ReplyToAuthorisation(obj, version, False)
                        End Select
                    Else

                        If Not version Is Nothing Then
                            Services.OpenDocument(version, DocOpenMode.Edit)
                        End If
                    End If
                Case "SAVEASEX"

                    obj = App.ActiveExplorer().Selection.Item(1)
                    MyBase.RunCommand(obj, "OMS;SAVEAS")

                Case Else
                    MyBase.RunCommand(obj, cmd)
            End Select


        Finally
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            If (isdetached) Then
                If (Not oi Is Nothing) Then
                    oi.Detach()
                End If
            End If

        End Try

    End Sub


#End Region

#End Region

#Region "Printing"

    Public Overrides Sub InternalPrint(ByVal obj As Object, ByVal copies As Integer)
        CheckObjectIsDoc(obj)

        Dim item As OutlookItem = obj

        Dim copy As OutlookItem = Nothing
        Dim gotCopy As Boolean = True
        Try 'if the object is an email which is an attachment within another email copy will fail as its parent is wrong
            'in this case we print the original email with the user properties, 
            copy = obj.Copy()
            copy.Attach()
        Catch ex As Exception
            gotCopy = False
            copy = obj
        End Try

        Dim ctr As Integer = 0

        For ctr = 1 To copies
            copy.PrintOut()
        Next ctr

        If gotCopy Then
            copy.Save()
            OutlookOMS.DeleteItem(copy, True)
        End If
    End Sub

#End Region

#Region "Saving"

    Protected Overrides Sub InternalSave(ByVal obj As Object, ByVal createFileIfNew As Boolean)
        CheckObjectIsDoc(obj)

        If Not obj.IsNew And Not obj.Saved Then
            obj.Save()
        End If
    End Sub

    Protected Overrides Sub BeforeDocumentSave(ByVal obj As Object, ByVal doc As OMSDocument, ByVal version As DocumentVersion)
        CheckObjectIsDoc(obj)
        Dim pi As IProfileItem = GetProfileItem(obj)
        If (pi Is Nothing OrElse Not pi.BeforeDocumentSave(Me, obj, doc, version)) Then
            MyBase.BeforeDocumentSave(obj, doc, version)
        End If
    End Sub

    Protected Overrides Sub AttachCustomDocumentVars(ByVal obj As Object, ByVal doc As OMSDocument, ByVal version As FWBS.OMS.DocumentManagement.DocumentVersion)
        Dim ass As Associate = GetCurrentAssociate(obj)
        If Not ass Is Nothing Then
            'Add extra meta data to the email.
            If (IsCompanyDocument(obj)) Then
                SetDocVariable(obj, "CLNO", ass.OMSFile.Client.ClientNo, True)
                SetDocVariable(obj, "FILENO", ass.OMSFile.FileNo, True)
                SetDocVariable(obj, "CLNAME", ass.OMSFile.Client.ClientName, True)
                SetDocVariable(obj, "FILEDESC", ass.OMSFile.FileDescription, True)
                SetDocVariable(obj, "ASSOCNAME", ass.Contact.Name, True)
            Else
                DettachCustomDocumentVars(obj)
            End If
        End If
    End Sub

    Protected Overrides Sub DettachCustomDocumentVars(ByVal obj As Object)
        RemoveDocVariable(obj, "CLNO")
        RemoveDocVariable(obj, "FILENO")
        RemoveDocVariable(obj, "CLNAME")
        RemoveDocVariable(obj, "FILEDESC")
        RemoveDocVariable(obj, "ASSOCNAME")
    End Sub

    Protected Overloads Overrides Function BeginSave(ByVal obj As Object, ByVal settings As SaveSettings) As DocSaveStatus
        CheckObjectIsDoc(obj)

        Dim profileitem As IProfileItem = GetProfileItem(obj)
        Dim status As DocSaveStatus

        If ((profileitem Is Nothing) OrElse (Not profileitem.BeginSave(Me, obj, settings, status))) Then

            Dim disableSaveForwardedItem As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Outlook", "DisableSaveDraftOptions", "false")
            Dim disableBoolean As Boolean = disableSaveForwardedItem.ToBoolean()

            If disableBoolean Then
                Diagnostics.Debug.WriteLine("DisableSaveDraftOptions is turned on")
            Else
                Dim outlookMailObj As OutlookMail = TryCast(obj, OutlookMail)
                If Not outlookMailObj Is Nothing AndAlso Not String.IsNullOrEmpty(outlookMailObj.VotingResponse) Then
                    'Note: If this is an outlook mail object with a voting response then disable the save of this draft anyway
                    Diagnostics.Debug.WriteLine("DisableSaveDraftOptions is turned off but object is a vote response")
                Else
                    Diagnostics.Debug.WriteLine("Outlook BeginSave method - Saving object to keep mail body")
                    If Not obj.Saved Then obj.Save()
                End If

            End If

            status = MyBase.BeginSave(obj, settings)

        End If

        If (status = DocSaveStatus.Error) Then
            InternalSave(obj, False)
        End If

        Return status

    End Function

    Protected Overloads Overrides Sub InternalDocumentSave(ByVal obj As Object, ByVal saveMode As FWBS.OMS.PrecSaveMode, ByVal printMode As FWBS.OMS.PrecPrintMode, ByVal doc As OMSDocument, ByVal version As DocumentVersion)
        Call CheckObjectIsDoc(obj)

        Dim oi As OutlookItem = obj

        'Store an OutlookItem document record if the email is a OMSDocument
        'and the storage provider is not exchanged based.  The exchange
        'based providers already store this.
        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = doc.GetStorageProvider()
        provider.SaveMode = saveMode

        Dim ed As EmailDocument = New EmailDocument(doc, oi)
        ed.Update()


        Dim si As IStorageItem = version
        If si Is Nothing Then
            si = doc
        End If


        Dim file As System.IO.FileInfo = provider.GetLocalFile(si)
        oi.SaveAs(file.FullName)


        'Add the sent date and sent from if not already specified.
        If (oi.SentOn = OutlookOMS.MAX_DATE) Then
            Dim f As OutlookFolder = GetTempFolder()
            Dim tempdoc As OutlookItem = f.Import(file.FullName, ed.FromName, ed.From, ed.Sent, True, True)
            tempdoc.SaveAs(file.FullName)
            Try : tempdoc.Delete(True) : Catch : End Try
        End If

        provider.Store(si, file, oi, True, Me)

        oi.Save()

    End Sub

    Protected Overrides Sub InternalPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent)
        InternalPrecedentSave(obj, saveMode, printMode, prec, Nothing)
    End Sub

    Protected Overrides Sub InternalPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent, ByVal version As PrecedentVersion)

        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = prec.GetStorageProvider()

        Dim item As IStorageItem = version
        If item Is Nothing Then
            item = prec
        End If

        Dim oi As OutlookItem = obj
        Dim dt As DateTime = New DateTime(4501, 1, 1)
        Dim tmppath As System.IO.FileInfo = FWBS.OMS.Global.GetTempFile(item.Extension)
        Dim tmppath2 As System.IO.FileInfo = FWBS.OMS.Global.GetTempFile(item.Extension)

        If oi.SentOn <> dt AndAlso String.IsNullOrEmpty(oi.MessageHeaders) Then
            oi.SentOn = dt
            oi.ReceivedTime = dt
            oi.SaveAs(tmppath.FullName, Outlook.OlSaveAsType.olTemplate)
            oi = ImportPrecedentFile(tmppath, False) ' Workaround: need to reimport precedent file, otherwise SentOn date will not be cleared
        End If

        oi.MessageHeaders = Text.RegularExpressions.Regex.Replace(If(oi.MessageHeaders, String.Empty), "^Date:.+\n*", String.Empty, Text.RegularExpressions.RegexOptions.CultureInvariant Or Text.RegularExpressions.RegexOptions.Multiline)
        oi.SentOn = dt
        oi.SaveAs(tmppath.FullName, Outlook.OlSaveAsType.olTemplate)
        oi.SaveAs(tmppath2.FullName, Outlook.OlSaveAsType.olTemplate)

        Dim storeres As StoreResults = provider.Store(item, tmppath, obj, True, Me)

        'Resave the document to the the locally cached location of the document so that it frees up the temp file
        Dim file As System.IO.FileInfo = provider.GetLocalFile(storeres.Item)

        oi.SaveAs(file.FullName, Outlook.OlSaveAsType.olTemplate)
        StorageManager.CurrentManager.LocalDocuments.Set(storeres.Item, file, True)

        Try
            If oi IsNot obj Then DeleteItem(oi, True)
        Catch
        End Try
        Try
            tmppath.Delete()
        Catch
        End Try
        Try
            tmppath2.Delete()
        Catch
        End Try

    End Sub


    Protected Overrides Sub EndDocumentSave(ByVal obj As Object, ByVal saveMode As FWBS.OMS.PrecSaveMode, ByVal printMode As FWBS.OMS.PrecPrintMode, ByVal doc As OMSDocument, ByVal version As DocumentVersion)

        MyBase.EndDocumentSave(obj, saveMode, printMode, doc, version)

        Call CheckObjectIsDoc(obj)

        Dim oi As OutlookItem = obj

        Try
            oi.Save()


            'INFO: The following code was to be used for auto sending on a process job email
            'but too many problems appear to occur.
            If HasDocVariable(oi, SENTON) = False And GetActiveDocDirection(oi, Nothing) = DocumentDirection.Out And saveMode <> PrecSaveMode.None Then
                oi.Synchronise()
                Move(oi)
                oi.Send()
                'If the item was not sent then stop the item from closing or moving
                If (Not oi.IsDeleted) Then
                    doc.ContinueAfterSave = True
                End If
            End If

        Catch invcomex As System.Runtime.InteropServices.InvalidComObjectException
            Debug.WriteLine(invcomex, "EndDocumentSave")
        Catch ex As Exception
            MessageBox.Show(ActiveWindow, ex)
        End Try
    End Sub

    Protected Overrides Sub EndPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent)

        Dim win As IWin32Window = App.GetWindow(obj)

        MyBase.EndPrecedentSave(obj, saveMode, printMode, prec)

        Call CheckObjectIsDoc(obj)

        Try
            obj.Save()
            OutlookOMS.DeleteItem(obj, True)
        Catch invcomex As System.Runtime.InteropServices.InvalidComObjectException
            Debug.WriteLine(invcomex, "EndPrecedentSave")
        Catch ex As Exception
            MessageBox.Show(win, ex)
        End Try

    End Sub

    Friend Sub MoveItem(ByVal obj As OutlookItem)
        Move(obj)
    End Sub

    Protected Overrides Sub Move(ByVal obj As Object)
        Try
            If (Not IsCompanyDocument(obj)) Then
                Return
            End If


            Dim moveoption As EmailSaveLocation = CheckEmailSaveLocationOption(obj)
            Dim item As OutlookItem = obj

            Select Case moveoption
                Case EmailSaveLocation.Delete
                    If item.Sent OrElse Not item.HasProperty("SaveSentMessageFolder") Then
                        OutlookOMS.DeleteItem(item)
                    Else
                        item.SetProperty("SaveSentMessageFolder", item.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems))
                    End If
                Case EmailSaveLocation.Move
                    FileIt(obj)
                Case EmailSaveLocation.Leave

            End Select



        Catch ex As Exception
            WriteLog("Moving Profiled Mail Error", String.Format("An error has occured whilst trying to move the profiled email '{0}'.", obj.Subject), "Refer to the following exception...", ex)
        End Try
    End Sub

    Protected Overrides Function AttachSubDocuments(ByVal obj As Object) As SubDocument()
        Call CheckObjectIsDoc(obj)
        Dim att As Outlook.Attachment
        Dim item As OutlookItem = obj

        Dim subdocs As New System.Collections.Generic.List(Of SubDocument)

        If (item.Attachments Is Nothing) Then
            Return subdocs.ToArray()
        End If

        Dim excludeExtensions As New HashSet(Of String)(Session.CurrentSession.ExcludeFileExtensions.Split("|"c), StringComparer.InvariantCultureIgnoreCase)
        Dim maxFileSize As Long = Session.CurrentSession.ExcludeFileSize * 1024

        Dim pdfConverted As Boolean = HasDocVariable(item, PDF_CONVERTED)
        For Each att In item.Attachments
            Try
                Dim displayName As String = att.DisplayName
                Dim tmp As System.IO.FileInfo
                If (att.Type = Outlook.OlAttachmentType.olByReference) Then
                    tmp = New System.IO.FileInfo(att.PathName)
                Else
                    Dim ext As String = System.IO.Path.GetExtension(att.FileName)
                    If (att.Type = 7) Then ' OlAttachmentType.olByWebReference
                        ext = ".url"
                        displayName &= ext
                    End If
                    tmp = FWBS.OMS.[Global].GetTempFile(System.IO.Path.GetFileNameWithoutExtension(att.FileName), ext)
                    tmp = New System.IO.FileInfo(System.IO.Path.Combine(tmp.Directory.FullName, Guid.NewGuid().ToString("N"), tmp.Name))
                    tmp.Directory.Create()
                    att.SaveAsFile(tmp.FullName)
                End If

                Dim subdoc As SubDocument = New SubDocument(displayName, tmp)

                If (obj.ReceivedTime <> MAX_DATE) Then
                    subdoc.AuthoredDate = item.ReceivedTime
                End If


                If (Not String.IsNullOrEmpty(item.SenderName)) Then
                    subdoc.From = item.SenderName
                Else
                    subdoc.From = item.SenderEmailAddress
                End If

                subdoc.To = item.To
                subdoc.CC = item.CC
                subdoc.BCC = item.BCC


                'this determins if the item will be selected by default or not
                Dim store As Boolean = CheckEmailOption(EmailOption.optAutoSaveAtt)
                Dim extension As String = tmp.Extension.TrimStart("."c)
                If store Then
                    If (excludeExtensions.Contains(extension) AndAlso tmp.Length <= maxFileSize) Then
                        store = False
                    End If
                End If

                'CM 26.11.13 (WI:2920) - This should always run, regardless of AutoSaveAttachment option
                If extension.Equals("PDF", StringComparison.InvariantCultureIgnoreCase) Then
                    If pdfConverted Then
                        'Uncheck converted PDF Documents by default
                        store = False
                        'Incase user wishes to save the pdf, save as new OMSDocument
                        subdoc.Detach()
                    End If
                End If

                subdoc.Store = store
                subdocs.Add(subdoc)
            Catch ex As Exception
                WriteLog("Extracting Attachments Error", String.Format("An error has occurred whilst extracting the attachments of the message item '{0}.", item.Subject), "Refer to the following exception...", ex)
            End Try

        Next
        RemoveDocVariable(item, PDF_CONVERTED)
        Return subdocs.ToArray()
    End Function

    Protected Overrides Function GenerateDocDesc(ByVal obj As Object) As String
        Dim strbuild As String = MyBase.GenerateDocDesc(obj)
        Dim override As String = obj.Subject
        If override <> "" Then
            strbuild = override
        End If
        Return strbuild
    End Function

    Public Function GetDocumentToAuthorise(ByVal obj As Object) As DocumentVersion
        Try
            CheckObjectIsDoc(obj)

            If HasDocVariable(obj, AUTHORISE_DOCUMENT) Then
                'Add the authorise button if applicable.
                Dim authorisedocid As Long = Convert.ToInt64(GetDocVariable(obj, AUTHORISE_DOCUMENT, -1))
                Dim authorisedversionid As Guid = New Guid(Convert.ToString(GetDocVariable(obj, AUTHORISE_DOCUMENT_VERSION, Guid.Empty.ToString())))

                Dim doc As OMSDocument = OMSDocument.GetDocument(authorisedocid)
                If (Not doc Is Nothing) Then
                    If authorisedversionid <> Guid.Empty Then
                        Return DirectCast(doc, IStorageItemVersionable).GetVersion(authorisedversionid)
                    Else
                        Return DirectCast(doc, IStorageItemVersionable).GetLatestVersion()
                    End If
                End If
                Return Nothing
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function OutwardAuthorisation(ByVal obj As Object) As Boolean
        Dim outauth As Boolean = False

        CheckObjectIsDoc(obj)

        outauth = Not GetDocumentToAuthorise(obj) Is Nothing

        If (outauth) Then

            If HasDocVariable(obj, AUTHORISE_RETURN) Then
                outauth = Not GetDocVariable(obj, AUTHORISE_RETURN)
            End If

        End If

        Return outauth

    End Function

    Public Function ReturnedAuthorisation(ByVal obj As Object) As Boolean
        Dim returnedauth As Boolean = False

        CheckObjectIsDoc(obj)

        returnedauth = Not GetDocumentToAuthorise(obj) Is Nothing

        If (returnedauth) Then

            If HasDocVariable(obj, AUTHORISE_RETURN) Then
                returnedauth = GetDocVariable(obj, AUTHORISE_RETURN)
            Else
                returnedauth = False
            End If

        End If

        Return returnedauth
    End Function


    Protected Overrides Function GetActiveDocDirection(ByVal obj As Object, ByVal prec As Precedent) As DocumentDirection
        Call CheckObjectIsDoc(obj)
        Dim pi As IProfileItem = GetProfileItem(obj)
        If (pi Is Nothing) Then
            Return DocumentDirection.In
        Else
            Return pi.GetDocumentDirection(Me, obj, prec)
        End If
    End Function

    Public Overrides ReadOnly Property DefaultDocType() As String
        Get
            If App.ActiveExplorer.CurrentFolder.DefaultItemType = Outlook.OlItemType.olMailItem Then
                Return "EMAIL"
            Else
                Return ""
            End If
        End Get
    End Property

    Public Overrides Function GetActiveDocType(ByVal obj As Object) As String
        Dim ret As String = MyBase.GetActiveDocType(obj)

        Dim pi As IProfileItem = GetProfileItem(obj)
        If (pi Is Nothing) Then
            If ret = "" Then
                Return "EMAIL"
            Else
                Return ret
            End If
        Else

            Return pi.GetDefaultDocType(Me, obj)
        End If
    End Function

#End Region

#Region "Template Routines"

    Public Sub AttachRecipients(ByVal [property] As String, ByVal doc As Outlook.MailItem, ByVal file As OMSFile, Optional ByVal prompt As Boolean = False, Optional ByVal email_list As String = "", Optional ByVal assocPrompt As Boolean = True)
        Dim win As IWin32Window = App.GetWindow(doc)
        Dim result As DialogResult = DialogResult.Yes

        If (prompt) Then
            Dim msg As ResourceItem

            Select Case [property]
                Case "TO"
                    msg = Session.CurrentSession.Resources.GetMessage("EMAILASSOCTO", "Would you like to add another associate of the %FILE% to the recipient list?", "")
                Case "CC"
                    msg = Session.CurrentSession.Resources.GetMessage("EMAILCOPYASSOC", "Would you like to copy this email to an associate of the %FILE%?", "")
                Case "BCC"
                    msg = Session.CurrentSession.Resources.GetMessage("EMAILBLCPYASSOC", "Would you like to blind copy this email to an associate of the %FILE%?", "")
                Case Else
                    msg = Session.CurrentSession.Resources.GetMessage("EMAILCOPYASSOC", "Would you like to copy this email to an associate of the %FILE%?", "")

            End Select

            result = MessageBox.Show(win, msg, "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2)
        End If


        If result = System.Windows.Forms.DialogResult.Yes And assocPrompt Then

            Dim assocs As Associate() = Services.Searches.PickAssociates(win, file)

            If (assocs Is Nothing) Then
                Return
            End If

            For Each assoc As Associate In assocs
                If Not assoc Is Nothing Then
                    Dim temp As String = Trim(assoc.DefaultEmail)
                    If temp <> String.Empty Then
                        If (email_list = String.Empty) Then
                            email_list = temp
                        Else
                            email_list &= ";" & temp
                        End If
                    Else

                    End If

                End If
            Next
        End If

        Select Case [property]
            Case "TO", "TO_ADD"
                If (doc.To Is Nothing) Then
                    doc.To = email_list
                Else
                    If doc.To.Length = 0 Then
                        doc.To = email_list
                    Else
                        doc.To &= ";" & email_list
                    End If
                End If

            Case "CC", "CC_ADD"
                If (doc.CC Is Nothing) Then
                    doc.CC = email_list
                Else
                    If doc.CC.Length = 0 Then
                        doc.CC = email_list
                    Else
                        doc.CC &= ";" & email_list
                    End If
                End If
            Case "BCC", "BCC_ADD"
                If (doc.BCC Is Nothing) Then
                    doc.BCC = email_list
                Else
                    If doc.BCC.Length = 0 Then
                        doc.BCC = email_list
                    Else
                        doc.BCC &= ";" & email_list
                    End If
                End If
        End Select

        doc.Recipients.ResolveAll()

    End Sub

    Private Function AppWindow(ByVal obj As Object) As IWin32Window
        Return App.GetWindow(obj)
    End Function

    Protected Overloads Overrides Function TemplateStart(ByVal obj As Object, ByVal preclink As PrecedentLink) As Object


        Dim item As OutlookItem = Nothing
        Dim tmppath As System.IO.FileInfo

        Call ActivateApplication()

        ' Get the Precedent File to Load...
        Dim fetch As FWBS.OMS.DocumentManagement.Storage.FetchResults = preclink.Merge()
        If (fetch Is Nothing) Then
            Return item
        End If

        tmppath = fetch.LocalFile

        If Not tmppath Is Nothing Then

            ' Start new document with fullname returned from Precedent Object
            System.Windows.Forms.Application.DoEvents()

            item = App.CreateItemFromTemplate(tmppath.FullName)

            'Display the item straight away so that the cleanup routine does not dispose of the object if the SelectionChange event fires.
            'This exact scenrio happens in Work Item Valencia_2191.
            item.Display()
            item.GetInspector.Activate()

            'Add the company disclaimer
            If (Session.CurrentSession.Disclaimer <> "") Then
                item.AddBodyText(String.Concat(Environment.NewLine, Environment.NewLine, Session.CurrentSession.Disclaimer), False)
            End If

            'Code to work around issue where with some templates and at some times but only found to be online environment 
            'Email body's where reverted to the content prior to edit or merge
            'MW agrees issue with Outlook in online mode 
            Dim disableSaveOnEmailTemplateStart As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Outlook", "DisableSaveOnEmailTemplateStart", "false")
            Dim disableBoolean As Boolean = disableSaveOnEmailTemplateStart.ToBoolean()

            If disableBoolean Then
                Debug.WriteLine("OMSOffice:SaveOnEmailTemplateStart has been disabled.")
            Else
                Debug.WriteLine("OMSOffice:SaveOnEmailTemplateStart is enabled.")
                Debug.WriteLine(String.Format("OMSOffice:Exchange Mode detected : {0}", item.Application.GetNamespace("MAPI").ExchangeConnectionMode))

                If item.Application.GetNamespace("MAPI").ExchangeConnectionMode = Outlook.OlExchangeConnectionMode.olOnline Then
                    Debug.WriteLine("OMSOffice:Online mode detected.  Performing save of email item.")
                    item.Save()
                End If
            End If

            If (item.RealUserProperties.Count > 0) Then
                item.Save()
            End If


            DettachPrecedentVars(item)

            If TypeOf preclink Is PrecedentJob Then
                Dim job As PrecedentJob = preclink
                Dim assoc As Associate = job.Associate
                Dim ConvertToPDF As Boolean

                If Not job Is Nothing Then
                    If job.Params.Contains("TO") Then
                        If job.Params("TO").Value = True Then
                            AttachRecipients("TO", item, assoc.OMSFile, True)
                        End If
                    End If
                    If job.Params.Contains("TO_ADD") Then
                        AttachRecipients("TO_ADD", item, assoc.OMSFile, False, job.Params("TO_ADD").Value, False)
                    End If
                    If job.Params.Contains("CC") Then
                        If job.Params("CC").Value = True Then
                            AttachRecipients("CC", item, assoc.OMSFile, True)
                        End If
                    End If
                    If job.Params.Contains("CC_ADD") Then
                        AttachRecipients("CC_ADD", item, assoc.OMSFile, False, job.Params("CC_ADD").Value, False)
                    End If
                    If job.Params.Contains("BCC") Then
                        If job.Params("BCC").Value = True Then
                            AttachRecipients("BCC", item, assoc.OMSFile, True)
                        End If
                    End If
                    If job.Params.Contains("BCC_ADD") Then
                        AttachRecipients("BCC_ADD", item, assoc.OMSFile, False, job.Params("BCC_ADD").Value, False)
                    End If
                    If job.Params.Contains("BODY") Then
                        item.Body = job.Params("BODY").Value
                    End If
                    If job.Params.Contains("HTMLBODY") Then
                        item.BodyFormat = Outlook.OlBodyFormat.olFormatHTML
                        item.HTMLBody = job.Params("HTMLBODY").Value
                    End If
                    If job.Params.Contains("SUBJECT") Then
                        item.Subject = job.Params("SUBJECT").Value
                    End If
                    If job.Params.Contains("CONVERTPDF") Then
                        ConvertToPDF = job.Params("CONVERTPDF").Value
                    End If

                    If job.Params.Contains("SENDLINK") Then
                        item.InputAddress = job.Params("SENDLINK").Value
                    End If

                    If job.Params.Contains("ASPDF") Then
                        ConvertToPDF = job.Params("ASPDF").Value
                    End If

                    If job.Params.Contains("ATTACH_ADD") Then
                        If TypeOf job.Params("ATTACH_ADD").Value Is IStorageItem() Then
                            Dim docs() As IStorageItem = job.Params("ATTACH_ADD").Value
                            Dim s As String
                            Dim publishDocShowingMarkUp As Boolean

                            If ConvertToPDF Then
                                publishDocShowingMarkUp = PublishDocumentShowingMarkUp(AppWindow(obj), DocumentFileInfoList(docs))
                            End If

                            s = AddAttachment(docs, item, True, ConvertToPDF, Not publishDocShowingMarkUp)
                            If s.Length > 0 Then
                                Throw New Exception(s)
                            End If


                        ElseIf TypeOf job.Params("ATTACH_ADD").Value Is IStorageItem Then
                            Dim d As IStorageItem = job.Params("ATTACH_ADD").Value
                            AddAttachment(item, d, True, ConvertToPDF)
                        Else
                            item.Attachments.Add(job.Params("ATTACH_ADD").Value, Outlook.OlAttachmentType.olByValue, 1, Session.CurrentSession.Resources.GetResource("DOCSENDISPLAY", "Attached Document", "").Text)
                        End If
                    End If

                    If job.Params.Contains("ATTACH_FILES") Then
                        If TypeOf job.Params("ATTACH_FILES").Value Is System.IO.FileInfo() Then
                            Dim files() As System.IO.FileInfo = job.Params("ATTACH_FILES").Value
                            For Each fi As System.IO.FileInfo In files
                                item.Attachments.Add(fi.FullName, Outlook.OlAttachmentType.olByValue, 1, fi.Name)
                            Next
                        ElseIf TypeOf job.Params("ATTACH_FILES").Value Is System.IO.FileInfo Then
                            Dim fi As System.IO.FileInfo = job.Params("ATTACH_ADD").Value
                            item.Attachments.Add(fi.FullName, Outlook.OlAttachmentType.olByValue, 1, fi.Name)
                        Else
                            item.Attachments.Add(job.Params("ATTACH_ADD").Value, Outlook.OlAttachmentType.olByValue, 1, Session.CurrentSession.Resources.GetResource("DOCSENDISPLAY", "Attached Document", "").Text)
                        End If
                    End If
                End If
            End If

            LinkDocument(item, preclink)
        End If

        Return item

    End Function


    Public Overrides Function ProcessJob(ByVal precjob As PrecedentJob) As ProcessJobStatus
        Dim obj As Object = Nothing
        Dim ret As ProcessJobStatus = ProcessJob(precjob, obj)

        Dim item As OutlookItem = TryCast(obj, OutlookItem)
        If (Not item Is Nothing) Then
            If (Not item.IsDeleted) Then
                If (Not item.Saved And item.IsDraft) Then
                    item.Save()
                    item.Synchronise() 'RJA 14/05/2010 Moved inside of the if statement because this can be null if the autowizard has been cancelled
                End If
            End If
        End If
        Return ret

    End Function
#End Region





#Region "Attachments"

    Private Function AddAttachment(ByVal docs() As IStorageItem, ByVal itm As Object, Optional ByVal silent As Boolean = False, Optional ByVal convertToPDF As Boolean = False, Optional ByVal acceptRevisions As Boolean = True) As String

        Dim exmsg As String = String.Empty
        For Each doc As IStorageItem In docs
            If Not doc Is Nothing Then
                Try
                    Call AddAttachment(itm, doc, silent, convertToPDF, acceptRevisions)
                Catch ex As Exception
                    exmsg = String.Format("{0}{1} ({2}){3}{3}", exmsg, ex.Message, doc.Name, Environment.NewLine)
                End Try
            End If
        Next

        Return exmsg
    End Function


    Public Sub AddAttachment(ByVal obj As Object, ByVal item As IStorageItem, ByVal silent As Boolean, Optional ByVal convertToPDF As Boolean = False, Optional ByVal acceptRevisions As Boolean = True)
        Dim win As IWin32Window = App.GetWindow(obj)

        If item Is Nothing Then
            Throw New ArgumentNullException("item")
        End If

        Dim doc As OMSDocument = Nothing
        Dim version As DocumentVersion = Nothing

        If (TypeOf item Is OMSDocument) Then
            doc = item
        ElseIf TypeOf item Is DocumentVersion Then
            version = item
            doc = version.ParentDocument
        End If

        'DM - 24/11/06
        'This method as a whole now is reused for all attaching and now allows silent attaching.

        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = item.GetStorageProvider()


        Dim fr As FetchResults

        Try

            Dim settings As StorageSettingsCollection = provider.GetDefaultSettings(item, SettingsType.Fetch)
            Dim lsettings As LockableFetchSettings = settings.GetSettings(Of LockableFetchSettings)()
            If Not lsettings Is Nothing Then
                lsettings.CheckOut = False
            End If
            Dim vsettings As VersionFetchSettings = settings.GetSettings(Of VersionFetchSettings)()
            If Not vsettings Is Nothing Then
                vsettings.Version = VersionFetchSettings.FetchAs.Current
            End If

            fr = provider.Fetch(item, True, settings)
        Catch ex As CancelStorageException
            Return
        End Try

        Dim file As System.IO.FileInfo = fr.LocalFile
        Dim fileStream As IO.FileStream = file.OpenRead() ' Prevent file from being discarded from the cache until we make a copy for attachment
        item = fr.Item

        Dim fileid As Long = GetDocVariable(obj, OMSApp.FILE, 0)

        If (silent = False) Then
            If Not doc Is Nothing Then
                If doc.Associate.OMSFile.ID <> fileid Then
                    If FWBS.OMS.UI.Windows.MessageBox.Show(win, Session.CurrentSession.Resources.GetMessage("DOCDIFFFILE2", "This document '%1%' may not relate to the currently selected %FILE%.  Would you still like to attach this document?", "", True, doc.Description), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                        fileStream.Dispose()
                        Return
                    End If
                End If
            End If
        End If

        Dim strDesc As String = doc.Description

        If Not version Is Nothing Then
            strDesc = strDesc & " (" & version.Label & ")"
        End If

        If (silent = False) Then
            strDesc = InputBox.Show(win, Session.CurrentSession.Resources.GetMessage("ADDATTACHDESC2", "Please enter the description for this attachment (This may not work with all versions of Outlook)", "").Text, "", strDesc)
        End If

        If (strDesc = InputBox.CancelText OrElse strDesc.Trim() = "") Then
            fileStream.Dispose()
            Return
        Else

            If strDesc.EndsWith(file.Extension) Then
                strDesc = strDesc.Substring(0, strDesc.Length - file.Extension.Length)
            End If

            strDesc = FWBS.Common.FilePath.ExtractInvalidChars(strDesc)


            Dim attachment As System.IO.FileInfo = FWBS.OMS.Global.GetTempFile(strDesc, file.Extension)
            Dim attachmentpath As String = System.IO.Path.Combine(attachment.Directory.FullName, Guid.NewGuid().ToString("N"), attachment.Name)
            attachment = New System.IO.FileInfo(attachmentpath)
            Try

                If (attachment.Directory.Exists = False) Then
                    attachment.Directory.Create()
                End If

                If (attachment.Exists) Then
                    attachment.Delete()
                End If

                Using fileStream
                    Using attStream As IO.FileStream = attachment.Create()
                        fileStream.CopyTo(attStream)
                    End Using
                End Using
                fileStream = Nothing

                'Convert to PDF if user confirms to do so
                If (Not attachment.Extension.ToLower() = ".pdf") Then
                    'If FWBS.OMS.UI.Windows.MessageBox.Show(win, Session.CurrentSession.Resources.GetMessage("PDF_CONVERT", "Would you like to convert '%1%' to PDF?", "", True, strDesc), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                    If convertToPDF Then
                        Dim convertPDF As New ConvertPDF()
                        Try
                            attachment = New System.IO.FileInfo(convertPDF.SingleFile(attachment, acceptRevisions))
                            SetDocVariable(obj, PDF_CONVERTED, "True")

                            'Write to history to say Document has been converted
                            If version Is Nothing Then
                                doc.GetLatestVersion.AddActivity("CONVERTPDF", "", "")
                            Else
                                version.AddActivity("CONVERTPDF", "", "")
                            End If

                        Catch ex As Exception
                            ErrorBox.Show(ex)
                        End Try

                    End If
                End If


                AttachOutboundDocumentVars(attachment, item)

                If TypeOf obj Is Outlook.Inspector Then
                    obj.CurrentItem.Attachments.Add(attachment.FullName, 1, , strDesc)
                Else
                    obj.Attachments.Add(attachment.FullName, 1, , strDesc)
                End If


            Finally
                Try
                    If (attachment.Directory.Exists) Then
                        attachment.Directory.Delete(True)
                    End If
                Catch
                Finally
                    If Not fileStream Is Nothing Then fileStream.Dispose()
                End Try
            End Try
        End If

    End Sub

#End Region

#Region "Methods"

    Friend Sub ContextMenuButtonClick(ByVal button As Microsoft.Office.Core.CommandBarButton, ByRef cancelDefault As Boolean)
        Try
            If (contextselection Is Nothing) Then
                Return
            End If

            RunCommand(contextselection, button.Tag)

        Catch ex As Exception
            FWBS.OMS.UI.Windows.ErrorBox.Show(ex)
        End Try
    End Sub


    Friend Function CheckEmailSaveLocation(ByVal obj As Object) As String

        Dim moveoption As EmailSaveLocation = CheckEmailSaveLocationOption(obj)

        Dim location As String = Session.CurrentSession.CurrentUser.SavedEmailFolderLocation

        If String.IsNullOrEmpty(location) Then
            location = Session.CurrentSession.SavedEmailFolderLocation
        End If

        Return location
    End Function


    Friend Function CheckEmailSaveLocationOption(ByVal obj As Object) As EmailSaveLocation
        Dim moveoption As String = Nothing

        If (Not obj Is Nothing) Then

            If (GetDocDirection(obj, Nothing) = DocumentDirection.Out) Then
                moveoption = Session.CurrentSession.CurrentUser.SavedSentEmailOption
                If (String.IsNullOrEmpty(moveoption)) Then
                    moveoption = Session.CurrentSession.SavedSentEmailOption
                End If
            End If
        End If

        If (String.IsNullOrEmpty(moveoption)) Then
            moveoption = Session.CurrentSession.CurrentUser.SavedEmailOption
            If (String.IsNullOrEmpty(moveoption)) Then
                moveoption = Session.CurrentSession.SavedEmailOption()
            End If
        End If

        Select Case moveoption
            Case "D"
                Return EmailSaveLocation.Delete
            Case "M"
                Return EmailSaveLocation.Move
            Case Else
                Return EmailSaveLocation.Leave
        End Select
    End Function

    Friend Function CheckEmailOption(ByVal opt As EmailOption) As Boolean
        Select Case opt
            Case EmailOption.optQuickSave
                If ((FWBS.OMS.Session.CurrentSession.CurrentUser.EmailQuickSave = FWBS.Common.TriState.Null And FWBS.OMS.Session.CurrentSession.EmailQuickSave) Or FWBS.OMS.Session.CurrentSession.CurrentUser.EmailQuickSave = FWBS.Common.TriState.True) Then
                    Return True
                Else
                    Return False
                End If
            Case EmailOption.optAutoSaveAtt
                If ((FWBS.OMS.Session.CurrentSession.CurrentUser.AutoSaveAttachments = FWBS.Common.TriState.Null And FWBS.OMS.Session.CurrentSession.AutoSaveAttachments) Or FWBS.OMS.Session.CurrentSession.CurrentUser.AutoSaveAttachments = FWBS.Common.TriState.True) Then
                    Return True
                Else
                    Return False
                End If
            Case EmailOption.optResolveEmail
                If ((FWBS.OMS.Session.CurrentSession.CurrentUser.EmailResolveAddress = FWBS.Common.TriState.Null And FWBS.OMS.Session.CurrentSession.EmailResolveAddress) Or FWBS.OMS.Session.CurrentSession.CurrentUser.EmailResolveAddress = FWBS.Common.TriState.True) Then
                    Return True
                Else
                    Return False
                End If
            Case EmailOption.optUseDefAssoc
                If ((FWBS.OMS.Session.CurrentSession.CurrentUser.UseDefaultAssociate = FWBS.Common.TriState.Null And FWBS.OMS.Session.CurrentSession.UseDefaultAssociate) Or FWBS.OMS.Session.CurrentSession.CurrentUser.UseDefaultAssociate = FWBS.Common.TriState.True) Then
                    Return True
                Else
                    Return False
                End If
            Case EmailOption.optChecksum
                Return Session.CurrentSession.EmailChecksum
        End Select
        Return False
    End Function

    Public Function CheckEmailProfileOption(ByVal opt As EmailProfileOption) As Boolean

        Dim licensed As Boolean = Session.CurrentSession.IsLicensedFor("EMAIL")

        Select Case (Session.CurrentSession.CurrentUser.EmailProfileLevel)
            Case "S", "M", "D", "C"
                Select Case opt
                    Case EmailProfileOption.epoClose
                        Return Session.CurrentSession.CurrentUser.EmailProfileOnClose And licensed
                    Case EmailProfileOption.epoMove
                        Return Session.CurrentSession.CurrentUser.EmailProfileOnMove And licensed
                    Case EmailProfileOption.epoNew
                        Return Session.CurrentSession.CurrentUser.EmailProfileOnNew And licensed
                    Case EmailProfileOption.epoReply
                        Return Session.CurrentSession.CurrentUser.EmailProfileOnReply And licensed
                    Case EmailProfileOption.epoForward
                        Return Session.CurrentSession.CurrentUser.EmailProfileOnForward And licensed
                    Case EmailProfileOption.epoDelete
                        Return Session.CurrentSession.CurrentUser.EmailProfileOnDelete And licensed
                    Case EmailProfileOption.epoAllowEdit
                        Return Session.CurrentSession.CurrentUser.EmailProfileAllowEdit
                End Select
            Case Else
                Select Case opt
                    Case EmailProfileOption.epoClose
                        Return Session.CurrentSession.EmailProfileOnClose And licensed
                    Case EmailProfileOption.epoMove
                        Return Session.CurrentSession.EmailProfileOnMove And licensed
                    Case EmailProfileOption.epoNew
                        Return Session.CurrentSession.EmailProfileOnNew And licensed
                    Case EmailProfileOption.epoReply
                        Return Session.CurrentSession.EmailProfileOnReply And licensed
                    Case EmailProfileOption.epoForward
                        Return Session.CurrentSession.EmailProfileOnForward And licensed
                    Case EmailProfileOption.epoDelete
                        Return Session.CurrentSession.EmailProfileOnDelete And licensed
                    Case EmailProfileOption.epoAllowEdit
                        Return Session.CurrentSession.EmailProfileAllowEdit
                End Select
        End Select
        Return False
    End Function

    Private Sub AddressItem(ByVal item As OutlookItem, ByVal assoc As Associate)
        Dim win As IWin32Window = App.GetWindow(item)

        If item Is Nothing Then Return

        If assoc Is Nothing Then Return

        Dim email As String = assoc.DefaultEmail

        If GetDocVariable(item, ISREPLY, False) = False Then
            If (item.InputAddress = False) Then
                If (Not String.IsNullOrEmpty(email)) Then
                    If String.IsNullOrEmpty(item.To) Then
                        item.To = email
                    End If
                End If
            Else
                item.To = String.Empty
            End If
        Else
            RemoveDocVariable(item, ISREPLY)
        End If

    End Sub

#End Region

#Region "Commands"

    Protected Overrides Sub DisplayDocVariables(ByVal obj As Object, ByVal val As String)

        CheckObjectIsDoc(obj)
        Dim item As OutlookItem = obj

        Dim v As System.Text.StringBuilder = New System.Text.StringBuilder()

        For Each p As Outlook.UserProperty In item.UserProperties
            v.Append(p.Name)
            v.Append(": ")
            v.Append(Convert.ToString(p.Value))
            v.Append(Environment.NewLine)
        Next

        MsgBox(v.ToString(), , "Item Properties")
    End Sub
    Private Sub CommandAttachDocumentByRef(ByVal itm As Object, ByVal askVersions As Boolean, Optional ByVal attachPDF As Boolean = False)
        Try
            Call CheckObjectIsDoc(itm)
            Dim strdocid As String = InputBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("ATTACHDOCREF", "Please enter the document reference:", "").Text, "")
            If (strdocid = InputBox.CancelText Or strdocid.Trim() = "") Then
                Return
            End If

            Dim list As System.Collections.Generic.List(Of IStorageItem) = New System.Collections.Generic.List(Of IStorageItem)
            Dim doc As OMSDocument = OMSDocument.GetDocument(strdocid)
            If doc Is Nothing Then
                Return
            End If


            If (askVersions) Then
                If (doc.GetVersions().Length <> 1) Then
                    Dim vp As FWBS.OMS.UI.Windows.DocumentManagement.DocumentVersionPicker = New FWBS.OMS.UI.Windows.DocumentManagement.DocumentVersionPicker
                    vp.Title = Session.CurrentSession.Resources.GetMessage("PICKDOCVER2ATT", String.Format("Pick Version to Attach for {0}", doc.DisplayID), "").Text
                    vp.Document = doc
                    Dim svers As IStorageItem() = vp.Show(ActiveWindow)
                    If svers Is Nothing Then
                        Return
                    End If

                    list.AddRange(svers)
                Else
                    list.Add(doc)
                End If
            Else
                list.Add(doc)
            End If

            Call AddAttachment(list.ToArray(), itm, False, attachPDF)

        Catch ex As Exception
            FWBS.OMS.UI.Windows.ErrorBox.Show(ex)
        End Try
    End Sub

    Private Sub ConvertAttachmentToPDF(ByVal obj As Object)
        Dim email As Outlook.MailItem = TryCast(obj, Outlook.MailItem)
        Dim mailatt As Outlook.Attachments = email.Attachments
        Dim oatt As Outlook.Attachment
        Dim convertedAttachments As List(Of NamedAttachment) = New List(Of NamedAttachment)
        Dim olkPA As Outlook.PropertyAccessor
        Dim IsEmbedded As Boolean = False
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim publishDocumentsWithMarkup As Boolean = PublishDocumentShowingMarkUp(AppWindow(obj), EmailAttachmentFileInfoList(email.Attachments))

        For i As Integer = mailatt.Count To 1 Step -1

            Dim tempAttachmentFileName As String = FWBS.OMS.Global.GetTempPath().ToString() & "\" & mailatt(i).FileName
            Dim attachment As System.IO.FileInfo = New System.IO.FileInfo(tempAttachmentFileName)

            olkPA = mailatt.Item(i).PropertyAccessor
            Dim ContentID As String = mailatt(i).PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x3712001E")
            If ContentID <> "" Then
                IsEmbedded = True
            End If

            If (Not attachment.Extension.ToLower() = ".pdf") And (IsEmbedded = False) Then
                Dim convertPDF As New ConvertPDF()
                Try
                    mailatt(i).SaveAsFile(tempAttachmentFileName)
                    Dim convertedAttachment As NamedAttachment = New NamedAttachment()
                    attachment = New System.IO.FileInfo(convertPDF.SingleFile(tempAttachmentFileName, Not publishDocumentsWithMarkup))
                    convertedAttachment.FileName = attachment.FullName
                    convertedAttachment.DisplayName = attachment.Name.Remove(attachment.Name.IndexOf(attachment.Extension))
                    convertedAttachments.Add(convertedAttachment)
                    SetDocVariable(obj, PDF_CONVERTED, "True")
                    mailatt.Remove(i)
                Catch ex As Exception
                    ErrorBox.Show(ex)
                End Try
            End If
        Next

        For Each attachment As NamedAttachment In convertedAttachments
            oatt = email.Attachments.Add(attachment.FileName, Outlook.OlAttachmentType.olByValue, 1, attachment.DisplayName)
        Next
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        email.Save()
        email.Refresh()
        email.Display()
    End Sub

    Private Sub ConvertAttachDocument(ByVal itm As Object, Optional ByVal attachPDF As Boolean = False)
        Call CheckObjectIsDoc(itm)
    End Sub

    Private Sub CommandAttachDocument(ByVal itm As Object, ByVal askVersions As Boolean, Optional ByVal attachPDF As Boolean = False)

        Call CheckObjectIsDoc(itm)
        Dim assocobj As FWBS.OMS.Associate = AttachDocumentVars(itm, CheckEmailOption(EmailOption.optUseDefAssoc), GetCurrentAssociate(itm))
        If assocobj Is Nothing Then
            Return
        End If

        Dim docs() As OMSDocument
        Dim exmsg As String = String.Empty

        docs = FWBS.OMS.UI.Windows.Services.Searches.PickDocuments(ActiveWindow, assocobj.OMSFile)

        Dim list As System.Collections.Generic.List(Of IStorageItem) = New System.Collections.Generic.List(Of IStorageItem)
        If docs Is Nothing Then
            Return
        End If

        If (askVersions) Then
            For Each doc As OMSDocument In docs
                If (doc.GetVersions().Length <> 1) Then
                    Dim vp As FWBS.OMS.UI.Windows.DocumentManagement.DocumentVersionPicker = New FWBS.OMS.UI.Windows.DocumentManagement.DocumentVersionPicker
                    vp.MultiSelect = True
                    vp.Title = Session.CurrentSession.Resources.GetMessage("PICKDOCVER2ATT", String.Format("Pick Version to Attach for {0}", doc.DisplayID), "").Text
                    vp.Document = doc
                    Dim svers As IStorageItem() = vp.Show(ActiveWindow)

                    If svers Is Nothing Then
                        Continue For
                    End If

                    list.AddRange(svers)

                Else
                    list.Add(doc)
                End If
            Next
        Else
            list.AddRange(docs)
        End If


        If attachPDF Then
            exmsg &= AddAttachment(list.ToArray(), itm, False, attachPDF, Not PublishDocumentShowingMarkUp(AppWindow(itm), DocumentFileInfoList(docs)))
        Else
            exmsg &= AddAttachment(list.ToArray(), itm, False, attachPDF, Not Session.CurrentSession.PromptToPublishPDFWithTrackChanges)
        End If

        If exmsg <> String.Empty Then
            MessageBox.Show(ActiveWindow, exmsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub ReplyToAuthorisation(ByVal mail As Outlook.MailItem, ByVal version As DocumentVersion, ByVal authorised As Boolean)

        Dim command As FWBS.OMS.UI.DocumentManagement.ReplyToAuthoriseCommand = New FWBS.OMS.UI.DocumentManagement.ReplyToAuthoriseCommand()
        command.AllowUI = True
        command.Authorised = authorised
        command.ContinueOnError = False
        command.Item = version
        command.OriginalMessage = mail.Body
        command.To = mail.SenderEmailAddress
        command.Execute()

    End Sub

    Private Sub FileIt(ByVal obj As Object)
        Dim item As OutlookItem = obj
        Dim path As String = CheckEmailSaveLocation(obj)
        If (String.IsNullOrEmpty(path)) Then
            Return
        End If
        Dim doc As OMSDocument = Me.GetCurrentDocument(obj)
        If (Not doc Is Nothing) Then
            Dim fp As FieldParser = New FieldParser(doc.Associate)
            path = fp.ParseString(path, "")
            While (path.IndexOf("\\") > 0)
                path = path.Replace("\\", "\")
            End While
            Dim created As Boolean = False

            'NOTES: DJRM - 08/06/05 - If the parent folder of the mail items 
            'is the public folder store just store into the local mailbox.
            Dim f As OutlookFolder = Nothing

            Try
                If (TypeOf item.Parent Is Outlook.MAPIFolder) Then
                    If item.Parent.StoreID = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders).StoreID Then
                        f = CreateFolder(Nothing, path.Split("\"), True, created)
                    End If
                End If
            Catch
            End Try

            If f Is Nothing Then
                f = CreateFolder(_ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox), path.Split("\"), True, created)
            End If


            Call SetAsProfileFolder(f, doc.OMSFile, created)

            If Not item.HasProperty("SaveSentMessageFolder") OrElse item.Sent Then
                OutlookOMS.MoveItem(obj, f)
            Else
                item.SetProperty("SaveSentMessageFolder", f)
            End If

        End If
    End Sub

    Public Sub SetAsProfileFolder(ByVal folder As Outlook.MAPIFolder, ByVal file As OMSFile, ByVal forceView As Boolean)

        Session.CurrentSession.CheckLoggedIn()

        Try
            'IMPORTANT: Keep this as an object otherwise Office12 on onwards think the hidden message is a StorageItem 
            'rather than a MailItem
            Dim fm As Object = GetFolderMessage(folder, True)
            SetDocVariable(fm, OMSApp.COMPANY, Session.CurrentSession.CompanyID)
            SetDocVariable(fm, OMSApp.DATAKEY, Session.CurrentSession.DataKey)
            SetDocVariable(fm, OMSApp.BRANCH, Session.CurrentSession.SerialNumber)
            SetDocVariable(fm, OMSApp.CLIENT, file.Client.ClientID)
            SetDocVariable(fm, OMSApp.FILE, file.ID)
            'Added this as Outlook 2003 does not detect a change in the out item.
            fm.Body = "OMS Folder Settings - " & DateTime.UtcNow.ToString()
            fm.Save()
        Catch ex As Exception
            WriteLog("Folder Properties Error", String.Format("An error has occured whilst trying to set the folder properties for the profiled mail folder '{0}'.  Dragging and dropping to profile an email may not work for this folder.", folder.FolderPath), "Add the required users mail store to your mail profile and make sure you have permissions to read and write.", ex)
        End Try

        SetFolderView(folder, forceView)

    End Sub

    Friend ReadOnly Property CheckNamesOnSend() As Boolean
        Get
            Dim reg As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "OutlookUseChecknames", True)
            Return reg.ToBoolean()
        End Get
    End Property

    Friend Function SaveProfiledEmail(ByVal obj As Object) As Boolean

        If CheckEmailSaveLocationOption(obj) = EmailSaveLocation.Move Then

            If FWBS.OMS.UI.Windows.MessageBox.Show(Nothing, Session.CurrentSession.Resources.GetMessage("MOVEEMAIL_OPT", "Please note this email is already profiled. Do you want to perform the File Email option?", ""), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                Call CheckObjectIsDoc(obj)
                FileIt(obj)
                Return True
            End If
        ElseIf CheckEmailSaveLocationOption(obj) = EmailSaveLocation.Leave Then
            FWBS.OMS.UI.Windows.MessageBox.Show(Nothing, Session.CurrentSession.Resources.GetMessage("LEAVEEMAIL_OPT", "Please note this email is already profiled.", ""), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            Return True

        ElseIf CheckEmailSaveLocationOption(obj) = EmailSaveLocation.Delete Then
            If FWBS.OMS.UI.Windows.MessageBox.Show(Nothing, Session.CurrentSession.Resources.GetMessage("DELETEEMAIL_OPT", "Please note this email is already profiled. Do you want move to Deleted Items?", ""), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                Call CheckObjectIsDoc(obj)
                Move(obj)
                Return True
            End If
        End If
        Return False
    End Function

    Friend Sub SaveAndSend(ByVal obj As Object)
        Call CheckObjectIsDoc(obj)


        If (CheckNamesOnSend) Then
            ExecuteCheckNames(obj)
        End If

        If obj.Recipients.Count = 0 Then
            MessageBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("EMAILTOEMPTY", "The To Field must be filled in before the email is sent.", ""), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
        Else
            Dim doc As OMSDocument = Nothing
            Dim assocobj As FWBS.OMS.Associate = AttachDocumentVars(obj, CheckEmailOption(EmailOption.optUseDefAssoc), GetCurrentAssociate(obj))
            If assocobj Is Nothing Then
                Return
            End If

            Dim pi As IProfileItem = GetProfileItem(obj)
            If (pi Is Nothing) Then
                Return
            End If

            Dim args As BeforeItemEventArgs = New BeforeItemEventArgs(obj, False)
            pi.BeforeSend(Me, args)
            If (args.Cancel) Then
                Return
            End If

            obj.Send()
        End If
    End Sub


    Private Sub FileAll()
        Dim win As IWin32Window = App.GetWindow(App)

        Dim itms As ArrayList = New ArrayList

        Dim exp As Outlook.Explorer = App.ActiveExplorer()

        Dim sel As Outlook.Selection = exp.Selection

        Dim failed As Integer = 0

        Try
            Using (App.BeginProcess())
                Using items As DetachableItems = New DetachableItems(App, sel)

                    Dim ctr As Integer = 1
                    Dim count As Integer = items.Count

                    OnProgressStart(Session.CurrentSession.Resources.GetResource("FILING", "Filing", "").Text,
                                    Session.CurrentSession.Resources.GetResource("FILINGSELITEMS", "Filing the selected items.", "").Text,
                                    count)

                    For Each item As OutlookItem In items

                        OnProgress(Session.CurrentSession.Resources.GetResource("FILINGITEMS", "Filing %1%/%2% Items - Failed: %3%.", "", ctr.ToString(), count.ToString(), failed.ToString()).Text, ctr)

                        Dim doc As OMSDocument = Me.GetCurrentDocument(item)

                        If Not doc Is Nothing Then
                            FileIt(item)
                        Else
                            failed += 1
                        End If

                        ctr += 1
                    Next
                End Using
            End Using

        Finally
            OnProgressFinished()
        End Try

        If (failed > 0) Then
            MessageBox.ShowInformation("FILEALLFAILED", "%1% items were not profiled", Convert.ToString(failed))
        End If


    End Sub

    Private Sub SaveAllEx()
        Dim exp As OutlookExplorer = App.ActiveExplorer()
        Dim args As BeforeSaveItemsEventArgs = New BeforeSaveItemsEventArgs(exp, exp.Selection, True, EventSource.None)
        SaveItemsExAction(args)
    End Sub

    Private Sub SaveAll(ByVal quick As Boolean)
        Dim exp As OutlookExplorer = App.ActiveExplorer()
        Dim args As BeforeSaveItemsEventArgs = New BeforeSaveItemsEventArgs(exp, exp.Selection, True, EventSource.None)
        SaveItems(args, quick)
    End Sub

    Private Sub RunModelessWizard(ByVal cmd As String, Optional ByVal param As Object = Nothing)
        Dim form As Form = CreateModelessWizard(cmd, WizardStyle.Dialog, param)
        If (Not form Is Nothing) Then

            AddHandler form.FormClosed,
            Sub(sender As Form, e As FormClosedEventArgs)
                If (sender.DialogResult = DialogResult.OK AndAlso Not sender.Tag Is Nothing) Then
                    param = sender.Tag

                    If (TypeOf param Is OMSFile) Then
                        If Convert.ToBoolean(form.GetType().GetProperty("ViewFile")?.GetValue(form)) Then
                            Services.ShowFile(DirectCast(param, OMSFile))
                        End If
                        If DirectCast(param, OMSFile).GenerateJobsOnCreation Then
                            Services.CheckJobList(Me)
                        End If
                    ElseIf (TypeOf param Is Client) Then
                        If Convert.ToBoolean(form.GetType().GetProperty("ViewClient")?.GetValue(form)) Then
                            Services.ShowClient(DirectCast(param, Client))
                        End If
                    ElseIf (TypeOf param Is Contact) Then
                        Services.ShowContact(DirectCast(param, Contact))
                    End If
                End If
            End Sub

            form.ShowModeless(ActiveWindow)
        End If
    End Sub

#End Region

#Region "Folder Routines"

    Friend Function CreateFolder(ByVal parent As Outlook.MAPIFolder, ByVal path As String(), Optional ByVal useFolderRoot As Boolean = False, Optional ByRef created As Boolean = False) As Outlook.MAPIFolder
        Return App.CreateFolder(parent, path, useFolderRoot, created)
    End Function

    Public Function GetFolderMessage(ByVal folder As Outlook.MAPIFolder, ByVal create As Boolean) As OutlookItem
        folder = App.GetFolder(folder)
        If (Session.CurrentSession.IsLoggedIn) Then
            Return DirectCast(folder, OutlookFolder).GetHiddenMessage(Session.CurrentSession.CompanyID & "_" & Session.CurrentSession.DataKey, create)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetFolderFile(ByVal folder As Outlook.MAPIFolder) As OMSFile
        Dim clid As Long = 0
        Dim fileid As Long = 0
        Dim compid As Long = 0
        Dim datakey As String = String.Empty


        Try
            'IMPORTANT: Keep this as an object otherwise Office12 on onwards think the hidden message is a StorageItem 
            'rather than a MailItem
            Dim fm As OutlookItem = GetFolderMessage(folder, False)

            If Not fm Is Nothing Then

                fileid = FWBS.Common.ConvDef.ToInt64(GetDocVariable(fm, OMSApp.FILE, 0), 0)
                If (fileid = 0) Then
                    fm.Refresh()
                    fileid = FWBS.Common.ConvDef.ToInt64(GetDocVariable(fm, OMSApp.FILE, 0), 0)
                End If
                compid = FWBS.Common.ConvDef.ToInt64(GetDocVariable(fm, OMSApp.COMPANY, 0), 0)
                datakey = Convert.ToString(GetDocVariable(fm, OMSApp.DATAKEY, ""))
                clid = FWBS.Common.ConvDef.ToInt64(GetDocVariable(fm, OMSApp.CLIENT, 0), 0)



            End If


        Catch ex As Exception
            WriteLog("Folder Properties Error", String.Format("An error has occured whilst trying to read the folder properties for the profiled mail folder '{0}'.  The folder may not be recognised as a profiled folder when pasting / moving mail items.", folder.FolderPath), "Add the required users mail store to your mail profile and make sure you have permissions to read and write.", ex)
        End Try



        If clid <> 0 _
            And fileid <> 0 _
            And (compid = 0 Or compid = Session.CurrentSession.CompanyID) _
            And (String.IsNullOrEmpty(datakey) Or datakey.ToUpper().Equals(Session.CurrentSession.DataKey)) Then
            Return OMSFile.GetFile(fileid)
        End If

        Return Nothing
    End Function

    Friend Sub SetFolderView(ByVal folder As Outlook.MAPIFolder, Optional ByVal force As Boolean = False)

        Try

            Dim vw As Outlook.View = folder.Views("OMS")
            If Not vw Is Nothing Then vw.Delete()
            vw = folder.Views("OMS (Default)")
            If vw Is Nothing Then
                vw = folder.Views.Add("OMS (Default)", Outlook.OlViewType.olTableView, Outlook.OlViewSaveOption.olViewSaveOptionAllFoldersOfType)
                Dim strm As System.IO.Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("FWBS.OMS.UI.Windows.Office.OMSVIEW.xml")
                Dim xdoc As New System.Xml.XmlDocument
                Dim reader As New System.IO.StreamReader(strm)
                xdoc.LoadXml(reader.ReadToEnd())
                reader.Close()
                vw.LockUserChanges = False
                Dim xml As String = xdoc.OuterXml

                xml = xml.Replace("%COMPANYID%", Session.CurrentSession.CompanyID.ToString())
                xml = xml.Replace("%DOCID%", Session.CurrentSession.Resources.GetResource("DOCID", "Doc Id", "").Text)
                xml = xml.Replace("%CLNO%", Session.CurrentSession.Resources.GetResource("CLNO", "%CLIENT% No", "").Text)
                xml = xml.Replace("%FILENO%", Session.CurrentSession.Resources.GetResource("FILENO", "%FILE% No", "").Text)
                xml = xml.Replace("%CLNAME%", Session.CurrentSession.Resources.GetResource("CLNAME", "%CLIENT% Name", "").Text)
                xml = xml.Replace("%FILEDESC%", Session.CurrentSession.Resources.GetResource("FILEDESC", "%FILE% Desc", "").Text)
                xml = xml.Replace("%ASSOCNAME%", Session.CurrentSession.Resources.GetResource("ASSOCNAME", "%ASSOCIATE%", "").Text)

                'Replace the field names incase namespace prefix is used.
                xml = xml.Replace("%COMPANYID_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("COMPANYID"))
                xml = xml.Replace("%DOCID_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("DOCID"))
                xml = xml.Replace("%CLNO_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("CLNO"))
                xml = xml.Replace("%FILENO_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("FILENO"))
                xml = xml.Replace("%CLNAME_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("CLNAME"))
                xml = xml.Replace("%FILEDESC_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("FILEDESC"))
                xml = xml.Replace("%ASSOCNAME_PROP%", Session.CurrentSession.GetExternalDocumentPropertyName("ASSOCNAME"))

                vw.XML = xml
                vw.LockUserChanges = True
                vw.Save()
            End If
            If (force) Then
                vw.Apply()
            End If
        Catch ex As Exception
            WriteLog("Folder View Creation Error", String.Format("Unable to create the OMS folder view for '{0}' for the profiled email.", folder.FolderPath), String.Format("Please check the user's permissions for folder creation and edting for '{0}'.", folder.FolderPath), ex)
        End Try
    End Sub


#End Region

#Region "Application"

    Private _app As FWBS.Office.Outlook.OutlookApplication

    Friend Property App() As FWBS.Office.Outlook.OutlookApplication
        Get
            Return _app
        End Get
        Set(ByVal value As FWBS.Office.Outlook.OutlookApplication)
            If Not value Is _app Then
                RemoveApplicationDelegates()
            End If
            _app = value

            AddApplicationDelegates()
        End Set
    End Property

    Private Sub AddApplicationDelegates()
        RemoveApplicationDelegates()


        If (Not App Is Nothing) Then
            If Me.IsAddinInstance Then
                AddHandler App.Closing, AddressOf App_Closing
                AddHandler App.ItemSend, AddressOf App_ItemSend
                AddHandler App.BeforeDeleteItems, AddressOf App_BeforeDeleteItems
                AddHandler App.BeforeMoveItems, AddressOf App_BeforeMoveItems
                AddHandler App.BeforeOpenItems, AddressOf App_BeforeOpenItems
                AddHandler App.BeforePrintItems, AddressOf App_BeforePrintItems
                AddHandler App.BeforeSaveItems, AddressOf App_BeforeSaveItems

                AddHandler App.BeforeCloseItem, AddressOf App_BeforeCloseItem
                AddHandler App.BeforeOpenItem, AddressOf App_BeforeOpenItem
                AddHandler App.BeforeReplyItem, AddressOf App_BeforeReplyItem
                AddHandler App.BeforeForwardItem, AddressOf App_BeforeForwardItem
                AddHandler App.BeforeDeleteItem, AddressOf App_BeforeDeleteItem
                AddHandler App.BeforeActivateItem, AddressOf App_BeforeActivateItem



                If ApplicationVersion <= 12 Then
                    AddHandler App.ItemContextMenuDisplay, AddressOf App_ItemContextMenuDisplay
                End If
            Else
                'Remove this as it is an application event when automating Outlook as messages seems to stay open
                'AddHandler App.Quit, AddressOf App_Quit
            End If
        End If
    End Sub

    Private Sub RemoveApplicationDelegates()
        On Error Resume Next

        If Not App Is Nothing Then

            RemoveHandler App.Closing, AddressOf App_Closing
            RemoveHandler App.ItemSend, AddressOf App_ItemSend
            RemoveHandler App.Quit, AddressOf App_Quit

            RemoveHandler App.BeforeDeleteItems, AddressOf App_BeforeDeleteItems
            RemoveHandler App.BeforeMoveItems, AddressOf App_BeforeMoveItems
            RemoveHandler App.BeforeOpenItems, AddressOf App_BeforeOpenItems
            RemoveHandler App.BeforePrintItems, AddressOf App_BeforePrintItems
            RemoveHandler App.BeforeSaveItems, AddressOf App_BeforeSaveItems

            RemoveHandler App.BeforeCloseItem, AddressOf App_BeforeCloseItem
            RemoveHandler App.BeforeOpenItem, AddressOf App_BeforeOpenItem
            RemoveHandler App.BeforeReplyItem, AddressOf App_BeforeReplyItem
            RemoveHandler App.BeforeForwardItem, AddressOf App_BeforeForwardItem
            RemoveHandler App.BeforeDeleteItem, AddressOf App_BeforeDeleteItem


            If ApplicationVersion <= 12 Then
                RemoveHandler App.ItemContextMenuDisplay, AddressOf App_ItemContextMenuDisplay
            End If
        End If
    End Sub

    Private contextselection As OutlookItem
    Private Sub App_ItemContextMenuDisplay(ByVal CommandBar As Microsoft.Office.Core.CommandBar, ByVal Selection As Outlook.Selection)

        If (Not Session.CurrentSession.IsLoggedIn) Then
            Return
        End If

        Try


            If (Selection.Count = 0) Then
                Return
            End If

            If (Selection.Count = 1) Then
                Dim count As Integer = CommandBar.Controls.Count

                contextselection = TryCast(Selection(1), OutlookItem)
                If contextselection Is Nothing Then
                    Return
                End If

                contextselection.Attach()

                If CanSaveItemAsDocument(contextselection) Then
                    InspectorExtensions.BuildCommandBars(Me, CommandBar, contextselection, AddressOf ContextMenuButtonClick, False)
                End If

                If (count < CommandBar.Controls.Count) Then
                    CommandBar.Controls(count + 1).BeginGroup = True
                End If
                Return
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub App_Quit()
        Dispose()
    End Sub

    Private Sub App_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        If Not Session.CurrentSession.IsLoggedIn Then
            ShuttingDown = True
            App.Dispose()
            Return
        End If

        For Each frm As Form In System.Windows.Forms.Application.OpenForms
            If (frm.Visible) Then
                e.Cancel = True
                MessageBox.ShowInformation("OPENFORMS", "Please close all forms before disconnecting from the system.")
                Return
            End If
        Next
        Dim reg As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "OutlookShutdownWarning", True)
        If reg.ToBoolean() Then
            If MessageBox.ShowYesNoQuestion("QUITOUTLOOK?", "Are you sure you want to quit Outlook?") = DialogResult.No Then
                e.Cancel = True
                Return
            End If
        End If
        ShuttingDown = True
        App.Dispose()
    End Sub

    Private Sub App_ItemSend(ByVal Item As Object, ByRef Cancel As Boolean)
        Dim win As IWin32Window = App.GetWindow(Item)

        Try
            Dim args As BeforeItemEventArgs = New BeforeItemEventArgs(Item, False)
            args.Cancel = Cancel

            Dim pi As IProfileItem = GetProfileItem(Item)
            If (pi Is Nothing) Then
                Return
            End If
            args.Handled = True
            Using contextBlock As DPIContextBlock = If(_dpiAwareness > 0, New DPIContextBlock(_dpiAwareness), Nothing)
                pi.BeforeSend(Me, args)
            End Using
            If (args.Handled) Then
                Cancel = args.Cancel
            End If

            'DM - 15/03/05 - This seems to have fixed the focus issue Mike found after
            'sending an email.
            If App Is Nothing Then
                Return
            End If

            'DM - 060706 - Only activates the explorer if the email was actually sent.
            If args.Cancel = False Then

            End If

        Catch ex As Exception
            Cancel = True
            MessageBox.Show(win, ex)
        End Try
    End Sub

#Region "Delete Items"

    Private Sub App_BeforeDeleteItems(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeDeleteItemsEventArgs)

        If (Not Session.CurrentSession.IsLoggedIn) Then
            e.Handled = False
            Return
        End If
        If e.Items.Count = 0 Then
            e.Handled = False
            Return
        End If
        If Not CheckEmailProfileOption(EmailProfileOption.epoDelete) Then
            e.Handled = False
            Return
        End If

        e.OnAction = New Action(Sub() App_BeforeDeleteItemsAction(e))

    End Sub

    Private Sub App_BeforeDeleteItemsAction(ByVal e As FWBS.Office.Outlook.BeforeDeleteItemsEventArgs)
        If e.Items.Count = 1 Then
            Call DeleteSingleItem(e)
        Else
            Call DeleteMultipleItems(e)
        End If
    End Sub


    Private Sub DeleteSingleItem(ByVal e As FWBS.Office.Outlook.BeforeDeleteItemsEventArgs)
        Dim item As OutlookItem = e.Items.ElementAt(0)
        Dim detached As Boolean = False
        Try
            detached = item.IsDetached
            If detached Then item.Attach()

            If (Not CanSaveItemAsDocument(item)) Then
                e.Handled = False
                GoTo PerformAction
                Return
            End If
            If IsCompanyDocument(item) Then
                e.Handled = False
                GoTo PerformAction
                Return
            End If

            Dim profileitem As IProfileItem = GetProfileItem(item)

            Dim res As DialogResult = MessageBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("SAVEOLITEM", "Would you like to save the following item to the system before it is deleted?" & Environment.NewLine & Environment.NewLine & "%1%", "", profileitem.GetDisplayText(Me, item)), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)

            Select Case res
                Case DialogResult.No
                    e.Handled = False
                Case DialogResult.Cancel
                    e.Cancel = True
                Case DialogResult.Yes
                    Dim saved As Boolean

                    If CheckEmailOption(EmailOption.optQuickSave) Then
                        saved = SaveQuick(item, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
                    Else
                        saved = Save(item, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
                    End If
                    If Not saved Then
                        e.Cancel = True
                    End If
            End Select

            If (e.Cancel) Then
                e.Handled = True
                Return
            End If

PerformAction:

            If (e.RequiresAction) Then
                e.Handled = True
                e.Cancel = True
                OutlookOMS.DeleteItem(item)
            End If
        Finally
            If detached Then item.Detach()
        End Try
    End Sub


    Private Sub DeleteMultipleItems(ByVal e As FWBS.Office.Outlook.BeforeDeleteItemsEventArgs)

        Dim parentWindow As IWin32Window = ActiveWindow
        Dim res As DialogResult = MessageBox.Show(parentWindow, Session.CurrentSession.Resources.GetMessage("SAVEDELOLITEMS", "Would you like to save the selected items to the system before they are deleted?", ""), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        Dim manualdelete As IEnumerable(Of OutlookItem) = e.Items

        Select Case res
            Case DialogResult.Cancel
                e.Cancel = True
            Case DialogResult.No
                e.Handled = False
            Case DialogResult.Yes

                Dim assoc As Associate = SelectAssociate(parentWindow)
                If assoc Is Nothing Then
                    e.Cancel = True
                Else
                    Try

                        Dim selector As frmEmailSelector = New frmEmailSelector(e.Items, Me, assoc, frmEmailSelector.EmailSelectionType.Delete)
                        _activeForm = selector

                        If (selector.ItemsCount > 0) Then
                            If (_activeForm.ShowDialog(parentWindow) = DialogResult.OK) Then
                                manualdelete = selector.ManualItems
                            Else
                                e.Cancel = True
                            End If
                        Else
                            e.Handled = False
                        End If
                    Finally
                        If (Not _activeForm Is Nothing) Then
                            _activeForm.Dispose()
                            _activeForm = Nothing
                        End If
                    End Try
                End If

        End Select

        If (e.Cancel) Then
            e.Handled = True
            Return
        End If

        If (e.RequiresAction) Then
            e.Handled = True
            e.Cancel = True

            Dim progress As Boolean = False
            Try
                Using (App.BeginProcess())
                    Using items As DetachableItems = New DetachableItems(App, manualdelete)
                        Dim ctr As Integer = 0
                        Dim failed As Integer = 0
                        Dim selcount As Integer = items.Count
                        If selcount > 0 Then progress = True

                        If progress Then OnProgressStart(Session.CurrentSession.Resources.GetResource("CALCULATING", "Calculating", "").Text,
                                                         Session.CurrentSession.Resources.GetResource("CALCTOTALDELETE", "Calculating the number of items to delete.", "").Text,
                                                         selcount)

                        For Each Item As OutlookItem In items
                            ctr += 1
                            Try
                                If progress Then OnProgress(Session.CurrentSession.Resources.GetResource("DELETINGITEMS", "Deleting %1%/%2% Items.", "", ctr.ToString(), selcount.ToString()).Text, ctr)

                                If (CanSaveItemAsDocument(Item)) Then
                                    SetDocVariable(Item, PROFILE, False)
                                End If
                                OutlookOMS.DeleteItem(Item, e.Permanent)
                            Catch ex As Exception
                                failed += 1
                                WriteLog("Item Delete", "Unable to Delete Item.", "Refer to the following exception", ex)
                            End Try
                        Next
                    End Using
                End Using
            Finally
                If progress Then OnProgressFinished()
            End Try
        End If

    End Sub

#End Region

#Region "Move Items"

    Private Sub App_BeforeMoveItems(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeMoveItemsEventArgs)

        If (Not Session.CurrentSession.IsLoggedIn) Then
            e.Handled = False
            Return
        End If
        If e.Items.Count = 0 Then
            e.Handled = False
            Return
        End If
        If Not CheckEmailProfileOption(EmailProfileOption.epoMove) Then
            e.Handled = False
            Return
        End If
        If (e.TargetFolder.IsSearchFolder) Then
            e.Handled = False
            Return
        End If

        Dim folderfile As OMSFile = Nothing


        Try
            folderfile = GetFolderFile(e.TargetFolder)
        Catch secex As FWBS.OMS.Security.SecurityException _
            When secex.HelpID = HelpIndexes.PasswordRequestCancelled
            e.Handled = True
            e.Cancel = True
            Return
        End Try

        If (folderfile Is Nothing) Then
            Dim dr As DialogResult = MessageBox.ShowYesNoCancel(ActiveWindow, "DOUWANTPROF", Session.CurrentSession.Resources.GetResource("DOUWANTPROF", "You have selected a folder that hasn't got Profile Information stored. Would you like to Profile these Items?", "").Text)
            Select Case dr
                Case DialogResult.No
                    e.Handled = False
                    Return
                Case DialogResult.Cancel
                    e.Cancel = True
                    e.Handled = True
                    Return
            End Select
        End If

        Try
            If e.Items.Count = 1 Then
                Call MoveSingleItem(folderfile, e)
            Else
                Call MoveMultipleItems(folderfile, e)
            End If
        Catch ex As Exception
            e.Cancel = True
            MessageBox.Show(ex)
        End Try

    End Sub

    Private Sub MoveSingleItem(ByVal folderfile As OMSFile, ByVal e As FWBS.Office.Outlook.BeforeMoveItemsEventArgs)

        Dim manualmove As OutlookItem = e.Items.ElementAt(0)
        Dim quick As Boolean = CheckEmailOption(EmailOption.optQuickSave)

        Dim assoc As Associate = Nothing
        If (GetAssociate(folderfile, e, assoc) = False) Then
            Return
        End If

        Try
            manualmove.Attach()

            Dim success As Boolean = False

            AttachDocumentVars(manualmove, CheckEmailOption(EmailOption.optUseDefAssoc), assoc)

            If quick Then
                success = SaveQuick(manualmove, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
            Else
                success = Save(manualmove, AddressOf GenericProfileItem.DisallowMoveSaveSettingsCallback)
            End If

            If success Then
                OutlookOMS.MoveItem(manualmove, e.TargetFolder)
            End If

        Catch ex As Exception
            WriteLog("Bulk Profiling Error", " There was an error when bulk profiling", "Refer to the exception information below", ex)
            Throw
        Finally
            manualmove.Detach()
        End Try

    End Sub


    Private Sub MoveMultipleItems(ByVal folderfile As OMSFile, ByVal e As FWBS.Office.Outlook.BeforeMoveItemsEventArgs)

        Dim parentWindow As IWin32Window = ActiveWindow
        Dim manualmove As IEnumerable(Of OutlookItem) = e.Items

        Dim assoc As Associate = Nothing
        If (GetAssociate(folderfile, e, assoc) = False) Then
            Return
        End If

        Try
            Dim selector As frmEmailSelector = New frmEmailSelector(e.Items, Me, assoc, frmEmailSelector.EmailSelectionType.Move, e.TargetFolder)
            _activeForm = selector

            If (selector.ItemsCount > 0) Then
                If (_activeForm.ShowDialog(parentWindow) = DialogResult.OK) Then
                    manualmove = selector.ManualItems
                Else
                    e.Cancel = True
                End If
            Else
                e.Handled = False
            End If
        Catch ex As Exception
            e.Cancel = True
            Throw
        Finally
            If (Not _activeForm Is Nothing) Then
                _activeForm.Dispose()
                _activeForm = Nothing
            End If
        End Try


        If (e.Cancel) Then
            e.Handled = True
            Return
        End If

        If (e.RequiresAction) Then
            e.Handled = True
            e.Cancel = True


            Dim progress As Boolean = False
            Try
                Using (App.BeginProcess())
                    Using items As DetachableItems = New DetachableItems(App, manualmove)
                        Dim ctr As Integer = 0
                        Dim failed As Integer = 0
                        Dim selcount As Integer = items.Count
                        If selcount > PROGRESS_AMOUNT Then progress = True

                        If (progress) Then OnProgressStart(Session.CurrentSession.Resources.GetResource("MOVING", "Moving", "").Text,
                                                           Session.CurrentSession.Resources.GetResource("MOVINGSELITEMS", "Moving the selected items.", "").Text,
                                                           selcount)

                        For Each Item As OutlookItem In items
                            ctr += 1
                            If (progress) Then OnProgress(Session.CurrentSession.Resources.GetResource("MOVINGITEMS", "Moving %1%/%2% Items.", "", ctr.ToString(), selcount.ToString()).Text, ctr)

                            Try
                                OutlookOMS.MoveItem(Item, e.TargetFolder)
                            Catch ex As Exception
                                failed += 1
                                WriteLog("Item Move", "Unable to Move Item.", "Refer to the following exception", ex)
                            End Try
                        Next
                    End Using
                End Using
            Finally
                If (progress) Then OnProgressFinished()
            End Try
        End If
    End Sub

    Private Function GetAssociate(ByVal folderfile As OMSFile, ByVal e As FWBS.Office.Outlook.BeforeMoveItemsEventArgs, ByRef associate As FWBS.OMS.Associate) As Boolean
        If (Not folderfile Is Nothing) Then
            associate = folderfile.DefaultAssociate
        End If

        If (associate Is Nothing) Then
            associate = SelectAssociate(CheckEmailOption(EmailOption.optUseDefAssoc), associate)
        End If

        If (associate Is Nothing) Then
            e.Handled = True
            e.Cancel = True
            Return False
        End If

REM Make sure that when moving items set the folder profile info as the FILEIT command will not be called which normally does this
        If (folderfile Is Nothing) Then
            SetAsProfileFolder(e.TargetFolder, associate.OMSFile, False)
        End If
        Return True
    End Function


#End Region

#Region "Open Items"

    Private Sub App_BeforeOpenItems(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeOpenItemsEventArgs)
        If (Not Session.CurrentSession.IsLoggedIn) Then
            e.Handled = False
            Return
        End If
        e.OnAction = New Action(Sub() App_BeforeOpenItemsAction(e))
    End Sub

    Private Sub App_BeforeOpenItemsAction(ByVal e As FWBS.Office.Outlook.BeforeOpenItemsEventArgs)
        RunCommand(Me, "SYSTEM;OPEN")
    End Sub

#End Region

#Region "Print Items"
    Private Sub App_BeforePrintItems(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforePrintItemsEventArgs)

    End Sub
#End Region

#Region "Save Items"

    Private Sub App_BeforeSaveItems(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeSaveItemsEventArgs)
        If (Not Session.CurrentSession.IsLoggedIn) Then
            e.Handled = False
            Return
        End If
        If e.Items.Count = 0 Then
            e.Handled = False
            Return
        End If

        e.OnAction = New Action(Sub() SaveItemsExAction(e))

    End Sub

    Private Sub SaveItems(ByVal e As BeforeSaveItemsEventArgs, ByVal quick As Boolean)
        If (Not Session.CurrentSession.IsLoggedIn) Then
            e.Handled = False
            Return
        End If
        If e.Items.Count = 0 Then
            e.Handled = False
            Return
        End If
        If e.Items.Count = 1 Then
            Call SaveSingleItem(e, quick)
        Else
            Call SaveMultipleItems(e, quick)
        End If
    End Sub


    Private Sub SaveSingleItem(ByVal e As BeforeSaveItemsEventArgs, ByVal quick As Boolean)
        Dim item As OutlookItem = e.Items.ElementAt(0)

        Try
            item.Attach()

            If (Not CanSaveItemAsDocument(item)) Then
                e.Handled = False
                Return
            End If

            Dim settings As SaveSettings = SaveSettings.Default
            If quick Then
                settings.UseDefaultAssociate = True
                settings.Printing.Mode = PrecPrintMode.None
                settings.Mode = PrecSaveMode.Quick
            End If

            Call Save(item, settings)
        Finally
            item.Detach()
        End Try
    End Sub

    Private Sub SaveMultipleItems(ByVal e As BeforeSaveItemsEventArgs, ByVal quick As Boolean)

        Dim tosave As System.Collections.Generic.List(Of OutlookItem) = New System.Collections.Generic.List(Of OutlookItem)

        Dim strInfo As System.Text.StringBuilder = New System.Text.StringBuilder()

        Dim res As FWBS.OMS.ResourceItem = FWBS.OMS.Session.CurrentSession.Resources.GetMessage("SAVESELECTEDITM", "Do you wish to save all of the selected items?" & vbCrLf & "Each item will be saved under the same %CLIENT%, %FILE% and %ASSOCIATE%" & vbCrLf & "%1%", "", True, strInfo.ToString())

        If Not MessageBox.Show(ActiveWindow, res, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = vbYes Then
            e.Cancel = True
            Return
        End If

        Try
            Using (App.BeginProcess())
                Using items As DetachableItems = New DetachableItems(App, e.Items)

                    Dim valid As Integer = 0
                    Dim ctr As Integer = 1
                    Dim count As Integer = items.Count

                    OnProgressStart(Session.CurrentSession.Resources.GetResource("CALCULATING", "Calculating", "").Text,
                                    Session.CurrentSession.Resources.GetResource("CALCTOTALSAVE", "Calculating the number of items to save.", "").Text,
                                    count)

                    For Each item As OutlookItem In items

                        OnProgress(Session.CurrentSession.Resources.GetResource("CALCITEMSUNSAVE", "Calculating %1%/%2% Items - Unsaved: %3%.", "", ctr.ToString(), count.ToString(), valid.ToString()).Text, ctr)

                        If IsCompanyDocument(item) = False Or GetDocVariable(item, OMSApp.DOCUMENT, 0) = 0 Then
                            valid += 1
                            If (CanSaveItemAsDocument(item)) Then
                                If (count <= 10) Then
                                    strInfo.AppendLine(item.Subject)
                                End If
                                tosave.Add(item)
                            End If
                        End If

                        ctr += 1
                    Next
                End Using
            End Using

        Finally
            OnProgressFinished()
        End Try


        Dim settings As SaveSettings = SaveSettings.Default
        settings.UseDefaultAssociate = CheckEmailOption(EmailOption.optUseDefAssoc)
        If quick Then
            settings.ContinueOnError = True
            settings.Printing.Mode = PrecPrintMode.None
            settings.Mode = PrecSaveMode.Quick
        End If

        Using (App.BeginProcess())
            Using itemstosave As DetachableItems = New DetachableItems(App, tosave)
                Call Save(itemstosave, itemstosave.Count, settings)
            End Using
        End Using
    End Sub

    Private Sub SaveItemsEx(ByVal e As BeforeSaveItemsEventArgs)
        If (Not Session.CurrentSession.IsLoggedIn) Then
            e.Handled = False
            Return
        End If
        If e.Items.Count = 0 Then
            e.Handled = False
            Return
        End If

        SaveItemsExAction(e)
    End Sub

    Private Sub SaveItemsExAction(ByVal e As BeforeSaveItemsEventArgs)

        If e.Items.Count = 1 Then
            Call SaveSingleItemEx(e)
        Else
            Call SaveMultipleItemsEx(e)
        End If
    End Sub

    Private Sub SaveSingleItemEx(ByVal e As FWBS.Office.Outlook.BeforeSaveItemsEventArgs)

        Dim item As OutlookItem = e.Items.ElementAt(0)
        Dim ret As Boolean
        Dim win As IWin32Window = App.GetWindow(App)

        Try
            item.Attach()

            If item.IsDraft = True Then
                If FWBS.OMS.UI.Windows.MessageBox.Show(win, Session.CurrentSession.Resources.GetMessage("SAVEDRAFT_OPT", "Please note this email will be sent to recipient after saving into 3E MatterSphere. Do you still want to proceed?", ""), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    e.Handled = False
                    Return
                End If
            End If

            If (Not CanSaveItemAsDocument(item)) Then
                e.Handled = False
                Return
            End If

            If IsStoredDocument(item) Then
                ret = (SaveProfiledEmail(item))
            ElseIf CheckEmailOption(EmailOption.optQuickSave) Then
                ret = SaveQuick(item)
            Else
                ret = Save(item)
            End If
            If (Not ret) Then
                e.Cancel = True
            End If
        Finally
            If (ret = True) Then
                item.Detach()
            End If

        End Try
    End Sub

    Private Sub SaveMultipleItemsEx(ByVal e As FWBS.Office.Outlook.BeforeSaveItemsEventArgs)
        Dim parentWindow As IWin32Window = ActiveWindow

        Dim assoc As Associate = SelectAssociate(ActiveWindow)
        If assoc Is Nothing Then
            Return
        End If
        _activeForm = New frmEmailSelector(e.Items, Me, assoc, frmEmailSelector.EmailSelectionType.Save)
        _activeForm.ShowDialog(parentWindow)
        _activeForm.Dispose()
        _activeForm = Nothing
    End Sub

#End Region

#Region "Close Items"

    Private Sub App_BeforeCloseItem(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeItemEventArgs)

        Dim pi As IProfileItem = GetProfileItem(e.Item)
        If (pi Is Nothing) Then
            Return
        End If
        pi.BeforeClose(Me, e)

        If (Session.CurrentSession.IsLoggedIn) Then

            If e.Item.GetType() Is GetType(FWBS.Office.Outlook.OutlookMail) Then

                Dim precID As String = Nothing
                Dim prec As Precedent = Nothing

                Dim omsprec As IStorageItem = GetCurrentPrecedent(e.Item)
                If Not omsprec Is Nothing Then
                    prec = omsprec
                    precID = prec.ID
                End If

                If precID Is Nothing Then
                    Dim omsprecVer As IStorageItem = GetCurrentPrecedentVersion(e.Item)
                    If Not omsprecVer Is Nothing Then
                        Dim precVersion As PrecedentVersion = omsprecVer
                        precID = precVersion.ParentDocument.ID
                    End If
                End If

                If Not precID Is Nothing And e.Cancel = False Then
                    UnlockPrecedent(precID)
                    If Not prec Is Nothing Then
                        prec.CheckIn()
                    End If
                End If
            End If

        End If

        'I think Phil added this for Interwoven
        If (e.Cancel = False) Then
            OnDocumentClosed()
        End If

    End Sub

#End Region

#Region "Before Item Events"

    Private Sub App_BeforeOpenItem(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeItemEventArgs)
        Dim pi As IProfileItem = GetProfileItem(e.Item)
        If (pi Is Nothing) Then
            Return
        End If

        pi.BeforeOpen(Me, e)
    End Sub

    Private Sub App_BeforeDeleteItem(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeItemEventArgs)
        Dim pi As IProfileItem = GetProfileItem(e.Item)
        If (pi Is Nothing) Then
            Return
        End If
        pi.BeforeDelete(Me, e)
    End Sub

    Private Sub App_BeforeActivateItem(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeItemEventArgs)
        Dim pi As IProfileItem = GetProfileItem(e.Item)
        If (pi Is Nothing) Then
            Return
        End If
        pi.BeforeActivate(Me, e)
    End Sub

    Private Sub App_BeforeForwardItem(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeItemEventArgs)
        Dim pi As IProfileItem = GetProfileItem(e.Item)
        If (pi Is Nothing) Then
            Return
        End If
        pi.BeforeForward(Me, e)
    End Sub

    Private Sub App_BeforeReplyItem(ByVal sender As Object, ByVal e As FWBS.Office.Outlook.BeforeItemEventArgs)
        Dim pi As IProfileItem = GetProfileItem(e.Item)
        If (pi Is Nothing) Then
            Return
        End If
        pi.BeforeReply(Me, e)
    End Sub

#End Region

#End Region

#Region "Explorers"

    Private _exps As Outlook.Explorers
    Private Property Explorers() As Outlook.Explorers
        Get
            Return _exps
        End Get
        Set(ByVal value As Outlook.Explorers)
            If Not value Is _exps Then
                RemoveExplorersDelegates()
            End If
            _exps = value
            AddExplorersDelegates()
        End Set
    End Property
    Private Sub AddExplorersDelegates()
        RemoveExplorersDelegates()
        If (Not Explorers Is Nothing And Me.IsAddinInstance) Then
            AddHandler Explorers.NewExplorer, AddressOf Explorers_NewExplorer
        End If
    End Sub
    Private Sub RemoveExplorersDelegates()
        On Error Resume Next
        If (Not Explorers Is Nothing) Then
            RemoveHandler Explorers.NewExplorer, AddressOf Explorers_NewExplorer
        End If
    End Sub

    Private Sub Explorers_NewExplorer(ByVal Explorer As Outlook.Explorer)
        Dim win As IWin32Window = App.GetWindow(Explorer)
        Try
            Dim olexp As FWBS.Office.Outlook.OutlookExplorer = Explorer

            If (Initialising) Then
                Return
            End If

            AddHandler olexp.Shown, AddressOf Explorer_Shown

        Catch ex As Exception
            MessageBox.Show(win, ex)
        End Try
    End Sub

    Private Sub Explorer_Shown(ByVal sender As Object, ByVal e As EventArgs)
        Dim olexp As FWBS.Office.Outlook.OutlookExplorer = sender
        RemoveHandler olexp.Shown, AddressOf Explorer_Shown
        olexp.BuildCommandBars(Me)
    End Sub

#End Region

#Region "Inspectors Events"

    Private _insps As Outlook.Inspectors
    Private Property Inspectors() As Outlook.Inspectors
        Get
            Return _insps
        End Get
        Set(ByVal value As Outlook.Inspectors)
            If Not value Is _insps Then
                RemoveInspectorsDelegates()
            End If
            _insps = value
            AddInspectorsDelegates()
        End Set
    End Property
    Private Sub AddInspectorsDelegates()
        RemoveInspectorsDelegates()
        If (Not Inspectors Is Nothing And Me.IsAddinInstance) Then
            AddHandler Inspectors.NewInspector, AddressOf Inspectors_NewInspector
        End If
    End Sub
    Private Sub RemoveInspectorsDelegates()
        On Error Resume Next
        If (Not Inspectors Is Nothing) Then
            RemoveHandler Inspectors.NewInspector, AddressOf Inspectors_NewInspector
        End If
    End Sub

    Private Sub Inspectors_NewInspector(ByVal Inspector As Outlook.Inspector)
        Dim win As IWin32Window = App.GetWindow(Inspector)
        Try
            Dim olinsp As FWBS.Office.Outlook.OutlookInspector = Inspector

            Dim pi As IProfileItem = GetProfileItem(olinsp.CurrentItem)
            If (pi Is Nothing) Then
                Return
            End If
            pi.Refresh(Me, olinsp.CurrentItem)

        Catch ex As Exception
            MessageBox.Show(win, ex)
        End Try

    End Sub

#End Region

#Region "Logging"

    Friend Sub WriteLog(ByVal title As String, ByVal text As String, ByVal resolution As String, ByVal ex As Exception)
        App.WriteLog(title, text, resolution, ex)
    End Sub

#End Region



    Public Overloads Function SelectAssociate(ByVal defoption As Boolean, ByVal assoc As FWBS.OMS.Associate) As FWBS.OMS.Associate
        Return MyBase.SelectAssociate(Nothing, defoption, assoc)
    End Function

    Public Function CanSaveItemAsDocument(ByVal obj As Object) As Boolean
        Dim pi As IProfileItem = GetProfileItem(obj)
        If (pi Is Nothing) Then
            Return False
        End If
        Return pi.CanSaveAsDocument(Me, obj)
    End Function

    Private Function GetProfileItem(ByVal item As OutlookItem) As IProfileItem
        If (item Is Nothing) Then
            Return Nothing
        End If


        Select Case item.Class
            Case Outlook.OlObjectClass.olNote
                Return New NoteProfileItem()
            Case Outlook.OlObjectClass.olTask
                Return New TaskProfileItem()
            Case Outlook.OlObjectClass.olAppointment
                Return New AppointmentProfileItem()
            Case Else
                Return New GenericProfileItem()
        End Select
    End Function


#Region "Publish Document Showing Markup related functions"

    Private Function EmailAttachmentFileInfoList(ByVal EmailAttachmentList As Outlook.Attachments) As List(Of System.IO.FileInfo)
        Dim attFileNames As List(Of System.IO.FileInfo) = New List(Of System.IO.FileInfo)
        For Each docFile As Outlook.Attachment In EmailAttachmentList
            attFileNames.Add(New System.IO.FileInfo(docFile.FileName))
        Next
        Return attFileNames
    End Function

#End Region


End Class

