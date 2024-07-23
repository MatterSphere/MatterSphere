using MSIndex.Common.Models;

namespace MSIndex.Common.Interfaces
{
    public interface IDbProvider
    {
        void Index(MSAddress[] addresses);
        void Index(MSAppointment[] appointments);
        void Index(MSAssociate[] associates);
        void Index(MSContact[] contacts);
        void Index(MSClient[] clients);
        void Index(MSDocument[] documents);
        void Index(MSFile[] files);
        void Index(MSPrecedent[] precedents);
        void Index(MSTask[] tasks);
        void Index(MSUser[] users);
    }
}
