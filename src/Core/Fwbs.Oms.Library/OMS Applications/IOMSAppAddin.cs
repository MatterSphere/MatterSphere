namespace FWBS.OMS.Apps
{
    public interface IOMSAppAddin
    {
        bool Online { get; }
        object Application { get; }
        object RunCommand(string command, object context);
        void RefreshUI(object context);
    }

}
