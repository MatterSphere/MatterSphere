﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;


// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 

namespace ELPLServicesLibrary
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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

        private CT_A2A_ClaimantLosses_Interim_ELPL[] claimantLossesField;

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
        public CT_A2A_ClaimantLosses_Interim_ELPL[] ClaimantLosses
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackRequestClaimantRepresentative
    {

        private CT_A2A_CompanyDetails companyDetailsField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_ClaimantLosses_Interim_ELPL
    {

        private C18_LossType_ELPL lossTypeField;

        private C00_YNFlag evidenceAttachedField;

        private string commentsField;

        private decimal grossValueClaimedField;

        private C00_YNFlag itemBeingPursuedField;

        private bool itemBeingPursuedFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C18_LossType_ELPL LossType
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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
}