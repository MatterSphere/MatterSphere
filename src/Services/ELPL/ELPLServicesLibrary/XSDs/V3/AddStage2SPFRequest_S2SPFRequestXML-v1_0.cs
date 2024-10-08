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
    public partial class Stage2SettlementPackRequest
    {

        private Stage2SettlementPackRequestClaimantRepresentative claimantRepresentativeField;

        private Stage2SettlementPackRequestMedicalReport medicalReportField;

        private CT_A2A_ClaimantLosses_ELPL[] claimantLossesField;

        private Stage2SettlementPackRequestStatementOfTruth statementOfTruthField;

        private Stage2SettlementPackRequestAgreementData agreementDataField;

        /// <remarks/>
        public Stage2SettlementPackRequestClaimantRepresentative ClaimantRepresentative
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
        public Stage2SettlementPackRequestMedicalReport MedicalReport
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
        [System.Xml.Serialization.XmlArrayItemAttribute("CurrentClaimantOffer", IsNullable = false)]
        public CT_A2A_ClaimantLosses_ELPL[] ClaimantLosses
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
        public Stage2SettlementPackRequestStatementOfTruth StatementOfTruth
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

        /// <remarks/>
        public Stage2SettlementPackRequestAgreementData AgreementData
        {
            get
            {
                return this.agreementDataField;
            }
            set
            {
                this.agreementDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackRequestClaimantRepresentative
    {

        private string contactNameField;

        private string contactMiddleNameField;

        private string contactSurnameField;

        private string emailAddressField;

        private string referenceNumberField;

        private string telephoneNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ContactName
        {
            get
            {
                return this.contactNameField;
            }
            set
            {
                this.contactNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ContactMiddleName
        {
            get
            {
                return this.contactMiddleNameField;
            }
            set
            {
                this.contactMiddleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ContactSurname
        {
            get
            {
                return this.contactSurnameField;
            }
            set
            {
                this.contactSurnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ReferenceNumber
        {
            get
            {
                return this.referenceNumberField;
            }
            set
            {
                this.referenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string TelephoneNumber
        {
            get
            {
                return this.telephoneNumberField;
            }
            set
            {
                this.telephoneNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackRequestMedicalReport
    {

        private C22_MedicalReport medicalReportStage2Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C22_MedicalReport MedicalReportStage2
        {
            get
            {
                return this.medicalReportStage2Field;
            }
            set
            {
                this.medicalReportStage2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackRequestStatementOfTruth
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackRequestAgreementData
    {

        private string commentsField;

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
    }
}