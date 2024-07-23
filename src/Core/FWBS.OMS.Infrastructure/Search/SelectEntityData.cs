namespace FWBS.OMS.Search
{
    /// <summary>
    /// Details of what parameters to prepopulate the search with
    /// </summary>
    public class SelectEntityData
    {
        public string SearchValue { get; set; }
        public EntityType SearchType { get; set; }
        public EntityType ParentType { get; set; }
        public long ParentId { get; set; }
    }
}
