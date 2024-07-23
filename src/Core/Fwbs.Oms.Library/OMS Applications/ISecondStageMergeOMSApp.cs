namespace FWBS.OMS.Interfaces
{
    /// <summary>
    /// An interface that allows an OMSApp to support second stage merging.
    /// </summary>
    public interface ISecondStageMergeOMSApp
    {
        bool IsSecondStageMergeDoc(object obj);
        void AddSecondStageMergeField(object obj, string name);
        void SecondStageMerge(object obj, System.IO.FileInfo file);
    }
}
