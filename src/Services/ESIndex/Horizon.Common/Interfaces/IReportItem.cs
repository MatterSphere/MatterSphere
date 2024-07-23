namespace Horizon.Common.Interfaces
{
    public interface IReportItem
    {
        string Type { get; set; }
        long Success { get; set; }
        long Failed { get; set; }
        long EmptyContent { get; set; }
    }
}
