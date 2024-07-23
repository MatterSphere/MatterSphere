using System;
using System.ComponentModel;
using System.Data;

namespace FWBS.OMS.Design
{
    public class DataListConverter : TypeConverter
    {

        private DataTable dt;
        private string valueMember;
        private string displayMember;
        private DataListAttribute dla;

        public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(ctx, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (dt == null)
                GetData(context, out dt, out valueMember, out displayMember, out dla);

            if (dt != null)
            {
                DataView vw = new DataView(dt);
                System.Text.StringBuilder filter = new System.Text.StringBuilder();

                if (dt.Columns[valueMember].DataType == typeof(string))
                    filter.Append("[{0}] like '{2}%'");
                else
                {
                    if (IsNumeric(value))
                        filter.Append("[{0}] = '{2}'");

                }

                if (dt.Columns[displayMember].DataType == typeof(string))
                {
                    if (filter.Length > 0) filter.Append(" or ");
                    filter.Append("[{1}] like '{2}%'");
                }
                else
                {
                    if (IsNumeric(value))
                    {
                        if (filter.Length > 0) filter.Append(" or ");
                        filter.Append("[{1}] = '{2}'");
                    }
                }


                bool isnull = false;
                if (dla.UseNull && value.Equals(dla.NullValue) || Convert.ToString(value) == String.Empty)
                    isnull = true;

                if (isnull)
                    return dla.NullValue;

                vw.RowFilter = String.Format(filter.ToString(), valueMember, displayMember, Convert.ToString(value));
                if (vw.Count > 0)
                {
                    return vw[0][valueMember];
                }
                else
                    throw new InvalidOperationException(String.Format("Specified value '{0}' not in data list.", value));
            }

            return base.ConvertFrom(context, culture, value);
        }


        public override bool CanConvertTo(ITypeDescriptorContext ctx, Type destinationType)
        {
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (dt == null)
                GetData(ctx, out dt, out valueMember, out displayMember, out dla);

            if (dt != null)
            {
                DataView vw = new DataView(dt);
                System.Text.StringBuilder filter = new System.Text.StringBuilder();

                if (dt.Columns[valueMember].DataType == typeof(string))
                    filter.Append("[{0}] like '{2}'");
                else
                {
                    if (IsNumeric(value))
                        filter.Append("[{0}] = '{2}'");

                }

                if (dt.Columns[displayMember].DataType == typeof(string))
                {
                    if (filter.Length > 0) filter.Append(" or ");
                    filter.Append("[{1}] like '{2}'");
                }
                else
                {
                    if (IsNumeric(value))
                    {
                        if (filter.Length > 0) filter.Append(" or ");
                        filter.Append("[{1}] = '{2}'");
                    }
                }


                bool isnull = false;
                if (dla.UseNull && value.Equals(dla.NullValue) || Convert.ToString(value) == String.Empty)
                    isnull = true;

                if (isnull)
                {
                    if (dla.UseNull)
                    {
                        value = dla.NullValue;
                        ctx.PropertyDescriptor.SetValue(ctx.Instance, value);
                    }
                    return Session.CurrentSession.Resources.GetResource("RESNOTSET", "{not set)", "").Text;

                }
                else
                {
                    vw.RowFilter = String.Format(filter.ToString(), valueMember, displayMember, Convert.ToString(value));
                    if (vw.Count > 0)
                    {
                        value = vw[0][valueMember];
                        ctx.PropertyDescriptor.SetValue(ctx.Instance, value);
                        return Convert.ToString(vw[0][displayMember]);
                    }
                    else
                    {
                        if (dla.UseNull)
                        {
                            value = dla.NullValue;
                            ctx.PropertyDescriptor.SetValue(ctx.Instance, value);
                        }
                        return Session.CurrentSession.Resources.GetResource("RESNOTSET", "{not set)", "").Text;
                    }
                }
            }
            return base.ConvertTo(ctx, culture, value, destinationType);
        }

        private static bool IsNumeric(object val)
        {
            try
            {
                long.Parse(Convert.ToString(val));
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static FWBS.OMS.EnquiryEngine.DataLists GetData(ITypeDescriptorContext context, out DataTable data, out string valueMember, out string displayMember, out DataListAttribute dla)
        {
            data = null;
            valueMember = String.Empty;
            displayMember = String.Empty;
            dla = null;

            FieldParser fp = new FieldParser(context.Instance);

            Common.KeyValueCollection pars = new Common.KeyValueCollection();

            System.Reflection.PropertyInfo prop = null;
            object instance = null;
            if (context.PropertyDescriptor is LookupPropertyDescriptor)
                instance = ((LookupPropertyDescriptor)context.PropertyDescriptor).Extender;

            if (instance == null)
                instance = context.Instance;

            prop = instance.GetType().GetProperty(context.PropertyDescriptor.Name);

            if (prop != null)
            {
                foreach (object attr in prop.GetCustomAttributes(true))
                {
                    if (attr is DataListAttribute)
                    {
                        dla = (DataListAttribute)attr;
                    }
                    else if (attr is ParameterAttribute)
                    {
                        ParameterAttribute par = (ParameterAttribute)attr;
                        if (par.Parse)
                            pars.Add(par.Key, fp.Parse(Convert.ToString(par.Value)));
                        else
                            pars.Add(par.Key, par.Value);
                    }
                }
            }

            if (dla == null || dla.Code == String.Empty)
            {
                return null;
            }
            else
            {
                FWBS.OMS.EnquiryEngine.DataLists _data = new FWBS.OMS.EnquiryEngine.DataLists(dla.Code);
                _data.ChangeParent(context.Instance);
                _data.ChangeParameters(pars);
                data = (DataTable)_data.Run();

                DataColumn bound;
                DataColumn display;

                if (dla.DisplayMember == String.Empty)
                {
                    if (data.Columns.Count > 1)
                    {
                        display = data.Columns[1];
                    }
                    else
                        display = data.Columns[0];
                }
                else
                    display = data.Columns[dla.DisplayMember];

                if (dla.ValueMember == String.Empty)
                    bound = data.Columns[0];
                else
                    bound = data.Columns[dla.ValueMember];

                if (dla.Parse)
                {
                    foreach (DataRow r in data.Rows)
                    {
                        r[1] = Session.CurrentSession.Terminology.Parse(Convert.ToString(r[display]), true);
                    }
                }

                DataView vw = new DataView(data);
                vw.RowFilter = String.Format("[{0}] is null", bound.ColumnName);
                if (vw.Count > 0)
                    vw.Delete(0);
                if (dla.UseNull)
                {
                    DataRow r = data.NewRow();
                    r[bound] = dla.NullValue;
                    r[display] = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(not set)", "").Text;
                    data.Rows.Add(r);
                }

                data.DefaultView.Sort = (dla.OrderBy == String.Empty ? display.ColumnName : dla.OrderBy);

                valueMember = bound.ColumnName;
                displayMember = display.ColumnName;
                return _data;

            }
        }
    }
}
