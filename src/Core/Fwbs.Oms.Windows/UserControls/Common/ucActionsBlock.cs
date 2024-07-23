using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucActionsBlock : UserControl
    {
        private OMSToolBarButton _oMSToolBarButton;
        private eToolbars _eToolBars;

        public ucActionsBlock(eToolbars toolBars)
        {
            InitializeComponent();
            _eToolBars = toolBars;
        }

        public void AddButton(OMSToolBarButton omsToolBarButton)
        {
            if (string.IsNullOrWhiteSpace(omsToolBarButton.PanelButton.Text))
            {
                return;
            }

            _oMSToolBarButton = omsToolBarButton;
            Button button = new Button
            {
                FlatStyle = FlatStyle.Flat,
                Text = omsToolBarButton.PanelButton.Text,
                Width = 114,
                Height = 24
            };
            button.Click += new EventHandler(LinkClicked);
            button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel.Controls.Add(button, true);
        }

        public void AddButton(ucNavCmdButtons cmdButton)
        {
            if (string.IsNullOrWhiteSpace(cmdButton.Text))
            {
                return;
            }

            Button button = new Button
            {
                FlatStyle = FlatStyle.Flat,
                Text = cmdButton.Text,
                Width = 114,
                Height = 24
            };
            button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            cmdButton.AttachButton(button);
            this.flowLayoutPanel.Controls.Add(button, true);
        }

        public void Clear()
        {
            var count = this.flowLayoutPanel.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                this.flowLayoutPanel.Controls.RemoveAt(0);
            }
        }

        private void LinkClicked(object sender, EventArgs e)
        {
            _eToolBars.OnLinkButtonClick(_eToolBars, new ToolBarButtonClickEventArgs(_oMSToolBarButton));
        }
    }
}
