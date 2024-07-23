using System;
using System.Collections.Generic;
using System.Text;
using RTAServicesLibrary.Enums;

namespace RTAServicesLibrary.SharedClasses
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_CompanyDetails
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
    public partial class CT_MedCoCase
    {

        private C00_YNFlag softTissueField;

        private bool softTissueFieldSpecified;

        private string medCoCaseIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag SoftTissue
        {
            get
            {
                return this.softTissueField;
            }
            set
            {
                this.softTissueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SoftTissueSpecified
        {
            get
            {
                return this.softTissueFieldSpecified;
            }
            set
            {
                this.softTissueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string MedCoCaseID
        {
            get
            {
                return this.medCoCaseIDField;
            }
            set
            {
                this.medCoCaseIDField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_CRUReference
    {

        private string cRUReferenceNumberField;

        private string cRUCommentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string CRUReferenceNumber
        {
            get
            {
                return this.cRUReferenceNumberField;
            }
            set
            {
                this.cRUReferenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string CRUComment
        {
            get
            {
                return this.cRUCommentField;
            }
            set
            {
                this.cRUCommentField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_Vehicle
    {

        private string vRNField;

        private string makeField;

        private string modelField;

        private string engineSizeField;

        private string colorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string VRN
        {
            get
            {
                return this.vRNField;
            }
            set
            {
                this.vRNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string Make
        {
            get
            {
                return this.makeField;
            }
            set
            {
                this.makeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "integer")]
        public string EngineSize
        {
            get
            {
                return this.engineSizeField;
            }
            set
            {
                this.engineSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string Color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }
    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_PoliceDetails
    {

        private string stationNameField;

        private string stationAddressField;

        private string reportingOfficerNameField;

        private string referenceNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string StationName
        {
            get
            {
                return this.stationNameField;
            }
            set
            {
                this.stationNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string StationAddress
        {
            get
            {
                return this.stationAddressField;
            }
            set
            {
                this.stationAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ReportingOfficerName
        {
            get
            {
                return this.reportingOfficerNameField;
            }
            set
            {
                this.reportingOfficerNameField = value;
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
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_AccidentCircumstances
    {

        private C00_YNFlag vhclHitSideRoadField;

        private bool vhclHitSideRoadFieldSpecified;

        private C00_YNFlag vhclHitInRearField;

        private bool vhclHitInRearFieldSpecified;

        private C00_YNFlag vhclHitWhilstParkedField;

        private bool vhclHitWhilstParkedFieldSpecified;

        private C00_YNFlag accidCarParkField;

        private bool accidCarParkFieldSpecified;

        private C00_YNFlag accidRoundaboutField;

        private bool accidRoundaboutFieldSpecified;

        private C00_YNFlag accidChangingLanesField;

        private bool accidChangingLanesFieldSpecified;

        private C00_YNFlag concertinaCollisionField;

        private bool concertinaCollisionFieldSpecified;

        private C00_YNFlag otherField;

        private bool otherFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag VhclHitSideRoad
        {
            get
            {
                return this.vhclHitSideRoadField;
            }
            set
            {
                this.vhclHitSideRoadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VhclHitSideRoadSpecified
        {
            get
            {
                return this.vhclHitSideRoadFieldSpecified;
            }
            set
            {
                this.vhclHitSideRoadFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag VhclHitInRear
        {
            get
            {
                return this.vhclHitInRearField;
            }
            set
            {
                this.vhclHitInRearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VhclHitInRearSpecified
        {
            get
            {
                return this.vhclHitInRearFieldSpecified;
            }
            set
            {
                this.vhclHitInRearFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag VhclHitWhilstParked
        {
            get
            {
                return this.vhclHitWhilstParkedField;
            }
            set
            {
                this.vhclHitWhilstParkedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VhclHitWhilstParkedSpecified
        {
            get
            {
                return this.vhclHitWhilstParkedFieldSpecified;
            }
            set
            {
                this.vhclHitWhilstParkedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag AccidCarPark
        {
            get
            {
                return this.accidCarParkField;
            }
            set
            {
                this.accidCarParkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccidCarParkSpecified
        {
            get
            {
                return this.accidCarParkFieldSpecified;
            }
            set
            {
                this.accidCarParkFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag AccidRoundabout
        {
            get
            {
                return this.accidRoundaboutField;
            }
            set
            {
                this.accidRoundaboutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccidRoundaboutSpecified
        {
            get
            {
                return this.accidRoundaboutFieldSpecified;
            }
            set
            {
                this.accidRoundaboutFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag AccidChangingLanes
        {
            get
            {
                return this.accidChangingLanesField;
            }
            set
            {
                this.accidChangingLanesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccidChangingLanesSpecified
        {
            get
            {
                return this.accidChangingLanesFieldSpecified;
            }
            set
            {
                this.accidChangingLanesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag ConcertinaCollision
        {
            get
            {
                return this.concertinaCollisionField;
            }
            set
            {
                this.concertinaCollisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConcertinaCollisionSpecified
        {
            get
            {
                return this.concertinaCollisionFieldSpecified;
            }
            set
            {
                this.concertinaCollisionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Other
        {
            get
            {
                return this.otherField;
            }
            set
            {
                this.otherField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OtherSpecified
        {
            get
            {
                return this.otherFieldSpecified;
            }
            set
            {
                this.otherFieldSpecified = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_RoadConditions
    {

        private C00_YNFlag dryField;

        private bool dryFieldSpecified;

        private C00_YNFlag wetField;

        private bool wetFieldSpecified;

        private C00_YNFlag snowField;

        private bool snowFieldSpecified;

        private C00_YNFlag iceField;

        private bool iceFieldSpecified;

        private C00_YNFlag mudField;

        private bool mudFieldSpecified;

        private C00_YNFlag oilField;

        private bool oilFieldSpecified;

        private C00_YNFlag otherField;

        private bool otherFieldSpecified;

        private string otherDetailsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Dry
        {
            get
            {
                return this.dryField;
            }
            set
            {
                this.dryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DrySpecified
        {
            get
            {
                return this.dryFieldSpecified;
            }
            set
            {
                this.dryFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Wet
        {
            get
            {
                return this.wetField;
            }
            set
            {
                this.wetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WetSpecified
        {
            get
            {
                return this.wetFieldSpecified;
            }
            set
            {
                this.wetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Snow
        {
            get
            {
                return this.snowField;
            }
            set
            {
                this.snowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SnowSpecified
        {
            get
            {
                return this.snowFieldSpecified;
            }
            set
            {
                this.snowFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Ice
        {
            get
            {
                return this.iceField;
            }
            set
            {
                this.iceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IceSpecified
        {
            get
            {
                return this.iceFieldSpecified;
            }
            set
            {
                this.iceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Mud
        {
            get
            {
                return this.mudField;
            }
            set
            {
                this.mudField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MudSpecified
        {
            get
            {
                return this.mudFieldSpecified;
            }
            set
            {
                this.mudFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Oil
        {
            get
            {
                return this.oilField;
            }
            set
            {
                this.oilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OilSpecified
        {
            get
            {
                return this.oilFieldSpecified;
            }
            set
            {
                this.oilFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Other
        {
            get
            {
                return this.otherField;
            }
            set
            {
                this.otherField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OtherSpecified
        {
            get
            {
                return this.otherFieldSpecified;
            }
            set
            {
                this.otherFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string OtherDetails
        {
            get
            {
                return this.otherDetailsField;
            }
            set
            {
                this.otherDetailsField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_WeatherConditions
    {

        private C00_YNFlag sunField;

        private bool sunFieldSpecified;

        private C00_YNFlag rainField;

        private bool rainFieldSpecified;

        private C00_YNFlag snowField;

        private bool snowFieldSpecified;

        private C00_YNFlag iceField;

        private bool iceFieldSpecified;

        private C00_YNFlag fogField;

        private bool fogFieldSpecified;

        private C00_YNFlag otherField;

        private bool otherFieldSpecified;

        private string otherDetailsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Sun
        {
            get
            {
                return this.sunField;
            }
            set
            {
                this.sunField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SunSpecified
        {
            get
            {
                return this.sunFieldSpecified;
            }
            set
            {
                this.sunFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Rain
        {
            get
            {
                return this.rainField;
            }
            set
            {
                this.rainField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RainSpecified
        {
            get
            {
                return this.rainFieldSpecified;
            }
            set
            {
                this.rainFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Snow
        {
            get
            {
                return this.snowField;
            }
            set
            {
                this.snowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SnowSpecified
        {
            get
            {
                return this.snowFieldSpecified;
            }
            set
            {
                this.snowFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Ice
        {
            get
            {
                return this.iceField;
            }
            set
            {
                this.iceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IceSpecified
        {
            get
            {
                return this.iceFieldSpecified;
            }
            set
            {
                this.iceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Fog
        {
            get
            {
                return this.fogField;
            }
            set
            {
                this.fogField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FogSpecified
        {
            get
            {
                return this.fogFieldSpecified;
            }
            set
            {
                this.fogFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C00_YNFlag Other
        {
            get
            {
                return this.otherField;
            }
            set
            {
                this.otherField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OtherSpecified
        {
            get
            {
                return this.otherFieldSpecified;
            }
            set
            {
                this.otherFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string OtherDetails
        {
            get
            {
                return this.otherDetailsField;
            }
            set
            {
                this.otherDetailsField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_INPUT_AlternativeCompany
    {

        private string companyNameField;

        private string addressField;

        private string telephoneNumberField;

        private string referenceNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string CompanyName
        {
            get
            {
                return this.companyNameField;
            }
            set
            {
                this.companyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string Address
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
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_HospitalAddress
    {

        private string addressLine1Field;

        private string addressLine2Field;

        private string addressLine3Field;

        private string addressLine4Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string AddressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string AddressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string AddressLine3
        {
            get
            {
                return this.addressLine3Field;
            }
            set
            {
                this.addressLine3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string AddressLine4
        {
            get
            {
                return this.addressLine4Field;
            }
            set
            {
                this.addressLine4Field = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CT_A2A_ClaimantLosses
    {

        private C18_LossType_A2A_R3 lossTypeField;

        private bool lossTypeFieldSpecified;

        private C00_YNFlag evidenceAttachedField;

        private string commentsField;

        private decimal grossValueClaimedField;

        private decimal percContribNegDeductionsField;

        private decimal interestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public C18_LossType_A2A_R3 LossType
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LossTypeSpecified
        {
            get
            {
                return this.lossTypeFieldSpecified;
            }
            set
            {
                this.lossTypeFieldSpecified = value;
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
}
