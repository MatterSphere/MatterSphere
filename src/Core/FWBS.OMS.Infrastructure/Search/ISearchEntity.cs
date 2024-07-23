namespace FWBS.OMS.Search
{
    /// <summary>
    /// Details of an Entity Returned from a search
    /// </summary>
    public interface ISearchEntity
    {
        object ID { get; set; }
        int Rank { get; set; }
        string Data { get; set; }
    }
}
