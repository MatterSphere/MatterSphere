﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 

namespace RTAServicesLibraryV7
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class InterimSettlementPackRequest
    {

        private InterimSettlementPackRequestClaimantRepresentative claimantRepresentativeField;

        private InterimSettlementPackRequestMedicalReport medicalReportField;

        private InterimSettlementPackRequestInterimPayment interimPaymentField;

        private CT_A2A_ClaimantLosses_Interim[] claimantLossesField;

        private InterimSettlementPackRequestStatementOfTruth statementOfTruthField;

        /// <remarks/>
        public InterimSettlementPackRequestClaimantRepresentative ClaimantRepresentative
        {
            get
            {
                return this.claimantRepresentativeField;
            }
            set
            {
                this.claimantRepresentativeField = value;
            }
        }

        /// <remarks/>
        public InterimSettlementPackRequestMedicalReport MedicalReport
        {
            get
            {
                return this.medicalReportField;
            }
            set
            {
                this.medicalReportField = value;
            }
        }

        /// <remarks/>
        public InterimSettlementPackRequestInterimPayment InterimPayment
        {
            get
            {
                return this.interimPaymentField;
            }
            set
            {
                this.interimPaymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ClaimantLossesToDate", IsNullable = false)]
        public CT_A2A_ClaimantLosses_Interim[] ClaimantLosses
        {
            get
            {
                return this.claimantLossesField;
            }
            set
            {
                this.claimantLossesField = value;
            }
        }

        /// <remarks/>
        public InterimSettlementPackRequestStatementOfTruth StatementOfTruth
        {
            get
            {
                return this.statementOfTruthField;
            }
            set
            {
                this.statementOfTruthField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackRequestClaimantRepresentative
    {

        private CT_A2A_CompanyDetails companyDetailsField;

        private CT_A2A_MedCoCase medCoCaseField;

        /// <remarks/>
        public CT_A2A_CompanyDetails CompanyDetails
        {
            get
            {
                return this.companyDetailsField;
            }
            set
            {
                this.companyDetailsField = value;
            }
        }

        /// <remarks/>
        public CT_A2A_MedCoCase MedCoCase
        {
            get
            {
                return this.medCoCaseField;
            }
            set
            {
                this.medCoCaseField = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //public partial class CT_A2A_CompanyDetails
    //{

    //    private string contactNameField;

    //    private string contactMiddleNameField;

    //    private string contactSurnameField;

    //    private string emailAddressField;

    //    private string referenceNumberField;

    //    private string telephoneNumberField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string ContactName
    //    {
    //        get
    //        {
    //            return this.contactNameField;
    //        }
    //        set
    //        {
    //            this.contactNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string ContactMiddleName
    //    {
    //        get
    //        {
    //            return this.contactMiddleNameField;
    //        }
    //        set
    //        {
    //            this.contactMiddleNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string ContactSurname
    //    {
    //        get
    //        {
    //            return this.contactSurnameField;
    //        }
    //        set
    //        {
    //            this.contactSurnameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string EmailAddress
    //    {
    //        get
    //        {
    //            return this.emailAddressField;
    //        }
    //        set
    //        {
    //            this.emailAddressField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string ReferenceNumber
    //    {
    //        get
    //        {
    //            return this.referenceNumberField;
    //        }
    //        set
    //        {
    //            this.referenceNumberField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string TelephoneNumber
    //    {
    //        get
    //        {
    //            return this.telephoneNumberField;
    //        }
    //        set
    //        {
    //            this.telephoneNumberField = value;
    //        }
    //    }
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_ClaimantLosses_Interim
    {

        private C18_LossType_R7 lossTypeField;

        private C00_YNFlag evidenceAttachedField;

        private string commentsField;

        private decimal grossValueClaimedField;

        private decimal percContribNegDeductionsField;

        private C00_YNFlag itemBeingPursuedField;

        private bool itemBeingPursuedFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C18_LossType_R7 LossType
        {
            get
            {
                return this.lossTypeField;
            }
            set
            {
                this.lossTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag EvidenceAttached
        {
            get
            {
                return this.evidenceAttachedField;
            }
            set
            {
                this.evidenceAttachedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string Comments
        {
            get
            {
                return this.commentsField;
            }
            set
            {
                this.commentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public decimal GrossValueClaimed
        {
            get
            {
                return this.grossValueClaimedField;
            }
            set
            {
                this.grossValueClaimedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public decimal PercContribNegDeductions
        {
            get
            {
                return this.percContribNegDeductionsField;
            }
            set
            {
                this.percContribNegDeductionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag ItemBeingPursued
        {
            get
            {
                return this.itemBeingPursuedField;
            }
            set
            {
                this.itemBeingPursuedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ItemBeingPursuedSpecified
        {
            get
            {
                return this.itemBeingPursuedFieldSpecified;
            }
            set
            {
                this.itemBeingPursuedFieldSpecified = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C18_LossType_R7
    //{

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("0")]
    //    Item0,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("1")]
    //    Item1,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("2")]
    //    Item2,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("3")]
    //    Item3,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("4")]
    //    Item4,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("5")]
    //    Item5,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("6")]
    //    Item6,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("7")]
    //    Item7,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("8")]
    //    Item8,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("9")]
    //    Item9,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("10")]
    //    Item10,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("11")]
    //    Item11,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("12")]
    //    Item12,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("13")]
    //    Item13,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("14")]
    //    Item14,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("15")]
    //    Item15,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("16")]
    //    Item16,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C00_YNFlag
    //{

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("1")]
    //    Item1,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("0")]
    //    Item0,
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_ClaimantRequestForInterimPayment
    {

        private string reasonsForInterimPaymentRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ReasonsForInterimPaymentRequest
        {
            get
            {
                return this.reasonsForInterimPaymentRequestField;
            }
            set
            {
                this.reasonsForInterimPaymentRequestField = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //public partial class CT_A2A_MedCoCase
    //{

    //    private C00_YNFlag softTissueField;

    //    private string medCoCaseIDField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C00_YNFlag SoftTissue
    //    {
    //        get
    //        {
    //            return this.softTissueField;
    //        }
    //        set
    //        {
    //            this.softTissueField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string MedCoCaseID
    //    {
    //        get
    //        {
    //            return this.medCoCaseIDField;
    //        }
    //        set
    //        {
    //            this.medCoCaseIDField = value;
    //        }
    //    }
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackRequestMedicalReport
    {

        private C22_MedicalReport medicalReportStage2_1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C22_MedicalReport MedicalReportStage2_1
        {
            get
            {
                return this.medicalReportStage2_1Field;
            }
            set
            {
                this.medicalReportStage2_1Field = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C22_MedicalReport
    //{

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("0")]
    //    Item0,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("1")]
    //    Item1,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("2")]
    //    Item2,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("3")]
    //    Item3,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("4")]
    //    Item4,
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackRequestInterimPayment
    {

        private CT_A2A_ClaimantRequestForInterimPayment claimantRequestForInterimPaymentField;

        /// <remarks/>
        public CT_A2A_ClaimantRequestForInterimPayment ClaimantRequestForInterimPayment
        {
            get
            {
                return this.claimantRequestForInterimPaymentField;
            }
            set
            {
                this.claimantRequestForInterimPaymentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackRequestStatementOfTruth
    {

        private C00_YNFlag retainedSignedCopyField;

        private C16_SignatoryType signatoryTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag RetainedSignedCopy
        {
            get
            {
                return this.retainedSignedCopyField;
            }
            set
            {
                this.retainedSignedCopyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C16_SignatoryType SignatoryType
        {
            get
            {
                return this.signatoryTypeField;
            }
            set
            {
                this.signatoryTypeField = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C16_SignatoryType
    //{

    //    /// <remarks/>
    //    S,

    //    /// <remarks/>
    //    C,
    //}
}
