namespace Fwbs.Office
{
    using Fwbs.Framework;
    using MSOffice = Microsoft.Office.Core;

    internal sealed partial class OfficeCommandBarButton : 
        OfficeCommandBarControl,
        MSOffice.CommandBarButton
    {
        private readonly MSOffice.CommandBarButton button;
        private readonly OfficeCommandBarControls parent;

 
        internal OfficeCommandBarButton(OfficeCommandBarControls parent, MSOffice.CommandBarButton button)
            : base(parent, button)
        {

            this.parent = parent;
            this.button = button;

            Init(button);

        }

        #region Overrides

        protected override void OnDetach()
        {
            if (click != null)
            {
                while (click != null)
                {
                    Click -= (MSOffice._CommandBarButtonEvents_ClickEventHandler)click.GetInvocationList()[0];
                }
            }
        }


       
        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();

            button.Click += button_Click;
            Stats.Increment(typeof(MSOffice._CommandBarButtonEvents_ClickEventHandler));
            
        }

        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            try
            {
                button.Click -= button_Click;
                Stats.Decrement(typeof(MSOffice._CommandBarButtonEvents_ClickEventHandler));
            }
            catch { }
        }
 

        #endregion


        #region _CommandBarButtonEvents_Event Members

        private MSOffice._CommandBarButtonEvents_ClickEventHandler click;
        public event MSOffice._CommandBarButtonEvents_ClickEventHandler Click
        {
            add
            {
                //Only allow one delegate per button.
                if (click == null)
                {
                    click += value;
                    Stats.Increment(typeof(MSOffice._CommandBarButtonEvents_ClickEventHandler));
                }
            }
            remove
            {
                if (click != null)
                {
                    click -= value;
                    Stats.Decrement(typeof(MSOffice._CommandBarButtonEvents_ClickEventHandler));
                }
            }
        }

        private void button_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            MSOffice._CommandBarButtonEvents_ClickEventHandler ev = click;
            if (ev != null)
                parent.OnButtonClick(this);
        }

        internal void OnClick(ref bool cancel)
        {
            MSOffice._CommandBarButtonEvents_ClickEventHandler ev = click;
            if (ev != null)
                ev(this, ref cancel);
        }

        #endregion


        #region _CommandBarButton Members

        public bool BuiltInFace
        {
            get
            {
                return button.BuiltInFace;
            }
            set
            {
                button.BuiltInFace = value;
            }
        }



        public void CopyFace()
        {
            button.CopyFace();
        }

      
        public int FaceId
        {
            get
            {
                return button.FaceId;
            }
            set
            {
                button.FaceId = value;
            }
        }


        public Microsoft.Office.Core.MsoCommandBarButtonHyperlinkType HyperlinkType
        {
            get
            {
                return button.HyperlinkType;
            }
            set
            {
                button.HyperlinkType = value;
            }
        }

        public stdole.IPictureDisp Mask
        {
            get
            {
                return button.Mask;
            }
            set
            {
                button.Mask = value;
            }
        }



        public void PasteFace()
        {
            button.PasteFace();
        }

        public stdole.IPictureDisp Picture
        {
            get
            {
                return button.Picture;
            }
            set
            {
                button.Picture = value;
            }
        }

      
        public string ShortcutText
        {
            get
            {
                return button.ShortcutText;
            }
            set
            {
                button.ShortcutText = value;
            }
        }

        public Microsoft.Office.Core.MsoButtonState State
        {
            get
            {
                return button.State;
            }
            set
            {
                button.State = value;
            }
        }

        public Microsoft.Office.Core.MsoButtonStyle Style
        {
            get
            {
                return button.Style;
            }
            set
            {
                button.Style = value;
            }
        }

   

        #endregion


 
    }
}
