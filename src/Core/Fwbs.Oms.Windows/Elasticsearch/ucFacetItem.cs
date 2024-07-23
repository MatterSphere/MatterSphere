using System;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class ucFacetItem : BlueCheckBox
    {
        public ucFacetItem()
        {
            this.AutoSize = true;
            this.Margin = new System.Windows.Forms.Padding(0);

            Key = Guid.NewGuid();
        }

        public Guid Key { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public int Number { get; set; }

        public override string Text {
            get
            {
                return Session.CurrentSession.IsConnected 
                    ? $"{Session.CurrentSession.Terminology.Parse(Value, true)} ({Number})"
                    : $"{Value} ({Number})";
            }
        }
    }
}
