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
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 

namespace ELPLServicesLibraryV5
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Stage2SettlementPackResponse
    {

        private Stage2SettlementPackResponseDefendantRepresentative defendantRepresentativeField;

        private CT_A2A_CurrentDefendantResponse_ELPL[] defendantRepliesField;

        private Stage2SettlementPackResponseTotal totalField;

        private Stage2SettlementPackResponseAgreementData agreementDataField;

        /// <remarks/>
        public Stage2SettlementPackResponseDefendantRepresentative DefendantRepresentative
        {
            get
            {
                return this.defendantRepresentativeField;
            }
            set
            {
                this.defendantRepresentativeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CurrentDefendantResponse", IsNullable = false)]
        public CT_A2A_CurrentDefendantResponse_ELPL[] DefendantReplies
        {
            get
            {
                return this.defendantRepliesField;
            }
            set
            {
                this.defendantRepliesField = value;
            }
        }

        /// <remarks/>
        public Stage2SettlementPackResponseTotal Total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }

        /// <remarks/>
        public Stage2SettlementPackResponseAgreementData AgreementData
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
    public partial class Stage2SettlementPackResponseDefendantRepresentative
    {

        private CT_A2A_DefendantsInsurer defendantsInsurerField;

        private CT_A2A_CRUReference cRUReferenceField;

        /// <remarks/>
        public CT_A2A_DefendantsInsurer DefendantsInsurer
        {
            get
            {
                return this.defendantsInsurerField;
            }
            set
            {
                this.defendantsInsurerField = value;
            }
        }

        /// <remarks/>
        public CT_A2A_CRUReference CRUReference
        {
            get
            {
                return this.cRUReferenceField;
            }
            set
            {
                this.cRUReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_DefendantsInsurer
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
    public partial class CT_A2A_AgreementDetails
    {

        private decimal grossAmountField;

        private decimal interimPaymentAmountField;

        private string commentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public decimal GrossAmount
        {
            get
            {
                return this.grossAmountField;
            }
            set
            {
                this.grossAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public decimal InterimPaymentAmount
        {
            get
            {
                return this.interimPaymentAmountField;
            }
            set
            {
                this.interimPaymentAmountField = value;
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_FinalAgreementDetails
    {

        private CT_A2A_AgreementDetails agreementDetailsField;

        private C20_SettlementPackDecision settlementPackDecisionField;

        /// <remarks/>
        public CT_A2A_AgreementDetails AgreementDetails
        {
            get
            {
                return this.agreementDetailsField;
            }
            set
            {
                this.agreementDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C20_SettlementPackDecision SettlementPackDecision
        {
            get
            {
                return this.settlementPackDecisionField;
            }
            set
            {
                this.settlementPackDecisionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_CurrentTotal
    {

        private decimal cRUDeductionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public decimal CRUDeductions
        {
            get
            {
                return this.cRUDeductionsField;
            }
            set
            {
                this.cRUDeductionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_CurrentDefendantResponse_ELPL
    {

        private string commentsField;

        private decimal grossValueOfferedField;

        private C00_YNFlag isGrossAmountAgreedField;

        private C18_LossType_ELPL lossTypeField;

        private decimal interestField;

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
        public decimal GrossValueOffered
        {
            get
            {
                return this.grossValueOfferedField;
            }
            set
            {
                this.grossValueOfferedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag IsGrossAmountAgreed
        {
            get
            {
                return this.isGrossAmountAgreedField;
            }
            set
            {
                this.isGrossAmountAgreedField = value;
            }
        }

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
        public decimal Interest
        {
            get
            {
                return this.interestField;
            }
            set
            {
                this.interestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum C18_LossType_ELPL
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackResponseTotal
    {

        private CT_A2A_CurrentTotal currentTotalField;

        /// <remarks/>
        public CT_A2A_CurrentTotal CurrentTotal
        {
            get
            {
                return this.currentTotalField;
            }
            set
            {
                this.currentTotalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackResponseAgreementData
    {

        private CT_A2A_FinalAgreementDetails finalAgreementDetailsField;

        /// <remarks/>
        public CT_A2A_FinalAgreementDetails FinalAgreementDetails
        {
            get
            {
                return this.finalAgreementDetailsField;
            }
            set
            {
                this.finalAgreementDetailsField = value;
            }
        }
    }
}