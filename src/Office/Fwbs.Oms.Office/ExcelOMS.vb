Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports FWBS.OMS.DocumentManagement
Imports FWBS.OMS.DocumentManagement.Storage
Imports Excel = Microsoft.Office.Interop.Excel
Imports MSOffice = Microsoft.Office.Core

<System.Runtime.InteropServices.Guid("4A2B2183-62B6-4AD2-8D1D-8892D7EF1048"), System.Runtime.InteropServices.ComVisible(False)> _
Public Class ExcelOMS
	Inherits OfficeOMSApp

#Region "Fields"

	Private WithEvents _app As Excel.Application
	Private _old_userName As String
    Private _wnd_close_hook As WindowCloseHook = New WindowCloseHook("XLMAIN", False)

	Private _openbutton As Microsoft.Office.Core.CommandBarButton
	Private _printbutton As Microsoft.Office.Core.CommandBarButton
	Private _printbutton2 As Microsoft.Office.Core.CommandBarButton
	Private _savebutton As Microsoft.Office.Core.CommandBarButton
	Private _saveasbutton As Microsoft.Office.Core.CommandBarButton

	Private Const PROTECTION As String = "$d20$e80"

	Private IsPrintPreviewOpen As Boolean = False

#End Region

#Region "Constructors"
	Public Sub New()
		MyBase.New("EXCEL", False, False)

		ActivateApplication()

		_old_userName = _app.UserName

	End Sub
	Public Sub New(ByVal app As Excel.Application, ByVal code As String)
		Me.New(app, code, True)
	End Sub
	Public Sub New(ByVal app As Excel.Application, ByVal code As String, ByVal useCommandBars As Boolean)
		MyBase.New(code, useCommandBars)

		RemoveDelegates()
		_app = app
		AddDelegates()

		If (useCommandBars) Then
			Dim cb As Microsoft.Office.Core.CommandBar

			For Each cb In _app.CommandBars
				Dim ctrl As Microsoft.Office.Core.CommandBarControl = cb.FindControl(, 23, , , True)
				If TypeOf ctrl Is Microsoft.Office.Core.CommandBarButton Then
					Dim btn As Microsoft.Office.Core.CommandBarButton = ctrl
					AddHandler btn.Click, AddressOf ButtonClick
					_openbutton = ctrl
					Exit For
				End If
			Next cb

			For Each cb In _app.CommandBars
				Dim ctrl As Microsoft.Office.Core.CommandBarControl = cb.FindControl(, 3, , , True)
				If TypeOf ctrl Is Microsoft.Office.Core.CommandBarButton Then
					Dim btn As Microsoft.Office.Core.CommandBarButton = ctrl
					AddHandler btn.Click, AddressOf ButtonClick
					_savebutton = ctrl
					Exit For
				End If
			Next cb


			For Each cb In _app.CommandBars
				Dim ctrl As Microsoft.Office.Core.CommandBarControl = cb.FindControl(, 2521, , , True)
				If TypeOf ctrl Is Microsoft.Office.Core.CommandBarButton Then
					Dim btn As Microsoft.Office.Core.CommandBarButton = ctrl
					AddHandler btn.Click, AddressOf ButtonClick
					_printbutton = ctrl
					Exit For
				End If
			Next cb
			For Each cb In _app.CommandBars
				Dim ctrl As Microsoft.Office.Core.CommandBarControl = cb.FindControl(, 748, , , True)
				If TypeOf ctrl Is Microsoft.Office.Core.CommandBarButton Then
					Dim btn As Microsoft.Office.Core.CommandBarButton = ctrl
					AddHandler btn.Click, AddressOf ButtonClick
					_saveasbutton = ctrl
					Exit For
				End If
			Next cb
			For Each cb In _app.CommandBars
				Dim ctrl As Microsoft.Office.Core.CommandBarControl = cb.FindControl(, 4, , , True)
				If TypeOf ctrl Is Microsoft.Office.Core.CommandBarButton Then
					Dim btn As Microsoft.Office.Core.CommandBarButton = ctrl
					AddHandler btn.Click, AddressOf ButtonClick
					_printbutton2 = ctrl
					Exit For
				End If
			Next cb
		End If

		AddinInitialisation()

		_old_userName = _app.UserName

	End Sub


	Private Sub RemoveDelegates()
		On Error Resume Next
		If (Not _app Is Nothing) Then
			'These events must fire regardless of addin insstance or not
			RemoveHandler _app.WorkbookOpen, AddressOf Application_NewWorkbook
			RemoveHandler _app.WorkbookOpen, AddressOf Application_WorkbookOpen
			RemoveHandler _app.WorkbookBeforeClose, AddressOf Application_WorkbookBeforeClose
		End If
	End Sub

	Private Sub AddDelegates()
		On Error Resume Next
		RemoveDelegates()
		If (Not _app Is Nothing) Then
			'These events must fire regardless of addin insstance or not
			AddHandler _app.NewWorkbook, AddressOf Application_NewWorkbook
			AddHandler _app.WorkbookOpen, AddressOf Application_WorkbookOpen
			AddHandler _app.WorkbookBeforeClose, AddressOf Application_WorkbookBeforeClose
		End If
	End Sub

	Protected Overrides Function IsDocumentSaved(ByVal doc As Object) As Boolean
		If doc.FullName.ToString().ToUpper().IndexOf("OMSFW") > 0 Then
			Return True
		Else
			Return doc.Saved
		End If
	End Function
	Protected Overrides Sub SetDocumentAsSaved(ByVal doc As Object)
		doc.Saved = True
	End Sub

	Private Sub Application_WorkbookBeforeClose(ByVal doc As Excel.Workbook, ByRef Cancel As Boolean)

		If Cancel Then
			Return
		End If

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
            Dim prec As Precedent = Nothing

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

        'The best way of determining if excel is about to close.
        'If the last workbook is about to close then clean up 
        'the application object using the dispose method.
        If Not IsAddinInstance Then
            If (_app.Workbooks.Count = 1) Then
                Dispose()
            End If
        End If
    End Sub

	Private Sub Application_NewWorkbook(ByVal doc As Excel.Workbook)
		ChangeLink(doc)
	End Sub

	Private Sub Application_WorkbookOpen(ByVal doc As Excel.Workbook)
		ChangeLink(doc)
	End Sub

	Private Function FindXLAFile(ByVal name As String) As System.IO.FileInfo
		Dim fn As String = System.IO.Path.Combine(_app.LibraryPath, name)
		If (System.IO.File.Exists(fn)) Then
			Return New System.IO.FileInfo(fn)
		End If
		fn = System.IO.Path.Combine(_app.StartupPath, name)
		If (System.IO.File.Exists(fn)) Then
			Return New System.IO.FileInfo(fn)
		End If
		fn = System.IO.Path.Combine(_app.Path, "XLSTART", name)
		If (System.IO.File.Exists(fn)) Then
			Return New System.IO.FileInfo(fn)
		End If
		fn = New System.IO.FileInfo(New Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath).Directory.FullName
		fn = System.IO.Path.Combine(fn, name)
		If (System.IO.File.Exists(fn)) Then
			Return New System.IO.FileInfo(fn)
		End If
		Return Nothing
	End Function

	Private Function InstallXLAAddin(ByVal name As String) As Excel.AddIn

		Dim addin As Excel.AddIn = Nothing

		Try
			addin = _app.AddIns(System.IO.Path.GetFileNameWithoutExtension(name))
			If (System.IO.File.Exists(addin.FullName) = False) Then
				addin.Installed = False
				addin = Nothing
			End If

		Catch ex As Exception
			addin = Nothing
		End Try

		If (addin Is Nothing) Then
			Dim f As System.IO.FileInfo = FindXLAFile(name)
			If f Is Nothing Then
				Return Nothing
			End If
			If (System.IO.File.Exists(f.FullName)) Then
				If _app.Workbooks.Count = 0 Then
					_app.Workbooks.Add()
				End If
				addin = _app.AddIns.Add(f.FullName)
			End If
		End If

		If addin Is Nothing Then
			Return Nothing
		End If

		If (System.IO.File.Exists(addin.FullName)) Then
			If (addin.Installed = False) Then
				addin.Installed = True
			End If
			Return addin
		End If
		Return Nothing


	End Function

	Private Sub ChangeLink(ByVal doc As Excel.Workbook)

		Dim possible As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)
		If Environment.Is64BitProcess Then
			possible.Add("OMSFW2007_64.XLA")
			possible.Add("OMSFW2007_64.XLAM")
			possible.Add("OMSFW2007NODM_64.XLA")
			possible.Add("OMSFW2007NODM_64.XLAM")
		Else
			possible.Add("OMSFW2007.XLA")
			possible.Add("OMSFW2007.XLAM")
			possible.Add("OMSFW2007NODM.XLA")
			possible.Add("OMSFW2007NODM.XLAM")
		End If

		If (possible.Contains(doc.Name.ToUpperInvariant())) Then
			Exit Sub
		End If

		If Not ActiveWindow Is Nothing Then
			For Each s As String In possible
				Dim addin As Excel.AddIn = InstallXLAAddin(s)
				If addin Is Nothing Then
					Continue For
				Else

					Dim xla As Excel.Workbook
					Dim links As Object
					Dim index As Integer

					links = doc.LinkSources(Excel.XlLink.xlExcelLinks)

					If (Not IsAddinInstance) Then
						Try
							xla = _app.Workbooks(addin.Name) 'Get template that is already loaded
						Catch ex As System.Runtime.InteropServices.COMException
							xla = _app.Workbooks.Open(addin.FullName) 'Open template if it isn't loaded
							xla.RunAutoMacros(Excel.XlRunAutoMacro.xlAutoOpen) 'Run AutoExec macros if we've loaded the template (it is normally loaded when Excel is opened by a user but not always when opened programatically)
						End Try

					End If
					If Not links Is Nothing Then
						For index = LBound(links) To UBound(links)
							Dim n As String = links(index)
							If n.IndexOf("OMSFW", StringComparison.CurrentCultureIgnoreCase) > 0 Then
								If Not n.Equals(addin.FullName, StringComparison.CurrentCultureIgnoreCase) Then
									Dim displayAlerts As Boolean = _app.DisplayAlerts
									Try
										_app.DisplayAlerts = False
										doc.ChangeLink(Name:=n, NewName:=addin.FullName, Type:=Excel.XlLinkType.xlLinkTypeExcelLinks)
									Finally
										_app.DisplayAlerts = displayAlerts
									End Try
								End If
							End If
						Next index
					End If

					Return
				End If

			Next

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
			Return "Excel"
		End Get
	End Property

	Public Overrides ReadOnly Property ApplicationVersion() As Integer
		Get
			Return Version.Parse(_app.Version).Major
		End Get
	End Property

#End Region

#Region "OMSApp"

    Public Overrides Function GetOpenFileFilter() As String
		Return "Excel Files|*.xls;*.xlsx"
	End Function

	Public Overrides Function GetDocumentCount() As Integer
		Return _app.Workbooks.Count
	End Function

	Public Overrides Function WillHandleRunCommand(ByVal obj As Object, ByVal cmd As String) As Boolean?
		Dim pars As String() = cmd.Split(";")
		Select Case pars(1)
			Case "PRINT"
				If IsPrintPreviewOpen Then
					Return Nothing
				End If
		End Select

		Return MyBase.WillHandleRunCommand(obj, cmd)
	End Function

	Public Overrides Sub RunCommand(ByVal obj As Object, ByVal cmd As String)
		Dim pars As String() = cmd.Split(";")

		ValidateEditMode()

		Select Case pars(1)
			Case "SHOWFIELDCODES"
				If _app.Windows.Count > 0 Then
					_app.ActiveWindow.DisplayFormulas = Common.ConvertDef.ToBoolean(pars(2), False)
				End If
			Case "PRINTPREVIEW"
				If (Me.DocumentManagementMode And DocumentManagementMode.Print) = OMS.DocumentManagement.DocumentManagementMode.None Then
					Exit Select
				End If
				If _app.Workbooks.Count > 0 Then
					Try
						IsPrintPreviewOpen = True
						_app.ActiveWorkbook.PrintPreview()
					Finally
						IsPrintPreviewOpen = False

					End Try
                End If
            Case "TEMPLATERUN"
                If (pars.Length > 2) Then
                    Call _app.Run(pars(2))
                End If

            Case "EMAILDOCUMENT"
                EmailDocument(_app.ActiveWorkbook)
            Case Else
                MyBase.RunCommand(obj, cmd)
        End Select
	End Sub

    Private Sub EmailDocument(ByRef _doc As Excel.Workbook)
        'Document needs to be saved before emailing
        If _doc.Saved Then
            SendDocViaEmail(_doc, Nothing)
        Else
            MessageBox.Show(Session.CurrentSession.Resources.GetMessage("PLSSAVEDOCEMAIL", "Please save the current document before sending via email", ""), "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        End If
    End Sub

    Public Overloads Function SendDocViaEmail(ByRef _doc As Excel.Workbook, ByVal callingForm As Form, Optional ByVal asPDF As Boolean = False) As Boolean
        If Session.CurrentSession.IsMailEnabled = False Then
            Throw New MailDisabledException
        End If

        'Get the original file name and the oms document reference.
        Dim origfile As System.IO.FileInfo = New System.IO.FileInfo(_doc.FullName)
        Dim omsdoc As FWBS.OMS.OMSDocument = Me.GetCurrentDocument(_doc)
        Me.SendDocViaEmail(_doc, origfile, asPDF)

        Return True
    End Function


	Protected Overrides Function GetCurrentFileLocation(ByVal obj As Object) As System.IO.FileInfo
		Call CheckObjectIsDoc(obj)
		Dim wb As Excel.Workbook = obj
		If wb.Path = String.Empty Then
			Return Nothing
		Else
			Return New System.IO.FileInfo(wb.FullName)
		End If
	End Function


	Public Overrides Sub InternalPrint(ByVal obj As Object, ByVal copies As Integer)
		Call CheckObjectIsDoc(obj)
		Dim wb As Excel.Workbook = obj
		wb.PrintOut(Copies:=copies, Collate:=True, PrintToFile:=False)
	End Sub

	Public Overrides ReadOnly Property ActiveWindow() As IWin32Window
		Get
			Try
				Return New Window(_app)
			Catch
				Return Nothing
			End Try
		End Get
	End Property

	Protected Overrides Sub CheckObjectIsDoc(ByRef obj As Object)
		If obj Is Me Then
			obj = _app.ActiveWorkbook
		ElseIf obj Is _app Then
			obj = _app.ActiveWorkbook
		ElseIf TypeOf obj Is Excel.Workbook Then
			obj = obj
		ElseIf TypeOf obj Is Excel.Worksheet Then
			obj = obj.Parent
		ElseIf TypeOf obj Is Excel.Window Then
			obj = obj.Parent
		Else
			Throw New Exception("The passed parameter is not a Excel.Workbook object.")
		End If
	End Sub


	Public Overrides Sub ActivateApplication()
		Dim ctr As Integer = 0
		Dim forceNewInstance As Boolean = False
		RemoveDelegates()
		If _app Is Nothing Then
Retry:
			Try
				_app = If(forceNewInstance, GetObject("", "Excel.Application"), GetObject(, "Excel.Application"))
			Catch
				_app = GetObject("", "Excel.Application")
			End Try
		End If

		AddDelegates()
		Try
			_app.Visible = True
			If (Not _app.ActiveWindow Is Nothing) Then
				_app.ActiveWindow.Activate()
			End If
		Catch ex As System.Runtime.InteropServices.COMException
			Dispose()
			If (ctr = 0) Then
				ctr = 1
				forceNewInstance = IsAddinInstance = False AndAlso (ex.HResult = -2146777998 Or ex.HResult = -2147418111) ' VBA_E_IGNORE Or RPC_E_CALL_REJECTED
				GoTo Retry
			End If
		Catch ex As Exception
			If (ctr = 0) Then
				ctr = 1
				GoTo Retry
			End If
		End Try

	End Sub


	Public Overrides ReadOnly Property ModuleName() As String
		Get
			Return "OMS Excel Integration Module"
		End Get
	End Property


	Protected Overrides Sub SetWindowCaption(ByVal obj As Object, ByVal caption As String)
	    Call CheckObjectIsDoc(obj)
        Dim activeWnd As Excel.Window = If(IsNothing(_app), Nothing, _app.ActiveWindow)
        If Not activeWnd Is Nothing Then
            activeWnd.Caption = caption
        End If
    End Sub

	Public Overrides Sub ActivateDocument(ByVal obj As Object)
		On Error Resume Next
		If Not obj Is Nothing Then
			Call CheckObjectIsDoc(obj)
			Call obj.Activate()
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

	Public Overrides Function ExtractPreview(ByVal obj As Object) As String
		Return ""
	End Function


	Public Overrides Sub Close(ByVal obj As Object)
		Try
            CheckObjectIsDoc(obj)
            obj.Saved = True                                    'adding this stops the extra message box from appearing - RN 02/04/15
            obj.Close(Excel.XlSaveAction.xlDoNotSaveChanges)
        Catch ex As Exception
        End Try
	End Sub

	Protected Overloads Overrides Function TemplateStart(ByVal obj As Object, ByVal preclink As PrecedentLink) As Object


		Dim wb As Excel.Workbook = Nothing
		Dim tmppath As System.IO.FileInfo

		'Activate or create and activate the word application
		ActivateApplication()

		' Get the Precedent File to Load...
		Dim fetch As FWBS.OMS.DocumentManagement.Storage.FetchResults = preclink.Merge()
		If (fetch Is Nothing) Then
			Return wb
		End If

		tmppath = fetch.LocalFile

		'Set DisplayAlerts to false to stop the link warning from appearing
		Dim ask As Boolean = _app.DisplayAlerts
		Try
			_app.DisplayAlerts = False

			If Not tmppath Is Nothing Then
				'No longer using Workbooks.Add as there is no way to suppress
				'the external links prompt when merging from Word to Excel.
				'It is open as readonly so that the template can not be overwritten.
				wb = _app.Workbooks.Open(tmppath.FullName, UpdateLinks:=0, ReadOnly:=True, Editable:=False)
				SetDocVariable(wb, ISPREC, False)
				Try
					If _app.ActiveWindow.Visible = False Then
						_app.ActiveWindow.Visible = True
					End If
					ActivateDocument(wb)
				Catch ex As Exception
				End Try

			End If

		Finally
			_app.DisplayAlerts = ask
		End Try

		Return wb

	End Function

	Public Overrides ReadOnly Property DefaultDocType() As String
		Get
			Return "SPREADSHEET"
		End Get
	End Property

	Public Overrides ReadOnly Property DefaultPrecedentType() As String
		Get
			Dim ret As String = MyBase.DefaultPrecedentType
			If ret = "" Then
				Return "SPREADSHEET"
			Else
				Return ret
			End If
		End Get
	End Property

	Public Overrides Function GetDocExtension(ByVal obj As Object) As String

		Dim excelDoc As Excel.Workbook = obj
		If String.IsNullOrEmpty(excelDoc.Path) Then ' Path is undefined for newly created documents.
			Return String.Empty ' Workaround to force usage of the file extension configured in AdminKit Document Types.
		End If
		Dim format As Integer = CInt(excelDoc.FileFormat)

		Select Case format
			Case 18 'XlFileFormat.xlAddIn, XlFileFormat.xlAddIn8
				Return "xla"
			Case 6, 22, 24, 23, 62 'XlFileFormat.xlCSV, XlFileFormat.xlCSVMac, XlFileFormat.xlCSVMSDOS, XlFileFormat.xlCSVWindows, XlFileFormat.xlCSVUTF8
				Return "csv"
			Case -4158, 19, 21, 20, 42 'XlFileFormat.xlCurrentPlatformText, XlFileFormat.xlTextMac, XlFileFormat.xlTextMSDOS, XlFileFormat.xlTextWindows, XlFileFormat.xlUnicodeText
				Return "txt"
			Case 7, 8, 11 'XlFileFormat.xlDBF2, XlFileFormat.xlDBF3, XlFileFormat.xlDBF4
				Return "dbf"
			Case 9 ' XlFileFormat.xlDIF
				Return "dif"
			Case 50 'XlFileFormat.xlExcel12
				Return "xlsb"
			Case 16, 27, 29, 33, 39, 56, 43, -4143 'XlFileFormat.xlExcel2,  XlFileFormat.xlExcel2FarEast, XlFileFormat.xlExcel3, XlFileFormat.xlExcel4, XlFileFormat.xlExcel5, XlFileFormat.xlExcel7, XlFileFormat.xlExcel8, XlFileFormat.xlExcel9795, XlFileFormat.xlWorkbookNormal
				Return "xls"
			Case 35 'XlFileFormat.xlExcel4Workbook
				Return "xlw"
			Case 44 'XlFileFormat.xlHtml
				Return "html"
			Case 26, 25 'XlFileFormat.xlIntlAddIn, XlFileFormat.xlIntlMacro
				Return ""
			Case 60 'XlFileFormat.xlOpenDocumentSpreadsheet
				Return "ods"
			Case 52 'XlFileFormat.xlOpenXMLWorkbookMacroEnabled
				Return "xlsm"
			Case 55 'XlFileFormat.xlOpenXMLAddIn
				Return "xlam"
			Case 54 'XlFileFormat.xlOpenXMLTemplate
				Return "xltx"
			Case 53 'XlFileFormat.xlOpenXMLTemplateMacroEnabled
				Return "xltm"
			Case 51, 61 'XlFileFormat.xlOpenXMLWorkbook, XlFileFormat.xlWorkbookDefault, XlFileFormat.xlOpenXMLStrictWorkbook
				Return "xlsx"
			Case 2 'XlFileFormat.xlSYLK
				Return "slk"
			Case 17 'XlFileFormat.xlTemplate, XlFileFormat.xlTemplate8
				Return "xlt"
			Case 19, 20, 21, 42, -4158 'XlFileFormat.xlTextMac, XlFileFormat.xlTextWindows, XlFileFormat.xlTextMSDOS, XlFileFormat.xlUnicodeText, XlFileFormat.xlCurrentPlatformText
				Return "txt"
			Case 36 'XlFileFormat.xlTextPrinter
				Return "prn"
			Case 45 'XlFileFormat.xlWebArchive
				Return "mht"
			Case 14 'XlFileFormat.xlWJ2WD1
				Return "wj2"
			Case 40, 41 'XlFileFormat.xlWJ3, XlFileFormat.xlWJ3FJ3
				Return "wj3"
			Case 5, 31, 30 'XlFileFormat.xlWK1, XlFileFormat.xlWK1ALL, XlFileFormat.xlWK1FMT
				Return "wkl"
			Case 15, 32 'XlFileFormat.xlWK3, XlFileFormat.xlWK3FM3
				Return "wk3"
			Case 38 'XlFileFormat.xlWK4
				Return "wk4"
			Case 4, 28 'XlFileFormat.xlWKS, XlFileFormat.xlWorks2FarEast
				Return "wks"
			Case 34 'XlFileFormat.xlWQ1
				Return "wql"
			Case 46 'XlFileFormat.xlXMLSpreadsheet
				Return "xml"
			Case Else
				Return String.Empty
		End Select

	End Function

    Private Function GetDocSaveFormat(ByVal ext As String) As Object
        Select Case ext
            Case "xls"
                Return Excel.XlFileFormat.xlExcel8
            Case "xlsx"
                Return Excel.XlFileFormat.xlOpenXMLWorkbook
            Case "xlsm"
                Return Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled
            Case "xlsb"
                Return Excel.XlFileFormat.xlExcel12
            Case "ods"
                Return Excel.XlFileFormat.xlOpenDocumentSpreadsheet
            Case "xml"
                Return Excel.XlFileFormat.xlXMLSpreadsheet
            Case Else
                Return Nothing
        End Select
    End Function

	Protected Overloads Overrides Function InternalDocumentOpen(ByVal document As OMSDocument, ByVal fetchData As FetchResults, ByVal settings As OpenSettings) As Object
		ActivateApplication()


		Try
			Dim doc As Excel.Workbook = Nothing

			Dim file As System.IO.FileInfo = fetchData.LocalFile

			Select Case settings.Mode
				Case DocOpenMode.Edit
					doc = _app.Workbooks.Open(file.FullName, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
				Case DocOpenMode.Print
					Try
						doc = _app.Workbooks.Open(file.FullName, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
					Catch ex As Exception
						If Err.Number = 5479 Then
							doc = _app.ActiveWorkbook 'Taken from WordOMS
						Else : Throw ex
						End If
					End Try
					Dim docs(0) As Object
					docs(0) = doc
					BeginPrint(docs, settings.Printing)
					Return doc
				Case DocOpenMode.View
					doc = _app.Workbooks.Open(file.FullName, ReadOnly:=True, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
			End Select
			Return doc
		Catch ex As Exception
			If Err.Number = 5479 Then
				Return _app.ActiveWorkbook
			Else
				Throw
			End If
		End Try

	End Function

	Protected Overloads Overrides Function InternalPrecedentOpen(ByVal precedent As Precedent, ByVal fetchData As FetchResults, ByVal settings As OpenSettings) As Object
		ActivateApplication()

		Try
			Dim doc As Excel.Workbook = Nothing

			Dim file As System.IO.FileInfo = fetchData.LocalFile

			Select Case settings.Mode
				Case DocOpenMode.Edit
					doc = _app.Workbooks.Open(file.FullName, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
				Case DocOpenMode.Print
					Try
						doc = _app.Workbooks.Open(file.FullName, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
					Catch ex As Exception
						If Err.Number = 5479 Then
							doc = _app.ActiveWorkbook	'Taken from wordOMS
						Else : Throw ex
						End If
					End Try
					Dim docs(0) As Object
					docs(0) = doc
					BeginPrint(docs, settings.Printing)
				Case DocOpenMode.View
					doc = _app.Workbooks.Open(file.FullName, ReadOnly:=True, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
					Try
						doc.Protect()
					Catch ex As Exception
					End Try
			End Select
			Return doc
		Catch ex As Exception
			If Err.Number = 5479 Then
				Return _app.ActiveWorkbook
			Else
				Throw
			End If
		End Try
	End Function

    Public Overloads Overrides Function Open(ByVal file As System.IO.FileInfo) As Object
        ActivateApplication()

        Try
            Dim doc As Excel.Workbook = _app.Workbooks.Open(file.FullName, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList, UpdateLinks:=0)
            Return doc
        Catch ex As Exception
            If Err.Number = 5479 Then
                Return _app.ActiveWorkbook
            Else
                MessageBox.Show(ex)
                Return Nothing
            End If
        End Try
    End Function

	Public Overrides Function Open(ByVal version As DocumentVersion, ByVal mode As DocOpenMode) As Object
	    ActivateApplication()
	    Dim pointer As IntPtr = IntPtr.Zero
	    Try
	        pointer = Marshal.GetIUnknownForObject(_app)
	        NativeMethods.OleLockRunning(pointer, True, False)
	        _app.Interactive = False
	        Return MyBase.Open(version, mode)
	    Finally
            Try
                _app.Interactive = True
            Finally
                If pointer <> IntPtr.Zero Then
                    NativeMethods.OleLockRunning(pointer, False, False)
                    Marshal.Release(pointer)
                End If
            End Try
	    End Try
	End Function

    Protected Overrides Sub InsertText(ByVal obj As Object, ByVal precLink As PrecedentLink)
		Call CheckObjectIsDoc(obj)
	End Sub

	Protected Overloads Overrides Sub InternalDocumentSave(ByVal obj As Object, ByVal saveMode As Fwbs.OMS.PrecSaveMode, ByVal printMode As Fwbs.OMS.PrecPrintMode, ByVal doc As OMSDocument, ByVal version As DocumentVersion)

		Call CheckObjectIsDoc(obj)

		Dim wb As Excel.Workbook = obj

        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = doc.GetStorageProvider()
        provider.SaveMode = saveMode

		Dim item As IStorageItem = version
		If item Is Nothing Then
			item = doc
		End If

		Dim itemExtension As String = item.Extension.ToLowerInvariant()
		Dim fileFormat As Object = GetDocSaveFormat(itemExtension)
		Dim tmppath As System.IO.FileInfo = FWBS.OMS.[Global].GetTempFile(itemExtension)
		Dim tmppath2 As System.IO.FileInfo = FWBS.OMS.[Global].GetTempFile(itemExtension)

		Try
			wb.SaveAs(Filename:=tmppath.FullName, FileFormat:=fileFormat, AddToMru:=False)

			If (wb.ReadOnly = False) Then
				wb.Save()
				ClearRecentFile(wb.Path)

				wb.SaveAs(Filename:=tmppath2.FullName, FileFormat:=fileFormat, AddToMru:=False)

				Dim storeres As StoreResults = provider.Store(item, tmppath, obj, True, Me)

                'Resave the document to the the locally cached location of the document so that it frees up the temp file
				Dim file As System.IO.FileInfo = provider.GetLocalFile(storeres.Item)


				wb.SaveAs(Filename:=file.FullName, FileFormat:=fileFormat, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList)
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

    Protected Overrides Sub InternalPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent)
        InternalPrecedentSave(obj, saveMode, printMode, prec, Nothing)
    End Sub

    Protected Overrides Sub InternalPrecedentSave(ByVal obj As Object, ByVal saveMode As PrecSaveMode, ByVal printMode As PrecPrintMode, ByVal prec As Precedent, ByVal version As PrecedentVersion)

        Call CheckObjectIsDoc(obj)

        Dim wb As Excel.Workbook = obj
        Dim provider As FWBS.OMS.DocumentManagement.Storage.StorageProvider = prec.GetStorageProvider()

        Dim item As IStorageItem = version
        If item Is Nothing Then
            item = prec
        End If

        Dim itemExtension As String = item.Extension.ToLowerInvariant()
        Dim tmppath As System.IO.FileInfo = FWBS.OMS.Global.GetTempFile(itemExtension)
        Dim tmppath2 As System.IO.FileInfo = FWBS.OMS.Global.GetTempFile(itemExtension)

        Dim fileFormat As Object = Excel.XlFileFormat.xlTemplate
        If itemExtension = "xltx" Then
            fileFormat = Excel.XlFileFormat.xlOpenXMLTemplate
        ElseIf itemExtension = "xltm" Then
            fileFormat = Excel.XlFileFormat.xlOpenXMLTemplateMacroEnabled
        End If

        Try
            wb.SaveAs(Filename:=tmppath.FullName, FileFormat:=fileFormat, AddToMru:=False)

            If (wb.ReadOnly = False) Then
                wb.Save()
                ClearRecentFile(wb.Path)

                wb.SaveAs(Filename:=tmppath2.FullName, FileFormat:=fileFormat, AddToMru:=False)
                Dim storeres As StoreResults = provider.Store(item, tmppath, obj, True, Me)

                'Resave the document to the the locally cached location of the document so that it frees up the temp file
                Dim file As System.IO.FileInfo = provider.GetLocalFile(storeres.Item)

                wb.SaveAs(Filename:=file.FullName, FileFormat:=fileFormat, AddToMru:=FWBS.OMS.Session.CurrentSession.CurrentUser.ShowMRIList)
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


	Private Sub ClearRecentFile(ByVal path As String)

		For ctr As Integer = 1 To _app.RecentFiles.Count
			Dim recent As Excel.RecentFile = _app.RecentFiles.Item(ctr)
			On Error Resume Next
			If (recent.Path = path) Then recent.Delete()
		Next
	End Sub

	Protected Overrides Function IsReadOnly(ByVal obj As Object) As Boolean
		Call CheckObjectIsDoc(obj)
		Dim doc As Excel.Workbook = obj
		Return doc.ReadOnly
	End Function

	Protected Overloads Overrides Sub InternalSave(ByVal obj As Object, ByVal createFileIfNew As Boolean)
		On Error Resume Next
		Call CheckObjectIsDoc(obj)
		Dim doc As Excel.Workbook = obj
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

	Protected Overrides Sub InternalOnNewVersion(ByVal current As Object, ByVal args As Fwbs.OMS.DocumentManagement.Storage.NewVersionEventArgs)
		CheckObjectIsDoc(current)

		Dim wb As Excel.Workbook = current
		'A bit of a bodge but make sure that all changes to the document 
		'caused by a new version are reflected in the file beoing passed to the storage
		'provider.
		Dim currentfile As String = wb.FullName
		If (System.IO.File.Exists(args.File.FullName)) Then
			System.IO.File.Delete(args.File.FullName)
		End If
		wb.SaveAs(Filename:=args.File.FullName, AddToMru:=False)
		If (System.IO.File.Exists(currentfile)) Then
			System.IO.File.Delete(currentfile)
		End If
		wb.SaveAs(Filename:=currentfile, AddToMru:=False)

	End Sub


#End Region

#Region "Table Routine"

	Public Overloads Overrides Sub BuildTable(ByVal obj As Object, ByVal code As String, ByVal vw As DataView, ByVal includeHeader As Boolean)
		Call CheckObjectIsDoc(obj)

		Try

			_app.ScreenUpdating = False

			Dim wb As Excel.Workbook = obj
			Dim ws As Excel.Worksheet = wb.ActiveSheet

			Dim numfmt As System.Globalization.NumberFormatInfo = Session.CurrentSession.DefaultCurrencyFormat
			Dim datefmt As System.Globalization.DateTimeFormatInfo = Session.CurrentSession.DefaultDateTimeFormat

			Dim assoc As Associate = GetCurrentAssociate(obj)

			If Not assoc Is Nothing Then
				numfmt = assoc.OMSFile.CurrencyFormat
				datefmt = assoc.OMSFile.DateTimeFormat
			End If

			Dim start As Excel.Range = ws.Cells(1, 1)
			Try
				start = wb.Names.Item(code).RefersToRange
			Catch
			End Try

			Dim startcol As Integer = start.Column
			Dim startrow As Integer = start.Row
			Dim totalcols As Integer = start.Columns.Count
			Dim totalrows As Integer = start.Rows.Count
			Dim minusrows As Integer = 0
			Dim colctr As Integer = start.Column
			Dim rowctr As Integer = start.Row


			If includeHeader Then
				For Each col As DataColumn In vw.Table.Columns
					If (colctr > (totalcols + startcol - 1) And totalcols > 1) Then Exit For
					Dim cell As Excel.Range = ws.Cells.Item(rowctr, colctr)
					cell.ID = col.ColumnName
					cell.Value = If(col.Caption = "", col.ColumnName, col.Caption)
					colctr = colctr + 1
				Next
				rowctr = rowctr + 1
				minusrows = 1
			End If

			Dim resultcount As Integer = vw.Count
			For Each row As DataRowView In vw
				colctr = start.Column
				If (rowctr > (totalrows + startrow - 1) And totalrows > 1) Then Exit For

				For Each col As DataColumn In vw.Table.Columns
					If (colctr > (totalcols + startcol - 1) And totalcols > 1) Then Exit For
					Dim alignment As String = Convert.ToString(col.ExtendedProperties.Item("ALIGNMENT"))
					Dim format As String = Convert.ToString(col.ExtendedProperties.Item("FORMAT"))
					ws.Cells.Item(rowctr, colctr).Value = Convert.ToString(FieldParser.GetFormattedValue(row(col.ColumnName), numfmt, datefmt, format))
					colctr = colctr + 1
				Next
				rowctr = rowctr + 1
			Next

		Finally
			_app.ScreenUpdating = True
		End Try

	End Sub


#End Region

#Region "Document Variables"

	Public Overrides Sub RemoveDocVariable(ByVal obj As Object, ByVal varName As String)
		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False

		Try
			Call CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

			Dim varsheet As Excel.Worksheet = CreateVariableSheetInternal(obj)
			Try
				varsheet.Unprotect(PROTECTION)
				Dim cellname As Excel.Range = Nothing
				Dim cellval As Excel.Range = Nothing
				cellname = GetVarCells(obj, varName, cellval)
				RemoveDocPropertyInternal(obj, varName)
				cellname.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp)
			Catch
			Finally

			End Try
		Catch ex As Exception
			Debug.WriteLine(ex.Message)
		Finally
            Try
                If Not doc Is Nothing And changed = False Then
                    If doc.Saved = False Then
                        doc.Saved = True
                    End If
                End If
            Catch
            End Try
		End Try
	End Sub

	Protected Overloads Overrides Function GetDocVariable(ByVal obj As Object, ByVal varName As String) As Object
		Dim val As Object = Nothing
		CheckObjectIsDoc(obj)

		If HasDocVariableInternal(obj, varName) Then
			val = GetDocVariableInternal(obj, varName)
		Else
			If Array.IndexOf(FieldExclusionList, varName) > -1 Then
				If (HasDocProperty(obj, varName)) Then
					val = GetDocProperty(obj, varName, Nothing)
					SetDocVariable(obj, varName, GetDocProperty(obj, varName, ""))
				End If
			End If
		End If

		If TypeOf val Is String Then
			If val = " " Then val = ""
		End If

		Return val

	End Function


	Private Function GetDocVariableInternal(ByVal obj As Object, ByVal varName As String) As Object
		Dim cellname As Excel.Range = Nothing
		Dim cellval As Excel.Range = Nothing
		cellname = GetVarCells(obj, varName, cellval)
		Return cellval.Value
	End Function


	Public Overrides Function HasDocVariable(ByVal obj As Object, ByVal varName As String) As Boolean

		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False
		Try
			CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

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
		Catch
			Return False
		Finally
            Try
                If Not doc Is Nothing And changed = False Then
                    If doc.Saved = False Then
                        doc.Saved = True
                    End If
                End If
            Catch
            End Try
		End Try
	End Function


	Private Function HasDocVariableInternal(ByVal obj As Object, ByVal varName As String) As Boolean
		Try
			Call CheckObjectIsDoc(obj)
			Dim cellname As Excel.Range
			cellname = GetVarCells(obj, varName)
			Dim n As String = cellname.Value
			If (n = String.Empty) Then
				Return False
			Else
				Return True
			End If
		Catch
			Return False
		End Try
	End Function


	Private Function SetDocVariableInternal(ByVal obj As Object, ByVal varName As String, ByVal val As Object) As Boolean
		Dim varsheet As Excel.Worksheet = CreateVariableSheetInternal(obj)

		Try

			varsheet.Unprotect(PROTECTION)

			Dim cellname As Excel.Range = Nothing
			Dim cellval As Excel.Range = Nothing

			cellname = GetVarCells(obj, varName, cellval)
			cellname.Value = varName
			cellval.Value = val

			If Array.IndexOf(FieldExclusionList, varName) > -1 Then
				Try
					RemoveDocPropertyInternal(obj, varName)
				Catch
				End Try
				SetDocPropertyInternal(obj, varName, val)
			End If

			Return True

		Catch ex As Exception
			Return False
		Finally

		End Try
	End Function
	Protected Overloads Overrides Function SetDocVariable(ByVal obj As Object, ByVal varName As String, ByVal val As Object) As Boolean
		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False

		Try
			Call CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

			Return SetDocVariableInternal(obj, varName, val)
		Catch
			Return False
		Finally
            Try
                If Not doc Is Nothing And changed = False Then
                    If doc.Saved = False Then
                        doc.Saved = True
                    End If
                End If
            Catch
            End Try
		End Try

	End Function

	Private Function CreateVariableSheet(ByVal obj As Object) As Excel.Worksheet
		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False
		Try
			CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

			Return CreateVariableSheetInternal(obj)
		Catch ex As Exception
			Return Nothing
		Finally
			If (Not doc Is Nothing And changed = False) Then
				If doc.Saved = False Then
					doc.Saved = True
				End If
			End If
		End Try
	End Function
	Private Const VARIABLESHEETNAME As String = "_VARIABLES"

	Private Function CreateVariableSheetInternal(ByVal obj As Object) As Excel.Worksheet
		On Error Resume Next
		Dim varsheet As Excel.Worksheet
		varsheet = obj.Worksheets(VARIABLESHEETNAME)
		If varsheet Is Nothing Then
			varsheet = obj.Worksheets.Add()
			varsheet.Name = VARIABLESHEETNAME
		End If

		If (varsheet.Visible <> Excel.XlSheetVisibility.xlSheetVeryHidden) Then
			varsheet.Visible = Excel.XlSheetVisibility.xlSheetVeryHidden
		End If
		Return varsheet
	End Function

	Private Function GetVarCells(ByVal obj As Object, ByVal varName As String, Optional ByRef cellVal As Excel.Range = Nothing) As Excel.Range
		Dim varsheet As Excel.Worksheet = CreateVariableSheetInternal(obj)
		Dim cellName As Excel.Range

		Dim ctr As Integer = 1

		cellName = varsheet.Cells.Item(ctr, 1)
		cellVal = varsheet.Cells(ctr, 2)

		Dim n As String = cellName.Value
		Do While n <> "" Or ctr > 1000
			If (n.ToUpper() = varName.ToUpper()) Then
				Exit Do
			End If
			ctr = ctr + 1
			cellName = varsheet.Cells(ctr, 1)
			cellVal = varsheet.Cells(ctr, 2)
			n = cellName.Value
		Loop

		Return cellName
	End Function

#End Region

#Region "Bookmark Routines"

	Protected Overrides Function GetBookmark(ByVal obj As Object, ByVal index As Integer) As String
		Call CheckObjectIsDoc(obj)
		index = index + 1
		Return obj.Names.Item(index).Name
	End Function

	Protected Overrides Function GetBookmarkCount(ByVal obj As Object) As Integer
		Try
			Call CheckObjectIsDoc(obj)
			Return obj.Names.Count
		Catch
			Return 0
		End Try
	End Function

	Protected Overrides Function HasBookmark(ByVal obj As Object, ByVal name As String) As Boolean
		Try
			Call CheckObjectIsDoc(obj)
			Dim n As Excel.Name = obj.Names.Item(name)
			Return True
		Catch
			Return False
		End Try
	End Function

	Protected Overrides Function SetBookmark(ByVal obj As Object, ByVal name As String, ByVal val As String) As Boolean
		Call CheckObjectIsDoc(obj)
		obj.Names.Item(name).RefersToRange.Value = val
		Return True
	End Function

#End Region

#Region "Custom Document Properties"

	Private Function HasDocProperty(ByVal obj As Excel.Workbook, ByVal varName As String) As Boolean
		Try
			'Add the global prefix
			Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)
			Return Not obj.CustomDocumentProperties.Item(globalname).Value Is Nothing
		Catch
			Return False
		End Try
	End Function

	Private Function GetDocProperty(ByVal obj As Excel.Workbook, ByVal varName As String, ByVal defVal As Object) As Object
		Try
			'Add the global prefix
			Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)
			Return obj.CustomDocumentProperties.Item(globalname).Value
		Catch
			Return defVal
		End Try
	End Function

	Private Function SetDocProperty(ByVal obj As Excel.Workbook, ByVal varName As String, ByVal val As Object) As Boolean
		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False
		Try
			CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

			Return SetDocPropertyInternal(obj, varName, val)
		Catch
			Return False
		Finally
            Try
                If Not doc Is Nothing And changed = False Then
                    If doc.Saved = False Then
                        doc.Saved = True
                    End If
                End If
            Catch
            End Try
		End Try
	End Function

	Private Function SetDocPropertyInternal(ByVal obj As Excel.Workbook, ByVal varName As String, ByVal val As Object) As Boolean

		Try
			'Add the global prefix
			Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)

			Dim prop As Microsoft.Office.Core.DocumentProperty = Nothing

			Try
				prop = obj.CustomDocumentProperties(globalname)
			Catch
			End Try

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
				t = Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeNumber
			End If

			If prop Is Nothing Then
				obj.CustomDocumentProperties.Add(Name:=globalname, LinkToContent:=False, value:=val, TYPE:=t)
			Else
				If prop.Type <> t Then
					prop.Type = t
				End If
				prop.Value = val
			End If
			Return True
		Catch
			Return False
		End Try
	End Function

	Private Sub RemoveDocProperty(ByVal obj As Excel.Workbook, ByVal varName As String)
		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False
		Try
			CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

			RemoveDocPropertyInternal(obj, varName)
		Catch
		Finally
            Try
                If Not doc Is Nothing And changed = False Then
                    If doc.Saved = False Then
                        doc.Saved = True
                    End If
                End If
            Catch
            End Try
		End Try
	End Sub

	Private Sub RemoveDocPropertyInternal(ByVal obj As Excel.Workbook, ByVal varName As String)
		Try
			'Add the global prefix
			Dim globalname As String = Session.CurrentSession.GetExternalDocumentPropertyName(varName)
			obj.CustomDocumentProperties.Item(globalname).Delete()
		Catch
		End Try
	End Sub

#End Region

#Region "Field Routines"

	Public Overrides Sub AddField(ByVal obj As Object, ByVal fieldName As String)
		MyBase.AddField(obj, fieldName)
		Call CheckObjectIsDoc(obj)
		On Error Resume Next
		_app.ActiveCell.Formula = "=OMSFIELD(""" + fieldName + """)"
	End Sub

	Public Overrides Sub DeleteField(ByVal obj As Object, ByVal fieldName As String)
		Dim val As String = GetDocVariable(obj, fieldName, "")
		MyBase.DeleteField(obj, fieldName)
		CheckObjectIsDoc(obj)
		FunctionFinder(obj, fieldName, True)
		ScreenRefresh()
	End Sub

	Public Overrides Function GetFieldCount(ByVal obj As Object) As Integer
		Dim doc As Excel.Workbook = Nothing
		Dim changed As Boolean = False
		Try
			CheckObjectIsDoc(obj)

			doc = obj
			changed = Not doc.Saved

			Dim fieldMatches As System.Text.RegularExpressions.MatchCollection = GetFieldRegExMatches(obj)
			Dim match As System.Text.RegularExpressions.Match
			For Each match In fieldMatches
				Dim field As String = match.Groups("FieldName").Value
				field = field.TrimStart("""")
				field = field.TrimEnd("""")
				If HasDocVariableInternal(obj, field) = False Then
					SetDocVariableInternal(obj, field, "")
				End If
			Next

            Dim xl As Excel.Workbook = obj
            If xl.ProtectWindows Or xl.ProtectStructure Then
                Throw New OMSException2("XLWBPROTECTED", "The Excel Workbook is protected and cannot be saved.", "")
            End If

			Dim varsheet As Excel.Worksheet = CreateVariableSheetInternal(obj)
			Dim ctr As Integer = 1
			Dim n As String = varsheet.Cells.Item(ctr, 1).Value

			Do Until n = ""
				ctr = ctr + 1
				n = varsheet.Cells.Item(ctr, 1).Value
			Loop

			Return ctr - 1

		Finally
			If (Not doc Is Nothing And changed = False) Then
				If doc.Saved = False Then
					doc.Saved = True
				End If
			End If
		End Try

	End Function

	Public Overrides Function GetFieldName(ByVal obj As Object, ByVal index As Integer) As String
		CheckObjectIsDoc(obj)
		index = index + 1
		Dim varsheet As Excel.Worksheet = CreateVariableSheet(obj)
		Return varsheet.Cells.Item(index, 1).Value
	End Function

	Public Overloads Overrides Function GetFieldValue(ByVal obj As Object, ByVal index As Integer) As Object
		Call CheckObjectIsDoc(obj)
		index = index + 1
		Dim varsheet As Excel.Worksheet = CreateVariableSheet(obj)
		Return varsheet.Cells.Item(index, 2).Value
	End Function

	Public Overloads Overrides Function GetFieldValue(ByVal obj As Object, ByVal name As String) As Object
		Call CheckObjectIsDoc(obj)
		Return GetDocVariable(obj, name)
	End Function

	Public Overloads Overrides Sub SetFieldValue(ByVal obj As Object, ByVal index As Integer, ByVal val As Object)
		Call CheckObjectIsDoc(obj)
		Dim field As String = GetFieldName(obj, index)
		SetDocVariable(obj, field, val)
	End Sub

	Public Overloads Overrides Sub SetFieldValue(ByVal obj As Object, ByVal name As String, ByVal val As Object)
		Call CheckObjectIsDoc(obj)
		SetDocVariable(obj, name, val)
	End Sub

	Private Function GetFieldRegExMatches(ByVal obj As Object) As System.Text.RegularExpressions.MatchCollection
		Call CheckObjectIsDoc(obj)

		Dim text As String = FunctionFinder(obj)

		Dim fieldregx As String = "OMSFIELD\((?<FieldName>.*?)\)"
		Dim matches As System.Text.RegularExpressions.MatchCollection = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline)
		Return matches
	End Function

	Private Function FunctionFinder(ByVal obj As Object, Optional ByVal field As String = "*", Optional ByVal delete As Boolean = False, Optional ByVal refresh As Boolean = False) As String
		Call CheckObjectIsDoc(obj)

		Dim fields As String = ""
		Dim rng As Excel.Range

        field = field.Replace("#", "[#]")

		Dim workbook As Excel.Workbook = obj

        For Each worksheet As Object In workbook.Sheets

            Dim ws As Excel.Worksheet = TryCast(worksheet, Excel.Worksheet)
            If ws Is Nothing Then
                Continue For
            Else
                worksheet = ws
            End If

            If worksheet.Name = VARIABLESHEETNAME Then
                Continue For
            End If

            rng = worksheet.Cells.Find("OMSFIELD(*)")

            Dim addresses As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)

            If Not rng Is Nothing Then
                Do
                    If (rng.Text Like "*" & field & "*") Then
                        If (delete) Then
                            rng.Clear()
                        Else
                            fields = fields & rng.Formula & Environment.NewLine
                        End If
                    End If
                    addresses.Add(rng.Address)
                    rng = worksheet.Cells.FindNext(rng)
                    If (refresh) Then
                        Dim val As String = Convert.ToString(rng.Calculate())
                        If (val = String.Empty) Then
                            rng.FormulaR1C1 = rng.Formula
                        End If
                    End If
                Loop While Not rng Is Nothing AndAlso Not addresses.Contains(rng.Address)

            End If
        Next worksheet

        Return fields

    End Function

	Public Overrides Sub CheckFields(ByVal obj As Object)
		Call CheckObjectIsDoc(obj)

		Try
			Dim opt As Excel.XlCalculation = _app.Calculation
			_app.Calculation = Excel.XlCalculation.xlCalculationManual
			FunctionFinder(obj, False, False, True)
			_app.Calculation = opt
		Finally
			_app.ScreenUpdating = True
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
		If (disposing) Then

			Try
				If (Not _app Is Nothing) Then
					RemoveDelegates()
					Common.COM.DisposeObject(_app)
					_app = Nothing
				End If
			Catch ex As SystemException
			Finally
                'Final garbage collection
                GC.Collect()
                GC.WaitForPendingFinalizers()
            End Try
		End If
	End Sub

#End Region

#Region "Application Events"

	Private Sub ValidateEditMode()

		Dim cb As MSOffice.CommandBar = CommandBarContainer("Worksheet Menu Bar")
		If (cb Is Nothing) Then
			Return
		End If

		Dim oNewMenu As MSOffice.CommandBarControl = CommandBarContainer("Worksheet Menu Bar").FindControl(1, 18, , , True)
		If (oNewMenu Is Nothing) Then
			Return
		End If

		If (Not oNewMenu.Enabled) Then
			Throw New OMSException2("RESEXCELEDIT", "")
        End If


	End Sub
	Private Sub ButtonClick(ByVal button As Microsoft.Office.Core.CommandBarButton, ByRef cancelDefault As Boolean)
		Try

			Dim disableprint As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "DisableOMSExcelPrint", "False")

			If Session.CurrentSession.IsLoggedIn Then


				Select Case button.Id
					Case 3
						If (Me.DocumentManagementMode And DocumentManagementMode.Save) <> DocumentManagementMode.None Then
							cancelDefault = True
							RunCommand(Application, "SYSTEM;SAVE")
						End If
					Case 4, 2521
						If (Me.DocumentManagementMode And DocumentManagementMode.Print) <> DocumentManagementMode.None And disableprint.ToBoolean = False Then
							cancelDefault = True
							RunCommand(Application, "SYSTEM;PRINT")
						End If
					Case 23
						If (Me.DocumentManagementMode And DocumentManagementMode.Open) <> DocumentManagementMode.None Then
							cancelDefault = True
							RunCommand(Application, "SYSTEM;OPEN")
						End If
					Case 748
						If (Me.DocumentManagementMode And DocumentManagementMode.Save) <> DocumentManagementMode.None Then
							cancelDefault = True
							RunCommand(Application, "SYSTEM;SAVEAS")
						End If
				End Select

			End If
		Catch ex As Exception
			FWBS.OMS.UI.Windows.ErrorBox.Show(ex)
		End Try
	End Sub


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

		Private _handle As Integer

		Private Sub New()
		End Sub

		Public Sub New(ByVal obj As Object)
			Dim caption As String = ""
			Try
				If TypeOf obj Is Excel.Workbook Then
					caption = obj.Application.Caption
				ElseIf TypeOf obj Is Excel.Application Then
					caption = obj.Caption
				ElseIf TypeOf obj Is Excel.Window Then
					caption = obj.Caption
				Else
					caption = obj.Application.Caption
				End If
			Catch ex As Exception
				_handle = 0
			End Try
			_handle = Common.Functions.FindWindow("XLMAIN", caption)
		End Sub

		Public ReadOnly Property Handle() As System.IntPtr Implements System.Windows.Forms.IWin32Window.Handle
			Get
				Return New IntPtr(_handle)
			End Get
		End Property

	End Class

    Private NotInheritable Class NativeMethods
        <System.Runtime.InteropServices.DllImport("Ole32.dll", ExactSpelling:=True, EntryPoint:="OleLockRunning")>
        Public Shared Function OleLockRunning(ByVal obj As IntPtr, ByVal fLock As Boolean, ByVal fLastUnlockCloses As Boolean) As Int32
        End Function
    End Class

End Class
