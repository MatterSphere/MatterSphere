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
// using ELPLServicesLibrary;

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
    public partial class Stage2SettlementPackCounterOfferByCR
    {

        private CT_A2A_ClaimantLosses_ELPL[] claimantLossesField;

        private Stage2SettlementPackCounterOfferByCRAgreementData agreementDataField;

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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
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