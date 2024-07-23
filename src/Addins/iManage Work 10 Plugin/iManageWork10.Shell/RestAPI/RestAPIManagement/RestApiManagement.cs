using System;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public abstract class RestApiManagement
    {

        protected readonly IRestApiClient _restApiClient;

        protected RestApiManagement(IRestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
        }

        protected virtual string GetRootUrl(string library = null)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? _restApiClient.PreferredLibrary : library)}";
        }

        protected void ValidateUriParameter(string parameterName, string parameterValue)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (parameterValue.Length == 0)
            {
                throw new ArgumentException("Value cannot be empty.", parameterName);
            }
        }
    }
}
