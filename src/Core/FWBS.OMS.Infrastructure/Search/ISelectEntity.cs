namespace FWBS.OMS.Search
{
    /// <summary>
    /// Provides interface into a searching system
    /// </summary>
    public interface ISelectEntity
    {
        void Initialise(SelectEntityData data);
        object Execute();
    }

}
