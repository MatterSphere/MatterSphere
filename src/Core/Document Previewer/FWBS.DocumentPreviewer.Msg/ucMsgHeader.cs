using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Aspose.Email;

namespace Fwbs.Documents.Preview.Msg
{

    internal partial class ucMsgHeader : UserControl
    {
        public ucMsgHeader()
        {
            InitializeComponent();
        }

        internal event EventHandler MessageClicked;
        private void OnMessageClicked(object sender)
        {
            EventHandler ev = this.MessageClicked;

            if (ev != null)
                ev(sender, EventArgs.Empty);
        }

        internal event EventHandler AttatchmentClicked;
        private void OnAttatchmentClicked(object sender)
        {
            EventHandler ev = this.AttatchmentClicked;

            if (ev != null)
                ev(sender, EventArgs.Empty);
        }

        internal const string attatchmentPrefix = "      ";
        private const string messageAttatchment = "Message";
        internal void SetupControl(MailMessage msg)
        {
            this.subject.Text = msg.Subject;

            string sendersAddress = null;
            if (msg.From != null)
                sendersAddress = msg.From.Address;

            if (string.IsNullOrEmpty(sendersAddress) ||sendersAddress.StartsWith("/o"))
                this.From.Text = msg.From.DisplayName;
            else
                this.From.Text = string.Format("{0} [{1}]", msg.From.DisplayName, sendersAddress);

            if (msg.Date == new DateTime(1899, 12, 30, 0, 0, 0).ToLocalTime())
                if (additionalProperties != null && additionalProperties.ContainsKey("SentDate"))
                    this.SentDate.Text = additionalProperties["SentDate"];
                else
                    pnlSent.Visible = false;
            else
            {
                if (msg.Date.Kind == DateTimeKind.Utc)
                    this.SentDate.Text = msg.Date.ToLocalTime().ToString();
                else
                    this.SentDate.Text = msg.Date.ToString();
            }

            this.listTo.Controls.Clear();
            this.lstCC.Controls.Clear();

            foreach(var recip in msg.To)
            {
                Label txt = new Label();
                txt.AutoSize = true;
                txt.Text = recip.DisplayName;
                txt.BorderStyle = BorderStyle.None;
                this.listTo.Controls.Add(txt);
                toolTip1.SetToolTip(txt, recip.Address);
            }

            foreach (var recip in msg.CC)
            {
                Label txt = new Label();
                txt.AutoSize = true;
                txt.Text = recip.DisplayName;
                txt.BorderStyle = BorderStyle.None;
                this.lstCC.Controls.Add(txt);
                toolTip1.SetToolTip(txt, recip.Address);
            }


            this.lstAttachments.Controls.Clear();

            Label defaultAttatchment = new Label();
            defaultAttatchment.AutoSize = true;
            defaultAttatchment.BorderStyle = BorderStyle.None;
            // TK - get rid of the icon source properly after conversion
            Icon icon = IconReader.GetFileIcon(messageAttatchment + ".msg", IconReader.IconSize.Small, false);
            defaultAttatchment.Image = icon.ToBitmap();
            icon.Dispose();
            icon = null;
            defaultAttatchment.ImageAlign = ContentAlignment.MiddleLeft;
            defaultAttatchment.Text = attatchmentPrefix + messageAttatchment;
            defaultAttatchment.TextAlign = ContentAlignment.MiddleRight;
            defaultAttatchment.Click += new EventHandler(defaultAttatchment_Click);
            this.lstAttachments.Controls.Add(defaultAttatchment);


            foreach (var attatchment in msg.Attachments)
            {

                Label txt = new Label();
                txt.AutoSize = true;
                
                txt.BorderStyle = BorderStyle.None;
                // TK - get rid of the icon source properly after conversion
                icon = IconReader.GetFileIcon(attatchment.Name, IconReader.IconSize.Small, false);
                txt.Image = icon.ToBitmap();
                icon.Dispose();
                icon = null;
                txt.ImageAlign = ContentAlignment.MiddleLeft;

                txt.Text = string.Format("{0}{1} ({2})", attatchmentPrefix , attatchment.Name,Utils.FileSize(attatchment.ContentStream.Length));
                txt.Tag = attatchment.Name;
                txt.TextAlign = ContentAlignment.MiddleRight;
                txt.Click += new EventHandler(txt_Click);
                this.lstAttachments.Controls.Add(txt);
            }


            pnlCC.Visible = lstCC.Controls.Count >= 1;
            
                

            if (lstAttachments.Controls.Count <= 1)
                pnlAttach.Visible = false;
            else
            {
                pnlAttach.Visible = true;
            }

            DrawTheLine();

            SetMessageToBeHighlighted();
            OnMessageClicked(this);

        }

        void txt_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;

            if (lbl == null)
                return;

            ResetAttatchmentBackgrounds();
            lbl.BackColor = Color.FromArgb(214, 230, 245);
            OnAttatchmentClicked(sender);
        }

        private void ResetAttatchmentBackgrounds()
        {
            foreach (Control ctrl in lstAttachments.Controls)
            {
                if (ctrl is Label)
                    ctrl.BackColor = ctrl.Parent.BackColor;
            }
        }

        void defaultAttatchment_Click(object sender, EventArgs e)
        {
            //Return to the original message
            SetMessageToBeHighlighted();
            OnMessageClicked(sender);
            
        }

        private void SetMessageToBeHighlighted()
        {
            Label lbl = lstAttachments.Controls[0] as Label;

            if (lbl == null)
                return;

            ResetAttatchmentBackgrounds();
            lbl.BackColor = Color.FromArgb(214, 230, 245);

        }

        private void ucMsgHeader_Resize(object sender, EventArgs e)
        {
            DrawTheLine();
        }

        internal void RemoveAttatchmentDisplayList(string attatchmentName)
        {
            foreach (Control ctrl in lstAttachments.Controls)
            {
                if (ctrl.Tag is string)
                {
                    if ((string)ctrl.Tag == attatchmentName)
                    {
                        lstAttachments.Controls.Remove(ctrl);
                        // TK - we are removing the control, how is it meant to be disposed?
                        ctrl.Dispose();
                    }
                }
            }

            if (lstAttachments.Controls.Count <= 1)
            {
                pnlAttach.Visible = false;
                DrawTheLine();
            }

        }

        private void DrawTheLine()
        {
            using (Graphics gphx = this.CreateGraphics())
            {
                using (Pen penny = new Pen(Color.FromArgb(101, 147, 207)))
                {
                    penny.Width = 2;
                    gphx.DrawLine(penny, 0, this.Height - 3, this.Width, this.Height - 3);
                }
            }


        }

        internal void Culture(Dictionary<string, string> cultureInfo)
        {
            if (cultureInfo.ContainsKey("Sent:"))
                sent.Text = cultureInfo["Sent:"];

            if (cultureInfo.ContainsKey("To:"))
                To.Text = cultureInfo["To:"];

            if (cultureInfo.ContainsKey("Cc:"))
                label2.Text = cultureInfo["Cc:"];

            if (cultureInfo.ContainsKey("Attachments:"))
                label3.Text = cultureInfo["Attachments:"];

        }

        private Dictionary<string, string> additionalProperties;
        internal void AdditionalProperties(Dictionary<string, string> additionalProperties)
        {
            this.additionalProperties = additionalProperties;

        }


    }
}
