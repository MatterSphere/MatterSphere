using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.ContextMenu
{
    public class ContextMenuPopupContainer : ToolStripDropDown
    {
        private const int _frames = 5;
        private const int _totalDuration = 100;
        private const int _frameDuration = _totalDuration / _frames;
        private Control _popup;
        private ToolStripControlHost _toolStripControlHost;
        private bool _fade = true;

        public ContextMenuPopupContainer()
        {

        }

        public ContextMenuPopupContainer(Control control) : this()
        {
            if (control == null)
            {
                throw new ArgumentNullException("content");
            }

            _popup = control;
            _fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;
            _toolStripControlHost = new ToolStripControlHost(control);
            Padding = Margin = _toolStripControlHost.Padding = _toolStripControlHost.Margin = Padding.Empty;
            control.Location = Point.Empty;
            this.Items.Add(_toolStripControlHost);
            control.Disposed += delegate (object sender, EventArgs e)
            {
                control = null;
                Dispose(true);
            };
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
            {
                return false;
            }

            return base.ProcessDialogKey(keyData);
        }

        public void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Show(control, control.ClientRectangle);
        }

        private void Show(Control control, Rectangle area)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Point location = GetLocation(control, area);
            ShowPopup(control, location);
        }

        protected virtual Point GetLocation(Control control, Rectangle area)
        {
            Point location = control.PointToScreen(new Point(area.Left - this.Width / 2, area.Top - this.Size.Height / 2));

            return control.PointToClient(location);
        }

        protected virtual void ShowPopup(Control control, Point location)
        {
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && _fade)
            {
                Opacity = 0;
            }

            base.SetVisibleCore(visible);
            if (!visible || !_fade)
            {
                return;
            }

            for (int i = 1; i <= _frames; i++)
            {
                if (i > 1)
                {
                    System.Threading.Thread.Sleep(_frameDuration);
                }

                Opacity = opacity * (double)i / (double)_frames;
            }

            Opacity = opacity;
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (_popup.IsDisposed || _popup.Disposing)
            {
                e.Cancel = true;
                return;
            }

            base.OnOpening(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            _popup.Focus();
            base.OnOpened(e);
        }
    }
}
