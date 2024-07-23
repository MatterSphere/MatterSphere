using System.Web;
using FWBS.OMS.HighQ.Models.Response;
using Newtonsoft.Json;
using RestSharp;

namespace FWBS.OMS.HighQ.Providers
{
    internal class ErrorHandler
    {
        private const string HIGHQ_API_ERROR = "HighQ API is unavailable. Status: (%1%) %2%.\r\nPlease contact your System Administrator.";

        public static HttpException GetError(IRestResponse response)
        {
            ErrorResponse errorResponse = null;
            if (response.ContentType != null && response.ContentType.Contains("application/json"))
            {
                errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
            }

            string errorMessage = response.ErrorMessage;
            if (errorMessage == null && response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                errorMessage = Session.CurrentSession.Resources.GetMessage("HQAPIERR", HIGHQ_API_ERROR, "", ((int)response.StatusCode).ToString(), response.StatusDescription).Text;
            }
            return errorResponse != null
                ? new HttpException((int) response.StatusCode, errorResponse.ErrorMessage)
                : new HttpException((int) response.StatusCode, errorMessage);
        }

        public static HttpException GetDocumentError(IRestResponse response, string docName)
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);

            return errorResponse != null
                ? new HttpException((int)response.StatusCode, $"{docName}: {errorResponse.ErrorMessage}")
                : new HttpException((int)response.StatusCode, $"{docName}: {response.ErrorMessage}");
        }
    }
}
