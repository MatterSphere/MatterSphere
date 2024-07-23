using iManage.Work.Tools;

namespace iManageWork10.Shell.RestAPI
{
    public abstract class RestApiWorker
    {
        public abstract IWHttpRequester GetWHttpRequester { get; }

        public abstract string AuthToken { get; }

        public abstract string PreferredLibrary { get; }

        public abstract bool Connect();

        public abstract bool Disconnect();
    }
}