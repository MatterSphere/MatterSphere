namespace FWBS.OMS.HighQ.Interfaces
{
    internal interface ISessionProvider
    {
        int GetUserId();
        string GetSpecificData(string parameter);
    }
}
