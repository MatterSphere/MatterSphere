namespace Fwbs.Office
{
    public abstract class OfficeApplication : OfficeObject
    {
        protected OfficeApplication(bool isAddinInstance)
            : base(!isAddinInstance)
        {
            IsAddinInstance = isAddinInstance;
        }

        public bool IsAddinInstance { get; private set; }
    }
}
