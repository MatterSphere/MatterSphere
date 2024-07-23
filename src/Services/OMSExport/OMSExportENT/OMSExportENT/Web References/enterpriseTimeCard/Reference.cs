﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.269.
// 
#pragma warning disable 1591

namespace FWBS.enterpriseTimeCard {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="TimeCardSoap", Namespace="http://www.elite.com/openapi")]
    public partial class TimeCard : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private user userValueField;
        
        private System.Threading.SendOrPostCallback CreateOperationCompleted;
        
        private System.Threading.SendOrPostCallback EraseByWhereOperationCompleted;
        
        private System.Threading.SendOrPostCallback Create2OperationCompleted;
        
        private System.Threading.SendOrPostCallback Create3OperationCompleted;
        
        private System.Threading.SendOrPostCallback Create1OperationCompleted;
        
        private System.Threading.SendOrPostCallback Create4OperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public TimeCard() {
            this.Url = global::FWBS.Properties.Settings.Default.OMSExportENT_enterpriseTimeCard_TimeCard;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public user userValue {
            get {
                return this.userValueField;
            }
            set {
                this.userValueField = value;
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
        public event CreateCompletedEventHandler CreateCompleted;
        
        /// <remarks/>
        public event EraseByWhereCompletedEventHandler EraseByWhereCompleted;
        
        /// <remarks/>
        public event Create2CompletedEventHandler Create2Completed;
        
        /// <remarks/>
        public event Create3CompletedEventHandler Create3Completed;
        
        /// <remarks/>
        public event Create1CompletedEventHandler Create1Completed;
        
        /// <remarks/>
        public event Create4CompletedEventHandler Create4Completed;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("userValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.elite.com/openapi/Create", RequestNamespace="http://www.elite.com/openapi", ResponseNamespace="http://www.elite.com/openapi", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Create(string date, string timeKeeperInitials, string clientName, string matterNumber, string billStatus, string amount, string ledgerCode, string primaryActivityCode, string udf1, string udf2, string udf3, string udf4, string udf5, string description, string invocationType) {
            this.Invoke("Create", new object[] {
                        date,
                        timeKeeperInitials,
                        clientName,
                        matterNumber,
                        billStatus,
                        amount,
                        ledgerCode,
                        primaryActivityCode,
                        udf1,
                        udf2,
                        udf3,
                        udf4,
                        udf5,
                        description,
                        invocationType});
        }
        
        /// <remarks/>
        public void CreateAsync(string date, string timeKeeperInitials, string clientName, string matterNumber, string billStatus, string amount, string ledgerCode, string primaryActivityCode, string udf1, string udf2, string udf3, string udf4, string udf5, string description, string invocationType) {
            this.CreateAsync(date, timeKeeperInitials, clientName, matterNumber, billStatus, amount, ledgerCode, primaryActivityCode, udf1, udf2, udf3, udf4, udf5, description, invocationType, null);
        }
        
        /// <remarks/>
        public void CreateAsync(
                    string date, 
                    string timeKeeperInitials, 
                    string clientName, 
                    string matterNumber, 
                    string billStatus, 
                    string amount, 
                    string ledgerCode, 
                    string primaryActivityCode, 
                    string udf1, 
                    string udf2, 
                    string udf3, 
                    string udf4, 
                    string udf5, 
                    string description, 
                    string invocationType, 
                    object userState) {
            if ((this.CreateOperationCompleted == null)) {
                this.CreateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateOperationCompleted);
            }
            this.InvokeAsync("Create", new object[] {
                        date,
                        timeKeeperInitials,
                        clientName,
                        matterNumber,
                        billStatus,
                        amount,
                        ledgerCode,
                        primaryActivityCode,
                        udf1,
                        udf2,
                        udf3,
                        udf4,
                        udf5,
                        description,
                        invocationType}, this.CreateOperationCompleted, userState);
        }
        
        private void OnCreateOperationCompleted(object arg) {
            if ((this.CreateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("userValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.elite.com/openapi/EraseByWhere", RequestNamespace="http://www.elite.com/openapi", ResponseNamespace="http://www.elite.com/openapi", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string EraseByWhere(string where, string reason) {
            object[] results = this.Invoke("EraseByWhere", new object[] {
                        where,
                        reason});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void EraseByWhereAsync(string where, string reason) {
            this.EraseByWhereAsync(where, reason, null);
        }
        
        /// <remarks/>
        public void EraseByWhereAsync(string where, string reason, object userState) {
            if ((this.EraseByWhereOperationCompleted == null)) {
                this.EraseByWhereOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEraseByWhereOperationCompleted);
            }
            this.InvokeAsync("EraseByWhere", new object[] {
                        where,
                        reason}, this.EraseByWhereOperationCompleted, userState);
        }
        
        private void OnEraseByWhereOperationCompleted(object arg) {
            if ((this.EraseByWhereCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EraseByWhereCompleted(this, new EraseByWhereCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("userValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.elite.com/openapi/Create2", RequestNamespace="http://www.elite.com/openapi", ResponseNamespace="http://www.elite.com/openapi", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Create2(TimeLoadFormat[] time, string userID, string batchNumber, string batchPeriod) {
            object[] results = this.Invoke("Create2", new object[] {
                        time,
                        userID,
                        batchNumber,
                        batchPeriod});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void Create2Async(TimeLoadFormat[] time, string userID, string batchNumber, string batchPeriod) {
            this.Create2Async(time, userID, batchNumber, batchPeriod, null);
        }
        
        /// <remarks/>
        public void Create2Async(TimeLoadFormat[] time, string userID, string batchNumber, string batchPeriod, object userState) {
            if ((this.Create2OperationCompleted == null)) {
                this.Create2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreate2OperationCompleted);
            }
            this.InvokeAsync("Create2", new object[] {
                        time,
                        userID,
                        batchNumber,
                        batchPeriod}, this.Create2OperationCompleted, userState);
        }
        
        private void OnCreate2OperationCompleted(object arg) {
            if ((this.Create2Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Create2Completed(this, new Create2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("userValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.elite.com/openapi/Create3", RequestNamespace="http://www.elite.com/openapi", ResponseNamespace="http://www.elite.com/openapi", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Create3(TimeLoadFormat[] time, string userID) {
            object[] results = this.Invoke("Create3", new object[] {
                        time,
                        userID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void Create3Async(TimeLoadFormat[] time, string userID) {
            this.Create3Async(time, userID, null);
        }
        
        /// <remarks/>
        public void Create3Async(TimeLoadFormat[] time, string userID, object userState) {
            if ((this.Create3OperationCompleted == null)) {
                this.Create3OperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreate3OperationCompleted);
            }
            this.InvokeAsync("Create3", new object[] {
                        time,
                        userID}, this.Create3OperationCompleted, userState);
        }
        
        private void OnCreate3OperationCompleted(object arg) {
            if ((this.Create3Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Create3Completed(this, new Create3CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("userValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.elite.com/openapi/Create1", RequestNamespace="http://www.elite.com/openapi", ResponseNamespace="http://www.elite.com/openapi", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TimeLoadResult Create1(TimeLoadFormat1 time, string userID, string source, string invalidTime) {
            object[] results = this.Invoke("Create1", new object[] {
                        time,
                        userID,
                        source,
                        invalidTime});
            return ((TimeLoadResult)(results[0]));
        }
        
        /// <remarks/>
        public void Create1Async(TimeLoadFormat1 time, string userID, string source, string invalidTime) {
            this.Create1Async(time, userID, source, invalidTime, null);
        }
        
        /// <remarks/>
        public void Create1Async(TimeLoadFormat1 time, string userID, string source, string invalidTime, object userState) {
            if ((this.Create1OperationCompleted == null)) {
                this.Create1OperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreate1OperationCompleted);
            }
            this.InvokeAsync("Create1", new object[] {
                        time,
                        userID,
                        source,
                        invalidTime}, this.Create1OperationCompleted, userState);
        }
        
        private void OnCreate1OperationCompleted(object arg) {
            if ((this.Create1Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Create1Completed(this, new Create1CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("userValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.elite.com/openapi/Create4", RequestNamespace="http://www.elite.com/openapi", ResponseNamespace="http://www.elite.com/openapi", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Create4(
                    string date, 
                    string timeKeeperInitials, 
                    string clientName, 
                    string matterNumber, 
                    string billStatus, 
                    string hour, 
                    string rate, 
                    string amount, 
                    string ledgerCode, 
                    string primaryActivityCode, 
                    string udf1, 
                    string udf2, 
                    string udf3, 
                    string udf4, 
                    string udf5, 
                    string description, 
                    string source, 
                    string invalidTime) {
            object[] results = this.Invoke("Create4", new object[] {
                        date,
                        timeKeeperInitials,
                        clientName,
                        matterNumber,
                        billStatus,
                        hour,
                        rate,
                        amount,
                        ledgerCode,
                        primaryActivityCode,
                        udf1,
                        udf2,
                        udf3,
                        udf4,
                        udf5,
                        description,
                        source,
                        invalidTime});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void Create4Async(
                    string date, 
                    string timeKeeperInitials, 
                    string clientName, 
                    string matterNumber, 
                    string billStatus, 
                    string hour, 
                    string rate, 
                    string amount, 
                    string ledgerCode, 
                    string primaryActivityCode, 
                    string udf1, 
                    string udf2, 
                    string udf3, 
                    string udf4, 
                    string udf5, 
                    string description, 
                    string source, 
                    string invalidTime) {
            this.Create4Async(date, timeKeeperInitials, clientName, matterNumber, billStatus, hour, rate, amount, ledgerCode, primaryActivityCode, udf1, udf2, udf3, udf4, udf5, description, source, invalidTime, null);
        }
        
        /// <remarks/>
        public void Create4Async(
                    string date, 
                    string timeKeeperInitials, 
                    string clientName, 
                    string matterNumber, 
                    string billStatus, 
                    string hour, 
                    string rate, 
                    string amount, 
                    string ledgerCode, 
                    string primaryActivityCode, 
                    string udf1, 
                    string udf2, 
                    string udf3, 
                    string udf4, 
                    string udf5, 
                    string description, 
                    string source, 
                    string invalidTime, 
                    object userState) {
            if ((this.Create4OperationCompleted == null)) {
                this.Create4OperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreate4OperationCompleted);
            }
            this.InvokeAsync("Create4", new object[] {
                        date,
                        timeKeeperInitials,
                        clientName,
                        matterNumber,
                        billStatus,
                        hour,
                        rate,
                        amount,
                        ledgerCode,
                        primaryActivityCode,
                        udf1,
                        udf2,
                        udf3,
                        udf4,
                        udf5,
                        description,
                        source,
                        invalidTime}, this.Create4OperationCompleted, userState);
        }
        
        private void OnCreate4OperationCompleted(object arg) {
            if ((this.Create4Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Create4Completed(this, new Create4CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.elite.com/openapi")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.elite.com/openapi", IsNullable=false)]
    public partial class user : System.Web.Services.Protocols.SoapHeader {
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        private string[] textField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.elite.com/openapi")]
    public partial class TimeLoadResult {
        
        private string resultField;
        
        private string timecard_tindexField;
        
        private string wv_timecard_tindexField;
        
        private string errorField;
        
        private string warningField;
        
        private string reportField;
        
        /// <remarks/>
        public string result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
            }
        }
        
        /// <remarks/>
        public string timecard_tindex {
            get {
                return this.timecard_tindexField;
            }
            set {
                this.timecard_tindexField = value;
            }
        }
        
        /// <remarks/>
        public string wv_timecard_tindex {
            get {
                return this.wv_timecard_tindexField;
            }
            set {
                this.wv_timecard_tindexField = value;
            }
        }
        
        /// <remarks/>
        public string error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
        
        /// <remarks/>
        public string warning {
            get {
                return this.warningField;
            }
            set {
                this.warningField = value;
            }
        }
        
        /// <remarks/>
        public string report {
            get {
                return this.reportField;
            }
            set {
                this.reportField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.elite.com/openapi")]
    public partial class TimeLoadFormat1 {
        
        private string dateField;
        
        private string timeKeeperInitialsField;
        
        private string clientNameField;
        
        private string matterNumberField;
        
        private string billStatusField;
        
        private string hourField;
        
        private string rateField;
        
        private string amountField;
        
        private string ledgerCodeField;
        
        private string primaryActivityCodeField;
        
        private string udf1Field;
        
        private string udf2Field;
        
        private string udf3Field;
        
        private string udf4Field;
        
        private string udf5Field;
        
        private string descriptionField;
        
        /// <remarks/>
        public string date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        public string timeKeeperInitials {
            get {
                return this.timeKeeperInitialsField;
            }
            set {
                this.timeKeeperInitialsField = value;
            }
        }
        
        /// <remarks/>
        public string clientName {
            get {
                return this.clientNameField;
            }
            set {
                this.clientNameField = value;
            }
        }
        
        /// <remarks/>
        public string matterNumber {
            get {
                return this.matterNumberField;
            }
            set {
                this.matterNumberField = value;
            }
        }
        
        /// <remarks/>
        public string billStatus {
            get {
                return this.billStatusField;
            }
            set {
                this.billStatusField = value;
            }
        }
        
        /// <remarks/>
        public string hour {
            get {
                return this.hourField;
            }
            set {
                this.hourField = value;
            }
        }
        
        /// <remarks/>
        public string rate {
            get {
                return this.rateField;
            }
            set {
                this.rateField = value;
            }
        }
        
        /// <remarks/>
        public string amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string ledgerCode {
            get {
                return this.ledgerCodeField;
            }
            set {
                this.ledgerCodeField = value;
            }
        }
        
        /// <remarks/>
        public string primaryActivityCode {
            get {
                return this.primaryActivityCodeField;
            }
            set {
                this.primaryActivityCodeField = value;
            }
        }
        
        /// <remarks/>
        public string udf1 {
            get {
                return this.udf1Field;
            }
            set {
                this.udf1Field = value;
            }
        }
        
        /// <remarks/>
        public string udf2 {
            get {
                return this.udf2Field;
            }
            set {
                this.udf2Field = value;
            }
        }
        
        /// <remarks/>
        public string udf3 {
            get {
                return this.udf3Field;
            }
            set {
                this.udf3Field = value;
            }
        }
        
        /// <remarks/>
        public string udf4 {
            get {
                return this.udf4Field;
            }
            set {
                this.udf4Field = value;
            }
        }
        
        /// <remarks/>
        public string udf5 {
            get {
                return this.udf5Field;
            }
            set {
                this.udf5Field = value;
            }
        }
        
        /// <remarks/>
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.elite.com/openapi")]
    public partial class TimeLoadFormat {
        
        private string dateField;
        
        private string timeKeeperInitialsField;
        
        private string clientNameField;
        
        private string matterNumberField;
        
        private string billStatusField;
        
        private string amountField;
        
        private string ledgerCodeField;
        
        private string primaryActivityCodeField;
        
        private string udf1Field;
        
        private string udf2Field;
        
        private string udf3Field;
        
        private string udf4Field;
        
        private string udf5Field;
        
        private string descriptionField;
        
        /// <remarks/>
        public string date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        public string timeKeeperInitials {
            get {
                return this.timeKeeperInitialsField;
            }
            set {
                this.timeKeeperInitialsField = value;
            }
        }
        
        /// <remarks/>
        public string clientName {
            get {
                return this.clientNameField;
            }
            set {
                this.clientNameField = value;
            }
        }
        
        /// <remarks/>
        public string matterNumber {
            get {
                return this.matterNumberField;
            }
            set {
                this.matterNumberField = value;
            }
        }
        
        /// <remarks/>
        public string billStatus {
            get {
                return this.billStatusField;
            }
            set {
                this.billStatusField = value;
            }
        }
        
        /// <remarks/>
        public string amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string ledgerCode {
            get {
                return this.ledgerCodeField;
            }
            set {
                this.ledgerCodeField = value;
            }
        }
        
        /// <remarks/>
        public string primaryActivityCode {
            get {
                return this.primaryActivityCodeField;
            }
            set {
                this.primaryActivityCodeField = value;
            }
        }
        
        /// <remarks/>
        public string udf1 {
            get {
                return this.udf1Field;
            }
            set {
                this.udf1Field = value;
            }
        }
        
        /// <remarks/>
        public string udf2 {
            get {
                return this.udf2Field;
            }
            set {
                this.udf2Field = value;
            }
        }
        
        /// <remarks/>
        public string udf3 {
            get {
                return this.udf3Field;
            }
            set {
                this.udf3Field = value;
            }
        }
        
        /// <remarks/>
        public string udf4 {
            get {
                return this.udf4Field;
            }
            set {
                this.udf4Field = value;
            }
        }
        
        /// <remarks/>
        public string udf5 {
            get {
                return this.udf5Field;
            }
            set {
                this.udf5Field = value;
            }
        }
        
        /// <remarks/>
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CreateCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void EraseByWhereCompletedEventHandler(object sender, EraseByWhereCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EraseByWhereCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EraseByWhereCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Create2CompletedEventHandler(object sender, Create2CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Create2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Create2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Create3CompletedEventHandler(object sender, Create3CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Create3CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Create3CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Create1CompletedEventHandler(object sender, Create1CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Create1CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Create1CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TimeLoadResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TimeLoadResult)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Create4CompletedEventHandler(object sender, Create4CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Create4CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Create4CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591