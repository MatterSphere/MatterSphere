using System;
using System.Runtime.InteropServices;
using FWBS.OMS;
namespace Fwbs.Oms.Office.Common
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IExternalOfficeOMSAddin
    {
        void BuildDataListTable(object obj, string code, bool includeHeader, string[] fieldNames, object[] values);
        void BuildSearchListTable(object obj, string code, bool includeHeader, string[] fieldNames, object[] values);
        BillInfo CreateBill(string category);
        ADODB.Recordset FetchDataList(string code, string[] fieldNames, object[] values);
        ADODB.Recordset FetchSearchList(string code, string[] fieldNames, object[] values);
        object GetDocVariable(string name);
        bool GetDocVariableB(string name, bool def);
        long GetDocVariableN(string name, long def);
        string GetDocVariableS(string name, string def);

        bool GetDocumentVariableB(object obj, string name, bool def);
        long GetDocumentVariableN(object obj, string name, long def);
        string GetDocumentVariableS(object obj, string name, string def);

        void SetDocVariableS(object obj, string varName, string val);
        void SetDocVariableL(object obj, string varName, long val);
        void SetDocVariableB(object obj, string varName, bool val);
        void RemoveDocVariable(object obj, string name);
        bool Online { get; }
        object Parse(string field, object defVal);
        void RefreshUI(object obj);
        object RunCommand(string command, object application);
        string RunScript(string commandName, string[] fieldNames, object[] values);
        IAssociate SelectAssociate();
        IFile GetFile(long fileId);
        IClient GetClient(long clientId);
        void AttachDocumentVars(object doc, long assocId);
        void AttachDocumentVariables(object doc, string clientNo, string fileNo);
        bool IsCompanyDocument(object doc);

        FWBS.OMS.Mappers.IMappingManager GetMappingManager();

        string GetSpecificData(string code);

        string GetCodeLookup(string type, string code);

        string GetResourceText(string code, string description, string help);
    }

}