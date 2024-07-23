using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace FWBS.Common.UI.Windows
{

    /*
    /// <summary>
    /// ****************************************************************************************************
    /// THE ALL IMPORTANT EXTENDED COLOUR CLASS
    /// ****************************************************************************************************
    /// </summary>
    /// */

    public enum ExtColorPresets { None, TaskPainBackColor, TaskPainBoxHeaderForeColor, TaskPainBoxHeaderBackColor, TaskPainBoxHeaderForeColorLight, TaskPainBoxBackColor, TaskPainBoxLinkForeColor, FrameLineForeColor, FrameForeColor, TaskPainBoxTopHeaderBlend1, TaskPainBoxTopHeaderBlend2, TaskPainBoxTopHeaderForeColor, TaskPainBoxTopHeaderForeColorLight, TaskPainBoxHeaderBlend, SearchPainBackColor }
    public enum ExtColorTheme { None, Auto, Blue, Silver, OliveGreen, Classic, OfficeBlack, OfficeBlue, OfficeSilver, Windows8 };

    [System.ComponentModel.TypeConverter(typeof(ExtColorConverter))]
    [Editor(typeof(ColorDisplayEditor), typeof(UITypeEditor))]
    public class ExtColor
    {
        /// <summary>
        /// The Override Colour
        /// </summary>
        private System.Drawing.Color _color = Color.Empty;
        /// <summary>
        /// The Return Colour
        /// </summary>
        private System.Drawing.Color _retcolor;
        /// <summary>
        /// The Colour Presets
        /// </summary>
        private ExtColorPresets _presets = ExtColorPresets.None;
        /// <summary>
        /// The The Current Systems Color Theme
        /// </summary>
        private ExtColorTheme _xptheme = ExtColorTheme.Auto;
        /// <summary>
        /// The Operatings System Color Theme
        /// </summary>
        private ExtColorTheme _currenttheme = ExtColorTheme.None;
        /// <summary>
        /// A Hashtable to store the Colours agains the Preset Names
        /// </summary>
        private Dictionary<string, Color> _lookups = new Dictionary<string, Color>();

        /// <summary>
        /// Event Handler that is called when a property has been changed
        /// </summary>
        public event EventHandler SettingsChanged = null;

        public ExtColor()
        {
        }

        /// <summary>
        /// Contructs a Extended Colour Object passing Overide Color and Event Handler
        /// </summary>
        /// <param name="color">Override Colour</param>
        public ExtColor(System.Drawing.Color color)
            : this(color, ExtColorPresets.None, ExtColorTheme.None)
        {
        }

        /// <summary>
        /// Contructs a Extended Colour Object with a Colour Preset and Theme
        /// </summary>
        /// <param name="presets">The Colour Preset</param>
        /// <param name="theme">The Operating Systems Theme</param>
        public ExtColor(ExtColorPresets presets, ExtColorTheme theme)
            : this(Color.Empty, presets, theme)
        {
        }


        /// <summary>
        /// Internal Contructor to Create a Extended Colour Object
        /// </summary>
        /// <param name="color">The Override Colour</param>
        /// <param name="presets">The Preset Colour</param>
        /// <param name="theme">The Preset Theme</param>
        internal ExtColor(System.Drawing.Color color, ExtColorPresets presets, ExtColorTheme theme) : this()
        {
            _xptheme = theme;
            if (_xptheme != ExtColorTheme.Auto)
                _currenttheme = _xptheme;


            RefreshColors();

            _presets = presets;
            _color = color;

            if (_presets != ExtColorPresets.None)
            {
                if (_lookups.ContainsKey(_presets.ToString()))
                    _retcolor = _lookups[_presets.ToString()];

            }
            else
                _retcolor = color;
            OnSettingsChanged();
        }

        [Browsable(false)]
        public Color Color
        {
            get
            {
                return _retcolor;
            }
        }

        [Browsable(false)]
        public ExtColorTheme CurrentTheme
        {
            get
            {
                return _currenttheme;
            }
        }

        public void OnSettingsChanged()
        {
            if (SettingsChanged != null)
                SettingsChanged(this, EventArgs.Empty);
        }


        public void RefreshColors()
        {
            _lookups.Clear();
            _currenttheme = ExtColorTheme.Windows8;

            _lookups.Add("None", Color.Empty);

            // If the _xptheme is NOT Automatic then set to the one in the Property
            if (_xptheme != ExtColorTheme.Auto) _currenttheme = _xptheme;

            if (_currenttheme == ExtColorTheme.Blue)
            {
                _lookups.Add("FrameLineForeColor", Color.FromArgb(208, 208, 191));
                _lookups.Add("FrameForeColor", Color.FromArgb(0, 70, 213));
            }
            else if (_currenttheme == ExtColorTheme.Classic)
            {
                _lookups.Add("FrameLineForeColor", SystemColors.ControlDark);
                _lookups.Add("FrameForeColor", Color.FromArgb(0, 70, 213));
            }

            if (_currenttheme == ExtColorTheme.Blue || _currenttheme == ExtColorTheme.None || _currenttheme == ExtColorTheme.Classic)
            {
                #region Blue
                _lookups.Add("TaskPainForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(101, 122, 215));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(33, 93, 198));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", Color.FromArgb(66, 142, 255));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(199, 211, 247));
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(214, 223, 247));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(33, 93, 198));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(199, 211, 247));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(0, 73, 181));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(41, 93, 206));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", Color.FromArgb(66, 142, 255));
                _lookups.Add("SearchPainBackColor", Color.FromArgb(214, 223, 247));
                #endregion
            }
            else if (_currenttheme == ExtColorTheme.Silver)
            {
                #region Silver
                _lookups.Add("TaskPainForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(180, 182, 201));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", Color.FromArgb(126, 124, 124));
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(240, 241, 245));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("FrameLineForeColor", Color.FromArgb(191, 184, 191));
                _lookups.Add("FrameForeColor", Color.FromArgb(0, 70, 213));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(214, 215, 224));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(214, 215, 224));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(119, 119, 146));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(179, 181, 198));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", Color.FromArgb(230, 230, 230));
                _lookups.Add("SearchPainBackColor", Color.FromArgb(240, 241, 245));
                #endregion
            }
            else if (_currenttheme == ExtColorTheme.Windows8)
            {
                #region Windows8
                _lookups.Add("TaskPainForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("FrameLineForeColor", Color.FromArgb(97, 49, 49));
                _lookups.Add("FrameForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(255, 255, 255));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", Color.FromArgb(0, 0, 0));
                _lookups.Add("SearchPainBackColor", Color.FromArgb(255, 255, 255));

                _lookups.Add("FrameTitleBackColor", Color.FromArgb(245, 245, 245));
                _lookups.Add("FrameBorderColor", Color.FromArgb(218, 222, 214));
                _lookups.Add("FrameBackColor", Color.White);

                #endregion
            }
            else if (_currenttheme == ExtColorTheme.OliveGreen)
            {
                #region OliveGreen
                _lookups.Add("TaskPainForeColor", Color.FromArgb(153, 84, 10));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(200, 214, 169));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(86, 102, 45));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", Color.FromArgb(114, 146, 29));
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(246, 246, 236));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("FrameLineForeColor", Color.FromArgb(208, 208, 191));
                _lookups.Add("FrameForeColor", Color.FromArgb(153, 84, 10));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(224, 231, 184));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(224, 231, 184));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(119, 140, 64));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(150, 168, 103));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", Color.FromArgb(224, 231, 184));
                _lookups.Add("SearchPainBackColor", Color.FromArgb(246, 246, 236));
                #endregion
            }
            else if (_currenttheme == ExtColorTheme.OfficeBlue)
            {
                #region Blue
                _lookups.Add("FrameLineForeColor", Color.FromArgb(208, 208, 191));
                _lookups.Add("FrameForeColor", Color.FromArgb(21, 66, 139));

                _lookups.Add("TaskPainForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(185, 212, 243));
                _lookups.Add("TaskPainBoxHeaderBlend1", Color.FromArgb(161, 185, 239));
                _lookups.Add("TaskPainBoxHeaderBlend2", Color.FromArgb(227, 235, 245));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(161, 185, 239));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(21, 66, 139));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", Color.Orange);
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(33, 93, 198));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(199, 211, 247));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(161, 185, 239));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(227, 235, 245));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(21, 66, 139));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", Color.Orange);
                _lookups.Add("SearchPainBackColor", Color.FromArgb(214, 223, 247));
                #endregion

            }
            else if (_currenttheme == ExtColorTheme.OfficeBlack)
            {
                #region Black
                _lookups.Add("FrameLineForeColor", Color.FromArgb(208, 208, 191));
                _lookups.Add("FrameForeColor", Color.FromArgb(0, 0, 0));

                _lookups.Add("TaskPainForeColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(83, 83, 83));
                _lookups.Add("TaskPainBoxHeaderBlend1", Color.FromArgb(120, 120, 120));
                _lookups.Add("TaskPainBoxHeaderBlend2", Color.FromArgb(152, 152, 152));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(161, 185, 239));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", System.Drawing.Color.Orange);
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(189, 189, 189));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(33, 93, 198));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(199, 211, 247));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(120, 120, 120));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(152, 152, 152));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", System.Drawing.Color.Orange);
                _lookups.Add("SearchPainBackColor", Color.FromArgb(214, 223, 247));
                #endregion
            }
            else if (_currenttheme == ExtColorTheme.OfficeSilver)
            {
                _lookups.Add("TaskPainForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBackColor", Color.FromArgb(180, 182, 201));
                _lookups.Add("TaskPainBoxHeaderForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("TaskPainBoxHeaderForeColorLight", System.Drawing.Color.Orange);
                _lookups.Add("TaskPainBoxBackColor", Color.FromArgb(240, 241, 245));
                _lookups.Add("TaskPainBoxLinkForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("FrameLineForeColor", Color.FromArgb(191, 184, 191));
                _lookups.Add("FrameForeColor", Color.FromArgb(0, 0, 0));
                _lookups.Add("TaskPainBoxHeaderBackColor", Color.FromArgb(214, 215, 224));
                // Task Pain Box Header Top
                _lookups.Add("TaskPainBoxHeaderBlend", Color.FromArgb(214, 215, 224));
                _lookups.Add("TaskPainBoxTopHeaderBlend1", Color.FromArgb(255, 255, 255));
                _lookups.Add("TaskPainBoxTopHeaderBlend2", Color.FromArgb(214, 215, 224));
                _lookups.Add("TaskPainBoxTopHeaderForeColor", Color.FromArgb(63, 61, 61));
                _lookups.Add("TaskPainBoxTopHeaderForeColorLight", System.Drawing.Color.Orange);
                _lookups.Add("SearchPainBackColor", Color.FromArgb(240, 241, 245));
            }
        }

        public Color this[string Name]
        {
            get
            {
                if (_lookups.ContainsKey(Name))
                    return _lookups[Name];
                else
                    return Color.Empty;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public ExtColorTheme ThemeXP
        {
            get
            {
                return _xptheme;
            }
            set
            {
                if (_xptheme != value)
                {
                    _xptheme = value;
                    if (_xptheme != ExtColorTheme.Auto)
                        _currenttheme = _xptheme;
                    RefreshColors();
                    if (_presets != ExtColorPresets.None)
                        Presets = _presets;
                    OnSettingsChanged();
                }
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public System.Drawing.Color SetColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                _retcolor = value;
                _presets = ExtColorPresets.None;
                _xptheme = ExtColorTheme.None;
                OnSettingsChanged();
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        public ExtColorPresets Presets
        {
            get
            {
                return _presets;
            }
            set
            {
                if (_presets != value)
                {
                    _presets = value;
                    if (_lookups.ContainsKey(_presets.ToString()))
                        _retcolor = _lookups[_presets.ToString()];
                    else
                        _retcolor = _lookups["None"];
                    _color = Color.Empty;
                    if (_xptheme == ExtColorTheme.None && _presets != ExtColorPresets.None) 
                        _xptheme = ExtColorTheme.Auto;
                    OnSettingsChanged();
                }
            }
        }

        #region Favorite Properties
        [Browsable(false)]
        public Color TaskPainBoxHeaderBackColor
        {
            get
            {
                return this["TaskPainBoxHeaderBackColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBackColor
        {
            get
            {
                return this["TaskPainBackColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainForeColor
        {
            get
            {
                return this["TaskPainForeColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxHeaderForeColor
        {
            get
            {
                return this["TaskPainBoxHeaderForeColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxHeaderForeColorLight
        {
            get
            {
                return this["TaskPainBoxHeaderForeColorLight"];
            }
        }


        [Browsable(false)]
        public Color TaskPainBoxBackColor
        {
            get
            {
                return this["TaskPainBoxBackColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxLinkForeColor
        {
            get
            {
                return this["TaskPainBoxLinkForeColor"];
            }
        }

        [Browsable(false)]
        public Color FrameLineForeColor
        {
            get
            {
                return this["FrameLineForeColor"];
            }
        }

        [Browsable(false)]
        public Color FrameForeColor
        {
            get
            {
                return this["FrameForeColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxTopHeaderBlend1
        {
            get
            {
                return this["TaskPainBoxTopHeaderBlend1"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxTopHeaderBlend2
        {
            get
            {
                return this["TaskPainBoxTopHeaderBlend2"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxTopHeaderForeColor
        {
            get
            {
                return _lookups["TaskPainBoxTopHeaderForeColor"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxTopHeaderForeColorLight
        {
            get
            {
                return this["TaskPainBoxTopHeaderForeColorLight"];
            }
        }

        [Browsable(false)]
        public Color TaskPainBoxHeaderBlend
        {
            get
            {
                return this["TaskPainBoxHeaderBlend"];
            }
        }
        #endregion

        public static Color ConvertToRGB(Color color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

    }
}
