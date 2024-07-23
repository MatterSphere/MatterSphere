Imports FWBS.Office.Outlook

Friend Interface IProfileItem

    Sub BeforeDelete(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs)
    Sub BeforeReply(ByVal appcontroller As OutlookOMS, ByVal args As BeforeReplyItemEventArgs)
    Sub BeforeForward(ByVal appcontroller As OutlookOMS, ByVal args As BeforeForwardItemEventArgs)
    Sub BeforeClose(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs)
    Sub BeforeOpen(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs)
    Sub BeforeSend(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs)
    Sub BeforeActivate(ByVal appcontroller As OutlookOMS, ByVal args As BeforeItemEventArgs)

    Sub Refresh(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem)

    Function BeginSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal settings As SaveSettings, ByRef status As DocSaveStatus) As Boolean
    Function BeforeDocumentSave(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal doc As OMSDocument, ByVal version As FWBS.OMS.DocumentManagement.DocumentVersion) As Boolean

    Function GetDocumentDirection(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal prec As Precedent) As DocumentDirection
    Function GetDisplayText(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String

    Function GetDocKey(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByRef key() As String) As Boolean
    Function SetDocKey(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem, ByVal key As String) As Boolean

    Function GetDefaultDocType(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String
    Function GenerateChecksum(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As String
    Function CanSaveAsDocument(ByVal appcontroller As OutlookOMS, ByVal item As OutlookItem) As Boolean


End Interface
