namespace FWBS.OMS.Script
{
    public interface IReference
    {
        string Name { get; }

        string AssemblyName { get; }

        string Location { get; }

        bool IsGlobal { get; }

        bool IsRequired { get; }
    }

}
