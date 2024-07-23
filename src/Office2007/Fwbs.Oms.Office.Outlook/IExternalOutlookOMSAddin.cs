using System;
using System.Runtime.InteropServices;
using Fwbs.Oms.Office.Common;

namespace Fwbs.Oms.Office.Outlook
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IExternalOutlookOMSAddin
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
        bool Online { get; }
        object Parse(string field, object defVal);
        void RefreshUI(object obj);
        object RunCommand(string command, object application);


        ucFolderPage GetFolderHomePage();
        void WinFormsControlCleanUp();
    }
}
