using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iManageWork10.Shell.JsonResponses
{
    public class WorkspaceDataResponse
    {
        public readonly List<Workspace> Workspaces = new List<Workspace>();

        [JsonExtensionData]
        private readonly IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            JToken dataValue;
            if (_additionalData.TryGetValue("data", out dataValue))
            {
                if (dataValue?.Type == JTokenType.Object)
                {
                    dataValue = dataValue["results"];
                }
                if (dataValue?.Type == JTokenType.Array)
                {
                    Workspaces.AddRange((IEnumerable<Workspace>)dataValue.ToObject(Workspaces.GetType()));
                }
            }
        }
    }
}
