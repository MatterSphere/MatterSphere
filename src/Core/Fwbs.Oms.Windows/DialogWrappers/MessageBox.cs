using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Displays a message box that deals with the right to left aspect.
    /// </summary>
    public sealed class MessageBox
    {
        #region Fields

        private ResourceItem _text;
        private string _caption = FWBS.OMS.Global.ApplicationName;

        private MessageBoxIcon _icon = MessageBoxIcon.None;
        private string[] _buttons = new string[1] { "OK" };
        private string _defaultButton = "OK";
        private string _cancelButton = "OK";

        #endregion

        #region Constructors

        private MessageBox()
        {
        }

        public MessageBox(ResourceItem text)
        {
            _text = text;
        }

        public const MessageBoxIcon MessageBoxIconGear = (MessageBoxIcon)128;

        public MessageBox(MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            switch (buttons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    _buttons = new string[3] { "ABORT", "RETRY", "IGNORE" };
                    _cancelButton = "ABORT";
                    _defaultButton = "RETRY";
                    break;
                case MessageBoxButtons.OK:
                    _buttons = new string[1] { "OK" };
                    _cancelButton = "OK";
                    _defaultButton = "OK";
                    break;
                case MessageBoxButtons.OKCancel:
                    _buttons = new string[2] { "OK", "CANCEL" };
                    _cancelButton = "CANCEL";
                    _defaultButton = "OK";
                    break;
                case MessageBoxButtons.RetryCancel:
                    _buttons = new string[2] { "RETRY", "CANCEL" };
                    _cancelButton = "CANCEL";
                    _defaultButton = "RETRY";
                    break;
                case MessageBoxButtons.YesNo:
                    _buttons = new string[2] { "YES", "NO" };
                    _cancelButton = "NO";
                    _defaultButton = "YES";
                    break;
                case MessageBoxButtons.YesNoCancel:
                    _buttons = new string[3] { "YES", "NO", "CANCEL" };
                    _cancelButton = "CANCEL";
                    _defaultButton = "YES";
                    break;
                default:
                    goto case MessageBoxButtons.OK;
            }

            switch (defaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    if (_buttons.Length >= 1) _defaultButton = _buttons[0];
                    break;
                case MessageBoxDefaultButton.Button2:
                    if (_buttons.Length >= 2) _defaultButton = _buttons[1];
                    break;
                case MessageBoxDefaultButton.Button3:
                    if (_buttons.Length >= 3) _defaultButton = _buttons[2];
                    break;
                default:
                    goto case MessageBoxDefaultButton.Button1;
            }

            _icon = icon;
        }

        #endregion

        #region Properties

        public MessageBoxIcon Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }

        public string[] Buttons
        {
            get
            {
                return _buttons;
            }
            set
            {
                _buttons = value;
            }
        }

        public string DefaultButton
        {
            get
            {
                return _defaultButton;
            }
            set
            {
                _defaultButton = value;
            }
        }

        public string CancelButton
        {
            get
            {
                return _cancelButton;
            }
            set
            {
                _cancelButton = value;
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        public ResourceItem Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        #endregion

        #region Methods

        public string Show()
        {
            return Show(null as IWin32Window);
        }

        public string Show(IWin32Window owner)
        {
            System.Text.StringBuilder text = new System.Text.StringBuilder();
            text.Append(_text.Text);
            if (_text.Help != String.Empty)
            {
                text.Append(Environment.NewLine);
                text.Append(Environment.NewLine);
                text.Append(_text.Help);
            }

            if (_caption == null || _caption == String.Empty) _caption = FWBS.OMS.Global.ApplicationName;

            using (frmMessageBox frm = new frmMessageBox(text.ToString(), _caption, _buttons, _icon, _defaultButton, _cancelButton))
            {
                frm.ShowDialog(owner);
                return frm.CustomDialogResult;
            }
        }

        #endregion

        #region Static Methods

        public static DialogResult Show(Exception exception)
        {
            ErrorBox.Show(exception);
            return DialogResult.OK;
        }

        public static DialogResult Show(IWin32Window owner, Exception exception)
        {
            ErrorBox.Show(owner, exception);
            return DialogResult.OK;
        }

        public static DialogResult ShowYesNoQuestion(string text)
        {
            return Show(null, text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowYesNoCancel(string resourcecode, string resourcedescription, params string[] resourceparams)
        {
            return Show(null, FWBS.OMS.Session.CurrentSession.Resources.GetResource(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }


        public static DialogResult ShowYesNoQuestion(string resourcecode, string resourcedescription, params string[] resourceparams)
        {
            return Show(null, FWBS.OMS.Session.CurrentSession.Resources.GetResource(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }
        //added darren 17/11/2003 wanted the ability to specify default button
        public static DialogResult ShowYesNoQuestion(string resourcecode, string resourcedescription, bool defaultyes, params string[] resourceparams)
        {
            return Show(null, FWBS.OMS.Session.CurrentSession.Resources.GetResource(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, defaultyes ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2);
        }



        public static DialogResult ShowInformation(string resourcecode, string resourcedescription, params string[] resourceparams)
        {
            return Show(null, FWBS.OMS.Session.CurrentSession.Resources.GetResource(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowInformation(string text)
        {
            return Show(null, text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }



        public static DialogResult ShowYesNoCancel(IWin32Window owner, string resourcecode, string resourcedescription, params string[] resourceparams)
        {
            return Show(owner, FWBS.OMS.Session.CurrentSession.Resources.GetMessage(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowYesNoQuestion(IWin32Window owner, string resourcecode, string resourcedescription, params string[] resourceparams)
        {
            return Show(owner, FWBS.OMS.Session.CurrentSession.Resources.GetMessage(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowInformation(IWin32Window owner, string resourcecode, string resourcedescription, params string[] resourceparams)
        {
            return Show(owner, FWBS.OMS.Session.CurrentSession.Resources.GetMessage(resourcecode, resourcedescription, "", resourceparams).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }


        public static DialogResult Show(string text)
        {
            return Show(null, text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(ResourceItem resource)
        {
            return Show(null, resource, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(ResourceItem resource, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, resource, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(null, text, caption, buttons, icon, defaultButton);
        }

        public static DialogResult Show(ResourceItem resource, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(null, resource, caption, buttons, icon, defaultButton);
        }


        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Show(owner, text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(IWin32Window owner, ResourceItem resource)
        {
            return Show(owner, resource, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }


        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(IWin32Window owner, ResourceItem resource, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            System.Text.StringBuilder text = new System.Text.StringBuilder();
            text.Append(resource.Text);
            if (resource.Help != String.Empty)
            {
                text.Append(Environment.NewLine);
                text.Append(Environment.NewLine);
                text.Append(resource.Help);
            }

            return Show(owner, text.ToString(), caption, buttons, icon, defaultButton);

        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    {
                        if (string.IsNullOrEmpty(text))
                            return DialogResult.OK;
                        break;
                    }
                case MessageBoxButtons.OKCancel:
                    {
                        if (text == "BUTTONOK")
                            return DialogResult.OK;
                        else if (text == "BUTTONCANCEL")
                            return DialogResult.Cancel;

                        break;
                    }
                case MessageBoxButtons.YesNo:
                case MessageBoxButtons.YesNoCancel:
                    {
                        if (text == "BUTTONYES")
                            return DialogResult.Yes;
                        else if (text == "BUTTONNO")
                            return DialogResult.No;
                        break;

                    }


            }
           



            if (caption == null || caption == String.Empty) caption = FWBS.OMS.Global.ApplicationName;

            bool rtl = false;
            if (Session.CurrentSession.IsLoggedIn)
                rtl = Services.OMS.CurrentUser.RightToLeft;

            if (rtl)
                return System.Windows.Forms.MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            else
                return System.Windows.Forms.MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);

        }


        #endregion


    }

    
}
