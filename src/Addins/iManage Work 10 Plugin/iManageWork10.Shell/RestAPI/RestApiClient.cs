using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using iManage.Work.Tools;
using iManageWork10.Shell.JsonResponses;
using Newtonsoft.Json;

namespace iManageWork10.Shell.RestAPI
{
    public class RestApiClient : IRestApiClient
    {
        private readonly RestApiWorker _restApiWorker;

        public RestApiClient(RestApiWorkerProvider restApiWorkerProvider)
        {
            _restApiWorker = restApiWorkerProvider.GetRestApiWorker();
        }
        
        public bool Connect()
        {
            return _restApiWorker.Connect();
        }

        public bool Disconnect()
        {
            return _restApiWorker.Disconnect();
        }

        public string AuthToken
        {
            get
            {
                return _restApiWorker.AuthToken;
            }
        }

        public string PreferredLibrary
        {
            get
            {
                return _restApiWorker.PreferredLibrary;
            }
        }

        public T ExecuteRequest<T>(string url, HttpMethod httpMethod, object payload = null, string localFilePath = null)
        {
            IWHttpRequester wHttpRequester = _restApiWorker.GetWHttpRequester;
            string payloadJson = payload != null
                ? JsonConvert.SerializeObject(payload,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })
                : string.Empty;
            string responseJson = string.Empty;
            int responseCode;
            switch (httpMethod.Method)
            {
                case "GET":
                    responseCode = string.IsNullOrEmpty(localFilePath)
                        ? wHttpRequester.GetRequest($"{url}{BuildQuery(payloadJson)}", ref responseJson)
                        : wHttpRequester.GetStreamRequest($"{url}{BuildQuery(payloadJson)}", localFilePath);
                    break;
                case "POST":
                    responseCode = string.IsNullOrEmpty(localFilePath)
                        ? wHttpRequester.PostRequest(url, payloadJson, ref responseJson)
                        : wHttpRequester.PostStreamRequest(url, localFilePath, payloadJson, ref responseJson);
                    break;
                case "PUT":
                    responseCode = string.IsNullOrEmpty(localFilePath)
                        ? wHttpRequester.PutRequest(url, payloadJson, ref responseJson)
                        : wHttpRequester.PutStreamRequest(url, localFilePath, payloadJson, ref responseJson);
                    break;
                case "DELETE":
                    responseCode = wHttpRequester.DeleteRequest($"{url}{BuildQuery(payloadJson)}", ref responseJson);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(httpMethod), httpMethod, null);
            }
            if (IsNotSuccessStatusCode(responseCode))
            {
                string httpError = string.Empty;
                try
                {
                    httpError = JsonConvert.DeserializeObject<HttpError>(responseJson).ToString();
                }
                catch (Exception)
                { }
                throw new HttpException(responseCode, $"Couldn't execute {httpMethod} to: {url}. {httpError}");
            }

            return JsonConvert.DeserializeObject<T>(responseJson);
        }
        
        private string BuildQuery(string payloadJson)
        {
            if (!string.IsNullOrEmpty(payloadJson))
            {
                var queryParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(payloadJson);

                if (queryParameters.Keys.Count > 0)
                {
                    NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
                    foreach (var parameter in queryParameters)
                    {
                        nameValueCollection.Add(parameter.Key, parameter.Value);
                    }
                    return $"?{nameValueCollection}";
                }
            }

            return string.Empty;
        }

        private bool IsNotSuccessStatusCode(int responseCode)
        {
            return !(responseCode >= 200 && responseCode <= 299);
        }
        
    }
}
