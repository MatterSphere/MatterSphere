using IndexingController.Models.Logging;
using Models.Interfaces;

namespace IndexingController.Models
{
    public static class LoggerExt
    {
        public static void Log(this ILogger logger, IndexingProcessResult logInfo)
        {
            if (logInfo.HasErrors)
            {
                logger.Error(logInfo.ToString());
            }
            else
            {
                logger.Info(logInfo.ToString());
            }
        }
    }
}
