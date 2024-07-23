Imports FWBS.Office.Outlook
Imports Outlook = Microsoft.Office.Interop.Outlook

Friend Class EmailDocument
    Inherits FWBS.OMS.CommonObject

#Region "Constructors"

    Private _doc As OMSDocument

    Private Sub New()
    End Sub

    Public Sub New(ByVal doc As OMSDocument)
        _doc = doc
        Try
            MyBase.Fetch(doc.ID)
        Catch
            MyBase.Create()
            SetExtraInfo("docID", _doc.ID)
        End Try
    End Sub

    Public Sub New(ByVal doc As OMSDocument, ByVal item As OutlookItem)
        Me.New(doc)

        With Me
            .Class = item.MessageClass
            .ConversationIndex = item.ConversationIndex
            .ConversationTopic = item.ConversationTopic
            .Attachments = item.Attachments.Count
            .Received = item.ReceivedTime
            If item.SentOn = OutlookOMS.MAX_DATE Then
                .Sent = System.DateTime.Now
            Else
                .Sent = item.SentOn
            End If

            Dim senderName As String = item.SenderName
            .From = If(TypeOf item Is OutlookMail, GetSenderEmailAddress(item, senderName), item.SenderEmailAddress)
            .FromName = If(senderName, item.Session.CurrentUser.Name)

            If Convert.ToString(item.To) = "" Then
                item.Save()
            End If
            .To = Convert.ToString(item.To)
            .CC = item.CC
        End With
    End Sub

    Private Function GetSenderEmailAddress(ByVal mailItem As OutlookMail, ByRef senderName As String) As String
        Dim address As String = mailItem.SenderEmailAddress
        If mailItem.SenderEmailType <> "SMTP" Then
            Dim addressEntry As Outlook.AddressEntry = If(mailItem.SenderEmailType = "EX", mailItem.Sender, mailItem.Session.CurrentUser.AddressEntry)
            If addressEntry Is Nothing Then
                Dim recipient As Outlook.Recipient = mailItem.Session.CreateRecipient(address)
                addressEntry = recipient.AddressEntry
                Runtime.InteropServices.Marshal.ReleaseComObject(recipient) : recipient = Nothing
            End If
            senderName = addressEntry.Name

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
        End If
        GetSenderEmailAddress = address
    End Function

#End Region

#Region "CommonObject"
    Protected Overrides ReadOnly Property DefaultForm() As String
        Get
            Return String.Empty
        End Get
    End Property

    Public Overrides ReadOnly Property FieldPrimaryKey() As String
        Get
            Return "docID"
        End Get
    End Property

    Public Overrides ReadOnly Property Parent() As Object
        Get
            Return _doc.Parent
        End Get
    End Property

    Protected Overrides ReadOnly Property PrimaryTableName() As String
        Get
            Return "dbDocumentEmail"
        End Get
    End Property

    Protected Overrides ReadOnly Property SelectStatement() As String
        Get
            Return "select * from dbDocumentEmail"
        End Get
    End Property

#End Region

#Region "Properties"

    Public Property StoreID() As String
        Get
            Return Convert.ToString(GetExtraInfo("docStoreID"))
        End Get
        Set(ByVal Value As String)
            If Value = String.Empty Or Value Is Nothing Then
                SetExtraInfo("docStoreID", DBNull.Value)
            Else
                SetExtraInfo("docStoreID", Value)
            End If
        End Set
    End Property

    Public Property EntryID() As String
        Get
            Return Convert.ToString(GetExtraInfo("docEntryID"))
        End Get
        Set(ByVal Value As String)
            If Value = String.Empty Or Value Is Nothing Then
                SetExtraInfo("docEntryID", DBNull.Value)
            Else
                SetExtraInfo("docEntryID", Value)
            End If
        End Set
    End Property


    Public Property ConversationTopic() As String
        Get
            If _data.Columns.Contains("docConversationTopic") Then
                Return Convert.ToString(GetExtraInfo("docConversationTopic"))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            If _data.Columns.Contains("docConversationTopic") Then
                If Value = String.Empty Or Value Is Nothing Then
                    SetExtraInfo("docConversationTopic", DBNull.Value)
                Else
                    If Value.Length > 250 Then
                        Value = Value.Substring(0, 250)
                    End If
                    SetExtraInfo("docConversationTopic", Value)
                End If
            End If
        End Set
    End Property

    Public Property ConversationIndex() As String
        Get
            If _data.Columns.Contains("docConversationIndex") Then
                Return Convert.ToString(GetExtraInfo("docConversationIndex"))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            If _data.Columns.Contains("docConversationIndex") Then
                If Value = String.Empty Or Value Is Nothing Then
                    SetExtraInfo("docConversationIndex", DBNull.Value)
                Else
                    If Value.Length > 250 Then
                        Value = Value.Substring(0, 250)
                    End If
                    SetExtraInfo("docConversationIndex", Value)
                End If
            End If
        End Set
    End Property

    Public Property FromName() As String
        Get
            Return Convert.ToString(GetExtraInfo("docFrom"))
        End Get
        Set(ByVal Value As String)
            If Value = String.Empty Or Value Is Nothing Then
                SetExtraInfo("docFrom", DBNull.Value)
            Else
                If Value.Length > 1000 Then
                    Value = Value.Substring(0, 1000)
                End If
                SetExtraInfo("docFrom", Value)
            End If
        End Set
    End Property

    Private fromemail As String
    Public Property From() As String
        Get
            Return fromemail
        End Get
        Set(ByVal Value As String)
            fromemail = Value
        End Set
    End Property

    Public Property [To]() As String
        Get
            Return Convert.ToString(GetExtraInfo("docTo"))
        End Get
        Set(ByVal Value As String)
            If Value = String.Empty Or Value Is Nothing Then
                SetExtraInfo("docTo", DBNull.Value)
            Else
                If Value.Length > 1000 Then
                    Value = Value.Substring(0, 1000)
                End If
                SetExtraInfo("docTo", Value)
            End If
        End Set
    End Property

    Public Property CC() As String
        Get
            Return Convert.ToString(GetExtraInfo("docCC"))
        End Get
        Set(ByVal Value As String)
            If Value = String.Empty Or Value Is Nothing Then
                SetExtraInfo("docCC", DBNull.Value)
            Else
                If Value.Length > 1000 Then
                    Value = Value.Substring(0, 1000)
                End If
                SetExtraInfo("docCC", Value)
            End If
        End Set
    End Property

    Public Property [Class]() As String
        Get
            Return Convert.ToString(GetExtraInfo("docClass"))
        End Get
        Set(ByVal Value As String)
            If Value = String.Empty Or Value Is Nothing Then
                SetExtraInfo("docClass", DBNull.Value)
            Else
                SetExtraInfo("docClass", Value)
            End If
        End Set
    End Property

    Public Property Attachments() As Integer
        Get
            Return Convert.ToInt32(GetExtraInfo("docAttachments"))
        End Get
        Set(ByVal Value As Integer)
            SetExtraInfo("docAttachments", Value)
        End Set
    End Property

    Public Property Sent() As DateTime
        Get
            Dim ret As Object = GetExtraInfo("docSent")
            If ret Is DBNull.Value Then
                Return OutlookOMS.MAX_DATE
            Else
                Return Convert.ToDateTime(ret).ToLocalTime()
            End If
        End Get
        Set(ByVal Value As DateTime)
            If Convert.ToDateTime(Value).ToLocalTime() = OutlookOMS.MAX_DATE Then
                SetExtraInfo("docSent", DBNull.Value)
            Else
                SetExtraInfo("docSent", Value)
            End If
        End Set
    End Property

    Public Property Received() As DateTime
        Get
            Dim ret As Object = GetExtraInfo("docReceived")
            If ret Is DBNull.Value Then
                Return OutlookOMS.MAX_DATE
            Else
                Return Convert.ToDateTime(ret).ToLocalTime()
            End If
        End Get
        Set(ByVal Value As DateTime)
            If Convert.ToDateTime(Value).ToLocalTime() = OutlookOMS.MAX_DATE Then
                SetExtraInfo("docReceived", DBNull.Value)
            Else
                SetExtraInfo("docReceived", Value)
            End If
        End Set
    End Property


#End Region
End Class