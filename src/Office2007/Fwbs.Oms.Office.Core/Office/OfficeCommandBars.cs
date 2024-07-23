using System;
using System.Collections.Generic;

namespace Fwbs.Office
{
    using System.Linq;
    using MSOffice = Microsoft.Office.Core;

    public sealed partial class OfficeCommandBars : 
        OfficeObject,
        MSOffice.CommandBars
    {
        private readonly MSOffice.CommandBars bars;
        private Dictionary<int, OfficeCommandBar> items = new Dictionary<int, OfficeCommandBar>() ;

        public OfficeCommandBars(MSOffice.CommandBars bars)
        {
            if (bars == null)
                throw new ArgumentNullException("bars");

            this.bars = bars;

            Init(bars);
        }

        #region Overrides

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (items != null)
                    {
                        foreach (var itm in items.Values)
                        {
                            itm.Dispose();
                        }
                        items.Clear();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region _CommandBars Members

        public Microsoft.Office.Core.CommandBarControl ActionControl
        {
            get { return bars.ActionControl; }
        }

   
        public Microsoft.Office.Core.CommandBar ActiveMenuBar
        {
            get
            {
                var active = bars.ActiveMenuBar;
                if (active == null)
                    return null;

                OfficeCommandBar bar;
                if (items.TryGetValue(active.InstanceId, out bar))
                    return bar;

                return active;
            }
                
        }

        public bool AdaptiveMenus
        {
            get
            {
                return bars.AdaptiveMenus;
            }
            set
            {
                bars.AdaptiveMenus = value;
            }
        }

        public MSOffice.CommandBar Add(object Name, object Position, object MenuBar, object Temporary)
        {
            var bar = bars.Add(Name, Position, MenuBar, Temporary);
            var obar = new OfficeCommandBar(bar);
            items.Add(bar.InstanceId, obar);
            return obar;
        }

        public MSOffice.CommandBar AddEx(object TbidOrName, object Position, object MenuBar, object Temporary, object TbtrProtection)
        {
            var bar = bars.AddEx(TbidOrName, Position, MenuBar, Temporary, TbtrProtection);
            var obar = new OfficeCommandBar(bar);
            items.Add(bar.InstanceId, obar);
            return obar;
        }

        public object Application
        {
            get { return bars.Application; }
        }

        public int Count
        {
            get { return bars.Count; }
        }

        public int Creator
        {
            get { return bars.Creator; }
        }

        public bool DisableAskAQuestionDropdown
        {
            get
            {
                return bars.DisableAskAQuestionDropdown;
            }
            set
            {
                bars.DisableAskAQuestionDropdown = value;
            }
        }

        public bool DisableCustomize
        {
            get
            {
                return bars.DisableCustomize;
            }
            set
            {
                bars.DisableCustomize = value;
            }
        }

        public bool DisplayFonts
        {
            get
            {
                return bars.DisplayFonts;
            }
            set
            {
                bars.DisplayFonts = value;
            }
        }

        public bool DisplayKeysInTooltips
        {
            get
            {
                return bars.DisplayKeysInTooltips;
            }
            set
            {
                bars.DisplayKeysInTooltips = value;
            }
        }

        public bool DisplayTooltips
        {
            get
            {
                return bars.DisplayTooltips;
            }
            set
            {
                bars.DisplayTooltips = value;
            }
        }


        public Microsoft.Office.Core.CommandBarControl FindControl(object Type, object Id, object Tag, object Visible)
        {
            return  bars.FindControl(Type, Id, Tag, Visible);
        }

        public Microsoft.Office.Core.CommandBarControls FindControls(object Type, object Id, object Tag, object Visible)
        {
            return bars.FindControls(Type, Id, Tag, Visible);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var cb in bars.Cast < MSOffice.CommandBar>())
            {
                yield return GetBar(cb);
            }

            yield break;
        }

     
        public bool LargeButtons
        {
            get
            {
                return bars.LargeButtons;
            }
            set
            {
                bars.LargeButtons = value;
            }
        }

        public Microsoft.Office.Core.MsoMenuAnimation MenuAnimationStyle
        {
            get
            {
                return bars.MenuAnimationStyle;
            }
            set
            {
                bars.MenuAnimationStyle = value;
            }
        }

        public object Parent
        {
            get { return bars.Parent; }
        }

        public void ReleaseFocus()
        {
            bars.ReleaseFocus();
        }

        public int get_IdsString(int ids, out string pbstrName)
        {
            return bars.get_IdsString(ids, out pbstrName);
        }

        public int get_TmcGetName(int tmc, out string pbstrName)
        {
            return bars.get_TmcGetName(tmc, out pbstrName);
        }

        public Microsoft.Office.Core.CommandBar this[object Index]
        {
            get 
            {
                var bar = bars[Index];
                if (bar == null)
                    return null;

                return GetBar(bar);

            }
        }

        #endregion

        #region _CommandBarsEvents_Event Members

        public event Microsoft.Office.Core._CommandBarsEvents_OnUpdateEventHandler OnUpdate
        {
            add 
            {
                bars.OnUpdate += value;
            }
            remove
            {
                bars.OnUpdate -= value;
            }
        }

        #endregion

        private MSOffice.CommandBar GetBar(MSOffice.CommandBar bar)
        {
            OfficeCommandBar obar;
            if (items.TryGetValue(bar.InstanceId, out obar))
                return obar;

            return bar;
        }
    }
}
