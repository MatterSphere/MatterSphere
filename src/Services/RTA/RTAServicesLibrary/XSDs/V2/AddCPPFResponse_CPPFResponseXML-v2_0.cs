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
    public partial class CourtProceedingPackResponse
    {

        private CourtProceedingPackResponseDefendantRepresentative defendantRepresentativeField;

        private CourtProceedingPackResponseCourtProceedingsPackPartA courtProceedingsPackPartAField;

        private CourtProceedingPackResponseDisbursementDisputedRequestResponse[] disbursmentDisputedField;

        /// <remarks/>
        public CourtProceedingPackResponseDefendantRepresentative DefendantRepresentative
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
        public CourtProceedingPackResponseCourtProceedingsPackPartA CourtProceedingsPackPartA
        {
            get
            {
                return this.courtProceedingsPackPartAField;
            }
            set
            {
                this.courtProceedingsPackPartAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DisbursementDisputedRequestResponse", IsNullable = false)]
        public CourtProceedingPackResponseDisbursementDisputedRequestResponse[] DisbursmentDisputed
        {
            get
            {
                return this.disbursmentDisputedField;
            }
            set
            {
                this.disbursmentDisputedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class CourtProceedingPackResponseDefendantRepresentative
    {

        private CourtProceedingPackResponseDefendantRepresentativeDefendantsInsurer defendantsInsurerField;

        /// <remarks/>
        public CourtProceedingPackResponseDefendantRepresentativeDefendantsInsurer DefendantsInsurer
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class CourtProceedingPackResponseDefendantRepresentativeDefendantsInsurer
    {

        private CourtProceedingPackResponseDefendantRepresentativeDefendantsInsurerAddress addressField;

        private string contactNameField;

        private string contactMiddleNameField;

        private string contactSurnameField;

        private string emailAddressField;

        private string referenceNumberField;

        private string telephoneNumberField;

        /// <remarks/>
        public CourtProceedingPackResponseDefendantRepresentativeDefendantsInsurerAddress Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

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
    public partial class CourtProceedingPackResponseDefendantRepresentativeDefendantsInsurerAddress
    {

        private C01_AddressType addressTypeField;

        private bool addressTypeFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C01_AddressType AddressType
        {
            get
            {
                return this.addressTypeField;
            }
            set
            {
                this.addressTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AddressTypeSpecified
        {
            get
            {
                return this.addressTypeFieldSpecified;
            }
            set
            {
                this.addressTypeFieldSpecified = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    //[System.SerializableAttribute()]
    //public enum C01_AddressType {

    //    /// <remarks/>
    //    P,

    //    /// <remarks/>
    //    A,

    //    /// <remarks/>
    //    F,
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class CourtProceedingPackResponseCourtProceedingsPackPartA
    {

        private decimal cRUBenefitsReceivedField;

        private string cRUBenefitsReceivedCommentsField;

        private C00_YNFlag upToDateCRUCertificateAttachedField;

        private string upToDateCRUCertificateAttachedCommentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public decimal CRUBenefitsReceived
        {
            get
            {
                return this.cRUBenefitsReceivedField;
            }
            set
            {
                this.cRUBenefitsReceivedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string CRUBenefitsReceivedComments
        {
            get
            {
                return this.cRUBenefitsReceivedCommentsField;
            }
            set
            {
                this.cRUBenefitsReceivedCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag UpToDateCRUCertificateAttached
        {
            get
            {
                return this.upToDateCRUCertificateAttachedField;
            }
            set
            {
                this.upToDateCRUCertificateAttachedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string UpToDateCRUCertificateAttachedComments
        {
            get
            {
                return this.upToDateCRUCertificateAttachedCommentsField;
            }
            set
            {
                this.upToDateCRUCertificateAttachedCommentsField = value;
            }
        }
    }

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    //[System.SerializableAttribute()]
    //public enum C00_YNFlag {

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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class CourtProceedingPackResponseDisbursementDisputedRequestResponse
    {

        private string disbursementIdField;

        private string reasonForNotPayingFullDisbursementField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "integer")]
        public string DisbursementId
        {
            get
            {
                return this.disbursementIdField;
            }
            set
            {
                this.disbursementIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ReasonForNotPayingFullDisbursement
        {
            get
            {
                return this.reasonForNotPayingFullDisbursementField;
            }
            set
            {
                this.reasonForNotPayingFullDisbursementField = value;
            }
        }
    }
}