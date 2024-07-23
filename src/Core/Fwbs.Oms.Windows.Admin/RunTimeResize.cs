using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for RunTimeResize.
    /// </summary>
    public class RunTimeResize
	{
		public enum  NippleMode {acNone,acUDLR,acLR,acUD};
		static Point ClickOffset;

		public static SelectContainer AddNipplesToObject(Control cnt)
		{
			return AddNipplesToObject(cnt,NippleMode.acUDLR);
		}
 
		public static SelectContainer AddNipplesToObject(Control cnt, NippleMode mode)
		{
			cnt.SuspendLayout();
			uBar sTL = new uBar();
			sTL.Location = new Point(0,0);
			sTL.Size = new Size(5,5);
			cnt.Controls.Add(sTL);
			cnt.Controls.SetChildIndex(sTL,0);

			uBar sTR = new uBar();
			sTR.Location = new Point(cnt.Width-5,0);
			sTR.Size = new Size(5,5);
			sTR.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			cnt.Controls.Add(sTR);
			cnt.Controls.SetChildIndex(sTR,0);

			uBar sTM = new uBar();
			sTM.Location = new Point((cnt.Width - 5) / 2,0);
			sTM.Size = new Size(5,5);
			sTM.Anchor = AnchorStyles.Top;
			cnt.Controls.Add(sTM);
			cnt.Controls.SetChildIndex(sTM,0);
			
			uBar sML = new uBar();
			sML.Location = new Point(0,(cnt.Height - 5) / 2);
			sML.Size = new Size(5,5);
			sML.Anchor = AnchorStyles.Left;
			cnt.Controls.Add(sML);
			cnt.Controls.SetChildIndex(sML,0);

			uBar sMR = new uBar();
			sMR.Location = new Point(cnt.Width-5,(cnt.Height - 5) / 2);
			sMR.Size = new Size(5,5);
			sMR.Anchor = AnchorStyles.Right;
			if (mode == NippleMode.acLR || mode == NippleMode.acUDLR)
				AddMoveEvents(sMR,NippleMode.acLR);
			cnt.Controls.Add(sMR);
			cnt.Controls.SetChildIndex(sMR,0);

			uBar sBM = new uBar();
			sBM.Location = new Point((cnt.Width - 5) / 2,cnt.Height-5);
			sBM.Size = new Size(5,5);
			if (mode == NippleMode.acUD || mode == NippleMode.acUDLR)
				AddMoveEvents(sBM,NippleMode.acUD);
			sBM.Anchor = AnchorStyles.Bottom;
			cnt.Controls.Add(sBM);
			cnt.Controls.SetChildIndex(sBM,0);

			uBar sBR = new uBar();
			sBR.Location = new Point(cnt.Width-5,cnt.Height-5);
			sBR.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			sBR.Size = new Size(5,5);
			if (mode == NippleMode.acUDLR)
				AddMoveEvents(sBR,NippleMode.acUDLR);
			cnt.Controls.Add(sBR);
			cnt.Controls.SetChildIndex(sBR,0);

			uBar sBL = new uBar();
			sBL.Location = new Point(0,cnt.Height-5);
			sBL.Size = new Size(5,5);
			sBL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			cnt.Controls.Add(sBL);
			cnt.Controls.SetChildIndex(sBL,0);
			cnt.ResumeLayout();
			return new SelectContainer(sTL,sTR,sTM,sML,sMR,sBL,sBR,sBM);
		}

		static void AddMoveEvents(Control ctl,NippleMode ipos)
		{
			ctl.BackColor = Color.Black;
			ctl.MouseEnter += new System.EventHandler(pnlCanvasNipple_MouseEnter);
			switch(ipos)
			{
				case NippleMode.acLR:
				{
					ctl.MouseMove += new System.Windows.Forms.MouseEventHandler(pnlCanvasNipple_MouseMoveLR);
					ctl.Cursor = Cursors.SizeWE;
					break;
				}
				case NippleMode.acUDLR:
				{
					ctl.MouseMove += new System.Windows.Forms.MouseEventHandler(pnlCanvasNipple_MouseMoveUDLR);
					ctl.Cursor = Cursors.SizeNWSE;
					break;

				}
				case NippleMode.acUD:
				{
					ctl.MouseMove += new System.Windows.Forms.MouseEventHandler(pnlCanvasNipple_MouseMoveUD);
					ctl.Cursor = Cursors.SizeNS;
					break;
				}
			}
			ctl.MouseLeave += new System.EventHandler(pnlCanvasNipple_MouseLeave);
			ctl.MouseDown += new System.Windows.Forms.MouseEventHandler(pnlCanvasNipple_MouseDown);
			ctl.MouseUp += new System.Windows.Forms.MouseEventHandler(pnlCanvasNipple_MouseUp);
		}

		public static void RemoveNipples(Control item, object sc)
		{
			if (sc is SelectContainer)
			{
				RemoveNipples(item,(SelectContainer)sc);
			}
		}

		public static void RemoveNipples(Control item, SelectContainer sc)
		{
			item.Controls.Remove(sc.TL);
			item.Controls.Remove(sc.TR);
			item.Controls.Remove(sc.TM);
			item.Controls.Remove(sc.ML);
			item.Controls.Remove(sc.MR);
			item.Controls.Remove(sc.BL);
			item.Controls.Remove(sc.BR);
			item.Controls.Remove(sc.BM);
		}

		// *************************************************************************************************
		// Nipples
		// *************************************************************************************************
		
		static void pnlCanvasNipple_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Control ocnt = (Control)sender;
			ocnt.Visible = true;
			Control cnt = ((Control)sender).Parent;
		}

		static void pnlCanvasNipple_MouseMoveUDLR(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				ocnt.Visible=false;
				Point p = ((Point)ocnt.Parent.PointToClient(Control.MousePosition));
				p.X = p.X - ClickOffset.X;
				p.Y = p.Y - ClickOffset.Y;
				Point n = p;
				n.Y = n.Y +ocnt.Height;
				n.X = n.X +ocnt.Width;
				ocnt.Parent.Size = (Size)n;
				ocnt.Location = p;
			}
		}

		static void pnlCanvasNipple_MouseMoveUD(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				ocnt.Visible=false;
				Point p = ((Point)ocnt.Parent.PointToClient(Control.MousePosition));
				p.Y = p.Y - ClickOffset.Y;
				p.X = (ocnt.Parent.Width - 5) /2;
				Point n =p;
				n.X = ocnt.Parent.Width;
				n.Y = n.Y + ocnt.Height;
				ocnt.Parent.Size = (Size)n;
				ocnt.Location = p;
			}
		}

		static void pnlCanvasNipple_MouseMoveLR(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				ocnt.Visible=false;
				Point p = ((Point)ocnt.Parent.PointToClient(Control.MousePosition));
				p.X = p.X - ClickOffset.X;
				p.Y = (ocnt.Parent.Height - 5) /2;
				Point n =p;
				n.Y = ocnt.Parent.Height;
				n.X = n.X +ocnt.Width;
				ocnt.Parent.Size = (Size)n;
				ocnt.Location = p;
			}
		}

		static void pnlCanvasNipple_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ClickOffset = new Point(e.X,e.Y);
		}

		static void pnlCanvasNipple_MouseEnter(object sender, System.EventArgs e)
		{
			Control ocnt = (Control)sender;
			ocnt.BackColor = Color.Black;
		}

		static void pnlCanvasNipple_MouseLeave(object sender, System.EventArgs e)
		{
			Control ocnt = (Control)sender;
			ocnt.BackColor = Color.Black;
		}

	}

	/// <summary>
	/// Summary description for uBar.
	/// </summary>
	public class uBar : System.Windows.Forms.Control
	{
		public uBar()
		{
			// uBar
			this.BackColor = System.Drawing.Color.Red;
			this.Size = new System.Drawing.Size(150, 5);
		}
	}
	
	/// <summary>
	/// Value key pair object item.
	/// </summary>
	public class SelectContainer
	{
		public uBar TL;
		public uBar TR;
		public uBar TM;
		public uBar ML;
		public uBar MR;
		public uBar BL;
		public uBar BR;
		public uBar BM;
		public uBar CML;

		public SelectContainer(uBar tl,uBar tr,uBar tm,uBar ml,uBar mr,uBar bl,uBar br,uBar bm)
		{
			TL = tl;
			TR = tr;
			TM = tm;
			ML = ml;
			MR = mr;
			BL = bl;
			BR = br;
			BM = bm;
		}
	}


}
