using System;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    internal class ucNavigationContainer : Panel
    {
        private int _index;

        public override bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                base.AutoScroll = value;
                _index = 0;
                if (value)
                    ShowAll();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (!AutoScroll && Controls.Count != 0)
            {
                var possibleCount = System.Math.Min(Height / Controls[0].Height, Controls.Count);
                var showedCount = Controls.Count - _index;
                if (possibleCount > showedCount)
                {
                    for (int i = 0; i < possibleCount; i++)
                    {
                        if (i < possibleCount)
                        {
                            Controls[i].Show();
                        }
                        else
                        {
                            Controls[i].Hide();
                        }
                    }

                    _index = Controls.Count - possibleCount;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (AutoScroll)
            {
                base.OnMouseWheel(e);
                return;
            }

            if (e.Delta < 0)
            {
                var possibleCount = Height / Controls[0].Height;
                if (_index < Controls.Count - 1 && _index < Controls.Count - possibleCount)
                {
                    _index++;
                    Controls[Controls.Count - _index].Hide();
                }
            }
            else
            {
                if (_index > 0)
                {
                    this.Controls[this.Controls.Count - _index].Show();
                    _index--;
                }
            }

            if (e is HandledMouseEventArgs)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }

        private void ShowAll()
        {
            foreach (Control control in this.Controls)
            {
                control.Show();
            }
        }
    }
}
