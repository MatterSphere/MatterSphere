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
    public partial class Stage2SettlementPackCounterOfferByCR
    {

        private CT_A2A_ClaimantLosses[] claimantLossesField;

        private Stage2SettlementPackCounterOfferByCRAgreementData agreementDataField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CurrentClaimantOffer", IsNullable = false)]
        public CT_A2A_ClaimantLosses[] ClaimantLosses
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
        public Stage2SettlementPackCounterOfferByCRAgreementData AgreementData
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

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //public partial class CT_A2A_ClaimantLosses
    //{

    //    private C18_LossType_R7 lossTypeField;

    //    private C00_YNFlag evidenceAttachedField;

    //    private string commentsField;

    //    private decimal grossValueClaimedField;

    //    private decimal percContribNegDeductionsField;

    //    private decimal interestField;

    //    private C33_TariffType tariffTypeField;

    //    private bool tariffTypeFieldSpecified;

    //    private C34_SelectDurationOfTheInjuryCR selectDurationOfTheInjuryField;

    //    private bool selectDurationOfTheInjuryFieldSpecified;

    //    private C00_YNFlag excepCircumstancesUpliftField;

    //    private bool excepCircumstancesUpliftFieldSpecified;

    //    private C36_ExcepCircumstancesUpliftPerc excepCircumstancesUpliftPercField;

    //    private bool excepCircumstancesUpliftPercFieldSpecified;

    //    private string excepCircumstancesUpliftNoteField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C18_LossType_R7 LossType
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
    //    public C00_YNFlag EvidenceAttached
    //    {
    //        get
    //        {
    //            return this.evidenceAttachedField;
    //        }
    //        set
    //        {
    //            this.evidenceAttachedField = value;
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
    //    public decimal GrossValueClaimed
    //    {
    //        get
    //        {
    //            return this.grossValueClaimedField;
    //        }
    //        set
    //        {
    //            this.grossValueClaimedField = value;
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
    //    public decimal Interest
    //    {
    //        get
    //        {
    //            return this.interestField;
    //        }
    //        set
    //        {
    //            this.interestField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C33_TariffType TariffType
    //    {
    //        get
    //        {
    //            return this.tariffTypeField;
    //        }
    //        set
    //        {
    //            this.tariffTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlIgnoreAttribute()]
    //    public bool TariffTypeSpecified
    //    {
    //        get
    //        {
    //            return this.tariffTypeFieldSpecified;
    //        }
    //        set
    //        {
    //            this.tariffTypeFieldSpecified = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C34_SelectDurationOfTheInjuryCR SelectDurationOfTheInjury
    //    {
    //        get
    //        {
    //            return this.selectDurationOfTheInjuryField;
    //        }
    //        set
    //        {
    //            this.selectDurationOfTheInjuryField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlIgnoreAttribute()]
    //    public bool SelectDurationOfTheInjurySpecified
    //    {
    //        get
    //        {
    //            return this.selectDurationOfTheInjuryFieldSpecified;
    //        }
    //        set
    //        {
    //            this.selectDurationOfTheInjuryFieldSpecified = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C00_YNFlag ExcepCircumstancesUplift
    //    {
    //        get
    //        {
    //            return this.excepCircumstancesUpliftField;
    //        }
    //        set
    //        {
    //            this.excepCircumstancesUpliftField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlIgnoreAttribute()]
    //    public bool ExcepCircumstancesUpliftSpecified
    //    {
    //        get
    //        {
    //            return this.excepCircumstancesUpliftFieldSpecified;
    //        }
    //        set
    //        {
    //            this.excepCircumstancesUpliftFieldSpecified = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public C36_ExcepCircumstancesUpliftPerc ExcepCircumstancesUpliftPerc
    //    {
    //        get
    //        {
    //            return this.excepCircumstancesUpliftPercField;
    //        }
    //        set
    //        {
    //            this.excepCircumstancesUpliftPercField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlIgnoreAttribute()]
    //    public bool ExcepCircumstancesUpliftPercSpecified
    //    {
    //        get
    //        {
    //            return this.excepCircumstancesUpliftPercFieldSpecified;
    //        }
    //        set
    //        {
    //            this.excepCircumstancesUpliftPercFieldSpecified = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public string ExcepCircumstancesUpliftNote
    //    {
    //        get
    //        {
    //            return this.excepCircumstancesUpliftNoteField;
    //        }
    //        set
    //        {
    //            this.excepCircumstancesUpliftNoteField = value;
    //        }
    //    }
    //}

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

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C33_TariffType
    //{

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("1")]
    //    Item1,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("2")]
    //    Item2,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C34_SelectDurationOfTheInjuryCR
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
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //public enum C36_ExcepCircumstancesUpliftPerc
    //{

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

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("17")]
    //    Item17,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("18")]
    //    Item18,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("19")]
    //    Item19,

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlEnumAttribute("20")]
    //    Item20,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //public partial class CT_A2A_AgreementDetails
    //{

    //    private decimal grossAmountField;

    //    private decimal interimPaymentAmountField;

    //    private string commentsField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public decimal GrossAmount
    //    {
    //        get
    //        {
    //            return this.grossAmountField;
    //        }
    //        set
    //        {
    //            this.grossAmountField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    //    public decimal InterimPaymentAmount
    //    {
    //        get
    //        {
    //            return this.interimPaymentAmountField;
    //        }
    //        set
    //        {
    //            this.interimPaymentAmountField = value;
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
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackCounterOfferByCRAgreementData
    {

        private Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails finalAgreementDetailsField;

        /// <remarks/>
        public Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails FinalAgreementDetails
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails
    {

        private CT_A2A_AgreementDetails agreementDetailsField;

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
    }
}