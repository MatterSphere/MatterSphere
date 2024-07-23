namespace FWBS.WF.OMS.ActivityLibrary
{
    public class KeyParameters
    {
        public KeyParameters()
        {
            Key = string.Empty;
            Value = new System.Activities.InArgument<object>();
        }

        public string Key { get; set; }
        public System.Activities.Argument Value { get; set; }
    }
}
