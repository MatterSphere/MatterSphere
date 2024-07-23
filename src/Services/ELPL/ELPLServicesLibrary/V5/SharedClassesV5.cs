using System;
using System.Collections.Generic;
using System.Text;

namespace ELPLServicesLibraryV5
{
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
}
