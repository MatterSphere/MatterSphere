using System;
using System.Linq;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;

namespace iManageWork10.Shell.Validators
{
    public class LibrariesValidator
    {

        private LibrariesManagement _librariesManagement;

        public LibrariesValidator(IRestApiClient restApiClient)
        {
            _librariesManagement = new LibrariesManagement(restApiClient);
        }

        public void Validate(string libraryIds)
        {
            var libraries = _librariesManagement.GetLibraries();
            foreach (var id in libraryIds.Split(','))
            {
                if (!libraries.Any(l => l.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException($"Library with id {id} is not found.", nameof(libraryIds));
                }
            }
        }

    }
}
