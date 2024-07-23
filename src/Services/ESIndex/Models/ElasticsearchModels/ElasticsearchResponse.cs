using System;
using System.Collections.Generic;

namespace Models.ElasticsearchModels
{
    public class ElasticsearchResponse
    {
        public ElasticsearchResponse(int success, int failed)
        {
            Result = true;
            SuccessNumber = success;
            FailedNumber = failed;
            Logs = new Dictionary<Guid, ProcessingItemLog>();
        }

        public ElasticsearchResponse(string error)
        {
            ErrorMessage = error;
            HasErrors = true;
        }

        public bool Result { get; set; }
        public int SuccessNumber { get; set; }
        public int FailedNumber { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasErrors { get; set; }
        public IDictionary<Guid, ProcessingItemLog> Logs { get; private set; }
    }
}
