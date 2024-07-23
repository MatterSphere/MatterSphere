namespace Horizon.Common
{
    public interface IMarkerControl
    {
        string Key { get; set; }
        string Label { get; set; }
        string Description { get; set; }
        string ErrorMessage { get; set; }
        bool IsValid { get; }
        bool WasChanged { get; }
        void ResetChangeTracking();
    }
}
