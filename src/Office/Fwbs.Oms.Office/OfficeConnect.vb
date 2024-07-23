Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Forms
Imports Extensibility
Imports Excel = Microsoft.Office.Interop.Excel
Imports Outlook = Microsoft.Office.Interop.Outlook
Imports Word = Microsoft.Office.Interop.Word


<Guid("AD82970C-6AA8-4834-8482-2B0DB119ED5C"), ProgId("omsofficexp.Connect"), ComVisible(True)> _
 Public Class OfficeConnect
    Implements IDTExtensibility2


#Region "Fields"

    ' This Addin instance.
    Private _addin As Object = Nothing

    'Main application object, be it Word, Excel, Outlook etc...
    Private _app As Object = Nothing

    'Application Controller object WORDOMS, EXCELOMS, OUTLOOKOMS etc...
    Private _appcontroller As Office.OfficeOMSApp = Nothing

#End Region

#Region "Constructors"

    Public Sub New()
        Dim regvisstyle As FWBS.Common.Reg.ApplicationSetting = New FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "DisableVisualStyles")
        If Convert.ToBoolean(regvisstyle.GetSetting(False)) = False Then
            Call Application.EnableVisualStyles()
        End If
    End Sub

#End Region

#Region "IDTExtensibility"

    Public Sub OnConnection(ByVal application As Object, ByVal connectMode As ext_ConnectMode, ByVal addInInst As Object, ByRef custom As System.Array) Implements IDTExtensibility2.OnConnection
        Try
            ' Subscribe to thread (unhandled) exception events
            Dim currentDomain As AppDomain = AppDomain.CurrentDomain
            AddHandler currentDomain.UnhandledException, AddressOf ExceptionHandler
            AddHandler System.Windows.Forms.Application.ThreadException, AddressOf ThreadExceptionHandler


        Catch ex As Exception

        End Try


        Try
            'Outlook quick restart bodge code.
            If TypeOf application Is Outlook.Application Then
                Dim regol As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "LastOutlookShutdown", System.DateTime.MinValue)
                Dim dte As DateTime = DateTime.MinValue
                Try
                    dte = Convert.ToDateTime(regol.GetSetting(), System.Globalization.CultureInfo.InvariantCulture)
                Catch

                End Try
                If dte > DateTime.MinValue Then
                    dte = dte.AddSeconds(10)
                    If dte > DateTime.Now And application.Explorers.Count = 0 Then
                        Return
                    End If
                End If
            End If
            _app = application
            _addin = addInInst

            'Set the APIConsumer for licensing checks.
            Session.CurrentSession.APIConsumer = System.Reflection.Assembly.GetExecutingAssembly()

            SyncObjects()

            If (Session.CurrentSession.IsLoggedIn = False) Then
                If (Session.CurrentSession.GetConnectedClients().Length > 0) Then
                    Dim autologon As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "AutoLogon", True)
                    If (autologon.ToBoolean()) Then
                        _appcontroller.RunCommand(Me, "SYSTEM;CONNECT")
                    End If
                Else
                    If (Not _appcontroller Is Nothing) Then
                        Dim autologon As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "AutoLogon", False)
                        Dim forcelogon As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "ForceLogon", -1)

                        If (autologon.ToBoolean()) Then
                            If (Convert.ToInt32(forcelogon.GetSetting()) >= 0) Then
                                _appcontroller.RunCommand(Me, String.Format("SYSTEM;CONNECT;FORCE;{0}", forcelogon.ToString()))
                            Else
                                _appcontroller.RunCommand(Me, "SYSTEM;CONNECT")
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub SyncObjects()
        If (_appcontroller Is Nothing) Then

            Dim addin As Object = "OMSOfficeXP.Connect"

            If (TypeOf _app Is Word.Application) Then
                Dim app As Word.Application = _app
                app.COMAddIns.Item(addin).Object = Me
                _appcontroller = New FWBS.OMS.UI.Windows.Office.WordOMS2(app, "WORD")
            ElseIf (TypeOf _app Is Outlook.Application) Then
                Dim app As Outlook.Application = _app
                app.COMAddIns.Item(addin).Object = Me
                _appcontroller = New FWBS.OMS.UI.Windows.Office.OutlookOMS(app, "OUTLOOK")
            ElseIf (TypeOf _app Is Excel.Application) Then
                Dim app As Excel.Application = _app
                app.COMAddIns.Item(addin).Object = Me
                _appcontroller = New FWBS.OMS.UI.Windows.Office.ExcelOMS(app, "EXCEL")
            Else
                Throw New NotSupportedException()
            End If

        End If


    End Sub

    '''
    ''' Handles the thread exception.
    '''
    Sub ExceptionHandler(ByVal sender As Object, ByVal args As UnhandledExceptionEventArgs)
        Dim e As Exception = DirectCast(args.ExceptionObject, Exception)
        If e.Message.StartsWith("Handle is not initialized") Then
            Return
        End If
        Console.WriteLine("Thread Handler caught : " + e.Message)
        ShowThreadExceptionDialog(e)
    End Sub 'MyUnhandledExceptionEventHandler

    Sub ThreadExceptionHandler(ByVal sender As Object, ByVal args As ThreadExceptionEventArgs)
        If args.Exception.Message.StartsWith("Handle is not initialized") Then
            Return
        End If
        Console.WriteLine("Thread Handler caught : " + args.Exception.Message)
        ShowThreadExceptionDialog(args.Exception)
    End Sub 'MyUnhandledExceptionEventHandler

    Private Sub ShowThreadExceptionDialog(ByVal ex As Exception)

        FWBS.OMS.UI.Windows.ErrorBox.Show(ex)

    End Sub


    Public Sub OnDisconnection(ByVal disconnectMode As ext_DisconnectMode, ByRef custom As System.Array) Implements IDTExtensibility2.OnDisconnection
        If (TypeOf _appcontroller Is IDisposable) Then
            DirectCast(_appcontroller, IDisposable).Dispose()
        End If
        Call LogOutlookShutdown()
    End Sub

    Public Sub OnAddInsUpdate(ByRef custom As System.Array) Implements IDTExtensibility2.OnAddInsUpdate
    End Sub

    Public Sub OnStartupComplete(ByRef custom As System.Array) Implements IDTExtensibility2.OnStartupComplete
    End Sub


    Public Sub OnBeginShutdown(ByRef custom As System.Array) Implements IDTExtensibility2.OnBeginShutdown
        Call LogOutlookShutdown()
        Try
            FWBS.OMS.Session.CurrentSession.Disconnect()
            _appcontroller.Dispose()
        Catch
        End Try
    End Sub

    Private Sub LogOutlookShutdown()
        If TypeOf _app Is Outlook.Application Then
            Dim regol As FWBS.Common.ApplicationSetting = New FWBS.Common.ApplicationSetting(FWBS.OMS.[Global].ApplicationKey, FWBS.OMS.[Global].VersionKey, "", "LastOutlookShutdown", System.DateTime.MinValue)
            regol.SetSetting(DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture))
        End If
    End Sub

#End Region

#Region "Methods"

    Public Function Parse(ByVal field As String, ByVal defVal As Object) As Object
        Dim parser As FieldParser = New FieldParser

        If (Not _appcontroller Is Nothing) Then
            Dim assoc As Associate = _appcontroller.GetCurrentAssociate(_appcontroller)
            parser.ChangeObject(assoc)
        End If

        Return parser.Parse(field, defVal)
    End Function

    'Runs the global command button commands.
    Public Sub RunCommand(ByVal cmd As String, ByVal app As Object)

        If (_appcontroller Is Nothing) Then
            _app = app
            SyncObjects()
        End If

        If (cmd = "") Then
            Return
        End If
        Dim pars() As String
        pars = cmd.Split(";")

        Try

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If (pars.Length < 2) Then
                MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("INVALIDRUNCMD", "Invalid run command!", "", False), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            End If

            _appcontroller.RunCommand(_app, cmd)


        Catch ex As Exception
            FWBS.OMS.UI.Windows.ErrorBox.Show(ex)
        Finally
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try

    End Sub
    Public Function RunScript(ByVal commandName As String, ByVal fieldNames() As String, ByVal values() As Object) As String

        Dim pars As FWBS.Common.KeyValueCollection = New FWBS.Common.KeyValueCollection
        Dim ctr As Integer = 0

        If (Not fieldNames Is Nothing) Then
            For Each fld As String In fieldNames
                If (Not fld Is Nothing) Then
                    If (values Is Nothing Or values.Length <= ctr) Then
                        pars.Add(fld, Nothing)
                    Else
                        pars.Add(fld, values(ctr))
                        ctr = ctr + 1
                    End If
                End If
            Next
        End If

        Return _appcontroller.RunScriptCommand(commandName, pars)
    End Function


    Public Function GetDocVariable(ByVal varName As String) As Object
        Return _appcontroller.GetDocVariable(_appcontroller, varName, DirectCast(Nothing, Object))
    End Function

    Public Function GetDocVariableS(ByVal varName As String, ByVal def As String) As String
        Return _appcontroller.GetDocVariable(_appcontroller, varName, def)
    End Function

    Public Function GetDocVariableN(ByVal varName As String, ByVal def As Long) As Long
        Return _appcontroller.GetDocVariable(_appcontroller, varName, def)
    End Function

    Public Function GetDocVariableB(ByVal varName As String, ByVal def As Boolean) As Boolean
        Return _appcontroller.GetDocVariable(_appcontroller, varName, def)
    End Function

    Public Sub SetDocVariableS(ByVal obj As Object, ByVal varName As String, ByVal val As String)
        _appcontroller.SetDocVariable(obj, varName, val)
    End Sub
    Public Sub SetDocVariableL(ByVal obj As Object, ByVal varName As String, ByVal val As Long)
        _appcontroller.SetDocVariable(obj, varName, val)
    End Sub
    Public Sub SetDocVariableB(ByVal obj As Object, ByVal varName As String, ByVal val As Boolean)
        _appcontroller.SetDocVariable(obj, varName, val)
    End Sub
    Public Function GetDocumentVariableS(ByVal obj As Object, ByVal varName As String, ByVal def As String) As String
        Return _appcontroller.GetDocVariable(obj, varName, def)
    End Function

    Public Function GetDocumentVariableN(ByVal obj As Object, ByVal varName As String, ByVal def As Long) As Long
        Return _appcontroller.GetDocVariable(obj, varName, def)
    End Function

    Public Function GetDocumentVariableB(ByVal obj As Object, ByVal varName As String, ByVal def As Boolean) As Boolean
        Return _appcontroller.GetDocVariable(obj, varName, def)
    End Function
    Public Sub RemoveDocVariable(ByVal obj As Object, ByVal name As String)
        _appcontroller.RemoveDocVariable(obj, name)
    End Sub


    Public Function CreateBill(ByVal category As String) As BillInfo
        Dim b As BillInfo = New BillInfo(_appcontroller.GetDocVariable(_appcontroller, OMSApp.ASSOCIATE, -1))
        b.Category = category
        Return b
    End Function


#End Region

#Region "IManageMethods (Please Rename)"

    Public Function IsCompanyDocument(ByVal doc As Object) As Boolean
        Return _appcontroller.IsCompanyDocument(doc)
    End Function

    Public Sub AttachDocumentVars(ByVal doc As Object, ByVal assocId As Long)

        Dim assoc As Associate = Associate.GetAssociate(assocId)
        _appcontroller.AttachDocumentVars(doc, True, assoc)

    End Sub
    Public Sub AttachDocumentVariables(ByVal doc As Object, ByVal clientNo As String, ByVal fileNo As String)

        If (String.IsNullOrEmpty(clientNo) Or String.IsNullOrEmpty(fileNo)) Then
            Exit Sub
        End If

        Dim cli As Client = Client.GetClient(clientNo)

        Dim files As DataView = cli.GetFiles()
        files.RowFilter = String.Format("FILENO = '{0}'", fileNo)
        If files.Count > 0 Then

            Dim file As OMSFile = OMSFile.GetFile(files(0)("FILEID"))
            _appcontroller.AttachDocumentVars(doc, True, file.DefaultAssociate)

        End If

    End Sub

    Public Function SelectAssociate() As IAssociate

        Dim assoc As Associate
        If (Session.CurrentSession.CurrentUser.UseDefaultAssociate = FWBS.Common.TriState.Null And Not Session.CurrentSession.UseDefaultAssociate) Or Session.CurrentSession.CurrentUser.UseDefaultAssociate = FWBS.Common.TriState.False Then
            assoc = FWBS.OMS.UI.Windows.Services.SelectAssociate()
        Else
            assoc = FWBS.OMS.UI.Windows.Services.SelectDefaultAssociate()
        End If

        Return assoc
    End Function


    Public Function GetFile(ByVal fileId As Long) As IFile
        Return OMSFile.GetFile(fileId)
    End Function

    Public Function GetClient(ByVal clientId As Long) As IClient
        Return Client.GetClient(clientId)
    End Function

    Public Function GetMappingManager() As Mappers.IMappingManager

        Return Mappers.MappingManager.GetMappingManager

    End Function

#End Region


#Region "Properties"

    'Gets a flag that indicates whether the user is currently logged in.
    Public ReadOnly Property Online() As Boolean
        Get
            Return Session.CurrentSession.IsLoggedIn
        End Get
    End Property

#End Region

#Region "Recordset Methods"

    Public Function FetchDataList(ByVal code As String, ByVal fieldNames() As String, ByVal values() As Object) As ADODB.Recordset
        Dim pars As FWBS.Common.KeyValueCollection = New FWBS.Common.KeyValueCollection
        Dim ctr As Integer = 0

        If (Not fieldNames Is Nothing) Then
            For Each fld As String In fieldNames
                If (Not fld Is Nothing) Then
                    If (values Is Nothing Or values.Length <= ctr) Then
                        pars.Add(fld, Nothing)
                    Else
                        pars.Add(fld, values(ctr))
                        ctr = ctr + 1
                    End If
                End If
            Next
        End If

        Dim dl As FWBS.OMS.EnquiryEngine.DataLists = New FWBS.OMS.EnquiryEngine.DataLists(code)
        dl.ChangeParameters(pars)
        Dim dt As System.Data.DataTable = dl.Run()
        Return CreateRecordset(dt)
    End Function

    Public Function FetchSearchList(ByVal code As String, ByVal fieldNames() As String, ByVal values As Object()) As ADODB.Recordset
        Dim pars As FWBS.Common.KeyValueCollection = New FWBS.Common.KeyValueCollection
        Dim ctr As Integer = 0

        If (Not fieldNames Is Nothing) Then
            For Each fld As String In fieldNames
                If (Not fld Is Nothing) Then
                    If (values Is Nothing Or values.Length <= ctr) Then
                        pars.Add(fld, Nothing)
                    Else
                        pars.Add(fld, values(ctr))
                        ctr = ctr + 1
                    End If
                End If
            Next
        End If

        Dim sl As FWBS.OMS.SearchEngine.SearchList = New FWBS.OMS.SearchEngine.SearchList(code, Nothing, pars)
        Dim dt As System.Data.DataTable = sl.Run()
        Return CreateRecordset(dt)
    End Function

    Private Function CreateRecordset(ByVal dt As System.Data.DataTable) As ADODB.Recordset
        Dim rst As ADODB.Recordset = New ADODB.Recordset
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        If (Not dt Is Nothing) Then
            Dim cols() As String = Nothing
            Dim ctr As Integer = 0

            For Each col As System.Data.DataColumn In dt.Columns
                rst.Fields.Append(col.ColumnName, TypeToADOType(col.DataType), 0, ADODB.FieldAttributeEnum.adFldUpdatable Or ADODB.FieldAttributeEnum.adFldMayBeNull, Nothing)
                ReDim Preserve cols(ctr)
                cols(ctr) = col.ColumnName
                ctr = ctr + 1
            Next

            rst.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockBatchOptimistic, 0)

            Dim row As System.Data.DataRow
            For Each row In dt.Rows
                rst.AddNew(System.Reflection.Missing.Value, System.Reflection.Missing.Value)

                If (Not cols Is Nothing) Then
                    For Each col As String In cols
                        If (rst.Fields(col).Type = ADODB.DataTypeEnum.adGUID) Then
                        Else
                            Try

                                'UTCFIX: DM - 04/12/06 - Display local time.
                                If (TypeOf row(col) Is DateTime) Then
                                    rst.Fields(col).Value = row(col).ToLocalTime()
                                Else
                                    rst.Fields(col).Value = row(col)
                                End If
                            Catch
                            End Try
                        End If
                    Next
                End If
            Next

            dt.Clear()
            dt.Dispose()

            Try
                rst.MoveFirst()
            Catch
            End Try
        End If

        Return rst
    End Function


    Private Function TypeToADOType(ByVal Type As Object) As ADODB.DataTypeEnum
        If (TypeOf Type Is UInt64) Then
            Return ADODB.DataTypeEnum.adUnsignedBigInt
        ElseIf (TypeOf Type Is UInt32) Then
            Return ADODB.DataTypeEnum.adUnsignedInt
        ElseIf (TypeOf Type Is UInt16) Then
            Return ADODB.DataTypeEnum.adUnsignedSmallInt
        ElseIf (TypeOf Type Is Long) Then
            Return ADODB.DataTypeEnum.adBigInt
        ElseIf (TypeOf Type Is Integer) Then
            Return ADODB.DataTypeEnum.adInteger
        ElseIf (TypeOf Type Is Short) Then
            Return ADODB.DataTypeEnum.adSmallInt
        ElseIf (TypeOf Type Is Byte) Then
            Return ADODB.DataTypeEnum.adTinyInt
        ElseIf (TypeOf Type Is Decimal) Then
            Return ADODB.DataTypeEnum.adDecimal
        ElseIf (TypeOf Type Is Double) Then
            Return ADODB.DataTypeEnum.adDouble
        ElseIf (TypeOf Type Is Single) Then
            Return ADODB.DataTypeEnum.adSingle
        ElseIf (TypeOf Type Is Boolean) Then
            Return ADODB.DataTypeEnum.adBoolean
        ElseIf (TypeOf Type Is Guid) Then
            Return ADODB.DataTypeEnum.adGUID
        ElseIf (TypeOf Type Is String) Then
            Return ADODB.DataTypeEnum.adBSTR
        ElseIf (TypeOf Type Is DateTime) Then
            Return ADODB.DataTypeEnum.adDate
        ElseIf (TypeOf Type Is Byte()) Then
            Return ADODB.DataTypeEnum.adVarBinary
        Else
            Return ADODB.DataTypeEnum.adVariant
        End If
    End Function

#End Region

#Region "Table Routnines"
    Public Sub BuildDataListTable(ByVal obj As Object, ByVal code As String, ByVal includeHeader As Boolean, ByVal fieldNames() As String, ByVal values() As Object)
        If (Not _appcontroller Is Nothing) Then

            Dim pars As FWBS.Common.KeyValueCollection = New FWBS.Common.KeyValueCollection
            Dim ctr As Integer = 0

            If (Not fieldNames Is Nothing) Then
                For Each fld As String In fieldNames
                    If (Not fld Is Nothing) Then
                        If (values Is Nothing Or values.Length <= ctr) Then
                            pars.Add(fld, Nothing)
                        Else
                            pars.Add(fld, values(ctr))
                            ctr = ctr + 1
                        End If
                    End If
                Next
            End If

            Dim parser As FieldParser = New FieldParser(_appcontroller.GetCurrentAssociate(obj))
            parser.ChangeParameters(pars)
            _appcontroller.BuildTable(obj, code, parser.Parse(FieldParser.FieldPrefixDataList & code), includeHeader)
        End If
    End Sub

    Public Sub BuildSearchListTable(ByVal obj As Object, ByVal code As String, ByVal includeHeader As Boolean, ByVal fieldNames() As String, ByVal values() As Object)
        If (Not _appcontroller Is Nothing) Then

            Dim pars As FWBS.Common.KeyValueCollection = New FWBS.Common.KeyValueCollection
            Dim ctr As Integer = 0

            If (Not fieldNames Is Nothing) Then
                For Each fld As String In fieldNames
                    If (Not fld Is Nothing) Then
                        If (values Is Nothing Or values.Length <= ctr) Then
                            pars.Add(fld, Nothing)
                        Else
                            pars.Add(fld, values(ctr))
                            ctr = ctr + 1
                        End If
                    End If
                Next
            End If

            Dim parser As FieldParser = New FieldParser(_appcontroller.GetCurrentAssociate(obj))
            parser.ChangeParameters(pars)
            _appcontroller.BuildTable(obj, code, parser.Parse(FieldParser.FieldPrefixSearchList & code), includeHeader)

        End If
    End Sub

#End Region

End Class

<Guid("F0C6BDB7-9C00-44af-8222-FF8A732F45BC"), ProgId("omsofficexp.BillInfo"), ComVisible(True)> _
Public Class BillInfo
#Region "Fields"

    Private _bill As FWBS.OMS.BillingInfo = Nothing

#End Region

#Region "Constructors"

    Private Sub New()
    End Sub

    Friend Sub New(ByVal assocID As Long)
        _bill = New FWBS.OMS.BillingInfo(Associate.GetAssociate(assocID))
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property BillNo() As String
        Get
            Return _bill.BillNo
        End Get
    End Property

    Public Property BillDate() As System.DateTime
        Get
            Return _bill.BillDate.ToLocalTime()
        End Get
        Set(ByVal Value As DateTime)
            _bill.BillDate = Value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return _bill.Category
        End Get
        Set(ByVal Value As String)
            _bill.Category = Value
        End Set
    End Property

    Public ReadOnly Property VATRate() As Single
        Get
            Return _bill.VATRate
        End Get
    End Property

    Public Property OnAccountAmount() As Decimal
        Get
            Return _bill.OnAccountAmount
        End Get
        Set(ByVal Value As Decimal)
            _bill.OnAccountAmount = Value
        End Set
    End Property

    Public Property PaidDisbursments() As Decimal
        Get
            Return _bill.PaidDisbursments
        End Get
        Set(ByVal Value As Decimal)
            _bill.PaidDisbursments = Value
        End Set
    End Property

    Public Property UnpaidDisbursments() As Decimal
        Get
            Return _bill.UnpaidDisbursments
        End Get
        Set(ByVal Value As Decimal)
            _bill.UnpaidDisbursments = Value
        End Set
    End Property

    Public Property VATAmount() As Decimal
        Get
            Return _bill.VATAmount
        End Get
        Set(ByVal Value As Decimal)
            _bill.VATAmount = Value
        End Set
    End Property

    Public Property ProfessionalFees() As Decimal
        Get
            Return _bill.ProfessionalFees
        End Get
        Set(ByVal Value As Decimal)
            _bill.ProfessionalFees = Value
        End Set
    End Property

    Public ReadOnly Property TotalDisbursments() As Decimal
        Get
            Return _bill.TotalDisbursments
        End Get
    End Property

    Public ReadOnly Property TotalCost() As Decimal
        Get
            Return _bill.TotalCost
        End Get
    End Property

    Public ReadOnly Property NetCost() As Decimal
        Get
            Return _bill.NetCost
        End Get
    End Property

    Public ReadOnly Property TotalOutstanding() As Decimal
        Get
            Return _bill.TotalOutstanding
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub Update()
        _bill.Update()
    End Sub

#End Region
End Class
