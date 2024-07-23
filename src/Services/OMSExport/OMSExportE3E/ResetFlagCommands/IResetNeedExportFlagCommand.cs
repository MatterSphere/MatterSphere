namespace FWBS.OMS.OMSEXPORT.ResetFlagCommands
{
    /// <summary>
    /// Interface for classes capable of reset need export flag.
    /// </summary>
    public interface IResetNeedExportFlagCommand
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        long Id { get; }
        /// <summary>
        /// Resets need export flag.
        /// </summary>
        void Execute();
    }
}
