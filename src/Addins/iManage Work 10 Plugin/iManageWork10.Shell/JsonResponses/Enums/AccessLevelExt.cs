using iManageWork10.Shell.Exceptions;

namespace iManageWork10.Shell.JsonResponses.Enums
{
    public static class AccessLevelExt
    {
        public static void ValidateAccess(this AccessLevel accessLevel)
        {
            if (accessLevel != AccessLevel.FullAccess && accessLevel != AccessLevel.Read && accessLevel != AccessLevel.ReadWrite)
            {
                throw new AccessDeniedException();
            }
        }
    }
}
