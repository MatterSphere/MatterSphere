﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 

namespace RTAServicesLibraryV2
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class InterimSettlementPackResponse
    {

        private InterimSettlementPackResponseDefendantRepresentative defendantRepresentativeField;

        private InterimSettlementPackResponseInterimPayment interimPaymentField;

        private CT_A2A_DefendantLosses[] defendantRepliesField;

        private InterimSettlementPackResponseTotal totalField;

        /// <remarks/>
        public InterimSettlementPackResponseDefendantRepresentative DefendantRepresentative
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
        public InterimSettlementPackResponseInterimPayment InterimPayment
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
        [System.Xml.Serialization.XmlArrayItemAttribute("DefendantResponse", IsNullable = false)]
        public CT_A2A_DefendantLosses[] DefendantReplies
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
        public InterimSettlementPackResponseTotal Total
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackResponseDefendantRepresentative
    {

        private CT_A2A_CompanyDetails defendantsInsurerField;

        /// <remarks/>
        public CT_A2A_CompanyDetails DefendantsInsurer
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
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //public partial class CT_A2A_DefendantLosses
    //{

    //    private C18_LossType lossTypeField;

    //    private C00_YNFlag isGrossAmountAgreedField;

    //    private string commentsField;

    //    private decimal percContribNegDeductionsField;

    //    private decimal grossValueOfferedField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C18_LossType LossType
    //    {
    //        get
    //        {
    //            return this.lossTypeField;
    //        }
    //        set
    //        {
    //            this.lossTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C00_YNFlag IsGrossAmountAgreed
    //    {
    //        get
    //        {
    //            return this.isGrossAmountAgreedField;
    //        }
    //        set
    //        {
    //            this.isGrossAmountAgreedField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string Comments
    //    {
    //        get
    //        {
    //            return this.commentsField;
    //        }
    //        set
    //        {
    //            this.commentsField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public decimal PercContribNegDeductions
    //    {
    //        get
    //        {
    //            return this.percContribNegDeductionsField;
    //        }
    //        set
    //        {
    //            this.percContribNegDeductionsField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public decimal GrossValueOffered
    //    {
    //        get
    //        {
    //            return this.grossValueOfferedField;
    //        }
    //        set
    //        {
    //            this.grossValueOfferedField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    //[System.SerializableAttribute()]
    //public enum C18_LossType
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
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_DefendantResponseToInterimPaymentRequest
    {

        private string additionalCommentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string AdditionalComments
        {
            get
            {
                return this.additionalCommentsField;
            }
            set
            {
                this.additionalCommentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackResponseInterimPayment
    {

        private CT_A2A_DefendantResponseToInterimPaymentRequest defendantResponsesToInterimPaymentRequestField;

        /// <remarks/>
        public CT_A2A_DefendantResponseToInterimPaymentRequest DefendantResponsesToInterimPaymentRequest
        {
            get
            {
                return this.defendantResponsesToInterimPaymentRequestField;
            }
            set
            {
                this.defendantResponsesToInterimPaymentRequestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackResponseTotal
    {

        private InterimSettlementPackResponseTotalLossesTotal lossesTotalField;

        /// <remarks/>
        public InterimSettlementPackResponseTotalLossesTotal LossesTotal
        {
            get
            {
                return this.lossesTotalField;
            }
            set
            {
                this.lossesTotalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InterimSettlementPackResponseTotalLossesTotal
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
}