using System;

namespace Fwbs.Office.Outlook
{
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    [StructLayout(LayoutKind.Sequential)]
    internal struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }

    internal static class Helpers
    {
        internal static bool IsMatch(string input, string pattern)
        {
            try
            {
                return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static void AutoCleanup(this OutlookApplication app)
        {
            if (app.Settings.Memory.AutoGarbageCollection)
            {
                Cleanup();
            }
        }

        public static void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static MSOutlook.OlUserPropertyType ToOutlookType(int propid)
        {
            return ToPT(propid).ToOutlookType();
        }

        public static PT ToPT(int propid)
        {
            var spropid = propid.ToString("x8");

            var spt = spropid.Substring(spropid.Length - 4, 4);

            return (PT)Convert.ToInt32(spt, 16);
        }

        public static MSOutlook.OlUserPropertyType ToOutlookType(this PT type)
        {
            switch (type)
            {
                case PT.Currency:
                    return MSOutlook.OlUserPropertyType.olCurrency;
                case PT.AppTime:
                    return MSOutlook.OlUserPropertyType.olDateTime;
                case PT.String8:
                    return MSOutlook.OlUserPropertyType.olText;
                case PT.Double:
                    return MSOutlook.OlUserPropertyType.olNumber;
                case PT.I2:
                    return MSOutlook.OlUserPropertyType.olPercent;                
                case PT.Boolean:
                    return MSOutlook.OlUserPropertyType.olYesNo;
                case PT.I8:
                    return MSOutlook.OlUserPropertyType.olEnumeration;
                case PT.Long:
                    return MSOutlook.OlUserPropertyType.olInteger;
                default:
                    return MSOutlook.OlUserPropertyType.olText;
            }
        }
  
        public static PT ToPT(this MSOutlook.OlUserPropertyType type)
        {
            switch (type)
            {
                case MSOutlook.OlUserPropertyType.olCurrency:
                    return PT.Currency;
                case MSOutlook.OlUserPropertyType.olDateTime:
                    return PT.AppTime;
                case MSOutlook.OlUserPropertyType.olKeywords:
                    return PT.String8;
                case MSOutlook.OlUserPropertyType.olNumber:
                    return PT.Double;
                case MSOutlook.OlUserPropertyType.olPercent:
                    return PT.I2;
                case MSOutlook.OlUserPropertyType.olText:
                    return PT.String8;
                case MSOutlook.OlUserPropertyType.olYesNo:
                    return PT.Boolean;
                case MSOutlook.OlUserPropertyType.olEnumeration:
                    return PT.I8;
                case MSOutlook.OlUserPropertyType.olInteger:
                    return PT.Long;
                default:
                    return PT.String8;
            }
        }

        public static MSOutlook.OlUserPropertyType ToOutlookType(this Redemption.rdoUserPropertyType type)
        {
            switch (type)
            {
                case Redemption.rdoUserPropertyType.olCombination:
                    return MSOutlook.OlUserPropertyType.olCombination;
                case Redemption.rdoUserPropertyType.olCurrency:
                    return MSOutlook.OlUserPropertyType.olCurrency;
                case Redemption.rdoUserPropertyType.olDateTime:
                    return MSOutlook.OlUserPropertyType.olDateTime;
                case Redemption.rdoUserPropertyType.olDuration:
                    return MSOutlook.OlUserPropertyType.olDuration;
                case Redemption.rdoUserPropertyType.olFormula:
                    return MSOutlook.OlUserPropertyType.olFormula;
                case Redemption.rdoUserPropertyType.olKeywords:
                    return MSOutlook.OlUserPropertyType.olKeywords;
                case Redemption.rdoUserPropertyType.olNumber:
                    return MSOutlook.OlUserPropertyType.olNumber;
                case Redemption.rdoUserPropertyType.olOutlookInternal:
                    return MSOutlook.OlUserPropertyType.olOutlookInternal;
                case Redemption.rdoUserPropertyType.olPercent:
                    return MSOutlook.OlUserPropertyType.olPercent;
                case Redemption.rdoUserPropertyType.olText:
                    return MSOutlook.OlUserPropertyType.olText;
                case Redemption.rdoUserPropertyType.olYesNo:
                    return MSOutlook.OlUserPropertyType.olYesNo;
                case Redemption.rdoUserPropertyType.olInteger:
                    return MSOutlook.OlUserPropertyType.olInteger;
                default:
                    return MSOutlook.OlUserPropertyType.olText;
            }
        }

        public static Redemption.rdoUserPropertyType ToRedemptionType(this MSOutlook.OlUserPropertyType type)
        {
            switch (type)
            {
                case MSOutlook.OlUserPropertyType.olCombination:
                    return Redemption.rdoUserPropertyType.olCombination;
                case MSOutlook.OlUserPropertyType.olCurrency:
                    return Redemption.rdoUserPropertyType.olCurrency;
                case MSOutlook.OlUserPropertyType.olDateTime:
                    return Redemption.rdoUserPropertyType.olDateTime;
                case MSOutlook.OlUserPropertyType.olDuration:
                    return Redemption.rdoUserPropertyType.olDuration;
                case MSOutlook.OlUserPropertyType.olFormula:
                    return Redemption.rdoUserPropertyType.olFormula;
                case MSOutlook.OlUserPropertyType.olKeywords:
                    return Redemption.rdoUserPropertyType.olKeywords;
                case MSOutlook.OlUserPropertyType.olNumber:
                    return Redemption.rdoUserPropertyType.olNumber;
                case MSOutlook.OlUserPropertyType.olOutlookInternal:
                    return Redemption.rdoUserPropertyType.olOutlookInternal;
                case MSOutlook.OlUserPropertyType.olPercent:
                    return Redemption.rdoUserPropertyType.olPercent;
                case MSOutlook.OlUserPropertyType.olText:
                    return Redemption.rdoUserPropertyType.olText;
                case MSOutlook.OlUserPropertyType.olYesNo:
                    return Redemption.rdoUserPropertyType.olYesNo;
                case MSOutlook.OlUserPropertyType.olEnumeration:
                    return Redemption.rdoUserPropertyType.olInteger;
                case MSOutlook.OlUserPropertyType.olInteger:
                    return Redemption.rdoUserPropertyType.olInteger;
                case MSOutlook.OlUserPropertyType.olSmartFrom:
                    return Redemption.rdoUserPropertyType.olOutlookInternal;
                default:
                    return Redemption.rdoUserPropertyType.olText;
            }
        }

        internal static MSOutlook.OlUserPropertyType CreateDefaultType(this object value)
        {
            if (value == null)
                return MSOutlook.OlUserPropertyType.olText;

            switch (Convert.GetTypeCode(value))
            {
                case TypeCode.Boolean:
                    return MSOutlook.OlUserPropertyType.olYesNo;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.String:
                    return MSOutlook.OlUserPropertyType.olText;
                case TypeCode.DateTime:
                    return MSOutlook.OlUserPropertyType.olDateTime;
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                    return MSOutlook.OlUserPropertyType.olInteger;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Int64:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return MSOutlook.OlUserPropertyType.olNumber;
                default:
                    return MSOutlook.OlUserPropertyType.olText;

            }
        }

        internal static DateTime UtcToUtc(object val)
        {
            if (val is DateTime)
            {
                DateTime dte = (DateTime)val;
                if (dte.Kind == DateTimeKind.Unspecified)
                    return DateTime.SpecifyKind(dte, DateTimeKind.Utc);
                else
                    return dte.ToUniversalTime();
            }

            return OutlookConstants.MAX_DATE;

        }

        internal static DateTime LocalToLocal(object val)
        {
            if (val is DateTime)
            {
                DateTime dte = (DateTime)val;
                if (dte.Kind == DateTimeKind.Unspecified)
                    return DateTime.SpecifyKind(dte, DateTimeKind.Local);
                else
                    return dte.ToLocalTime();
            }

            return OutlookConstants.MAX_DATE;
        }

        internal static DateTime UtcToLocal(object val)
        {
            if (val is DateTime)
            {
                DateTime dte = (DateTime)val;
                if (dte.Kind == DateTimeKind.Unspecified)
                    return DateTime.SpecifyKind(dte, DateTimeKind.Utc).ToLocalTime();
                else
                    return dte.ToLocalTime();
            }

            return OutlookConstants.MAX_DATE;
        }

        internal static DateTime? LocalToUtc(DateTime? val)
        {
            if (!val.HasValue)
                return null;

            if (val.Value == OutlookConstants.MAX_DATE)
                return null;

            if (val.Value.Kind == DateTimeKind.Unspecified)
                val = DateTime.SpecifyKind(val.Value, DateTimeKind.Local);

            return val.Value.ToUniversalTime();

        }


        internal static int ToPropId(int propid, PT pt)
        {
            return Convert.ToInt32(propid.ToString("x4").Substring(0, 4) + ((int)pt).ToString("x4"), 16);
        }
    }
}
