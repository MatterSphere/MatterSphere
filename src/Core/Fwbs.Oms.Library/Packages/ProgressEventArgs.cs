namespace FWBS.OMS.Design.Import
{
    /// <summary>
    /// A delegate used to call the Set parameter method for those class / objects that
    /// do not inherit from Source but may use RunSource instead.
    /// </summary>
    public delegate void SetValueOverideHandler(string name, object fromvalue, out object value);

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    public class ProgressEventArgs : System.ComponentModel.CancelEventArgs
    {
        public string Label = "";
        public bool Close = false;

        public ProgressEventArgs(string label)
            : base()
        {
            Label = label;
        }

        public ProgressEventArgs(bool close)
            : base()
        {
            Close = close;
        }

    }
}