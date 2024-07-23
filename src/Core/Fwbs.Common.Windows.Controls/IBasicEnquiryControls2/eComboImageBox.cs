using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A list selection enquiry control which generally holds a caption and a combo box.  
    /// This particular control can be used for picking fixed items in a combo box style list.
    /// </summary>
    public class eComboImageBox : eComboBox2
	{
		#region Fields
		protected ImageList _imagelist = null;
		private DataView _dataview = null;
		protected ComboBox cmb;
        #endregion

        private int ImageSize => this.LogicalToDeviceUnits(16);

        #region Constructors
        /// <summary>
        /// Uses the already existing combo box object from its base control to change its
        /// dropdown style.
        /// </summary>
        public eComboImageBox () : base()
		{
			cmb = (ComboBox)_ctrl;
			cmb.DropDownStyle = ComboBoxStyle.DropDownList;
			cmb.DrawMode = DrawMode.OwnerDrawFixed;
			cmb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._ctrl_DrawItem);
            cmb.SizeChanged += CmbOnSizeChanged;
			cmb.ItemHeight = ImageSize;
            cmb.DataSourceChanged += new System.EventHandler(this._ctrl_DataSourceChanged);
			cmb.VisibleChanged += new System.EventHandler(this.eComboImageBox_VisibleChanged);

            // 16px image + 3px item padding top + 3px item padding bottom.
            this.Height = 23;
        }
        #endregion

		#region Properties
		[Category("Appearance")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return _imagelist;
			}
			set
			{
				_imagelist = value;
			}
		}

        #endregion

        #region Private
        private void CmbOnSizeChanged(object sender, EventArgs e)
        {
            ValidateCtrlSizeOnChildCtrlSize();
        }

        private void eComboImageBox_VisibleChanged(object sender, System.EventArgs e)
		{
			if (cmb.Visible)
			{
				if (cmb.DataSource == null && cmb.Items.Count == 0 && _imagelist != null)
					for (int i = 0; i<=_imagelist.Images.Count-1;i++)
						cmb.Items.Add(i.ToString());
			}
		}
		
		private void _ctrl_DataSourceChanged(object sender, System.EventArgs e)
		{
			if (cmb.DataSource is DataTable) _dataview = ((DataTable)cmb.DataSource).DefaultView;
			if (cmb.DataSource is DataView) _dataview = (DataView)cmb.DataSource;
		}

		private void _ctrl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			try
			{
				string drawString = "";
				try
                {
                    drawString = _dataview != null
                        ? Convert.ToString(_dataview[e.Index][cmb.DisplayMember])
                        : Convert.ToString(cmb.Items[e.Index]);
				}
				catch {}
                
                e.DrawBackground();

                if (_imagelist != null && e.Index <= _imagelist.Images.Count && e.Index > -1)
                {
                    var point = new PointF
                    {
                        X = e.Bounds.Location.X + 1,
                        Y = e.Bounds.Y + Convert.ToInt32((e.Bounds.Height - ImageSize) / 2)
                    };
                    try
                    {
                        e.Graphics.DrawImage(_imagelist.Images[e.Index], new RectangleF(point, new SizeF(ImageSize, ImageSize)));
                    }
                    catch { }
                }

                using (var stringFormat = new StringFormat {LineAlignment = StringAlignment.Center} )
                {
                    //TODO: Proper Page Colour handling?
                    var color = e.BackColor.Name == "Highlight"
                        ? Color.FromName("HighlightText")
                        : Color.FromName("ControlText");
                    using (var drawBrush = new SolidBrush(color))
                    {
                        Rectangle rect = new Rectangle
                        {
                            X = e.Bounds.X + ImageSize + 2,
                            Y = e.Bounds.Y,
                            Height = e.Bounds.Height,
                            Width = e.Bounds.Width - ImageSize - 2
                        };
                        e.Graphics.DrawString(drawString, this.Font, drawBrush, rect, stringFormat);
                    }
                }
            }
			catch { }
		}

        private void ValidateItemHeight()
        {
            using (var g = this.CreateGraphics())
            {
                var measuredItemHeight = Convert.ToInt32(g.MeasureString("T", this.Font).Height);
                cmb.ItemHeight = measuredItemHeight < ImageSize ? ImageSize : measuredItemHeight;
            }
        }
        #endregion

        #region Overrides
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ValidateItemHeight();
        }
        #endregion
    }
}
