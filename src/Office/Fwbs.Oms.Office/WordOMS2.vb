Imports System.Reflection
Imports System.Windows.Forms
Imports FWBS.OMS.DocumentManagement
Imports FWBS.OMS.DocumentManagement.Storage
Imports Word = Microsoft.Office.Interop.Word

<System.Runtime.InteropServices.Guid("78CA04DE-56B3-492B-A77A-7237AEBE1583"), System.Runtime.InteropServices.ComVisible(False)>
Public Class WordOMS2
    Inherits OfficeOMSApp
    Implements FWBS.OMS.Interfaces.ISecondStageMergeOMSApp


#Region "Fields"

    Private RunAutoExec As Boolean = False
    Private _app As Word.Application
    Private _old_Initals As String
    Private _old_userName As String
    Private _wnd_close_hook As WindowCloseHook = New WindowCloseHook("OpusApp", True)
    Private Const MAILMERGEFILE As String = "MAILMERGFILE"
    Private Const SIGNATUREMAXHEIGHT As Single = 112.3
    Private Const SIGNATUREMAXWIDTH As Single = 149.75

    Private MatchText As String
    Private ReplacementText As String
    Private Forward As Boolean
    Private MatchWholeWord As Boolean
    Private MatchWildcards As Boolean
    Private MatchSoundsLike As Boolean
    Private MatchAllWordForms As Boolean
    Private MatchPhrase As Boolean
    Private MatchFuzzy As Boolean
    Private MatchCase As Boolean
    Private Format As Boolean
    Private Wrap As Word.WdFindWrap

    Private _disposed As Boolean = False

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New("WORD", False, False)
        RemoveDelegates()
        ActivateApplication()
        AddDelegates()

        _old_Initals = _app.UserInitials
        _old_userName = _app.UserName

    End Sub
    Public Sub New(ByVal obj As Word.Application, ByVal code As String)
        Me.New(obj, code, True)
    End Sub
    Public Sub New(ByVal app As Word.Application, ByVal code As String, ByVal useCommandBars As Boolean)


        MyBase.New(code, useCommandBars)

        RemoveDelegates()
        _app = app
        AddDelegates()

        AddinInitialisation()

        _old_Initals = _app.UserInitials
        _old_userName = _app.UserName


    End Sub

    Private Sub RemoveDelegates()
        On Error Resume Next
        If (_app Is Nothing) Then
            Return
        End If

        RemoveHandler _app.DocumentOpen, AddressOf Application_DocumentOpen
        RemoveHandler _app.Quit, AddressOf Application_Quit
        RemoveHandler _app.DocumentBeforeClose, AddressOf Application_BeforeDocumentClose
        RemoveHandler _app.WindowActivate, AddressOf Application_WindowActivate

    End Sub

    Private Sub AddDelegates()

        If _app Is Nothing Then
            Return
        End If

        If IsAddinInstance Then
            AddHandler _app.DocumentOpen, AddressOf Application_DocumentOpen

            AddHandler _app.Quit, AddressOf Application_Quit
        End If

        Try
            AddHandler _app.DocumentBeforeClose, AddressOf Application_BeforeDocumentClose
        Catch
        End Try

        If (RunAutoExec) Then
            AddHandler _app.WindowActivate, AddressOf Application_WindowActivate
        End If

    End Sub

#End Region

#Region "OfficeOMSApp"

    Protected Overrides ReadOnly Property CommandBarContainer() As Microsoft.Office.Core.CommandBars
        Get
            Return _app.CommandBars
        End Get
    End Property

    Protected Overrides ReadOnly Property Application() As Object
        Get
            Return _app
        End Get
    End Property

    Public Overrides ReadOnly Property ApplicationName() As String
        Get
            Return "Word"
        End Get
    End Property

    Public Overrides ReadOnly Property ApplicationVersion() As Integer
        Get
            Return Version.Parse(_app.Version).Major
        End Get
    End Property

#End Region

#Region "Table Routine"

    Public Overloads Overrides Sub BuildTable(ByVal obj As Object, ByVal code As String, ByVal vw As DataView, ByVal includeHeader As Boolean)
        Call CheckObjectIsDoc(obj)

        Try

            _app.ScreenUpdating = False

            Dim doc As Word.Document = obj
            Dim tbl As Word.Table
            Dim numfmt As System.Globalization.NumberFormatInfo = Session.CurrentSession.DefaultCurrencyFormat
            Dim datefmt As System.Globalization.DateTimeFormatInfo = Session.CurrentSession.DefaultDateTimeFormat

            Dim assoc As Associate = GetCurrentAssociate(obj)
            If Not assoc Is Nothing Then
                numfmt = assoc.OMSFile.CurrencyFormat
                datefmt = assoc.OMSFile.DateTimeFormat
            End If

            Try
                Dim objcode As Object = code
                _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:=objcode)
            Catch
            End Try

            tbl = GetTable(obj, code)

            If tbl Is Nothing Then
                tbl = doc.Tables.Add(_app.Selection.Range, 1, vw.Table.Columns.Count)
            End If

            tbl.ID = code

            Dim totalcols As Integer = tbl.Columns.Count
            Dim totalrows As Integer = tbl.Rows.Count
            Dim minusrows As Integer = 0

            Dim colctr As Integer = 1
            Dim rowctr As Integer = 1

            If includeHeader Then
                For Each col As DataColumn In vw.Table.Columns
                    If (colctr > totalcols) Then Exit For
                    Dim cell As Word.Cell = tbl.Cell(1, colctr)
                    cell.ID = col.ColumnName
                    cell.Range.Text = If(col.Caption = "", col.ColumnName, col.Caption)
                    colctr = colctr + 1
                Next
                rowctr = rowctr + 1
                minusrows = 1
            End If

            Dim resultcount As Integer = vw.Count
            For Each row As DataRowView In vw
                Dim r As Word.Row
                If (rowctr > totalrows) Then
                    r = tbl.Rows.Add()
                    totalrows = totalrows + 1
                Else
                    r = tbl.Rows(rowctr)
                End If
                colctr = 1
                For Each col As DataColumn In vw.Table.Columns
                    If (colctr > totalcols) Then Exit For
                    Dim alignment As String = Convert.ToString(col.ExtendedProperties.Item("ALIGNMENT"))
                    Dim format As String = Convert.ToString(col.ExtendedProperties.Item("FORMAT"))
                    tbl.Cell(r.Index, colctr).Range.Text = Convert.ToString(FieldParser.GetFormattedValue(row(col.ColumnName), numfmt, datefmt, format))
                    colctr = colctr + 1
                Next
                rowctr = rowctr + 1
            Next

            totalrows = totalrows - minusrows
            If (resultcount < totalrows) Then
                Dim diff As Integer = (totalrows - resultcount) - 1
                For ctr As Integer = tbl.Rows.Count To tbl.Rows.Count - diff Step -1
                    tbl.Rows(ctr).Delete()
                Next
            End If

        Finally
            _app.ScreenUpdating = True
        End Try

    End Sub

    Private Function GetTable(ByVal obj As Object, ByVal code As String) As Word.Table
        For Each tbl As Word.Table In obj.Tables
            If tbl.ID = code Then Return tbl
        Next
        For Each tbl As Word.Table In obj.Tables
            If _app.Selection.InRange(tbl.Range) Then
                tbl.ID = code
                Return tbl
            End If
        Next
        Return Nothing
    End Function

#End Region

#Region "Bookmark Routines"

    Protected Overrides Function GetBookmark(ByVal obj As Object, ByVal index As Integer) As String
        Call CheckObjectIsDoc(obj)
        index = index + 1
        Return obj.Bookmarks.Item(index).Name
    End Function

    Protected Overrides Function GetBookmarkCount(ByVal obj As Object) As Integer
        Try
            Call CheckObjectIsDoc(obj)
            Return obj.Bookmarks.Count
        Catch
            Return 0
        End Try
    End Function

    Protected Overrides Function HasBookmark(ByVal obj As Object, ByVal name As String) As Boolean
        Try
            Call CheckObjectIsDoc(obj)
            Return obj.Bookmarks.Exists(name)
        Catch
            Return False
        End Try
    End Function

    Protected Overrides Function SetBookmark(ByVal obj As Object, ByVal name As String, ByVal val As String) As Boolean
        Call CheckObjectIsDoc(obj)
        obj.Bookmarks.Item(name).Select()
        obj.Application.Selection.TypeParagraph()
        Try
            obj.Bookmarks.Item(name).Range.Style = name
        Catch
            obj.Application.Selection.Font.Bold = True
        End Try
        obj.Bookmarks.Item(name).Select()
        obj.Application.Selection.TypeText(val)
        obj.Application.Selection.TypeBackspace()
        Return True
    End Function

#End Region

#Region "Document Variables"

    Protected Overrides Function SupportsOleProperties(ByVal obj As Object) As Boolean
        If (TypeOf obj Is Word.Document) Then
            Return True
        Else
            Return MyBase.SupportsOleProperties(obj)
        End If
    End Function
    Public Overrides Function GetDocEditingTime(ByVal obj As Object) As Integer
        CheckObjectIsDoc(obj)

        Dim oBuiltInProps As Object
        'Get the Built-in Document Properties collection.
        oBuiltInProps = obj.BuiltInDocumentProperties

        'Get the value of the Author property and display it
        Return If(oBuiltInProps.Item("Total Editing Time").Value() = 0, 1, oBuiltInProps.Item("Total Editing Time").Value())
    End Function

    Private Function ConvertToLegacyVariableName(ByVal varName As String) As String
        Select Case varName
            Case OMSApp.FILE : varName = "MATTERID"
        End Select
        Return varName
    End Function

    Private Function ConvertFromLegacyVariableName(ByVal varName As String) As String
        Select Case varName
            Case "MATTERID" : varName = OMSApp.FILE
        End Select
        Return varName
    End Function

    Protected Overloads Overrides Sub DisplayDocVariables(ByVal obj As Object, ByVal val As String)
        CheckObjectIsDoc(obj)
        _app.Selection.TypeText(Text:=UCase$(val))
    End Sub

    Public Overrides Sub RemoveDocVariable(ByVal obj As Object, ByVal varName As String)
        Dim doc As Word.Document = Nothing
        Dim changed As Boolean = False

        Try
            CheckObjectIsDoc(obj)

            doc = obj
            changed = Not doc.Saved

            If HasDocVariableInternal(obj, varName) Then
                obj.Variables.Item(varName).Delete()
            End If
            If (HasDocProperty(obj, varName)) Then
                RemoveDocPropertyInternal(obj, varName)
            End If

            Dim legacyname As String = ConvertToLegacyVariableName(varName)
            If HasDocVariableInternal(obj, legacyname) Then
                obj.Variables.Item(legacyname).Delete()
            End If
            If (HasDocProperty(obj, legacyname)) Then
                RemoveDocPropertyInternal(obj, legacyname)
            End If

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        Finally
            If Not doc Is Nothing And changed = False Then
                If doc.Saved = False Then
                    doc.Saved = True
                End If
            End If
        End Try
    End Sub

    Protected Overloads Overrides Function GetDocVariable(ByVal obj As Object, ByVal varName As String) As Object
        Dim val As Object = Nothing
        CheckObjectIsDoc(obj)

        Dim doc As Word.Document = obj
        Dim changed As Boolean = Not doc.Saved

        Try
            If HasDocVariableInternal(obj, varName) Then
                val = (obj.Variables.Item(varName).Value)
            Else

                Dim legacyname As String = ConvertToLegacyVariableName(varName)

                If HasDocVariableInternal(obj, legacyname) Then
                    val = obj.Variables.Item(legacyname).Value
                Else
                    If Array.IndexOf(FieldExclusionList, varName) > -1 Then
                        If (HasDocProperty(obj, varName)) Then
                            val = GetDocProperty(obj, varName, Nothing)
                            SetDocVariable(obj, varName, GetDocProperty(obj, varName, ""))
                        Else
                            If HasDocProperty(obj, legacyname) Then
                                val = GetDocProperty(obj, legacyname, Nothing)
                                SetDocVariable(obj, varName, GetDocProperty(obj, legacyname, ""))
                            End If
                        End If
                    End If
                End If
            End If

            If TypeOf val Is String Then
                If val = " " Then val = ""
            End If

            Return val
        Finally
            If Not doc Is Nothing And changed = False Then
                If doc.Saved = False Then
                    doc.Saved = True
                End If
            End If
        End Try
    End Function


    Public Overrides Function HasDocVariable(ByVal obj As Object, ByVal varName As String) As Boolean
        If HasDocVariableInternal(obj, varName) Then
            Return True
        Else
            If Array.IndexOf(FieldExclusionList, varName) > -1 Then
                If (HasDocProperty(obj, varName)) Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function


    Private Function HasDocVariableInternal(ByVal obj As Object, ByVal varName As String) As Boolean
        Try
            Call CheckObjectIsDoc(obj)
            Dim tmp As Object = obj.Variables.Item(varName).Value
            Return True
        Catch
            Return False
        End Try
    End Function

    Protected Overloads Overrides Function SetDocVariable(ByVal obj As Object, ByVal varName As String, ByVal val As Object) As Boolean
        Dim doc As Word.Document = Nothing
        Dim changed As Boolean = False

        Try
            Call CheckObjectIsDoc(obj)
            doc = obj
            changed = Not doc.Saved
            Try
                obj.Variables.Item(varName).Delete()
            Catch
            End Try
            If TypeOf val Is String Then
                If val = "" Then val = " "
            End If
            obj.Variables.Add(varName, val)
            If Array.IndexOf(FieldExclusionList, varName) > -1 Then
                Try
                    RemoveDocPropertyInternal(obj, varName)
                Catch
                End Try
                SetDocPropertyInternal(obj, varName, val)
            End If
            Return True
        Catch
            Return False
        Finally
            If Not doc Is Nothing And changed = False Then
                If doc.Saved = False Then
                    doc.Saved = True
                End If
            End If
        End Try

    End Function


#End Region

#Region "Custom Document Properties"
    Friend Function GetDocProperty(ByVal doc As Word.Document, ByVal varName As String, ByVal defVal As Object) As Object
        Try
            'Add the global prefix
            Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)
            Return doc.CustomDocumentProperties.Item(globalname).Value
        Catch
            Return defVal
        End Try
    End Function

    Private Function HasDocProperty(ByVal doc As Word.Document, ByVal varName As String) As Boolean
        Try
            'Add the global prefix
            Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)
            Return Not doc.CustomDocumentProperties.Item(globalname).Value Is Nothing
        Catch
            Return False
        End Try
    End Function

    Friend Function SetDocPropertyInternal(ByVal doc As Word.Document, ByVal varName As String, ByVal val As Object) As Boolean

        Try

            'Add the global prefix
            Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)


            Dim t As Microsoft.Office.Core.MsoDocProperties = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString

            If TypeOf val Is Boolean Then
                t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeBoolean
            ElseIf TypeOf val Is String Then
                t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString
            ElseIf TypeOf val Is DateTime Then
                t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeDate
            ElseIf TypeOf val Is Decimal Then
                t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeFloat
            ElseIf TypeOf val Is Double Then
                t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeFloat
            ElseIf TypeOf val Is Single Then
                t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeFloat
            ElseIf IsNumeric(val) Then
                Dim val64 As Long = Convert.ToInt64(val)
                If (val64 < Integer.MaxValue And val64 > Integer.MinValue) Then
                    t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeNumber
                Else
                    t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeFloat
                End If
            End If

            Dim custprops As Microsoft.Office.Core.DocumentProperties = doc.CustomDocumentProperties
            Dim prop As Microsoft.Office.Core.DocumentProperty

            Try
                prop = custprops.Add(Name:=globalname, LinkToContent:=False, Value:=val, Type:=t)
            Catch ex As Exception
                prop = doc.CustomDocumentProperties.item(globalname)
                If prop.Type <> t Then
                    prop.Delete()
                    prop = custprops.Add(Name:=globalname, LinkToContent:=False, Value:=val, Type:=t)
                Else
                    prop.Value = val
                End If
            End Try


            Return True
        Catch
            Return False
        End Try

    End Function
    Friend Function SetDocProperty(ByVal doc As Word.Document, ByVal varName As String, ByVal val As Object) As Boolean
        Dim changed As Boolean = False
        Try
            changed = doc.Saved

            Return SetDocPropertyInternal(doc, varName, val)
        Catch
            Return False
        Finally
            If Not doc Is Nothing And changed = False Then
                If doc.Saved = False Then
                    doc.Saved = True
                End If
            End If
        End Try
    End Function

    Private Sub RemoveDocProperty(ByVal doc As Word.Document, ByVal varName As String)
        Dim changed As Boolean = False
        Try
            RemoveDocPropertyInternal(doc, varName)
        Catch
        Finally
            If Not doc Is Nothing And changed = False Then
                If doc.Saved = False Then
                    doc.Saved = True
                End If
            End If
        End Try
    End Sub

    Private Sub RemoveDocPropertyInternal(ByVal doc As Word.Document, ByVal varName As String)
        Try
            'Add the global prefix
            Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)
            doc.CustomDocumentProperties.Item(globalname).Delete()
        Catch
        End Try
    End Sub

#End Region

#Region "Save Routines"

    Protected Overrides Sub BeforeDocumentSave(ByVal obj As Object, ByVal doc As OMSDocument, ByVal version As DocumentVersion)
        CheckObjectIsDoc(obj)

        If SpellingAndGrammarCheckRequired() Then
            doc.SpellingMistakes = obj.SpellingErrors.Count
            doc.GrammarMistakes = obj.GrammaticalErrors.Count
        End If

        If (version.Label.Split(".").Length - 1 >= FWBS.OMS.Session.CurrentSession.DocumentMaximumRevisionCount) Then
            doc.ReachDocMaxRevisionCount = True
        Else
            doc.ReachDocMaxRevisionCount = False
        End If

        _app.ActiveWindow.View.ReadingLayout = False

        '13/07/07
        'Check to see if the original document is from the cached location, if not then
        'we can assume that the document is coming back from and external source
        'and therefore must be saved as a new version by default.

        Dim docpath As String = obj.Path
        If (docpath.Length > 0 And doc.IsNew = False) Then
            Dim docdir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(docpath)
            Dim localdir As System.IO.DirectoryInfo = StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory
            If (localdir.FullName <> docdir.FullName) Then
                ' Consider documents from the virtual drive as local 
                If Session.CurrentSession.IsVirtualDriveFeatureInstalled(docpath) AndAlso String.IsNullOrEmpty(docpath) = False AndAlso docdir.FullName.StartsWith(docpath) Then
                    Exit Sub
                End If

                If (DirectCast(doc, IStorageItem).Supports(StorageFeature.Versioning)) Then

                    Dim settings As Storage.StorageSettingsCollection = doc.GetSettings()

                    Dim addAlert As Boolean = True
                    If Not settings Is Nothing Then
                        If settings.Count > 0 Then

                            For n As Integer = 0 To settings.Count

                                Dim vss As VersionStoreSettings = settings.Item(n)

                                If Not vss Is Nothing Then
                                    addAlert = False
                                    Exit For
                                End If


                            Next
                        End If
                    End If

                    If (addAlert) Then
                        doc.AddAlert(New Alert(Session.CurrentSession.Resources.GetMessage("ALERTPOSSATTACH", "Possible Attachment from an External Source. Will default to New Version.", "", True).Text, Alert.AlertStatus.Amber))


                        If settings Is Nothing Then
                            Dim provider As Storage.StorageProvider = doc.GetStorageProvider()
                            settings = provider.GetDefaultSettings(doc, SettingsType.Store)
                        End If

                        Dim verset As Storage.VersionStoreSettings = settings.GetSettings(Of VersionStoreSettings)()
                        verset.MarkAsLatest = False
                        verset.SaveItemAs = VersionStoreSettings.StoreAs.NewSubVersion
                        verset.Comments = ""
                        DirectCast(doc, Storage.IStorageItem).ApplySettings(settings)
                    End If
                End If
            End If
        End If
    End Sub
    Private Function SpellingAndGrammarCheckRequired() As Boolean

        If Session.CurrentSession.CurrentUser.SpellingAndGrammarCheckRequired = FWBS.Common.TriState.True Then
            Return True
        End If

        If Session.CurrentSession.CurrentUser.SpellingAndGrammarCheckRequired = FWBS.Common.TriState.False Then
            Return False
        End If

        If Session.CurrentSession.SpellingAndGrammarCheckRequired = False Then
            Return False
        Else
            Return True
        End If

    End Function

    Protected Overrides Function IsReadOnly(ByVal obj As Object) As Boolean
        Call CheckObjectIsDoc(obj)
        Dim doc As Word.Document = obj
        Return doc.ReadOnly
    End Function

    Protected Overloads Overrides Function BeginSave(ByVal obj As Object, ByVal settings As SaveSettings) As DocSaveStatus
        Call CheckObjectIsDoc(obj)

        Dim currentpos As Position = New Position(obj)

        Try

            currentpos.Set()

            Dim win As Window = New Window(obj)

            ' Fix issue if the document is in the wrong view mode

            'If the document is flagged to be a precedent and there is text selected then 
            'ask if the user wants to save the precedent as text only.
            If IsPrecedent(obj) Then
                If Len(_app.Selection.Text) > 1 Then
                    If MessageBox.Show(win, Session.CurrentSession.Resources.GetMessage("4018", "Would you like to take the Selected text to create a new Stand Only Precedent?", ""), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        Try
                            _app.Selection.Copy()
                        Catch
                        End Try
                        obj = _app.Documents.Add()
                        SetDocVariable(obj, OMSApp.ISPREC, True)
                        SetDocVariable(obj, OMSApp.TEXTONLY, True)
                        Try
                            _app.Selection.Paste()
                        Catch
                        End Try
                    End If
                End If
            Else
                ' If document is an attached template then run normal save routine
                If obj.Name.ToString() = Convert.ToString(obj.AttachedTemplate) Then
                    obj.SaveAs(obj.Path, AddToRecentFiles:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList)
                    Return DocSaveStatus.Error
                    Exit Function
                End If
                ' Need to check there are no fields or merge marks existing...
                If (Session.CurrentSession.CurrentUser.DisableStopCodeCheck = False) Then
                    If CheckStopCodesNoFields(obj) = False Then
                        Dim msg As MessageBox = New MessageBox(MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        msg.Text = FWBS.OMS.Session.CurrentSession.Resources.GetMessage("4014", "There are stopcodes that haven't been completed would you still like to save this document.", "")
                        If msg.Show(win) = "NO" Then
                            Return DocSaveStatus.Error
                            Exit Function
                        End If
                    End If
                End If
            End If


            Return MyBase.BeginSave(obj, settings)
        Finally
            Try
                currentpos.GoTo()
            Catch ex As Exception
            End Try
        End Try
    End Function
    Public Overrides Function GetDocExtension(ByVal obj As Object) As String

        Dim worddoc As Word.Document = obj
        If String.IsNullOrEmpty(worddoc.Path) Then ' Path is undefined for newly created documents.
            Return String.Empty ' Workaround to force usage of the file extension configured in AdminKit Document Types.
        End If

        Select Case worddoc.SaveFormat
            Case 0  'wdFormatDocument     0     Microsoft Office Word format.
                'wdFormatDocument97    0     Microsoft Word 97 document format.
                Return "doc"
            Case 1 'wdFormatTemplate      1     Word template format. 
                'wdFormatTemplate97 1      Word 97 template format.
                Return "dot"
            Case 2, 3, 4, 5 'wdFormatText 2     Microsoft Windows text format.
                'wdFormatTextLineBreaks   3     Windows text format with line breaks preserved.
                'wdFormatDOSText    4     Microsoft DOS text format.
                'wdFormatDOSTextLineBreaks      5     Microsoft DOS text with line breaks preserved.
                Return "txt"
            Case 6 'wdFormatRTF     6     Rich text format (RTF).
                Return "rtf"
                'wdFormatEncodedText      7     Encoded text format.
                'wdFormatUnicodeText      7     Unicode text format.

            Case 8, 10 'wdFormatHTML    8     Standard HTML format.
                'wdFormatFilteredHTML     10    Filtered HTML format.
                Return "html"
            Case 9 'wdFormatWebArchive 9     Web archive format.
                Return "mht"
            Case 11 'wdFormatXML    11    Extensible Markup Language (XML) format.
                Return "xml"
            Case 12 'wdFormatXMLDocument  12    XML document format.
                Return "docx"
            Case 13 'wdFormatXMLDocumentMacroEnabled      13    XML document format with macros enabled.
                Return "docm"
            Case 14 'wdFormatXMLTemplate      14    XML template format.
                Return "dotx"
            Case 15 'wdFormatXMLTemplateMacroEnabled      15    XML template format with macros enabled.
                Return "dotm"
            Case 16, 24 'wdFormatDocumentDefault    16    Word default document file format. For Microsoft Office Word 2007, this is the DOCX format.
                'wdFormatStrictOpenXMLDocument    24    Strict Open XML document format.
                Return "docx"
            Case 17 'wdFormatPDF    17    PDF format.
                Return "pdf"
            Case 18 'wdFormatXPS    18    XPS format.
                Return "xps"
            Case 23 'wdFormatOpenDocumentText    23    OpenDocument Text format.
                Return "odt"
            Case Else
                Return String.Empty
        End Select

    End Function

    Private Function GetDocSaveFormat(ByVal ext As String) As Object
        Select Case ext
            Case "doc"
                Return Word.WdSaveFormat.wdFormatDocument
            Case "docx"
                Return Word.WdSaveFormat.wdFormatXMLDocument
            Case "docm"
                Return Word.WdSaveFormat.wdFormatXMLDocumentMacroEnabled
            Case "rtf"
                Return Word.WdSaveFormat.wdFormatRTF
            Case "odt"
                Return Word.WdSaveFormat.wdFormatOpenDocumentText
            Case "xml"
                Return Word.WdSaveFormat.wdFormatXML
            Case Else
                Return Nothing
        End Select
    End Function

    Protected Overloads Overrides Sub InternalDocumentSave(ByVal obj As Object, ByVal saveMode As FWBS.OMS.PrecSaveMode, ByVal printMode As FWBS.OMS.PrecPrintMode, ByVal doc As OMSDocument, ByVal version As DocumentVersion)
        Call CheckObjectIsDoc(obj)

        Dim worddoc As Word.Document = obj

        Using pd As UnProtect = New UnProtect(worddoc)

            'Allow external document id's to be merged into the document
            If worddoc.ProtectionType = Word.WdProtectionType.wdNoProtection Then
                CheckFields(obj)



                UpdateHeaderFooterField(obj, "<<DOCIDEXT>>", doc.ExternalId)
                UpdateHeaderFooterField(obj, "<<DOCREF>>", Convert.ToString(doc.ID))
                UpdateHeaderFooterField(obj, "<<DOCID>>", Convert.ToString(doc.ID))
                UpdateHeaderFooterField(obj, "<<CLNO>>", Convert.ToString(doc.OMSFile.Client.ClientNo))
                UpdateHeaderFooterField(obj, "<<MATNO>>", Convert.ToString(doc.OMSFile.FileNo))

            End If

        End Using



        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = doc.GetStorageProvider()
        provider.SaveMode = saveMode

        Dim item As IStorageItem = version
        If item Is Nothing Then
            item = doc
        End If

        Dim itemExtension As String = item.Extension.ToLowerInvariant()
        Dim fileFormat As Object = GetDocSaveFormat(itemExtension)
        'Create a temporary location for the word file.  This is because we need to make a copy of the local
        'file as word holds an exclusive lock to the file.
        Dim tmppath As System.IO.FileInfo = FWBS.OMS.[Global].GetTempFile(itemExtension)
        Dim tmppath2 As System.IO.FileInfo = FWBS.OMS.[Global].GetTempFile(itemExtension)

        Try
            worddoc.SaveAs(FileName:=tmppath.FullName, FileFormat:=fileFormat, AddToRecentFiles:=False)
            'Do not perform the save if the document is read only.
            If (worddoc.ReadOnly = False) Then
                'The versions functionality will be removed in the next version of Office.
                worddoc.Save()
                ClearRecentFile(worddoc.Path)


                Dim addtorecentfiles As Object = FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList
                worddoc.SaveAs(FileName:=tmppath2.FullName, FileFormat:=fileFormat, AddToRecentFiles:=False)

                'lock by Word.  Send the temp fiole through to the Storage Provider.
                Dim storeres As StoreResults = provider.Store(item, tmppath, obj, True, Me)

                'Resave the document to the the localy cached location of the document so that it frees up the temp file
                Dim file As System.IO.FileInfo = provider.GetLocalFile(storeres.Item)

                worddoc.SaveAs(FileName:=file.FullName, FileFormat:=fileFormat, AddToRecentFiles:=addtorecentfiles)
                StorageManager.CurrentManager.LocalDocuments.Set(storeres.Item, file, True)

                provider.CheckSetting(storeres, file)
            Else
                Throw New OMSException2("SAVEREADONLYDOC", "Cannot save a read only document.", "")
            End If
        Finally
            Try
                tmppath.Delete()
            Catch
            End Try
            Try
                tmppath2.Delete()
            Catch
            End Try
        End Try
    End Sub


    Protected Overrides Sub InternalPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent)
        InternalPrecedentSave(obj, saveMode, printMode, prec, Nothing)
    End Sub

    Protected Overrides Sub InternalPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent, ByVal version As PrecedentVersion)
        'Variables
        Call CheckObjectIsDoc(obj)
        Dim worddoc As Word.Document = obj
        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = prec.GetStorageProvider()

        'Resolve IStorageItem
        Dim item As IStorageItem = version
        If item Is Nothing Then
            item = prec
        End If

        Dim itemExtension As String = item.Extension.ToLowerInvariant()
        'Create a temporary location for the word file.  This is because we need to make a copy of the local
        'file as word holds an exclusive lock to the file.
        Dim tmppath As System.IO.FileInfo = FWBS.OMS.[Global].GetTempFile(itemExtension)
        Dim tmppath2 As System.IO.FileInfo = FWBS.OMS.[Global].GetTempFile(itemExtension)

        Dim fileFormat As Object = Word.WdSaveFormat.wdFormatTemplate
        If itemExtension = "dotx" Then
            fileFormat = Word.WdSaveFormat.wdFormatXMLTemplate
        ElseIf itemExtension = "dotm" Then
            fileFormat = Word.WdSaveFormat.wdFormatXMLTemplateMacroEnabled
        End If

        Try
            worddoc.SaveAs(FileName:=tmppath.FullName, FileFormat:=fileFormat, AddToRecentFiles:=False)
            'Do not perform the save if the document is read only.
            If (worddoc.ReadOnly = False) Then
                'The versions functionality will be removed in the next version of Office.
                worddoc.Save()
                ClearRecentFile(worddoc.Path)

                Dim addtorecentfiles As Object = FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList
                worddoc.SaveAs(FileName:=tmppath2.FullName, FileFormat:=fileFormat, AddToRecentFiles:=False)

                'lock by Word.  Send the temp fiole through to the Storage Provider.
                Dim storeres As StoreResults = provider.Store(item, tmppath, obj, True, Me)

                'Resave the document to the the locally cached location of the document so that it frees up the temp file
                Dim file As System.IO.FileInfo = provider.GetLocalFile(storeres.Item)

                worddoc.SaveAs(FileName:=file.FullName, FileFormat:=fileFormat, AddToRecentFiles:=addtorecentfiles)
                StorageManager.CurrentManager.LocalDocuments.Set(storeres.Item, file, True)
            Else
                Throw New OMSException2("SAVEREADONLYDOC", "Cannot save a read only document.", "")
            End If
        Finally
            Try
                tmppath.Delete()
            Catch
            End Try
            Try
                tmppath2.Delete()
            Catch
            End Try
        End Try

    End Sub


    Protected Overloads Overrides Sub InternalSave(ByVal obj As Object, ByVal createFileIfNew As Boolean)
        On Error Resume Next
        Call CheckObjectIsDoc(obj)
        Dim doc As Word.Document = obj
        If (doc.ReadOnly = False And doc.Path <> "") Then
            If (doc.Saved = False) Then
                doc.Save()
                If (FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList = False) Then ClearRecentFile(doc.Path)
            End If
        Else
            If (doc.Path = String.Empty) Then
                If (createFileIfNew = False) Then
                    doc.Saved = True
                Else
                    doc.SaveAs(FWBS.OMS.Global.GetTempFile().FullName)
                    ClearRecentFile(doc.Path)
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub EndDocumentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal doc As OMSDocument, ByVal version As DocumentVersion)
        Dim again As Boolean = False

        If doc.DocumentType = "LETTERHEAD" Then
            If (Session.CurrentSession.CurrentUser.AutoWriteLetter) Then
                If (FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("WRITEANOTHERLET", "Would you like to write another letter?", ""), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
                    again = True
                End If
            End If
        End If
        MyBase.EndDocumentSave(obj, saveMode, printMode, doc, version)

        If (again) Then
            RunCommand(Me, "SYSTEM;TEMPLATESTART;LETTERHEAD;ASSOC")
            _activeDoc = _app.ActiveDocument
        End If

    End Sub

    Protected Overrides Sub InternalOnNewVersion(ByVal current As Object, ByVal args As FWBS.OMS.DocumentManagement.Storage.NewVersionEventArgs)
        CheckObjectIsDoc(current)

        'A bit of a bodge but make sure that all changes to the document 
        'caused by a new version are reflected in the file beoing passed to the storage
        'provider.
        Dim currentfile As String = current.FullName
        current.SaveAs(FileName:=args.File.FullName, AddToRecentFiles:=False)
        current.SaveAs(FileName:=currentfile, AddToRecentFiles:=False)

    End Sub


#End Region

#Region "Methods"

    Public Overrides Function GetDocumentCount() As Integer
        Return _app.Documents.Count
    End Function

    Protected Overrides ReadOnly Property CanCompare() As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overloads Overrides Function CompareDocument(ByVal master As Object, ByVal child As Object) As Object
        CheckObjectIsDoc(master)
        CheckObjectIsDoc(child)

        Dim w_master As Word.Document = master
        Dim w_child As Word.Document = child

        InternalSave(child, DirectCast(Nothing, FWBS.OMS.DocumentManagement.Storage.IStorageItem))
        InternalSave(master, DirectCast(Nothing, FWBS.OMS.DocumentManagement.Storage.IStorageItem))

        Dim childfile As String = w_child.FullName
        Dim masterfile As String = w_master.FullName


        w_master.Activate()
        w_master.ActiveWindow.View.Type = Word.WdViewType.wdPrintView

        Dim currentcomparetool As String
        If (String.IsNullOrEmpty(Session.CurrentSession.CurrentUser.CompareTool) = False) Then
            currentcomparetool = Session.CurrentSession.CurrentUser.CompareTool
        Else
            currentcomparetool = Session.CurrentSession.CompareTool
        End If

        If (String.IsNullOrEmpty(currentcomparetool) = False) Then
            Dim comparetool As String = CodeLookup.GetLookupHelp("DMCOMPARETOOL", currentcomparetool)
            If String.IsNullOrEmpty(comparetool) = False Then
                Try
                    comparetool = comparetool.Replace("%FIRSTFILE%", masterfile)
                    comparetool = comparetool.Replace("%SECONDFILE%", childfile)
                    Shell(comparetool, AppWinStyle.MaximizedFocus, False)
                    DisableVisibleFormsCheck = True
                    Return Nothing
                Catch ex As Exception
                    MsgBox("There was an error running external Compare Tool" + Environment.NewLine + "Command : " + comparetool + Environment.NewLine + "Error:" + ex.Message, MsgBoxStyle.Exclamation)
                End Try
            Else
                w_master.Compare(Name:=childfile, AuthorName:=Session.CurrentSession.CurrentUser.FullName.ToString(), CompareTarget:=Word.WdMergeTarget.wdMergeTargetNew, DetectFormatChanges:=True, IgnoreAllComparisonWarnings:=False, AddToRecentFiles:=False, RemovePersonalInformation:=False, RemoveDateAndTime:=True)
            End If
        Else
            w_master.Compare(Name:=childfile, AuthorName:=Session.CurrentSession.CurrentUser.FullName.ToString(), CompareTarget:=Word.WdMergeTarget.wdMergeTargetNew, DetectFormatChanges:=True, IgnoreAllComparisonWarnings:=False, AddToRecentFiles:=False, RemovePersonalInformation:=False, RemoveDateAndTime:=True)
        End If

        Return _app.ActiveDocument
    End Function


    Private Sub ClearRecentFile(ByVal path As String)
        Dim recent As Word.RecentFile
        For Each recent In _app.RecentFiles
            On Error Resume Next
            If (recent.Path = path) Then recent.Delete()
        Next
    End Sub



    Protected Overrides Sub OnJobProcessed()
        Try
            MyBase.OnJobProcessed()
            RefreshRibbon()
            _app.Run("JobProcessed")
        Catch ex As Exception

        End Try

    End Sub

    Public Overrides Sub AddSlogan(ByVal obj As Object, ByVal val As String)
        Call CheckObjectIsDoc(obj)
        _app.Selection.HomeKey()
        On Error Resume Next
        If _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:="FooterText") Is Nothing Then
            If _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:="ADDTEXTFOOT") Is Nothing Then
                _app.Selection.EndKey(Word.WdUnits.wdStory)
            End If
        End If
        Clipboard.GetDataObject().SetData(DataFormats.Rtf, True, val)
        Dim slogan As System.IO.FileInfo = New System.IO.FileInfo(System.IO.Path.ChangeExtension(FWBS.OMS.[Global].GetTempFile().FullName, "rtf"))
        Dim rdr As System.IO.StreamWriter = slogan.CreateText()
        rdr.Write(val)
        rdr.Close()
        _app.Selection.InsertFile(slogan.FullName)
        slogan.Delete()
    End Sub

    Private Sub AddSignature(ByVal obj As Object, ByVal company As Boolean)
        Call CheckObjectIsDoc(obj)

        Dim doc As Word.Document = obj

        Dim name As String = IIf(company, "SIGNATURE_COMPANY_", "SIGNATURE_FEEEARNER_")

        For ctr As Integer = 1 To doc.Bookmarks.Count + 1
            If doc.Bookmarks.Exists(name & ctr.ToString()) = False Then
                doc.Bookmarks.Add(name & ctr.ToString(), _app.Selection.Range)
                Return
            End If
        Next

    End Sub

    Private Sub SignDocument(ByVal obj As Object, ByVal sigfile As System.IO.FileInfo, ByVal name As String, ByVal bm As Word.Bookmark)
        If sigfile Is Nothing Then Return
        Dim doc As Word.Document = obj
        Dim embedSignature As Object = Session.CurrentSession.EmbedSignaturesIntoWordDocument
        Try
            doc.Shapes(name).Delete()
        Catch
        End Try

        Dim shape As Word.Shape
        If bm Is Nothing Then
            _app.Selection.MoveUp(Count:=1)
            shape = doc.Shapes.AddPicture(Anchor:=_app.Selection.Range, FileName:=sigfile.FullName, LinkToFile:=True, SaveWithDocument:=embedSignature)
        Else

            Dim range As Object = _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:=bm.Name)
            Dim ilshape As Word.InlineShape = _app.Selection.InlineShapes.AddPicture(FileName:=sigfile.FullName, LinkToFile:=True, SaveWithDocument:=embedSignature)

            ' Workaround: adding of InlineShape may delete the Bookmark, so restore it
            If doc.Bookmarks.Exists(name) = False Then
                bm = doc.Bookmarks.Add(name, range)
            End If

            'Get the x,y locations incase the ConvertToShape method fails when
            'the image goes over to another page (Unspecified error).
            Try
                shape = ilshape.ConvertToShape()
            Catch
                ilshape.Select()
                shape = ilshape.ConvertToShape()
            End Try
        End If
        shape.Select()

        If shape.Width > SIGNATUREMAXWIDTH Or shape.Height > SIGNATUREMAXHEIGHT Then
            Dim ratio As Single = Math.Max(shape.Width / SIGNATUREMAXWIDTH, shape.Height / SIGNATUREMAXHEIGHT)
            shape.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue
            shape.Width = shape.Width / ratio
        End If

        With _app.Selection.ShapeRange
            .Name = name
            .Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse
            .Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse
            .Fill.Transparency = 0.0#
            .Line.Weight = 0.75
            .Line.DashStyle = Microsoft.Office.Core.MsoLineDashStyle.msoLineSolid
            .Line.Style = Microsoft.Office.Core.MsoLineStyle.msoLineSingle
            .Line.Transparency = 0.0#
            .Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse
            .LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue

            .PictureFormat.Brightness = 0.5
            .PictureFormat.Contrast = 0.5
            .PictureFormat.ColorType = Microsoft.Office.Core.MsoPictureColorType.msoPictureAutomatic
            .PictureFormat.CropLeft = 0.0#
            .PictureFormat.CropRight = 0.0#
            .PictureFormat.CropTop = 0.0#
            .PictureFormat.CropBottom = 0.0#
            If bm Is Nothing Then
                .RelativeHorizontalPosition = Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionColumn
                .RelativeVerticalPosition = Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionParagraph
                .Left = _app.InchesToPoints(0)
                .Top = _app.InchesToPoints(0)
            Else
                .Top = .Top - .Height
            End If
            .LockAnchor = True
            .WrapFormat.Type = Word.WdWrapType.wdWrapNone
            .WrapFormat.Side = Word.WdWrapSideType.wdWrapBoth
            .WrapFormat.DistanceTop = _app.InchesToPoints(0)
            .WrapFormat.DistanceBottom = _app.InchesToPoints(0)
            .WrapFormat.DistanceLeft = _app.InchesToPoints(0.13)
            .WrapFormat.DistanceRight = _app.InchesToPoints(0.13)
            If bm Is Nothing Then
                .IncrementLeft(-4)
                .IncrementTop(18)
            End If
            .PictureFormat.TransparentBackground = Microsoft.Office.Core.MsoTriState.msoCTrue
            .PictureFormat.TransparencyColor = RGB(255, 255, 255)
            .Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse
            .ZOrder(Microsoft.Office.Core.MsoZOrderCmd.msoSendBehindText)
        End With

        _app.Selection.Collapse(Word.WdCollapseDirection.wdCollapseEnd)
    End Sub

    Private Sub SignDocument(ByVal obj As Object)
        Try
            Call CheckObjectIsDoc(obj)

            _app.ScreenUpdating = False

            Dim doc As Word.Document = obj

            Dim sig_fee As Signature = Nothing
            Dim sig_comp As Signature = Nothing

            Dim sigfile_fee As System.IO.FileInfo = Nothing
            Dim sigfile_comp As System.IO.FileInfo = Nothing

            If Session.CurrentSession.CurrentUser.WorksForMatterHandler Then

                Dim omsdoc As OMSDocument

                omsdoc = MyBase.GetCurrentDocument(obj)

                If omsdoc Is Nothing Then

                    Dim fileId As Object = GetDocVariable(obj, FILE)
                    If Not fileId Is Nothing Then
                        Session.CurrentSession.CurrentFile = OMSFile.GetFile(fileId)
                    End If

                    If Session.CurrentSession.CurrentFile Is Nothing Then
                        sig_fee = Session.CurrentSession.CurrentFeeEarner.Signature
                    Else
                        sig_fee = Session.CurrentSession.CurrentFile.PrincipleFeeEarner.Signature
                    End If

                Else
                    sig_fee = omsdoc.OMSFile.PrincipleFeeEarner.Signature
                End If

            Else
                sig_fee = Session.CurrentSession.CurrentFeeEarner.Signature
            End If



            If Not sig_fee Is Signature.Empty Then
                sigfile_fee = sig_fee.ToFile()
            End If


            sig_comp = Session.CurrentSession.CurrentBranch.CompanySignature
            If sig_comp Is Signature.Empty Then
                sig_comp = Session.CurrentSession.CompanySignature
            End If


            If Not sig_comp Is Signature.Empty Then
                sigfile_comp = sig_comp.ToFile()
            End If

            Dim show_msg As Boolean = True
            _app.Selection.MoveUp(Count:=1)
            For i As Integer = doc.Shapes.Count To 1 Step -1
                Dim s As Word.Shape = doc.Shapes(i)
                Try
                    If s.Name.StartsWith("SIGNATURE") Then
                        s.Delete()
                    End If
                Catch
                End Try
            Next

            Dim bookmarks As Generic.List(Of Word.Bookmark) = New Generic.List(Of Word.Bookmark)
            For Each bm As Word.Bookmark In doc.Bookmarks
                If bm.Name.StartsWith("SIGNATURE_") Then bookmarks.Add(bm)
            Next

            For Each bm As Word.Bookmark In bookmarks
                If bm.Name.StartsWith("SIGNATURE_FEEEARNER") Then
                    show_msg = False
                    If Session.CurrentSession.CurrentFeeEarner.CanUseSignature() Then
                        SignDocument(doc, sigfile_fee, bm.Name, bm)
                    Else
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("ERRSIGSEC", "You do not have permission to use the Signature")
                    End If
                ElseIf bm.Name.StartsWith("SIGNATURE_COMPANY") Then
                    show_msg = False
                    Dim s As String = CodeLookup.GetLookup("USRROLES", "COMSIGUSE")
                    If s <> "" Then
                        Session.CurrentSession.CurrentUser.ValidateRoles("COMSIGUSE")
                    End If
                    SignDocument(doc, sigfile_comp, bm.Name, bm)
                End If
            Next

            CaptureFindDialogValues(obj)

            _app.Selection.Find.ClearFormatting()
            With _app.Selection.Find
                .Text = Session.CurrentSession.Resources.GetResource("SIGNOFFNOTSIR", "Yours sincerely", "", False).Text
                .Replacement.Text = "1"
                .Forward = True
                .Wrap = Word.WdFindWrap.wdFindContinue
                .Format = False
                .MatchCase = False
                .MatchWholeWord = False
                .MatchWildcards = False
                .MatchSoundsLike = False
                .MatchAllWordForms = False
            End With
            If _app.Selection.Find.Execute = False Then
                With _app.Selection.Find
                    .Text = Session.CurrentSession.Resources.GetResource("SIGNOFFSIR", "Yours faithfully", "", False).Text
                    .Replacement.Text = "1"
                    .Forward = True
                    .Wrap = Word.WdFindWrap.wdFindContinue
                    .Format = False
                    .MatchCase = False
                    .MatchWholeWord = False
                    .MatchWildcards = False
                    .MatchSoundsLike = False
                    .MatchAllWordForms = False
                End With
                If _app.Selection.Find.Execute = False Then
                    If show_msg Then MessageBox.ShowInformation("ERRNOSIGPOS", "Unable to Add AutoSignature! Can't find the Position for it!")
                    ScreenRefresh()
                Else
                    Dim s As String
                    s = CodeLookup.GetLookup("USRROLES", "COMSIGUSE")
                    If s <> "" Then
                        Session.CurrentSession.CurrentUser.ValidateRoles("COMSIGUSE")
                    End If
                    SignDocument(obj, sigfile_comp, "SIGNATURE", Nothing)
                End If
            Else
                If Session.CurrentSession.CurrentFeeEarner.CanUseSignature() Then
                    SignDocument(obj, sigfile_fee, "SIGNATURE", Nothing)
                Else
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("ERRSIGSEC", "You do not have permission to use the Signature")
                End If
            End If
        Finally
            ScreenRefresh()
            ResetFindDialogValues(obj)
        End Try

    End Sub


    Private Sub RemoveSignature()
        If _app.Documents.Count = 0 Then Exit Sub
        On Error Resume Next
        Dim doc As Word.Document = _app.ActiveDocument
        For ctr As Integer = doc.Shapes.Count To 1 Step -1
            Dim sh As Word.Shape = doc.Shapes(ctr)
            If sh.Name.StartsWith("SIGNATURE") Then sh.Delete()
        Next
    End Sub

    Private Sub AddLogo(ByVal obj As Object, ByVal type As String)

        Try


            Call CheckObjectIsDoc(obj)


            Dim sig As Signature = Nothing
            Dim sigfile As System.IO.FileInfo
            Dim doc As Word.Document = obj


            Select Case type
                Case "FILECOPYIMAGE"
                    sig = Session.CurrentSession.FileCopyLogo
                Case "DRAFTIMAGE"
                    sig = Session.CurrentSession.DraftLogo
            End Select

            sigfile = sig.ToFile()
            _app.ScreenUpdating = False

            If _app.ActiveWindow.View.SplitSpecial <> Word.WdSpecialPane.wdPaneNone Then
                _app.ActiveWindow.Panes(2).Close()
            End If
            If _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdNormalView Or _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdOutlineView Or _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdMasterView Then
                _app.ActiveWindow.ActivePane.View.Type = _app.WdViewType.wdNormalView
            End If

            _app.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageHeader

            Try
                doc.Shapes.AddPicture(Anchor:=_app.Selection.Range, FileName:=sigfile.FullName, LinkToFile:=False, SaveWithDocument:=True)
            Catch ex As Exception
            End Try

            Dim namefound As Boolean = True
            Dim typenum As Integer = 0
            Dim typename As String = type

            Do While (namefound)

                Dim found As Boolean = False

                For Each shape As Microsoft.Office.Interop.Word.Shape In _app.Selection.HeaderFooter.Shapes
                    If shape.Name = typename Then
                        found = True
                        Exit For
                    End If
                Next

                If found Then
                    typenum += 1
                    typename = type + Convert.ToString(typenum)
                Else
                    namefound = False
                End If

            Loop

            _app.Selection.HeaderFooter.Shapes(_app.Selection.HeaderFooter.Shapes.Count).Name = typename
            _app.Selection.HeaderFooter.Shapes(_app.Selection.HeaderFooter.Shapes.Count).Select()
            _app.Selection.ShapeRange.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse
            _app.Selection.ShapeRange.Fill.Transparency = 0.0#
            _app.Selection.ShapeRange.Line.Weight = 0.75
            _app.Selection.ShapeRange.Line.DashStyle = Microsoft.Office.Core.MsoLineDashStyle.msoLineSolid
            _app.Selection.ShapeRange.Line.Style = Microsoft.Office.Core.MsoLineStyle.msoLineSingle
            _app.Selection.ShapeRange.Line.Transparency = 0.0#
            _app.Selection.ShapeRange.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse
            _app.Selection.ShapeRange.Height = 213.1
            _app.Selection.ShapeRange.Width = 216.0#
            _app.Selection.ShapeRange.PictureFormat.Brightness = 0.85
            _app.Selection.ShapeRange.PictureFormat.Contrast = 0.15
            _app.Selection.ShapeRange.PictureFormat.CropLeft = 0.0#
            _app.Selection.ShapeRange.PictureFormat.CropRight = 0.0#
            _app.Selection.ShapeRange.PictureFormat.CropTop = 0.0#
            _app.Selection.ShapeRange.PictureFormat.CropBottom = 0.0#
            _app.Selection.ShapeRange.RelativeHorizontalPosition = Word.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionColumn
            _app.Selection.ShapeRange.RelativeVerticalPosition = Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionParagraph
            _app.Selection.ShapeRange.Left = _app.InchesToPoints(0)
            _app.Selection.ShapeRange.Top = _app.InchesToPoints(0)
            _app.Selection.ShapeRange.LockAnchor = False
            _app.Selection.ShapeRange.WrapFormat.Type = Word.WdWrapType.wdWrapNone
            _app.Selection.ShapeRange.WrapFormat.Side = Word.WdWrapSideType.wdWrapBoth
            _app.Selection.ShapeRange.WrapFormat.DistanceTop = _app.InchesToPoints(0)
            _app.Selection.ShapeRange.WrapFormat.DistanceBottom = _app.InchesToPoints(0)
            _app.Selection.ShapeRange.WrapFormat.DistanceLeft = _app.InchesToPoints(0.13)
            _app.Selection.ShapeRange.WrapFormat.DistanceRight = _app.InchesToPoints(0.13)
            _app.Selection.ShapeRange.IncrementLeft(100.4)
            _app.Selection.ShapeRange.IncrementTop(112.8)
            _app.CommandBars("Picture").Visible = True
            _app.CommandBars("Picture").Visible = False
            _app.Selection.ShapeRange.IncrementTop(7.2)
            _app.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekMainDocument
        Finally
            ScreenRefresh()
        End Try


    End Sub

    Private Sub RemoveLogo(ByVal type As String)
        Try

            If _app.Documents.Count = 0 Then Exit Sub
            _app.ScreenUpdating = False

            Try
                If _app.ActiveWindow.View.SplitSpecial <> Word.WdSpecialPane.wdPaneNone Then
                    _app.ActiveWindow.Panes(2).Close()
                End If
                If _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdNormalView Or _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdOutlineView Or _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdMasterView Then
                    _app.ActiveWindow.ActivePane.View.Type = Word.WdViewType.wdNormalView
                End If
                _app.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageHeader

                Dim typefound As Boolean = True
                Dim typename As String = type
                Dim typenum As Integer = 0

                Do While (typefound)

                    Dim found As Boolean

                    For Each shape As Microsoft.Office.Interop.Word.Shape In _app.Selection.HeaderFooter.Shapes
                        If shape.Name = typename Then
                            found = True
                            Exit For
                        End If
                    Next

                    If found Then
                        _app.Selection.HeaderFooter.Shapes(typename).Delete()

                        typenum += 1
                        typename = type + Convert.ToString(typenum)
                    Else
                        typefound = False
                    End If

                Loop

                _app.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekMainDocument
            Catch ex As Exception

            End Try
        Finally
            ScreenRefresh()
        End Try
    End Sub

    Public Sub SecondStageMerge(ByVal obj As Object, ByVal file As System.IO.FileInfo) Implements FWBS.OMS.Interfaces.ISecondStageMergeOMSApp.SecondStageMerge
        CheckObjectIsDoc(obj)
        If file.Exists Then
            SetDocVariable(obj, MAILMERGEFILE, file.FullName)
            obj.MailMerge.OpenDataSource(Name:=file.FullName, Format:=Word.WdOpenFormat.wdOpenFormatUnicodeText, readonly:=True)
            obj.MailMerge.ViewMailMergeFieldCodes = 0
        End If
    End Sub

    Protected Overrides Sub SetWindowCaption(ByVal obj As Object, ByVal caption As String)
        Call CheckObjectIsDoc(obj)
        Try
            obj.ActiveWindow.Caption = caption
        Catch ex As Exception

        End Try

    End Sub

    Protected Overrides Sub InsertText(ByVal obj As Object, ByVal precLink As PrecedentLink)
        Call CheckObjectIsDoc(obj)
        Dim fetch As FWBS.OMS.DocumentManagement.Storage.FetchResults = precLink.Merge()
        If (fetch Is Nothing) Then Return
        Dim file As System.IO.FileInfo = fetch.LocalFile
        If Not file Is Nothing Then
            obj.Application.Selection.InsertFile(file.FullName)
            obj.Application.Selection.TypeBackspace()
        End If

        Dim a As Integer
        For a = obj.Fields.Count To 1 Step -1
            CreateDocVariableFromField(obj, obj.Fields(a))
        Next a

    End Sub

    Protected Overrides Sub CheckObjectIsDoc(ByRef obj As Object)
        If obj Is Me Then
            obj = _app.ActiveDocument
        ElseIf obj Is _app Then
            obj = _app.ActiveDocument
        ElseIf TypeOf obj Is Word.Document Then
            obj = obj
        ElseIf TypeOf obj Is Word.Window Then
            obj = obj.Document
        Else
            Throw New Exception("The passed parameter is not a Word.Document object.")
        End If
    End Sub


    Public Overrides Sub ActivateApplication()
        Dim ctr As Integer = IIf(IsAddinInstance Or _app Is Nothing, 0, -1)
        Dim forceNewInstance As Boolean = False, __ As Boolean
        RemoveDelegates()
        If _app Is Nothing Then
Retry:
            Try
                _app = If(forceNewInstance, GetObject("", "Word.Application"), GetObject(, "Word.Application"))
                RunAutoExec = RunAutoExec Or forceNewInstance
            Catch
                _app = GetObject("", "Word.Application")
                RunAutoExec = True
            End Try
        End If
        Try
            AddDelegates()
            _app.Visible = True
            Try
                'DM - 20/04/04 - Incase a dialog box is open.  Stemmed from a fancy script that GM developed for picking precedent inserts on the fly.
                _app.Activate()
            Catch
            End Try
            If (IsAddinInstance = False AndAlso _app.Documents.Count > 0) Then
                __ = _app.ActiveDocument.Saved ' Throws "Call was rejected by callee (RPC_E_CALL_REJECTED)" incase Word is busy
            End If

        Catch ex As System.Runtime.InteropServices.COMException
            Dispose()
            If (ctr = -1) Then
                ctr = 0
                If (ex.HResult = -2147023174) Then GoTo Retry ' RPC_S_SERVER_UNAVAILABLE
            End If
            If (ctr = 0) Then
                ctr = 1
                forceNewInstance = IsAddinInstance = False AndAlso (ex.HResult = -2146777998 Or ex.HResult = -2147418111) ' VBA_E_IGNORE Or RPC_E_CALL_REJECTED
                GoTo Retry
            End If
        Catch ex As Exception
            If (ctr < 1) Then
                ctr += 1
                GoTo Retry
            End If
        End Try

    End Sub

    Public Overrides Sub ActivateDocument(ByVal obj As Object)
        On Error Resume Next
        If Not obj Is Nothing Then
            Call CheckObjectIsDoc(obj)
            obj.Activate()
            RunCommand(obj, "REFRESHRIBBON")
            FWBS.Common.Functions.SetForegroundWindow(ActiveWindow().Handle)
        End If
    End Sub

    Protected Overrides Sub ScreenRefresh()
        Try
            _app.ScreenUpdating = True
            _app.Application.ScreenRefresh()
        Catch
        End Try
    End Sub

    Protected Overrides Sub PositionCursor()
        Try
            _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:="Positioncursorhere")
        Catch
        End Try
    End Sub


    Private Sub UpdateHeaderFooterField(ByRef doc As Word.Document, ByVal tofind As String, ByVal repl As String)

        On Error Resume Next
        ' Check for a Field DONTPROCESSHEADERFIELDS if this exists then Don't run UpdateHeaderFooterFields
        Dim tmpstr As Object = doc.Variables("DONTPROCESSHEADERFIELDS").Value
        If Err.Number = 0 Then
            Exit Sub
        End If

        If doc.ProtectionType <> Word.WdProtectionType.wdNoProtection Then
            Exit Sub
        End If


        CaptureFindDialogValues(doc)

        doc.Application.ScreenUpdating = False
        doc.Application.Selection.HomeKey(Unit:=Microsoft.Office.Interop.Word.WdUnits.wdStory)
        If doc.Application.ActiveWindow.View.SplitSpecial <> Microsoft.Office.Interop.Word.WdSpecialPane.wdPaneNone Then
            doc.Application.ActiveWindow.Panes(2).Close()
        End If
        ' if not in Page view set to page view
        If doc.Application.ActiveWindow.ActivePane.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdNormalView Or doc.Application.ActiveWindow.
         ActivePane.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdOutlineView Or doc.Application.ActiveWindow.ActivePane.View.Type _
          = Microsoft.Office.Interop.Word.WdViewType.wdMasterView Then
            doc.Application.ActiveWindow.ActivePane.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView
        End If
        doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader
        doc.Application.Selection.Find.ClearFormatting()
        doc.Application.Selection.Find.Replacement.ClearFormatting()
        With doc.Application.Selection.Find
            .Text = tofind
            .Replacement.Text = repl
            .Forward = True
            .Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue
            .Format = False
            .MatchCase = False
            .MatchWholeWord = False
            .MatchWildcards = False
            .MatchSoundsLike = False
            .MatchAllWordForms = False
        End With
        doc.Application.Selection.Find.Execute(Replace:=Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll)
        doc.Application.ActiveWindow.ActivePane.LargeScroll(Down:=2)
        If doc.Application.Selection.HeaderFooter.IsHeader = True Then
            doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter
        Else
            doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader
        End If
        doc.Application.Selection.Find.ClearFormatting()
        doc.Application.Selection.Find.Replacement.ClearFormatting()
        With doc.Application.Selection.Find
            .Text = tofind
            .Replacement.Text = repl
            .Forward = True
            .Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue
            .Format = False
            .MatchCase = False
            .MatchWholeWord = False
            .MatchWildcards = False
            .MatchSoundsLike = False
            .MatchAllWordForms = False
        End With
        doc.Application.Selection.Find.Execute(Replace:=Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll)

        ' If more than one page then do the Header footer on the Second Page
        doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument
        doc.Application.Selection.Find.Execute(Replace:=Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll)
        If doc.Application.ActiveDocument.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages) > 1 Then
            doc.Application.Selection.EndKey(Microsoft.Office.Interop.Word.WdUnits.wdStory)
            doc.GoTo(What:=Microsoft.Office.Interop.Word.WdGoToItem.wdGoToPage, Which:=Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToNext, Count:=1)
            doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader
            doc.Application.Selection.Find.ClearFormatting()
            doc.Application.Selection.Find.Replacement.ClearFormatting()
            With doc.Application.Selection.Find
                .Text = tofind
                .Replacement.Text = repl
                .Forward = True
                .Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue
                .Format = False
                .MatchCase = False
                .MatchWholeWord = False
                .MatchWildcards = False
                .MatchSoundsLike = False
                .MatchAllWordForms = False
            End With
            doc.Application.Selection.Find.Execute(Replace:=Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll)
            doc.Application.ActiveWindow.ActivePane.LargeScroll(Down:=2)
            If doc.Application.Selection.HeaderFooter.IsHeader = True Then
                doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter
            Else
                doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader
            End If
            doc.Application.Selection.Find.ClearFormatting()
            doc.Application.Selection.Find.Replacement.ClearFormatting()
            With doc.Application.Selection.Find
                .Text = tofind
                .Replacement.Text = repl
                .Forward = True
                .Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue
                .Format = False
                .MatchCase = False
                .MatchWholeWord = False
                .MatchWildcards = False
                .MatchSoundsLike = False
                .MatchAllWordForms = False
            End With
            doc.Application.Selection.Find.Execute(Replace:=Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll)
            doc.Application.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument
            doc.Application.Selection.Find.Execute(Replace:=Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll)
        End If

        ResetFindDialogValues(doc)

        doc.Application.ScreenUpdating = True
        doc.Application.ScreenRefresh()
    End Sub



    Protected Function CheckStopCodesNoFields(ByVal obj As Object) As Boolean
        Call CheckObjectIsDoc(obj)
        Dim doc As Word.Document = obj

        'If the document is protected this code cannot be executed (wdNoProtection -1)
        If doc.ProtectionType <> -1 Then
            CheckStopCodesNoFields = True
            Exit Function
        End If

        'Capture intitial Find dialog setting
        CaptureFindDialogValues(doc)

        'Perform the search
        doc.Application.Selection.HomeKey(Unit:=Microsoft.Office.Interop.Word.WdUnits.wdStory)
top:
        doc.Application.Selection.Find.ClearFormatting()
        With doc.Application.Selection.Find
            .Text = "\[*\]"
            .Replacement.Text = ""
            .Forward = True
            .Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue
            .Format = False
            .MatchCase = False
            .MatchWholeWord = False
            .MatchSoundsLike = False
            .MatchAllWordForms = False
            .MatchPhrase = False
            .MatchPrefix = False
            .MatchSuffix = False
            .MatchFuzzy = False
            .MatchWildcards = True
        End With
        If doc.Application.Selection.Find.Execute Then
            CheckStopCodesNoFields = False
            'Exit Function
        Else
            doc.Application.Selection.Find.ClearFormatting()
            With doc.Application.Selection.Find
                .Text = "XXXX"
                .Replacement.Text = ""
                .Forward = True
                .Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue
                .Format = False
                .MatchCase = False
                .MatchWholeWord = False
                .MatchSoundsLike = False
                .MatchAllWordForms = False
                .MatchPhrase = False
                .MatchPrefix = False
                .MatchSuffix = False
                .MatchFuzzy = False
                .MatchWildcards = True
            End With
            If doc.Application.Selection.Find.Execute Then
                CheckStopCodesNoFields = False
            Else
                CheckStopCodesNoFields = True
            End If
        End If

        'Reset Find dialog setting to initial values
        ResetFindDialogValues(doc)

    End Function

    Private Sub ResetFindDialogValues(ByVal obj As Object)
        Try
            Call CheckObjectIsDoc(obj)
            Dim doc As Word.Document = obj

            doc.Application.Selection.Find.MatchWholeWord = MatchWholeWord
            doc.Application.Selection.Find.MatchWildcards = MatchWildcards
            doc.Application.Selection.Find.MatchSoundsLike = MatchSoundsLike
            doc.Application.Selection.Find.MatchAllWordForms = MatchAllWordForms
            doc.Application.Selection.Find.MatchPhrase = MatchPhrase
            doc.Application.Selection.Find.MatchFuzzy = MatchFuzzy
            doc.Application.Selection.Find.Text = MatchText
            doc.Application.Selection.Find.Replacement.Text = ReplacementText
            doc.Application.Selection.Find.Forward = Forward
            doc.Application.Selection.Find.Format = Format
            doc.Application.Selection.Find.Wrap = Wrap
        Catch
        End Try


    End Sub


    Private Sub CaptureFindDialogValues(ByVal obj As Object)
        Try
            Call CheckObjectIsDoc(obj)
            Dim doc As Word.Document = obj

            MatchText = doc.Application.Selection.Find.Text
            ReplacementText = doc.Application.Selection.Find.Replacement.Text
            Forward = doc.Application.Selection.Find.Forward
            MatchWholeWord = doc.Application.Selection.Find.MatchWholeWord
            MatchWildcards = doc.Application.Selection.Find.MatchWildcards
            MatchSoundsLike = doc.Application.Selection.Find.MatchSoundsLike
            MatchAllWordForms = doc.Application.Selection.Find.MatchAllWordForms
            MatchPhrase = doc.Application.Selection.Find.MatchPhrase
            MatchFuzzy = doc.Application.Selection.Find.MatchFuzzy
            MatchCase = doc.Application.Selection.Find.MatchFuzzy
            Format = doc.Application.Selection.Find.Format
            Wrap = doc.Application.Selection.Find.Wrap
        Catch
        End Try

    End Sub

#End Region

#Region "Field Routines"

    Public Function IsSecondStageMergeDoc(ByVal obj As Object) As Boolean Implements FWBS.OMS.Interfaces.ISecondStageMergeOMSApp.IsSecondStageMergeDoc
        Call CheckObjectIsDoc(obj)
        Dim a As Integer
        For a = obj.Fields.Count To 1 Step -1
            On Error Resume Next
            If obj.Fields.Item(a).Type = Word.WdFieldType.wdFieldMergeField Then Return True
        Next a
        Return False
    End Function


    Public Sub AddSecondStageMergeField(ByVal obj As Object, ByVal fieldName As String) Implements FWBS.OMS.Interfaces.ISecondStageMergeOMSApp.AddSecondStageMergeField
        Call CheckObjectIsDoc(obj)
        On Error Resume Next
        _app.Selection.Fields.Add(Range:=_app.Selection.Range, Type:=Word.WdFieldType.wdFieldMergeField, Text:="""" & fieldName & """", PreserveFormatting:=True)
    End Sub

    Public Overrides Sub AddField(ByVal obj As Object, ByVal fieldName As String)
        MyBase.AddField(obj, fieldName)
        Call CheckObjectIsDoc(obj)
        On Error Resume Next
        _app.Selection.Fields.Add(Range:=_app.Selection.Range, Type:=Word.WdFieldType.wdFieldDocVariable, Text:="""" & fieldName & """", PreserveFormatting:=True)
    End Sub

    Public Overrides Sub DeleteField(ByVal obj As Object, ByVal fieldName As String)
        MyBase.DeleteField(obj, fieldName)
        CheckObjectIsDoc(obj)
        Dim indoc As Word.Document = DirectCast(obj, Word.Document)
        Dim a As Integer

        For a = indoc.Fields.Count To 1 Step -1
            On Error Resume Next
            Dim code As String = indoc.Fields.Item(a).Code.Text
            If code.IndexOf("""" & fieldName & """") > 0 Then
                indoc.Fields.Item(a).Delete()
            End If
        Next a

        ScreenRefresh()
    End Sub

    Public Overloads Overrides Sub SetFieldValue(ByVal obj As Object, ByVal index As Integer, ByVal val As Object)

        Call CheckObjectIsDoc(obj)

        Dim strval As String = Convert.ToString(val)

        If TypeOf val Is FWBS.OMS.Address Then
            Dim add As FWBS.OMS.Address
            add = CType(val, FWBS.OMS.Address)
            If Not (String.IsNullOrWhiteSpace(add.AddressFormat)) Then
                obj.Variables(index).Value = UseCountryAddressFormat(add)
            Else
                obj.Variables(index).Value = DefaultFieldFormat(strval)
            End If
        Else
            obj.Variables(index).Value = DefaultFieldFormat(strval)
        End If

        index = index + 1

    End Sub

    Public Overloads Overrides Sub SetFieldValue(ByVal obj As Object, ByVal name As String, ByVal val As Object)

        Call CheckObjectIsDoc(obj)

        Dim strval As String = Convert.ToString(val)

        If TypeOf val Is FWBS.OMS.Address Then
            Dim add As FWBS.OMS.Address
            add = CType(val, FWBS.OMS.Address)
            If Not (String.IsNullOrWhiteSpace(add.AddressFormat)) Then
                obj.Variables(name).Value = UseCountryAddressFormat(add)
            Else
                obj.Variables(name).Value = DefaultFieldFormat(strval)
            End If
        Else
            obj.Variables(name).Value = DefaultFieldFormat(strval)
        End If

    End Sub

    Dim partAdded As Boolean = False

    Private Function UseCountryAddressFormat(add As FWBS.OMS.Address) As String

        Dim strAdd As String = ""
        Dim strPrevious As String = ""
        Dim addParts As String() = add.AddressFormat.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

        For i As Integer = 0 To addParts.Length - 1
            If Not (String.IsNullOrWhiteSpace(addParts(i))) Then
                Select Case addParts(i).ToUpper()
                    Case "LINE1"
                        strAdd = strAdd & CheckAddressPart(add.Line1)
                    Case "LINE2"
                        strAdd = strAdd & CheckAddressPart(add.Line2)
                    Case "LINE3"
                        strAdd = strAdd & CheckAddressPart(add.Line3)
                    Case "LINE4"
                        strAdd = strAdd & CheckAddressPart(add.Line4)
                    Case "LINE5"
                        strAdd = strAdd & CheckAddressPart(add.Line5)
                    Case "COUNTRY"
                        strAdd = strAdd & CheckAddressPart(add.Country)
                    Case "POSTCODE", "ZIPCODE"
                        strAdd = strAdd & CheckAddressPart(add.Postcode)
                    Case "DELIMITER"
                        If addParts(i) <> strPrevious Then
                            strAdd = strAdd & Chr(13)
                            partAdded = True
                        End If
                End Select
            End If
            If (partAdded) Then
                strPrevious = addParts(i)
                partAdded = False
            End If
        Next

        Return strAdd

    End Function


    Private Function CheckAddressPart(addressPart As String) As String

        Dim result As String
        If (String.IsNullOrWhiteSpace(addressPart)) Then
            result = String.Empty
        Else
            result = addressPart.Trim() & " "
            partAdded = True
        End If

        Return result

    End Function


    Private Function DefaultFieldFormat(strval As String) As String

        If (strval = String.Empty) Then strval = " "
        strval = Replace(strval, Environment.NewLine, Chr(13))
        strval = Replace(strval, "\n", Chr(13))

        Return strval

    End Function


    Public Overrides Function GetFieldCount(ByVal obj As Object) As Integer
        Try
            Call CheckObjectIsDoc(obj)
            Return obj.Variables.Count
        Catch
            Return 0
        End Try
    End Function

    Public Overrides Function GetFieldName(ByVal obj As Object, ByVal index As Integer) As String
        Call CheckObjectIsDoc(obj)
        index = index + 1
        Return obj.Variables(index).Name
    End Function

    Public Overloads Overrides Function GetFieldValue(ByVal obj As Object, ByVal index As Integer) As Object
        Call CheckObjectIsDoc(obj)
        index = index + 1
        Return obj.Variables(index).Value
    End Function

    Public Overloads Overrides Function GetFieldValue(ByVal obj As Object, ByVal name As String) As Object
        Call CheckObjectIsDoc(obj)
        Return obj.Variables(name).Value
    End Function

    Protected Overrides Sub CustomUpdateDocFields(ByVal obj As Object, ByVal precLink As PrecedentLink)
        'GM wanted this to be added these two extra parsing routines - 09/01/07
        '********
        CheckObjectIsDoc(obj)

        Dim parser As FieldParser = precLink.Parser

        Disassemble(obj, parser)

        On Error Resume Next
        Dim tmpstr As Object = obj.Variables("PROCESSFORMFIELDS").Value

        If Err.Number = 0 Then

            On Error GoTo 0

            If obj.FormFields.Count > 0 Then
                For Each fField As Word.FormField In obj.FormFields

                    Dim strParse As String = String.Empty


                    If Left(fField.TextInput.Default, 1) = "^" Then
                        strParse = Mid(fField.TextInput.Default, 2)
                    ElseIf Left(fField.StatusText, 1) = "^" Then
                        strParse = Mid(fField.StatusText, 2)
                    ElseIf Left(fField.HelpText, 1) = "^" Then
                        strParse = Mid(fField.HelpText, 2)
                    End If


                    If strParse.Length > 0 Then
                        On Error Resume Next
                        fField.Result = "" & Convert.ToString(parser.Parse(strParse, "Error Parsing: " & strParse))
                        On Error GoTo 0
                    End If
                Next
            End If

            On Error Resume Next
            obj.Variables.Add("DONTUPDATEFORMFIELDS", " ")
            On Error GoTo 0

        End If
    End Sub

    Public Overrides Sub CheckFields(ByVal obj As Object)
        Call CheckObjectIsDoc(obj)

        Dim indoc As Word.Document = DirectCast(obj, Word.Document)
        Dim a As Integer

        '14/06/2004 - Added by GM to prevent update of fields on documents with this doc variable existing.  Improved speed for CDS11 for example.

        If (HasDocVariableInternal(indoc, "DONTUPDATEFORMFIELDS")) Then
            Exit Sub
        End If


        Using pd As WordOMS2.UnProtect = New WordOMS2.UnProtect(indoc)
            ' Used to detect and if the document is still protected then exit update fields
            If indoc.ProtectionType <> Word.WdProtectionType.wdNoProtection Then
                Exit Sub
            End If


            Try

                indoc.Application.ScreenUpdating = False



                Dim expflds(0) As Word.Field

                If indoc.MailMerge.State = Word.WdMailMergeState.wdMainAndDataSource Then
                    For a = indoc.Fields.Count To 1 Step -1
                        If indoc.Fields.Item(a).Type = Word.WdFieldType.wdFieldMergeField Then
                            If indoc.Fields.Item(a).Result.Text = "[__]" Then
                                indoc.Fields.Item(a).Unlink()
                            End If
                        End If
                    Next a
                    indoc.MailMerge.DataSource.Close()
                    indoc.MailMerge.MainDocumentType = Word.WdMailMergeMainDocType.wdNotAMergeDocument
                    Dim path As String = GetDocVariable(obj, MAILMERGEFILE, "")
                    If path <> "" Then
                        System.IO.File.Delete(path)
                    End If
                End If

                ' Normal Check Fields Functionality

                For a = indoc.Fields.Count To 1 Step -1
                    Dim fld As Word.Field = indoc.Fields(a)
                    ' Code now only works where the field is of a certain type
                    If CheckUpdateFieldType(fld.Type) Then
                        If fld.Type = Word.WdFieldType.wdFieldExpression Then
                            ReDim Preserve expflds(expflds.GetUpperBound(0) + 1)
                            expflds(expflds.GetUpperBound(0)) = fld
                        End If
                        LinkField(fld)
                    End If

                Next a

                For s As Integer = 1 To indoc.Sections.Count
                    Dim section As Word.Section = indoc.Sections(s)
                    For h As Integer = 1 To section.Headers.Count
                        Dim header As Word.HeaderFooter = section.Headers(h)
                        For b As Integer = header.Range.Fields.Count To 1 Step -1
                            Dim fld As Word.Field = header.Range.Fields(b)
                            If CheckUpdateFieldType(fld.Type) Then
                                If fld.Type = Word.WdFieldType.wdFieldExpression Then
                                    ReDim Preserve expflds(expflds.GetUpperBound(0) + 1)
                                    expflds(expflds.GetUpperBound(0)) = fld
                                End If
                                LinkField(fld)
                            End If
                        Next
                    Next
                    For f As Integer = 1 To section.Footers.Count
                        Dim footer As Word.HeaderFooter = section.Footers(f)
                        For b As Integer = footer.Range.Fields.Count To 1 Step -1
                            Dim fld As Word.Field = footer.Range.Fields(b)
                            If CheckUpdateFieldType(fld.Type) Then
                                If fld.Type = Word.WdFieldType.wdFieldExpression Then
                                    ReDim Preserve expflds(expflds.GetUpperBound(0) + 1)
                                    expflds(expflds.GetUpperBound(0)) = fld
                                End If
                                LinkField(fld)
                            End If
                        Next
                    Next
                Next

                'Added to update expressional fields that refer to each other.
                If Not expflds Is Nothing Then
                    For Each expfld As Word.Field In expflds
                        If expfld Is Nothing Then
                        Else
                            LinkField(expfld)
                        End If
                    Next
                End If

            Catch ex As Exception
                Throw
            Finally
                indoc.Application.ScreenUpdating = True
            End Try




        End Using
    End Sub

    Public Overrides Sub InsertTextIntoDocument(ByVal obj As Object, ByVal text As String)
        'Add's the text string to the template on merge
        Call CheckObjectIsDoc(obj)
        _app.Selection.TypeText(text)
    End Sub

    Private Function CheckUpdateFieldType(ByVal infld As Word.WdFieldType) As Boolean
        ' Function to validate if the Field is to be processed or updated;
        Select Case infld
            Case Word.WdFieldType.wdFieldDocVariable, Word.WdFieldType.wdFieldExpression, Word.WdFieldType.wdFieldDate, Word.WdFieldType.wdFieldTime
                Return True
            Case Else
                Return False
        End Select

    End Function

    Private Sub LinkField(ByVal fld As Word.Field)
        If fld.Type = Word.WdFieldType.wdFieldTime Or fld.Type = Word.WdFieldType.wdFieldDate Then
            'Unlink any date time fields
            fld.Unlink()
        Else
 
            If CheckUpdateFieldType(fld.Type) Then
                fld.Update()
            End If
        End If
    End Sub

    Private Sub CreateDocVariableFromField(ByVal doc As Word.Document, ByVal fld As Word.Field)
        'Create document variables that may not exist but the where the fields do.
        If fld.Type = Word.WdFieldType.wdFieldDocVariable Then
            Dim varname As String = fld.Code.Text
            If (varname.IndexOf("""") > 0) Then
                varname = varname.Substring(varname.IndexOf("""") + 1)
                If (varname.IndexOf("""") > 0) Then
                    varname = varname.Substring(0, varname.IndexOf(""""))
                    If varname <> "" Then
                        MyBase.AddField(doc, varname)
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "Properties"

    Public Overrides ReadOnly Property ModuleName() As String
        Get
            Return "OMS Word Integration Module"
        End Get
    End Property


    Public Overrides ReadOnly Property ActiveWindow() As IWin32Window
        Get
            Try
                If (_app.Documents.Count = 0) Then
                    Return New Window(_app)
                Else
                    Return New Window(_app.ActiveWindow)
                End If
            Catch
                Return Nothing
            End Try
        End Get
    End Property

    Public Overrides ReadOnly Property FieldExclusionList() As String()
        Get
            Dim exc() As String = MyBase.FieldExclusionList
            Dim newexc(exc.Length) As String
            exc.CopyTo(newexc, 0)
            newexc(exc.Length) = MAILMERGEFILE
            Return newexc
        End Get
    End Property

#End Region

#Region "IOMSApp Implementation"

    Public Overrides Function GetOpenFileFilter() As String
        Return "Word Files|*.doc;*.docx"
    End Function

    Protected Overrides Function GetCurrentFileLocation(ByVal obj As Object) As System.IO.FileInfo
        Call CheckObjectIsDoc(obj)
        Dim doc As Word.Document = obj
        If doc.Path = String.Empty Then
            Return Nothing
        Else
            Return New System.IO.FileInfo(doc.FullName)
        End If
    End Function

    Protected Overrides Function GenerateDocDesc(ByVal obj As Object) As String
        ' This utility will generate and return a string to display or use in the construction
        ' of the save document routine.

        Dim strbuild As String = MyBase.GenerateDocDesc(obj)
        Dim override As String = GetActiveDocType(obj)

        If override = "" Then
            If Not obj.AttachedTemplate Is Nothing Then
                strbuild &= Convert.ToString(obj.AttachedTemplate)
            End If
        Else
            strbuild &= MyBase.GenerateDocDesc(obj)
        End If

        Return strbuild
    End Function


    Public Overrides ReadOnly Property DefaultDocType() As String
        Get
            Dim ret As String = GetActiveDocType(Me)
            Return ret
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultPrecedentType() As String
        Get
            Dim ret As String = MyBase.DefaultPrecedentType
            If ret = "" Then
                Return "LETTERHEAD"
            Else
                Return ret
            End If
        End Get
    End Property



    Public Overrides Function ExtractPreview(ByVal obj As Object) As String
        Dim r As Word.Range
        If obj.Range.End > 1657 Then
            r = obj.Range(0, 1657)
        Else
            r = obj.Range()
        End If
        Return r.Text
    End Function

    Protected Overloads Overrides Function OpenFile(ByVal file As System.IO.FileInfo) As Object
        Try
            Dim addtorecentfiles As Object = FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList
            Return _app.Documents.Open(file.FullName, AddToRecentFiles:=addtorecentfiles, Format:=GetOpenFormat(file.FullName))
        Catch ex As Exception
            If Err.Number = 5479 Then
                Return _app.ActiveDocument
            Else
                MessageBox.Show(ex)
                Return Nothing
            End If
        End Try
    End Function

    Private Function GetOpenFormat(ByVal file As String) As Object
        If _app.FileConverters Is Nothing Then
            Return System.Type.Missing
        End If

        If ApplicationVersion < 12 Then
            Try


                If (FWBS.Documents.DocumentInfo.IsZipFile(file)) Then

                    Dim fc As Word.FileConverter = Nothing
                    Try
                        fc = _app.FileConverters("Word12")
                    Catch
                        fc = Nothing
                    End Try

                    If (Not fc Is Nothing) Then
                        Return fc.OpenFormat
                    End If
                End If
            Catch ex As IO.IOException
                'IOException may occur if the file is already opened by word
                'word has a lock on it and the IsZipFile command cannot be made
                'As the document is already open hopefully word will deal with it
                'sufficiently.
                Return System.Type.Missing
            End Try

        End If

        Return System.Type.Missing
    End Function
    Protected Overloads Overrides Function InternalDocumentOpen(ByVal document As OMSDocument, ByVal fetchData As FetchResults, ByVal settings As OpenSettings) As Object
        ActivateApplication()
        Try
            Dim doc As Word.Document = Nothing

            Dim file As System.IO.FileInfo = fetchData.LocalFile

            Dim addtorecentfiles As Object = FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList

            Select Case settings.Mode
                Case DocOpenMode.Edit
                    doc = _app.Documents.Open(file.FullName, AddToRecentFiles:=addtorecentfiles, Format:=GetOpenFormat(file.FullName))
                Case DocOpenMode.Print
                    Try
                        doc = _app.Documents.Open(file.FullName, AddToRecentFiles:=addtorecentfiles, Format:=GetOpenFormat(file.FullName))
                    Catch comex As System.Runtime.InteropServices.COMException
                        If comex.ErrorCode = -2146823683 Then
                            Throw DocumentBeingPreviewedException(comex)
                        Else
                            Throw CouldNotOpenDocumentException(comex)
                        End If
                    Catch ex As Exception
                        If Err.Number = 5479 Then
                            doc = _app.ActiveDocument
                        Else : Throw ex
                        End If
                    End Try
                    Dim docs(0) As Object
                    docs(0) = doc
                    BeginPrint(docs, settings.Printing)
                    Return doc
                Case DocOpenMode.View
                    doc = _app.Documents.Open(file.FullName, , True, False, Format:=GetOpenFormat(file.FullName))
            End Select
            Return doc
        Catch comex As System.Runtime.InteropServices.COMException
            If comex.ErrorCode = -2146823683 Then
                Throw DocumentBeingPreviewedException(comex)
            Else
                Throw CouldNotOpenDocumentException(comex)
            End If
        Catch ex As Exception
            If Err.Number = 5479 Then
                Return _app.ActiveDocument
            Else
                Throw
            End If
        End Try
    End Function

    Private Function DocumentBeingPreviewedException(ByVal comex As System.Runtime.InteropServices.COMException) As OMSException2
        Return New OMSException2("ERRWRDOCPENPREV", "%1% could not open the document. Please ensure that no Word documents are being previewed.", comex, False, FWBS.OMS.Global.ApplicationName)
    End Function

    Private Function CouldNotOpenDocumentException(ByVal comex As System.Runtime.InteropServices.COMException) As OMSException2
        Return New OMSException2("ERRWRDOCPENCOM", "%1% could not open the document.%2%COMException: %3%", comex, False, FWBS.OMS.Global.ApplicationName, Environment.NewLine, comex.ErrorCode)
    End Function

    Protected Overloads Overrides Function InternalPrecedentOpen(ByVal precedent As Precedent, ByVal fetchData As FetchResults, ByVal settings As OpenSettings) As Object
        ActivateApplication()
        Try
            Dim doc As Word.Document = Nothing

            Dim file As System.IO.FileInfo = fetchData.LocalFile

            Dim addtorecentfiles As Object = FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList

            Select Case settings.Mode
                Case DocOpenMode.Edit
                    doc = _app.Documents.Open(file.FullName, AddToRecentFiles:=addtorecentfiles, Format:=GetOpenFormat(file.FullName))
                Case DocOpenMode.Print
                    Try
                        doc = _app.Documents.Open(file.FullName, AddToRecentFiles:=addtorecentfiles, Format:=GetOpenFormat(file.FullName))
                    Catch ex As Exception
                        If Err.Number = 5479 Then
                            doc = _app.ActiveDocument
                        Else : Throw ex
                        End If
                    End Try
                    Dim docs(0) As Object
                    docs(0) = doc
                    BeginPrint(docs, settings.Printing)
                Case DocOpenMode.View
                    doc = _app.Documents.Open(file.FullName, , True, AddToRecentFiles:=addtorecentfiles, Format:=GetOpenFormat(file.FullName))
                    If (doc.ProtectionType = Word.WdProtectionType.wdNoProtection) Then
                        doc.Protect(Word.WdProtectionType.wdAllowOnlyReading)
                    End If
            End Select
            Return doc
        Catch ex As Exception
            If Err.Number = 5479 Then
                Return _app.ActiveDocument
            Else
                Throw
            End If
        End Try
    End Function

    Public Overrides Sub Close(ByVal obj As Object)
        Try
            CheckObjectIsDoc(obj)
            obj.Saved = True
            obj.Close(Word.WdSaveOptions.wdDoNotSaveChanges)
        Catch ex As Exception
        End Try
    End Sub

    Public Overrides Sub GotoNextStopCode(ByVal startAtTop As Boolean)
        Dim tyu, ty As Integer
        On Error Resume Next

        If _app.Documents.Count = 0 Then Exit Sub

        CaptureFindDialogValues(_app.Selection.Document)

        If startAtTop Then
            _app.Selection.HomeKey(Unit:=Word.WdUnits.wdStory)
        End If
top:
        With _app.Selection.Find
            .Text = "\[*\]"
            .Replacement.Text = ""
            .Forward = True
            .Wrap = Word.WdFindWrap.wdFindContinue
            .Format = False
            .MatchCase = False
            .MatchWholeWord = False
            .MatchSoundsLike = False
            .MatchAllWordForms = False
            .MatchPhrase = MatchPhrase
            .MatchPrefix = False
            .MatchSuffix = False
            .MatchFuzzy = False
            .MatchWildcards = True
        End With
        If _app.Selection.Find.Execute Then
            If Err.Number <> 0 Then Err.Number = 0 : Exit Sub
            If Mid(_app.Selection.FormattedText.Text, 2, 1) <> "_" Then
                tyu = MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("4015", "Keep this Paragraph Text ?   %1%", "", True, Mid(_app.Selection.FormattedText.Text, 2, Len(_app.Selection.FormattedText.Text) - 2)), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                If tyu = DialogResult.Cancel Then Exit Sub
                If tyu = DialogResult.Yes Then
                    ty = Len(_app.Selection.Text)
                    _app.Selection.MoveLeft(Unit:=Word.WdUnits.wdCharacter, Count:=1)
                    _app.Selection.Delete(Unit:=Word.WdUnits.wdCharacter, Count:=1)
                    _app.Selection.MoveRight(Unit:=Word.WdUnits.wdCharacter, Count:=ty - 2)
                    _app.Selection.Delete(Unit:=Word.WdUnits.wdCharacter, Count:=1)
                Else
                    _app.Selection.Text = ""
                End If
                GoTo top
            Else

            End If
        Else
            With _app.Selection.Find
                .Text = "XXXX"
                .Replacement.Text = ""
                .Forward = True
                .Wrap = Word.WdFindWrap.wdFindContinue
                .Format = False
                .MatchCase = False
                .MatchWholeWord = False
                .MatchSoundsLike = False
                .MatchAllWordForms = False
                .MatchPhrase = MatchPhrase
                .MatchPrefix = False
                .MatchSuffix = False
                .MatchFuzzy = False
                .MatchWildcards = True
            End With
            If _app.Selection.Find.Execute Then

            Else
                MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("4016", "No More Stop Codes", ""), "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

        ResetFindDialogValues(_app.Selection.Document)

    End Sub

    Public Overrides Sub BeginPrint(ByVal objs As Object(), ByVal settings As PrintSettings)
        Dim obj As Object
        For Each obj In objs
            Call CheckObjectIsDoc(obj)

            Dim handled As Boolean = False

            Try
                Dim mode As Object = settings.Mode
                Dim objhandled As Object = _app.Run("PrintOverride", obj, mode)
                handled = FWBS.Common.ConvertDef.ToBoolean(objhandled, False)
            Catch ex As Exception
            End Try

            If Not handled Then

                If settings.Mode = PrecPrintMode.None Then
                    Exit Sub
                End If

                If settings.BulkPrintMode = True Then
                    Dim printdCg As New frmPrint(obj, Me, settings)
                    If (settings.Mode Or PrecPrintMode.Dialog) = settings.Mode Then
                        printdCg.LoadDefaultPrintSettings()
                        printdCg.PrintNow()
                    Else
                        printdCg.Show()
                    End If
                    printdCg.Dispose()
                Else
                    Dim printdlg As New frmPrint(obj, Me, settings.Mode)
                    If (settings.Mode Or PrecPrintMode.Dialog) = settings.Mode Then
                        printdlg.ShowDialog()
                    Else
                        printdlg.Show()
                    End If
                    printdlg.Dispose()
                End If

            End If
        Next
    End Sub

    Public Overrides Sub InternalPrint(ByVal obj As Object, ByVal copies As Integer)
        Call CheckObjectIsDoc(obj)
        Dim objcopies As Object = copies
        _app.ActiveDocument.PrintOut(Item:=Word.WdPrintOutItem.wdPrintDocumentContent, Copies:=objcopies, Pages:="1", PageType:=Word.WdPrintOutPages.wdPrintAllPages, Collate:=True, Background:=False, PrintToFile:=False)
    End Sub

    Private Sub DisableWindowsInTaskBar()

        'The ShowWindowsInTaskBar property can throw an exception if Word is in certain states. eg 'The range does not specify a known AutoText entry' Wrapped in try catch as this isn't an essential part of the process and is likely to have succeeded at some point
        Try
            'DM - Turns off Windows in Taskbar.  This can only work if there is a document open
            If (_app.Documents.Count > 0) Then
                If Convert.ToBoolean(New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "OverrideShowInTaskbar", "True").GetSetting()) Then
                    If _app.ShowWindowsInTaskbar Then
                        _app.ShowWindowsInTaskbar = False
                    End If
                End If
            End If
        Catch ex As System.Runtime.InteropServices.COMException
        End Try

    End Sub


    Public Overrides Sub RunCommand(ByVal obj As Object, ByVal cmd As String)

        DisableWindowsInTaskBar()

        Dim pars As String() = cmd.Split(";")
        Select Case pars(1)
            Case "TOOLBARSCHANGED"
                If System.IO.File.Exists(_app.NormalTemplate.FullName) Then
                    _app.NormalTemplate.Saved = True
                    'Added to prevent the Attached Template requiring a save on toolbar change - GM - 12/09/2003
                    If _app.Documents.Count > 0 Then If UCase(_app.ActiveDocument.AttachedTemplate.name) <> "NORMAL.DOT" Then _app.ActiveDocument.AttachedTemplate.saved = True
                End If
            Case "ADDSIGNATURE"
                If pars.Length > 2 Then
                    AddSignature(obj, IIf(pars(2) = "COMPANY", True, False))
                Else
                    AddSignature(obj, False)
                End If
            Case "SIGNDOCUMENT"
                SignDocument(obj)
            Case "REMOVESIG"
                RemoveSignature()
            Case "ADDLOGO"
                AddLogo(obj, pars(2))
            Case "REMOVELOGO"
                RemoveLogo(pars(2))
            Case "ADDSLOGAN"
                AddSlogan(obj, FetchSlogan(obj))
            Case "SHOWFIELDCODES"
                If _app.Documents.Count > 0 Then
                    _app.ActiveWindow.View.ShowFieldCodes = Common.ConvertDef.ToBoolean(pars(2), False)
                End If
            Case "INSERTSTOP"
                _app.Selection.TypeText(Text:="[_____] ")
            Case "INSERTPARASTOP"
                _app.Selection.TypeText(Text:="[First paragraph here.")
                _app.Selection.TypeParagraph()
                _app.Selection.TypeText(Text:="][Second paragraph here.")
                _app.Selection.TypeParagraph()
                _app.Selection.TypeText(Text:="]")
                _app.Selection.TypeParagraph()
            Case "STRIPFORMATTING"
                _app.Selection.WholeStory()
                _app.Selection.Style = _app.ActiveDocument.Styles("Normal")
                _app.Selection.Cut()
                _app.Selection.PasteAndFormat(Word.WdRecoveryType.wdFormatPlainText)
            Case "STYLES"
                Call _app.Run("FormattingPane")
            Case "TEMPLATEPROP"
                Dim tempprop As frmTemplateProps = New frmTemplateProps(Me, _app.ActiveDocument)
                tempprop.ShowDialog(ActiveWindow)
            Case "EMAILDOCUMENT"
                EmailDocument(_app.ActiveDocument)
            Case "TEMPLATERUN"
                If (pars.Length > 2) Then
                    Call _app.Run(pars(2))
                End If
            Case Else
                MyBase.RunCommand(obj, cmd)
        End Select
    End Sub

    Private Sub EmailDocument(ByRef _doc As Word.Document)
        'Document needs to be saved before emailing
        If _doc.Saved Then
            SendDocViaEmail(_doc, Nothing)
        Else
            MessageBox.Show(Session.CurrentSession.Resources.GetMessage("PLSSAVEDOCEMAIL", "Please save the current document before sending via email", ""), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        End If
    End Sub

    Public Overloads Function SendDocViaEmail(ByRef _doc As Word.Document, ByVal callingForm As Form, Optional ByVal asPDF As Boolean = False, Optional ByVal trackChangesCheck As Boolean = False) As Boolean

        If Session.CurrentSession.IsMailEnabled = False Then
            Throw New MailDisabledException
        End If

        If Session.CurrentSession.EmbedSignaturesIntoWordDocument = False Then
            ' Remove any attached signature
            Try
                _doc.Shapes("SIGNATURE").Delete()
            Catch
            End Try
        End If

        'Get the original file name and the oms document reference.
        Dim origfile As System.IO.FileInfo = New System.IO.FileInfo(_doc.FullName)
        Dim omsdoc As FWBS.OMS.OMSDocument = Me.GetCurrentDocument(_doc)


        If trackChangesCheck And _doc.Revisions.Count > 0 Then
            If MessageBox.Show(Session.CurrentSession.Resources.GetMessage("TRACKCHANGES", "Would you like to accept the tracked changes in this document before proceeding?", ""), "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                'To remove track changes we must copy the current document to a temp file so that the original does not get changed.

                Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = omsdoc.GetStorageProvider()
                Dim newfile As System.IO.FileInfo = FWBS.OMS.Global.GetTempFile(System.IO.Path.GetFileNameWithoutExtension(origfile.Name), origfile.Extension)

                If (origfile.FullName <> newfile.FullName) Then
                    'Now close the original document as it is finished with - gm
                    _doc.Close()
                    System.IO.File.Copy(origfile.FullName, newfile.FullName, True)
                End If


                If (Not callingForm Is Nothing) Then
                    callingForm.Hide()
                End If

                Dim objnewfile As Object = newfile.FullName


                Dim doc As Word.Document
                Try
                    doc = _app.Documents.Open(objnewfile, Format:=GetOpenFormat(objnewfile))
                Catch comex As System.Runtime.InteropServices.COMException When comex.ErrorCode = -2146822809
                    '-2146822809 = You cannot close Microsoft Office Word because a dialog box is open. Click OK, switch to Word, and then close the dialog box.
                    doc = _app.Documents.Open(objnewfile, Format:=GetOpenFormat(objnewfile))
                End Try
                doc.SaveAs(objnewfile)
                doc.AcceptAllRevisions()


                Dim ctr As Integer
                'Loop thru doc not _doc - gm
                For ctr = doc.Versions.Count To 1 Step -1
                    doc.Versions(ctr).Delete()
                Next
                doc.Save()

                Me.SendDocViaEmail(doc, newfile, asPDF)



                If (origfile.FullName <> newfile.FullName) Then
                    doc.Close()
                    If newfile.Exists Then newfile.Delete()
                End If


            Else
                Me.SendDocViaEmail(_doc, New System.IO.FileInfo(_doc.FullName), asPDF)
            End If
        Else
            Me.SendDocViaEmail(_doc, New System.IO.FileInfo(_doc.FullName), asPDF)
        End If


        Return True
    End Function



    Protected Overloads Overrides Function TemplateStart(ByVal obj As Object, ByVal preclink As PrecedentLink) As Object

        Dim worddoc As Word.Document = Nothing
        Dim tmppath As System.IO.FileInfo

        'Activate or create and activate the word application
        ActivateApplication()

        Dim fetch As FWBS.OMS.DocumentManagement.Storage.FetchResults = preclink.Merge()
        If (fetch Is Nothing) Then
            Return worddoc
        End If

        ' Get the Precedent File to Load...
        tmppath = fetch.LocalFile


        If Not tmppath Is Nothing Then

            If (Session.CurrentSession.IsLicensedFor("VSTO")) Then

                Dim setting As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, String.Empty, String.Empty, "VSTO Override", String.Empty)
                Dim fp As String = Convert.ToString(setting.GetSetting())

                If (fp <> String.Empty And System.IO.Directory.Exists(fp)) Then
                    fp = System.IO.Path.Combine(fp, tmppath.Name)
                    If (System.IO.File.Exists(fp)) Then
                        tmppath = New System.IO.FileInfo(fp)
                    End If
                End If
            End If

            worddoc = _app.Documents.Add(tmppath.FullName) ' rem by MNW for possible smart doc issue, Visible:=True)

            Try
                If worddoc.ActiveWindow.Visible = False Then
                    worddoc.ActiveWindow.Visible = True
                End If
                ActivateDocument(worddoc)
            Catch ex As Exception
            End Try

        End If

        Return worddoc

    End Function


#End Region

#Region "Application Events"

    Private Sub Application_BeforeDocumentClose(ByVal doc As Word.Document, ByRef Cancel As Boolean)

        If Cancel Then
            Return
        End If

        System.Diagnostics.Debug.WriteLine("Do your thing")

        Dim args As System.ComponentModel.CancelEventArgs
        args = New System.ComponentModel.CancelEventArgs
        If IsAddinInstance Then
            Using contextBlock As DPIContextBlock = If(_dpiAwareness > 0, New DPIContextBlock(_dpiAwareness), Nothing)
                OnDocumentClose(doc, args)
            End Using
        End If

        Cancel = args.Cancel

        If Session.CurrentSession.IsConnected Then
            Dim precID As String = Nothing
            Dim prec As FWBS.OMS.Precedent = Nothing
            Dim omsprec As IStorageItem = GetCurrentPrecedent(doc)
            If Not omsprec Is Nothing Then
                prec = omsprec
                precID = prec.ID
            End If

            If precID Is Nothing Then
                Dim omsprecVer As IStorageItem = GetCurrentPrecedentVersion(doc)
                If Not omsprecVer Is Nothing Then
                    Dim precVersion As PrecedentVersion = omsprecVer
                    precID = precVersion.ParentDocument.ID
                End If
            End If

            If Not precID Is Nothing And Cancel = False Then
                UnlockPrecedent(precID)
                If Not prec Is Nothing Then
                    prec.CheckIn()
                End If
            End If
        End If

        If Not Cancel Then
            OnDocumentClosed()
        End If

    End Sub

    Protected Overrides Function IsDocumentSaved(ByVal doc As Object) As Boolean
        Return doc.Saved
    End Function

    Protected Overrides Sub SetDocumentAsSaved(ByVal doc As Object)
        doc.Saved = True
    End Sub

    Private Sub Application_DocumentOpen(ByVal doc As Word.Document)

        If Not doc Is Nothing Then
            If (Session.CurrentSession.IsLoggedIn) Then
                Try
                    If ((Session.CurrentSession.CurrentUser.EnableTrackChangesWarning = FWBS.Common.TriState.Null And Session.CurrentSession.EnableTrackChangesWarning) Or Session.CurrentSession.CurrentUser.EnableTrackChangesWarning = FWBS.Common.TriState.True) Then
                        If doc.Revisions.Count > 0 Then
                            MessageBox.Show(ActiveWindow, Session.CurrentSession.Resources.GetMessage("MSGTRKCHGWARN", "There are currently some revisions stored within the document using Word's Track Changes feature", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                        End If
                    End If
                Catch ex As Exception
                    ErrorBox.Show(ex)
                End Try
            End If
        End If

    End Sub

    Private Sub Application_WindowActivate(ByVal doc As Word.Document, ByVal win As Word.Window)
        RemoveHandler _app.WindowActivate, AddressOf Application_WindowActivate
        If (RunAutoExec) Then
            RunAutoExec = False
            Try
                _app.Run("OMSFW.basHooks.AutoExec")
            Catch comex As System.Runtime.InteropServices.COMException When comex.ErrorCode = -2147352573 'cannot run specified macro
                Trace.WriteLine(comex.Message)
            End Try
        End If
    End Sub

    Private Sub Application_Quit()
        Try
            Dim currentSession As Session = Session.CurrentSession
            Dim shutdownRequest As System.Delegate = currentSession.GetType().GetField("ShutdownRequest", BindingFlags.Instance Or BindingFlags.NonPublic).GetValue(currentSession)
            If Not shutdownRequest Is Nothing Then
                shutdownRequest.DynamicInvoke(currentSession, EventArgs.Empty)
            End If
            currentSession.Disconnect()
        Catch
        End Try

        Try
            Dispose()
        Catch
        End Try
    End Sub
#End Region

#Region "OMS Events"

    Protected Overrides Sub Connected(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.Connected(sender, e)
        Try
            If Session.CurrentSession.OverrideUsersInitials Then

                Dim initials As String
                Dim username As String

                initials = Session.CurrentSession.SystemwideInitials
                If String.IsNullOrEmpty(initials) Then
                    initials = Session.CurrentSession.CurrentUser.Initials
                End If

                username = Session.CurrentSession.SystemwideUserName
                If String.IsNullOrEmpty(username) Then
                    username = Session.CurrentSession.CurrentUser.FullName
                End If

                _app.UserInitials = initials
                _app.UserName = username
            End If
        Catch
        Finally
            _wnd_close_hook.Install()
        End Try
    End Sub

    Protected Overrides Sub Disconnected(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _app.UserInitials = _old_Initals
            _app.UserName = _old_userName
        Catch
        Finally
            _wnd_close_hook.Uninstall()
        End Try
        MyBase.Disconnected(sender, e)
    End Sub

#End Region

#Region "IDisposable Implementation"

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        RunAutoExec = False
        ' Check to see if Dispose has already been called.
        If Not _disposed Then

            ' If disposing equals true, dispose all managed 
            ' and unmanaged resources.
            If disposing Then
                RemoveDelegates()
            End If

            ' Call the appropriate methods to clean up unmanaged resources here.
            ' If disposing is false, only the following code is executed.
            If Not _app Is Nothing Then
                Common.COM.DisposeObject(_app)
                _app = Nothing
            End If

            'Note disposing has been done.
            _disposed = True

        End If
    End Sub

#End Region

#Region "Disassembly"

    Private Sub Disassemble(ByVal document As Word.Document)
        Dim parser As FieldParser = New FieldParser
        Dim assoc As Associate = GetCurrentAssociate(document)
        parser.ChangeObject(assoc)
        Disassemble(document, parser)
    End Sub

    Private Sub Disassemble(ByVal document As Word.Document, ByVal parser As FieldParser)


        On Error Resume Next
        Dim tmpstr As Object = document.Variables("PROCESSDISASSEMBLER").Value
        If Err.Number <> 0 Then Exit Sub


        Dim blnKeepPara As Boolean
        Dim blnKeepPara2 As Boolean
        Dim strOperator As String

        Dim lngErrorCount As Integer = 0

        Dim bmid As String = String.Format("ORIGINALPOSITION_{0}", DateTime.UtcNow.Ticks)

        document.Bookmarks.Add(bmid.ToString(), _app.Selection.Range)


        _app.Selection.HomeKey(Unit:=Word.WdUnits.wdStory)

        While DisassemblyFindText("\[IF,*\]")
            _app.ScreenUpdating = False

            Dim ret() As String = _app.Selection.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

            Select Case ret.Length
                Case 4  ' = IF,X,=,Y logic (no and)
                    Select Case ret(2)
                        Case "="
                            On Error Resume Next
                            blnKeepPara = Trim(DisassemblyGetExtendedData(parser, Trim(ret(1)), "ERROR", "BOOLEAN")) = Trim(Replace(ret(3), "]", ""))
                        Case "<"
                            On Error Resume Next
                            blnKeepPara = Trim(DisassemblyGetExtendedData(parser, Trim(ret(1)), "ERROR", "BOOLEAN")) < Val(Trim(Replace(ret(3), "]", "")))
                        Case ">"
                            On Error Resume Next
                            blnKeepPara = Trim(DisassemblyGetExtendedData(parser, Trim(ret(1)), "ERROR", "BOOLEAN")) > Val(Trim(Replace(ret(3), "]", "")))
                        Case ">="
                            On Error Resume Next
                            blnKeepPara = Trim(DisassemblyGetExtendedData(parser, Trim(ret(1)), "ERROR", "BOOLEAN")) >= Val(Trim(Replace(ret(3), "]", "")))
                        Case "<="
                            On Error Resume Next
                            blnKeepPara = Trim(DisassemblyGetExtendedData(parser, Trim(ret(1)), "ERROR", "BOOLEAN")) <= Val(Trim(Replace(ret(3), "]", "")))
                    End Select

                    If Err.Number <> 0 Then
                        DisassemblyReplaceWithError("Problem performing 3 parameter replacement : " & Err.Description)
                    End If
                    DisassemblyKeepParagraph(blnKeepPara)

                Case 6 ' = IF,X,=,Y & A,=,B
                    strOperator = "&"
                    If InStr(ret(3), "&") > 0 Then strOperator = "&" 'Follows AND Logic
                    If InStr(ret(3), "|") > 0 Then strOperator = "|" 'Follows OR Logic
                    On Error Resume Next

                    Select Case ret(2)
                        Case "="
                            blnKeepPara = Trim(parser.Parse(Trim(ret(1)), "ERROR")) = Trim(Left(ret(3), InStr(ret(3), " ")))
                        Case "<"
                            blnKeepPara = Trim(parser.Parse(Trim(ret(1)), "ERROR")) < Val(Trim(Left(ret(3), InStr(ret(3), " "))))
                        Case ">"
                            blnKeepPara = Trim(parser.Parse(Trim(ret(1)), "ERROR")) > Val(Trim(Left(ret(3), InStr(ret(3), " "))))
                        Case ">="
                            blnKeepPara = Trim(parser.Parse(Trim(ret(1)), "ERROR")) >= Val(Trim(Left(ret(3), InStr(ret(3), " "))))
                        Case "<="
                            blnKeepPara = Trim(parser.Parse(Trim(ret(1)), "ERROR")) <= Val(Trim(Left(ret(3), InStr(ret(3), " "))))
                    End Select

                    Select Case ret(4)
                        Case "="
                            blnKeepPara2 = Trim(parser.Parse(Trim(Mid(ret(3), InStr(ret(3), strOperator) + 2)), "ERROR")) = Trim(Replace(ret(5), "]", ""))
                        Case "<"
                            blnKeepPara2 = Trim(parser.Parse(Trim(Mid(ret(3), InStr(ret(3), strOperator) + 2)), "ERROR")) < Trim(Replace(ret(5), "]", ""))
                        Case ">"
                            blnKeepPara2 = Trim(parser.Parse(Trim(Mid(ret(3), InStr(ret(3), strOperator) + 2)), "ERROR")) > Trim(Replace(ret(5), "]", ""))
                        Case ">="
                            blnKeepPara2 = Trim(parser.Parse(Trim(Mid(ret(3), InStr(ret(3), strOperator) + 2)), "ERROR")) >= Trim(Replace(ret(5), "]", ""))
                        Case "<="
                            blnKeepPara2 = Trim(parser.Parse(Trim(Mid(ret(3), InStr(ret(3), strOperator) + 2)), "ERROR")) <= Trim(Replace(ret(5), "]", ""))
                    End Select
                    If Err.Number <> 0 Then
                        DisassemblyReplaceWithError("Problem performing 6 parameter replacement : " & Err.Description)
                    End If

                    Select Case strOperator
                        Case "&"
                            DisassemblyKeepParagraph(blnKeepPara And blnKeepPara2)
                        Case "|"
                            DisassemblyKeepParagraph(blnKeepPara Or blnKeepPara2)
                    End Select

                Case Else
                    DisassemblyReplaceWithError("No Functionality available with " & ret.Length & " parameters")
            End Select

        End While
        If lngErrorCount > 0 Then
            MsgBox(lngErrorCount & " error(s) occured disassembling this document.  Please check the document and perform relevant changes manually.", , ModuleName)
        End If

        On Error Resume Next
        If document.Bookmarks.Exists(bmid.ToString()) Then
            _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:=bmid.ToString())
            document.Bookmarks(bmid.ToString()).Delete()
        Else
            _app.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:="PositionCursorHere")
        End If

        Application.ScreenUpdating = True

    End Sub

    Private Sub DisassemblyReplaceWithError(ByVal strError As String)
        _app.Selection.BoldRun()
        _app.Selection.TypeText(strError)
    End Sub


    Private Sub DisassemblyKeepParagraph(ByVal blnKeep As Boolean)
        If blnKeep Then
            _app.Selection.Delete()
        Else
            _app.Selection.MoveDown(Unit:=Word.WdUnits.wdParagraph, Count:=1, Extend:=Word.WdMovementType.wdExtend)
            _app.Selection.Delete()
            _app.Selection.TypeBackspace()
        End If
    End Sub

    Function DisassemblyGetExtendedData(ByVal parser As FieldParser, ByVal strPath As String, ByVal strDefault As String, Optional ByVal strType As String = "") As Object
        On Error Resume Next
        DisassemblyGetExtendedData = parser.Parse(strPath, strDefault).ToString()
        If DisassemblyGetExtendedData = "ERROR" Then DisassemblyReplaceWithError("Error")
    End Function

    Function DisassemblyFindText(ByVal strText As String) As Boolean
        Try
            CaptureFindDialogValues(_app.Selection.Document)

            _app.Selection.Find.ClearFormatting()

            With _app.Selection.Find
                .Text = strText
                .Replacement.Text = ""
                .Forward = True
                .Wrap = Word.WdFindWrap.wdFindContinue
                .Format = False
                .MatchCase = False
                .MatchWholeWord = False
                .MatchSoundsLike = False
                .MatchAllWordForms = False
                .MatchPhrase = MatchPhrase
                .MatchPrefix = False
                .MatchSuffix = False
                .MatchFuzzy = False
                .MatchWildcards = True
            End With
            Return _app.Selection.Find.Execute()
            ResetFindDialogValues(_app.Selection.Document)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

    Public Sub AddinShutdown()
        Dim currentSession As Session = Session.CurrentSession
        Dim shutdownRequest As System.Delegate = If(currentSession.IsConnected, currentSession.GetType().GetField("ShutdownRequest", BindingFlags.Instance Or BindingFlags.NonPublic).GetValue(currentSession), Nothing)
        If Not shutdownRequest Is Nothing Then
            shutdownRequest.DynamicInvoke(currentSession, EventArgs.Empty)
        End If
    End Sub

    Private Class Window
        Implements IWin32Window

        Private _handle As IntPtr

        Public ReadOnly Property Handle() As System.IntPtr Implements System.Windows.Forms.IWin32Window.Handle
            Get
                Return _handle
            End Get
        End Property

        Public Sub New(ByVal obj As Object)
            Dim caption As String = ""
            Dim version As Double = 12

            Try
                If TypeOf obj Is Word.Document Then
                    caption = obj.Application.ActiveWindow.Caption
                    Double.TryParse(obj.Application.Version, version)
                ElseIf TypeOf obj Is Word.Application Then
                    caption = obj.Caption
                    Double.TryParse(obj.Version, version)
                ElseIf TypeOf obj Is Word.Window Then
                    caption = obj.Caption
                    Double.TryParse(obj.Application.Version, version)
                Else
                    caption = obj.Application.Caption
                    Double.TryParse(obj.Application.Version, version)
                End If
            Catch ex As Exception
                _handle = IntPtr.Zero
            End Try

            If (version >= 16) Then
                'The window caption in Word 2016 has changed. It now ends with 'Word'
                _handle = Common.Functions.FindWindow("OpusApp", caption + " - Word")
            End If
            If _handle = IntPtr.Zero Then
                _handle = Common.Functions.FindWindow("OpusApp", caption + " - Microsoft Word")
                If _handle = IntPtr.Zero Then
                    _handle = Common.Functions.FindWindow("OpusApp", caption)
                End If
            End If

        End Sub

    End Class

    Friend Class UnProtect
        Implements IDisposable
        Private doc As Word.Document
        Private type As Word.WdProtectionType

        Private Const p1 As String = ""
        Private Const p2 As String = "Fwbs$forms"
        Private p As String

        Public Sub New(ByVal doc As Word.Document)
            Me.doc = doc
            type = doc.ProtectionType

            If Not IsProtected Then
                Return
            End If

            Dim saved As Boolean = doc.Saved

            Try
                doc.Unprotect(p1)
                p = p1
            Catch comex As System.Runtime.InteropServices.COMException 'When comex.ErrorCode = -2146822803 - removed error code check incase different between versions
                Try
                    doc.Unprotect(p2)
                    p = p2
                Catch comex2 As System.Runtime.InteropServices.COMException 'When comex2.ErrorCode = -2146822803

                End Try
            Finally
                If doc.Saved <> saved Then
                    doc.Saved = saved
                End If
            End Try

        End Sub

        Public ReadOnly Property IsProtected As Boolean
            Get
                Return type <> Word.WdProtectionType.wdNoProtection
            End Get
        End Property

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not IsProtected Then
                Return
            End If

            Dim saved As Boolean = doc.Saved
            Dim pwd As Object = p
            Try

                doc.Protect(type, True, pwd)
            Catch ex As Exception

            Finally
                If doc.Saved <> saved Then
                    doc.Saved = saved
                End If
            End Try

        End Sub
    End Class

    Friend Class Position

        Private bookmarkname As String = String.Format("LASTCURSORPOSITION")
        Private bookmark As Word.Bookmark
        Private doc As Word.Document

        Public Sub New(ByVal doc As Word.Document)
            Me.doc = doc

        End Sub

        Public Sub [Set]()
            Try
                If (Me.doc.Bookmarks.Exists(bookmarkname)) Then
                    bookmark = doc.Bookmarks(bookmarkname)
                    bookmark.Delete()
                End If
            Catch
            End Try
            Try
                bookmark = Me.doc.Bookmarks.Add(bookmarkname, doc.Application.Selection.Range)
            Catch
            End Try
        End Sub

        Public Sub [GoTo]()
            Try
                If (Me.doc.Bookmarks.Exists(bookmarkname)) Then
                    If (Not bookmark Is Nothing) Then
                        doc.Application.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:=bookmarkname)
                    Else
                        doc.Application.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:="PositionCursorHere")
                    End If
                Else
                    doc.Application.Selection.GoTo(What:=Word.WdGoToItem.wdGoToBookmark, Name:="PositionCursorHere")
                End If
            Catch
            End Try
        End Sub
    End Class

End Class
