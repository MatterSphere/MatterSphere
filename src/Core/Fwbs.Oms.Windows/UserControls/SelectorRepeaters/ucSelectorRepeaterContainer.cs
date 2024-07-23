using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Allows the selection of one or more objects depending on a maximum number 
    /// parameter passed.  Objects may be added or created during the process.  
    /// These objects could be describing a contact, associate, precedent job etc...
    /// </summary>
    public class ucSelectorRepeaterContainer : UserControl
	{

		#region Events

		public event SelectorRepeaterChangedEventHandler SelectorRemoved = null;
		public event SelectorRepeaterChangedEventHandler SelectorAdded = null;
		public event SelectorRepeaterChangedEventHandler SelectorMoved = null;

		#endregion

		#region Event Methods

		protected void OnSelectorRemoved(SelectorRepeaterChangedEventArgs e)
		{
			if (SelectorRemoved != null)
			{
				SelectorRemoved(this, e);
			}
		}

		protected void OnSelectorAdded(SelectorRepeaterChangedEventArgs e)
		{
			if (SelectorAdded != null)
			{
				SelectorAdded(this, e);
			}
		}

		protected void OnSelectorMoved(SelectorRepeaterChangedEventArgs e)
		{
			if (SelectorMoved != null)
			{
				SelectorMoved(this, e);
			}
		}

		#endregion

		#region Control Fields

		protected FWBS.OMS.UI.Windows.ResourceLookup resLKP;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ErrorProvider req;
		protected System.Windows.Forms.Panel pnlContainer;
		protected System.Windows.Forms.ToolStrip toolBar1;
		protected System.Windows.Forms.ToolStripButton tbAdd;
		protected System.Windows.Forms.ToolStripButton tbRemove;
		protected System.Windows.Forms.ToolStripSeparator tbSep;
		protected System.Windows.Forms.ToolStripButton tbAssign;
		protected System.Windows.Forms.ToolStripButton tbRevoke;
		protected System.Windows.Forms.ToolStripButton tbFind;
		protected System.Windows.Forms.ToolStripSeparator tbSep2;
		protected System.Windows.Forms.ToolStripButton tbUp;
		protected System.Windows.Forms.ToolStripButton tbDown;
		protected System.Windows.Forms.ToolStripButton tbRemoveAll;

		#endregion

		#region Fields

		/// <summary>
		/// The type of ISelectorRepeater control.
		/// </summary>
		private Type _type = null;

		/// <summary>
		/// The minumum number of items allowed to be assigned to.
		/// </summary>
		private int _minimumCount = 1;

		/// <summary>
		/// The maximum number of items allowed to be assigned to.
		/// </summary>
		private int _maximumCount = 0;
		
		/// <summary>
		/// Activates the toggle selection routine.
		/// </summary>
		private bool _toggleSelect = false;

		/// <summary>
		/// Activates the multi select option.
		/// </summary>
		private bool _multiselect = false;


        /// <summary>
        /// A parameters array to pass onto the children controls.
        /// </summary>
        private object [] _params = new object[0];

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ucSelectorRepeaterContainer()
		{
			InitializeComponent();
            toolBar1.ImageList = ImageListSelector.GetImageList();
			if (Session.CurrentSession.IsLoggedIn) CheckButtonState();
		}

		/// <summary>
		/// Contructs the container control specifying the type of conmtrol to use.
		/// </summary>
		/// <param name="type">The type of control.</param>
		public ucSelectorRepeaterContainer(Type type) : this()
		{	
			_type = type;
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tbAdd = new System.Windows.Forms.ToolStripButton();
            this.tbRemove = new System.Windows.Forms.ToolStripButton();
            this.tbAssign = new System.Windows.Forms.ToolStripButton();
            this.tbRevoke = new System.Windows.Forms.ToolStripButton();
            this.tbFind = new System.Windows.Forms.ToolStripButton();
            this.tbUp = new System.Windows.Forms.ToolStripButton();
            this.tbDown = new System.Windows.Forms.ToolStripButton();
            this.tbRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.req = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.toolBar1 = new System.Windows.Forms.ToolStrip();
            this.tbSep = new System.Windows.Forms.ToolStripSeparator();
            this.tbSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.resLKP = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.req)).BeginInit();
            this.toolBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAdd
            // 
            this.tbAdd.Enabled = false;
            this.resLKP.SetLookup(this.tbAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("Add", "Add", ""));
            this.tbAdd.Name = "tbAdd";
            this.tbAdd.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbAdd.Size = new System.Drawing.Size(23, 6);
            // 
            // tbRemove
            // 
            this.tbRemove.Enabled = false;
            this.resLKP.SetLookup(this.tbRemove, new FWBS.OMS.UI.Windows.ResourceLookupItem("Remove", "Remove", ""));
            this.tbRemove.Name = "tbRemove";
            this.tbRemove.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbRemove.Size = new System.Drawing.Size(23, 6);
            // 
            // tbAssign
            // 
            this.tbAssign.Enabled = false;
            this.resLKP.SetLookup(this.tbAssign, new FWBS.OMS.UI.Windows.ResourceLookupItem("Assign", "Assign", ""));
            this.tbAssign.Name = "tbAssign";
            this.tbAssign.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbAssign.Size = new System.Drawing.Size(23, 6);
            // 
            // tbRevoke
            // 
            this.tbRevoke.Enabled = false;
            this.resLKP.SetLookup(this.tbRevoke, new FWBS.OMS.UI.Windows.ResourceLookupItem("Revoke", "Revoke", ""));
            this.tbRevoke.Name = "tbRevoke";
            this.tbRevoke.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbRevoke.Size = new System.Drawing.Size(23, 6);
            // 
            // tbFind
            // 
            this.tbFind.Enabled = false;
            this.resLKP.SetLookup(this.tbFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("Find", "Find", ""));
            this.tbFind.Name = "tbFind";
            this.tbFind.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbFind.Size = new System.Drawing.Size(23, 6);
            // 
            // tbUp
            // 
            this.tbUp.Enabled = false;
            this.resLKP.SetLookup(this.tbUp, new FWBS.OMS.UI.Windows.ResourceLookupItem("Up", "Up", ""));
            this.tbUp.Name = "tbUp";
            this.tbUp.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbUp.Size = new System.Drawing.Size(23, 6);
            // 
            // tbDown
            // 
            this.tbDown.Enabled = false;
            this.resLKP.SetLookup(this.tbDown, new FWBS.OMS.UI.Windows.ResourceLookupItem("Down", "Down", ""));
            this.tbDown.Name = "tbDown";
            this.tbDown.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbDown.Size = new System.Drawing.Size(23, 6);
            // 
            // tbRemoveAll
            // 
            this.resLKP.SetLookup(this.tbRemoveAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("REMOVEALL", "Remove All", ""));
            this.tbRemoveAll.Name = "tbRemoveAll";
            this.tbRemoveAll.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbRemoveAll.Size = new System.Drawing.Size(23, 6);
            // 
            // req
            // 
            this.req.ContainerControl = this;
            // 
            // pnlContainer
            // 
            this.pnlContainer.AutoScroll = true;
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(3, 27);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pnlContainer.Size = new System.Drawing.Size(410, 163);
            this.pnlContainer.TabIndex = 1;
            // 
            // toolBar1
            // 
            this.toolBar1.BackColor = System.Drawing.Color.White;
            this.toolBar1.CanOverflow = false;
            this.toolBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbAdd,
            this.tbRemove,
            this.tbRemoveAll,
            this.tbSep,
            this.tbAssign,
            this.tbRevoke,
            this.tbFind,
            this.tbSep2,
            this.tbUp,
            this.tbDown});
            this.toolBar1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolBar1.Location = new System.Drawing.Point(3, 2);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Padding = new System.Windows.Forms.Padding(1, 0, 1, 2);
            this.toolBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar1.ShowItemToolTips = false;
            this.toolBar1.Size = new System.Drawing.Size(410, 25);
            this.toolBar1.Stretch = true;
            this.toolBar1.TabIndex = 3;
            this.toolBar1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbSep
            // 
            this.tbSep.Name = "tbSep";
            this.tbSep.Size = new System.Drawing.Size(6, 23);
            // 
            // tbSep2
            // 
            this.tbSep2.Name = "tbSep2";
            this.tbSep2.Size = new System.Drawing.Size(6, 23);
            // 
            // ucSelectorRepeaterContainer
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.toolBar1);
            this.Name = "ucSelectorRepeaterContainer";
            this.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Size = new System.Drawing.Size(416, 192);
            this.SizeChanged += new System.EventHandler(this.ucSelectorRepeaterContainer_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucSelectorRepeaterContainer_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.req)).EndInit();
            this.toolBar1.ResumeLayout(false);
            this.toolBar1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion


		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the minimum number of items allowed to be assigned to.
		/// </summary>
		[Browsable(false)]
		public int MinimumCount
		{
			get
			{
				return _minimumCount;
			}
		}

		/// <summary>
		/// Gets the maximum number of items allowed to be assigned to.
		/// </summary>
		[Browsable(false)]
		public int MaximumCount
		{
			get
			{
				return _maximumCount;
			}
		}

		/// <summary>
		/// Gets or Sets the toggle select option.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		public bool ToggleSelection
		{
			get
			{
				return _toggleSelect;
			}
			set
			{
				_toggleSelect = value;
			}
		}

		
		/// <summary>
		/// Gets or Set s the multi select option.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		public bool MultiSelect
		{
			get
			{
				return _multiselect;
			}
			set
			{
				_multiselect = value;
			}
		}

		/// <summary>
		/// Gets the number of objects to fill in.  This will be an iteration
		/// of ucContactSelector controls within this container.
		/// </summary>
		[Browsable(false)]
		public int ObjectCount
		{
			get
			{
				return pnlContainer.Controls.Count;
			}
		}

		/// <summary>
		/// Gets or Sets the default object.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public object DefaultObject
		{
			get
			{
				try
				{
					return ((Interfaces.ISelectorRepeater)Default).Object;
				}
				catch
				{
					return null;
				}
			}
			set
			{
				((Interfaces.ISelectorRepeater)Default).Object = value;
			}
		}

		/// <summary>
		/// Gets the topmost default control.
		/// </summary>
		[Browsable(false)]
		private Control Default
		{
			get
			{
				if (ObjectCount == 0)
					return Add(null);
				else
					return pnlContainer.Controls[ObjectCount - 1];
			}
		}

		/// <summary>
		/// Gets an array of objects other than the default one that has been selected.
		/// </summary>
		[Browsable(false)]
		public object [] OtherObjects
		{
			get
			{
				object [] objects;
				ArrayList objs = new ArrayList();
				for (int ctr = ObjectCount - 1; ctr >= 0; ctr--)
				{
					Interfaces.ISelectorRepeater ctrl = (Interfaces.ISelectorRepeater)pnlContainer.Controls[ctr];
					if (Default != ctrl)
					{
						if (ctrl.Object != null)
							objs.Add(ctrl.Object);
					}
				}
				
				objects = new object[objs.Count];
				objs.CopyTo(objects);
				return objects;
			}
		}

		/// <summary>
		/// Gets or Sets the type of the control.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public Type SelectorRepeaterType
		{
			get
			{
				return _type;
			}
			set
			{
				if (value != _type)
				{
					_type = value;
					Clear();
					Add(null);
					Interfaces.ISelectorRepeater rpt = (Interfaces.ISelectorRepeater)Default;
					rpt.Select();
					tbAssign.Visible = rpt.HasMethod(SelectorRepeaterMethods.Assign);
					tbRevoke.Visible = rpt.HasMethod(SelectorRepeaterMethods.Revoke);
					tbFind.Visible = rpt.HasMethod(SelectorRepeaterMethods.Find);
					tbRemoveAll.Visible = false;
					tbSep2.Visible = (tbAssign.Visible || tbRevoke.Visible || tbFind.Visible);
				}
				CheckButtonState();
			}
		}

		/// <summary>
		/// Gets or Sets the visibility of the toolbar.
		/// </summary>
		[DefaultValue(true)]
		[Browsable(true)]
		public virtual bool ShowToolBar
		{
			get
			{
				return toolBar1.Visible;
			}
			set
			{
				toolBar1.Visible = value;
			}
		}

		/// <summary>
		/// Gets the number of ISelectorRepeater controls that are currently selected.
		/// </summary>
		[Browsable(false)]
		public int SelectedCount
		{
			get
			{
				int count = 0;
				foreach (Interfaces.ISelectorRepeater sel in pnlContainer.Controls)
				{
					if (sel.IsSelected)
						count ++;
				}
				return count;

			}
		}

		/// <summary>
		/// Gets the currently selected repeater control.
		/// </summary>
		[Browsable(false)]
		public Interfaces.ISelectorRepeater [] Selected
		{
			get
			{
				Interfaces.ISelectorRepeater [] selected = new Interfaces.ISelectorRepeater [SelectedCount];
				int ctr = 0;
				foreach (Interfaces.ISelectorRepeater sel in pnlContainer.Controls)
				{
					if (sel.IsSelected)
					{
						selected[ctr] = sel;
						ctr++;
					}
				}
				return selected;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the parameter array of info.
		/// </summary>
		/// <param name="param">The parameter setting methods.</param>
		public void SetInfo(params object [] param)
		{
			_params = param;
		}

		/// <summary>
		/// Runs a selector method.
		/// </summary>
		/// <param name="method">Method type.</param>
		internal void RunMethod(SelectorRepeaterMethods method)
		{
			try
			{
				((Interfaces.ISelectorRepeater)Default).RunMethod(method);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Clears all of the controls.
		/// </summary>
		public virtual void Clear()
		{
			Global.RemoveAndDisposeControls(pnlContainer);
		}

		/// <summary>
		/// Refreshes the object layout.
		/// </summary>
		/// <param name="count">The number of controls to create.</param>
		private void RefreshObjects(int count)
		{
			ArrayList objs = new ArrayList();

			if (count > ObjectCount)
			{
				for (int ctr = ObjectCount - 1; ctr >= 0; ctr--)
				{
					object obj = ((Interfaces.ISelectorRepeater)pnlContainer.Controls[ctr]).Object;
					if (obj != null)
						objs.Add(obj);
					Remove(pnlContainer.Controls[ctr]);
				}
			}

			for (int ctr = count ; ctr > 0; ctr--)
			{
				int idx = ctr - 1;
				Control ctrl = null;
				
				if (ctr > ObjectCount)
					ctrl = Add(null);
				else
					ctrl = pnlContainer.Controls[idx];
				
				if (ctrl == null) ctrl = Add(null);
			}

			CheckButtonState();

		}

		/// <summary>
		/// Removes a control from the collection.
		/// </summary>
		/// <param name="selector">Selector control to remove.</param>
		public void Remove(Control selector)
		{
			if (ObjectCount > MinimumCount)
			{
				Interfaces.ISelectorRepeater sel = (Interfaces.ISelectorRepeater)selector;
				sel.Selected -= new EventHandler(SelectEvent);
				sel.UnSelected -= new EventHandler(UnSelectEvent);
				OnSelectorRemoved(new SelectorRepeaterChangedEventArgs(sel, GetOrdinalValue(sel)));
				pnlContainer.Controls.Remove(selector);
				CheckButtonState();
			}
		}

		/// <summary>
		/// Gets the ordinal value of the selector control.
		/// </summary>
		/// <param name="selector">The selecot control to test fot.</param>
		/// <returns>An ordinal number from 0 to control count - 1.</returns>
		public int GetOrdinalValue(Interfaces.ISelectorRepeater selector)
		{
			return (ObjectCount - 1 - pnlContainer.Controls.GetChildIndex((Control)selector));
		}

		/// <summary>
		/// Adds a new control to the bottom of the list.
		/// </summary>
		/// <param name="obj">The default object to assign to the control.</param>
		/// <param name="param">Passes object parameters to the control.</param>
		/// <returns>The control reference of the newly created control.</returns>
		public Control Add(object obj)
		{
			if (ObjectCount == 1 && DefaultObject == null)
			{
				DefaultObject = obj;
			}
			if ((ObjectCount == 0) || (ObjectCount < MaximumCount) || (MaximumCount < MinimumCount))
			{
				Control ctrl = null;
                ctrl = (Control)_type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
				pnlContainer.Controls.Add(ctrl, true);
				ctrl.Dock = DockStyle.Top;
				ctrl.TabIndex = ObjectCount - 1;
				ctrl.RightToLeft = this.RightToLeft;
				ctrl.BringToFront();
				Interfaces.ISelectorRepeater sel = (Interfaces.ISelectorRepeater)ctrl;
				sel.Selected += new EventHandler(SelectEvent);
				sel.Closed += new EventHandler(SelectClosed);
				sel.UnSelected += new EventHandler(UnSelectEvent);
				sel.UnSelecting += new CancelEventHandler(UnSelectingEvent);
				CheckButtonState();
				OnSelectorAdded(new SelectorRepeaterChangedEventArgs(sel, GetOrdinalValue(sel)));
				((Interfaces.ISelectorRepeater)ctrl).SetInfo(_params);
				((Interfaces.ISelectorRepeater)ctrl).Object = obj;
				return ctrl;
			}
			return null;
		}

		/// <summary>
		/// Selects the control and highlights the first control.
		/// </summary>
		/// <param name="selectFirst">Selects the first control in the repeater.</param>
		public void Select (bool selectFirst)
		{
			base.Select();
			if (selectFirst)
			{
				if (ObjectCount > 0) ((Interfaces.ISelectorRepeater)Default).Select();
			}
		}

		/// <summary>
		/// Removes the Selected Control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SelectClosed(object sender, EventArgs e)
		{
			this.Remove((Control)sender);
		}


		/// <summary>
		/// A method that gets raised if the selector control is selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SelectEvent(object sender, EventArgs e)
		{
			Interfaces.ISelectorRepeater ctrl = (Interfaces.ISelectorRepeater)sender;
			bool temp = ToggleSelection;
			ToggleSelection = true;
			if (MultiSelect == false)
			{
				foreach (Interfaces.ISelectorRepeater sel in Selected)
					if (sel != ctrl) sel.UnSelect();
			}
			ToggleSelection = temp;
			CheckButtonState();
		}

		/// <summary>
		/// A method that gets raised if the selector control is deselected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UnSelectEvent(object sender, EventArgs e)
		{
			CheckButtonState();
		}

		/// <summary>
		/// A method that gets raised if the selector control is deselected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UnSelectingEvent(object sender, CancelEventArgs e)
		{
			e.Cancel = !ToggleSelection;
		}

		/// <summary>
		/// This method will get raised whenever one of the toolbar buttons ever get clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
			try
			{
				Interfaces.ISelectorRepeater[] selected = Selected;

				if (e.ClickedItem == tbAdd)
				{
					Interfaces.ISelectorRepeater sel = Add(null) as Interfaces.ISelectorRepeater;
					if (sel != null)
					{
						sel.Select();
						sel.RunMethod(SelectorRepeaterMethods.New);
						if (sel.Object == null)
							Remove((Control)sel);
					}
				}
				else if (e.ClickedItem == tbRemove)
				{
					foreach(Interfaces.ISelectorRepeater s in selected)
						Remove((Control)s);
				}
				else if (e.ClickedItem == tbAssign)
				{
					if (selected.Length > 0) selected[0].RunMethod(SelectorRepeaterMethods.Assign);
				}
				else if (e.ClickedItem == tbRevoke)
				{
					foreach(Interfaces.ISelectorRepeater s in selected)
						s.RunMethod(SelectorRepeaterMethods.Revoke);
				}
				else if (e.ClickedItem == tbFind)
				{
					if (selected.Length > 0) selected[0].RunMethod(SelectorRepeaterMethods.Find);
				}
				else if (e.ClickedItem == tbUp)
				{
					if (selected.Length > 0)
					{
						Control current = (Control)selected[0];

						int CurrentIdx = pnlContainer.Controls.GetChildIndex(current);
						if (CurrentIdx < ObjectCount - 1)
						{
							pnlContainer.Controls.SetChildIndex(current, CurrentIdx + 1);				
							selected[0].Select();
							OnSelectorMoved(new SelectorRepeaterChangedEventArgs(selected[0], GetOrdinalValue(selected[0])));
						}
						pnlContainer.ScrollControlIntoView(current);
					}
				}
				else if (e.ClickedItem == tbDown)
				{
					if (selected.Length > 0)
					{
						Control current = (Control)selected[0];
						int CurrentIdx = pnlContainer.Controls.GetChildIndex(current);
						if (CurrentIdx > 0)
						{
							pnlContainer.Controls.SetChildIndex(current, CurrentIdx - 1);				
							OnSelectorMoved(new SelectorRepeaterChangedEventArgs(selected[0], GetOrdinalValue(selected[0])));
						}
						selected[0].Select();
						pnlContainer.ScrollControlIntoView(current);
					}
				}
				else if (e.ClickedItem == tbRemoveAll)
				{
					for (int ctr = ObjectCount - 1; ctr >= 0; ctr--)
					{
						Control ctrl = pnlContainer.Controls[ctr];
						Remove(ctrl);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}


		/// <summary>
		/// UnSelects all the selector controls.
		/// </summary>
		public void UnSelectAll()
		{
			foreach (Interfaces.ISelectorRepeater sel in Selected)
				sel.UnSelect();
			CheckButtonState();
		}

		/// <summary>
		/// Enables or disables the toolbar buttons depending on the current selected
		/// item situation.
		/// </summary>
		protected virtual void CheckButtonState()
		{
			int sel_count = SelectedCount;

			if (sel_count > 1)
			{
				foreach (ToolStripItem btn in toolBar1.Items)
				{
					btn.Enabled = true;
				}
				tbAssign.Enabled = false;
				tbFind.Enabled = false;
			}
			else if (sel_count == 1)
			{
				foreach (ToolStripItem btn in toolBar1.Items)
				{
					btn.Enabled = true;
				}
			}
			else if (sel_count < 1)
			{
				foreach (ToolStripItem btn in toolBar1.Items)
				{
					btn.Enabled = false;
				}
				tbAdd.Enabled = true;
			}

			if ((ObjectCount == 0) || (ObjectCount < MaximumCount) || (MaximumCount < MinimumCount))
				tbAdd.Enabled = true;
			else
				tbAdd.Enabled = false;

			if (ObjectCount < MinimumCount)
				tbRemove.Enabled = false;

			tbRemoveAll.Enabled = (ObjectCount >  0) ;

		}


		/// <summary>
		/// Sets the minimum and maximum bounds of the container control.
		/// </summary>
		/// <param name="minimumCount">The minimum number of controls to be assigned to.</param>
		/// <param name="maximumCount">The maximum number of controls to be assigned to.</param>
		public void SetCountBounds(int minimumCount, int maximumCount)
		{
			_minimumCount = minimumCount;
			_maximumCount = maximumCount;
			if (_minimumCount < 1) _minimumCount = 0;
			
			if (_minimumCount > ObjectCount)
				RefreshObjects(_minimumCount);

			if (_maximumCount < ObjectCount || _maximumCount < _minimumCount)
				RefreshObjects(_maximumCount);
		}

		/// <summary>
		/// Validates that minimum count of objects have been assigned to.
		/// </summary>
		public void ValidateObjects()
		{
			if (((Interfaces.ISelectorRepeater)Default).Object == null)
				throw new OMSException2("ERDEFOBJMUSTASS", "The default object must be assigned to.");

			int count = 0;
			foreach (Interfaces.ISelectorRepeater sel in pnlContainer.Controls)
			{
				if (sel.Object != null)
					count++;
			}
			if (count < MinimumCount)
				throw new OMSException2("ERRMINIMUM_N", "Please assign a minimum of %1%", null, false,MinimumCount.ToString());
			else if (count > MaximumCount && MaximumCount > MinimumCount)
                throw new OMSException2("ERRMAXIMUM_N", "Please assign a maximum of %1%", null, false,MaximumCount.ToString());
		}

		#endregion

		private void ucSelectorRepeaterContainer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.SystemColors.ControlDark,1),0,0,this.Width-1,this.Height-1);
		}

		private void ucSelectorRepeaterContainer_SizeChanged(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

	}


	public delegate void SelectorRepeaterChangedEventHandler (ucSelectorRepeaterContainer sender, SelectorRepeaterChangedEventArgs e);

	public class SelectorRepeaterChangedEventArgs : EventArgs
	{
		private Interfaces.ISelectorRepeater _selector = null;
		private int _ordinalVal = 0;

		private SelectorRepeaterChangedEventArgs (){}
		internal protected  SelectorRepeaterChangedEventArgs (Interfaces.ISelectorRepeater selector, int ordinal)
		{
			_selector = selector;
			_ordinalVal = ordinal;
		}

		public Interfaces.ISelectorRepeater Selector
		{
			get
			{
				return _selector;
			}
		}

		public int Ordinal
		{
			get
			{
				return _ordinalVal;
			}
		}
	}
}
