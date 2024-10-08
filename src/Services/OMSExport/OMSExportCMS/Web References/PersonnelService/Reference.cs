﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.237.
// 
#pragma warning disable 1591

namespace FWBS.OMS.OMSEXPORT.PersonnelService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PersonnelServiceSoap", Namespace="http://cmsopen.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonnelDataBase))]
    public partial class PersonnelService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private CioSoapHeader cioSoapHeaderValueField;
        
        private System.Threading.SendOrPostCallback ReadSingleOperationCompleted;
        
        private System.Threading.SendOrPostCallback ReadOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PersonnelService() {
            this.Url = "http://vpc-aderant/CMSNet//FileOpening/FileOpeningWS/PersonnelService.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public CioSoapHeader CioSoapHeaderValue {
            get {
                return this.cioSoapHeaderValueField;
            }
            set {
                this.cioSoapHeaderValueField = value;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ReadSingleCompletedEventHandler ReadSingleCompleted;
        
        /// <remarks/>
        public event ReadCompletedEventHandler ReadCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("CioSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cmsopen.com/ReadSingle", RequestNamespace="http://cmsopen.com/", ResponseNamespace="http://cmsopen.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public PersonnelData ReadSingle(int employeeUno) {
            object[] results = this.Invoke("ReadSingle", new object[] {
                        employeeUno});
            return ((PersonnelData)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginReadSingle(int employeeUno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ReadSingle", new object[] {
                        employeeUno}, callback, asyncState);
        }
        
        /// <remarks/>
        public PersonnelData EndReadSingle(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((PersonnelData)(results[0]));
        }
        
        /// <remarks/>
        public void ReadSingleAsync(int employeeUno) {
            this.ReadSingleAsync(employeeUno, null);
        }
        
        /// <remarks/>
        public void ReadSingleAsync(int employeeUno, object userState) {
            if ((this.ReadSingleOperationCompleted == null)) {
                this.ReadSingleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReadSingleOperationCompleted);
            }
            this.InvokeAsync("ReadSingle", new object[] {
                        employeeUno}, this.ReadSingleOperationCompleted, userState);
        }
        
        private void OnReadSingleOperationCompleted(object arg) {
            if ((this.ReadSingleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReadSingleCompleted(this, new ReadSingleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("CioSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cmsopen.com/Read", RequestNamespace="http://cmsopen.com/", ResponseNamespace="http://cmsopen.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public PersonnelData[] Read(string filter) {
            object[] results = this.Invoke("Read", new object[] {
                        filter});
            return ((PersonnelData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRead(string filter, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Read", new object[] {
                        filter}, callback, asyncState);
        }
        
        /// <remarks/>
        public PersonnelData[] EndRead(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((PersonnelData[])(results[0]));
        }
        
        /// <remarks/>
        public void ReadAsync(string filter) {
            this.ReadAsync(filter, null);
        }
        
        /// <remarks/>
        public void ReadAsync(string filter, object userState) {
            if ((this.ReadOperationCompleted == null)) {
                this.ReadOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReadOperationCompleted);
            }
            this.InvokeAsync("Read", new object[] {
                        filter}, this.ReadOperationCompleted, userState);
        }
        
        private void OnReadOperationCompleted(object arg) {
            if ((this.ReadCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReadCompleted(this, new ReadCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cmsopen.com/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cmsopen.com/", IsNullable=false)]
    public partial class CioSoapHeader : System.Web.Services.Protocols.SoapHeader {
        
        private string localeField;
        
        private string formsTicketField;
        
        private string sessionField;
        
        private bool useOfficeXtensionField;
        
        private string satelliteDbTargetField;
        
        private bool satelliteDbOnlyField;
        
        /// <remarks/>
        public string locale {
            get {
                return this.localeField;
            }
            set {
                this.localeField = value;
            }
        }
        
        /// <remarks/>
        public string formsTicket {
            get {
                return this.formsTicketField;
            }
            set {
                this.formsTicketField = value;
            }
        }
        
        /// <remarks/>
        public string session {
            get {
                return this.sessionField;
            }
            set {
                this.sessionField = value;
            }
        }
        
        /// <remarks/>
        public bool useOfficeXtension {
            get {
                return this.useOfficeXtensionField;
            }
            set {
                this.useOfficeXtensionField = value;
            }
        }
        
        /// <remarks/>
        public string satelliteDbTarget {
            get {
                return this.satelliteDbTargetField;
            }
            set {
                this.satelliteDbTargetField = value;
            }
        }
        
        /// <remarks/>
        public bool satelliteDbOnly {
            get {
                return this.satelliteDbOnlyField;
            }
            set {
                this.satelliteDbOnlyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonnelData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cmsopen.com/")]
    public partial class PersonnelDataBase {
        
        private int appGroupUnoField;
        
        private int compYearField;
        
        private string currencyCodeField;
        
        private string deptField;
        
        private string deEdListReqdField;
        
        private System.DateTime editDateField;
        
        private string employeeCodeField;
        
        private string employeeNameField;
        
        private int emplUnoField;
        
        private int gradYearField;
        
        private System.DateTime hireDateField;
        
        private string hldyGroupCodeField;
        
        private string inactiveField;
        
        private string initialsField;
        
        private string internalNumField;
        
        private System.DateTime lastModifiedField;
        
        private string locationField;
        
        private string loginField;
        
        private System.DateTime modEffDateField;
        
        private int nameUnoField;
        
        private string offcField;
        
        private string persnlTypCodeField;
        
        private string phoneNoField;
        
        private string positionField;
        
        private System.DateTime prevHireDateField;
        
        private System.DateTime prevTermDateField;
        
        private string profField;
        
        private int sortPosField;
        
        private int supervEmplUnoField;
        
        private System.DateTime terminateDateField;
        
        private string workTypeCodeField;
        
        /// <remarks/>
        public int AppGroupUno {
            get {
                return this.appGroupUnoField;
            }
            set {
                this.appGroupUnoField = value;
            }
        }
        
        /// <remarks/>
        public int CompYear {
            get {
                return this.compYearField;
            }
            set {
                this.compYearField = value;
            }
        }
        
        /// <remarks/>
        public string CurrencyCode {
            get {
                return this.currencyCodeField;
            }
            set {
                this.currencyCodeField = value;
            }
        }
        
        /// <remarks/>
        public string Dept {
            get {
                return this.deptField;
            }
            set {
                this.deptField = value;
            }
        }
        
        /// <remarks/>
        public string DeEdListReqd {
            get {
                return this.deEdListReqdField;
            }
            set {
                this.deEdListReqdField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EditDate {
            get {
                return this.editDateField;
            }
            set {
                this.editDateField = value;
            }
        }
        
        /// <remarks/>
        public string EmployeeCode {
            get {
                return this.employeeCodeField;
            }
            set {
                this.employeeCodeField = value;
            }
        }
        
        /// <remarks/>
        public string EmployeeName {
            get {
                return this.employeeNameField;
            }
            set {
                this.employeeNameField = value;
            }
        }
        
        /// <remarks/>
        public int EmplUno {
            get {
                return this.emplUnoField;
            }
            set {
                this.emplUnoField = value;
            }
        }
        
        /// <remarks/>
        public int GradYear {
            get {
                return this.gradYearField;
            }
            set {
                this.gradYearField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime HireDate {
            get {
                return this.hireDateField;
            }
            set {
                this.hireDateField = value;
            }
        }
        
        /// <remarks/>
        public string HldyGroupCode {
            get {
                return this.hldyGroupCodeField;
            }
            set {
                this.hldyGroupCodeField = value;
            }
        }
        
        /// <remarks/>
        public string Inactive {
            get {
                return this.inactiveField;
            }
            set {
                this.inactiveField = value;
            }
        }
        
        /// <remarks/>
        public string Initials {
            get {
                return this.initialsField;
            }
            set {
                this.initialsField = value;
            }
        }
        
        /// <remarks/>
        public string InternalNum {
            get {
                return this.internalNumField;
            }
            set {
                this.internalNumField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime LastModified {
            get {
                return this.lastModifiedField;
            }
            set {
                this.lastModifiedField = value;
            }
        }
        
        /// <remarks/>
        public string Location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
        
        /// <remarks/>
        public string Login {
            get {
                return this.loginField;
            }
            set {
                this.loginField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ModEffDate {
            get {
                return this.modEffDateField;
            }
            set {
                this.modEffDateField = value;
            }
        }
        
        /// <remarks/>
        public int NameUno {
            get {
                return this.nameUnoField;
            }
            set {
                this.nameUnoField = value;
            }
        }
        
        /// <remarks/>
        public string Offc {
            get {
                return this.offcField;
            }
            set {
                this.offcField = value;
            }
        }
        
        /// <remarks/>
        public string PersnlTypCode {
            get {
                return this.persnlTypCodeField;
            }
            set {
                this.persnlTypCodeField = value;
            }
        }
        
        /// <remarks/>
        public string PhoneNo {
            get {
                return this.phoneNoField;
            }
            set {
                this.phoneNoField = value;
            }
        }
        
        /// <remarks/>
        public string Position {
            get {
                return this.positionField;
            }
            set {
                this.positionField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime PrevHireDate {
            get {
                return this.prevHireDateField;
            }
            set {
                this.prevHireDateField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime PrevTermDate {
            get {
                return this.prevTermDateField;
            }
            set {
                this.prevTermDateField = value;
            }
        }
        
        /// <remarks/>
        public string Prof {
            get {
                return this.profField;
            }
            set {
                this.profField = value;
            }
        }
        
        /// <remarks/>
        public int SortPos {
            get {
                return this.sortPosField;
            }
            set {
                this.sortPosField = value;
            }
        }
        
        /// <remarks/>
        public int SupervEmplUno {
            get {
                return this.supervEmplUnoField;
            }
            set {
                this.supervEmplUnoField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime TerminateDate {
            get {
                return this.terminateDateField;
            }
            set {
                this.terminateDateField = value;
            }
        }
        
        /// <remarks/>
        public string WorkTypeCode {
            get {
                return this.workTypeCodeField;
            }
            set {
                this.workTypeCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cmsopen.com/")]
    public partial class PersonnelData : PersonnelDataBase {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ReadSingleCompletedEventHandler(object sender, ReadSingleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReadSingleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReadSingleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public PersonnelData Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((PersonnelData)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ReadCompletedEventHandler(object sender, ReadCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReadCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReadCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public PersonnelData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((PersonnelData[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591