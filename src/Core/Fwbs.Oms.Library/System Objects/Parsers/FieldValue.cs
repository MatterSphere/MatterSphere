namespace FWBS.OMS.Parsers
{
    public sealed class FieldValue
    {
        private readonly object _value;

        public FieldValue(object value)
        {
            this._value = value;
        }

        public object Value 
        {
            get
            {
                return _value;
            }
        }
    }
}
