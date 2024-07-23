using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace iManageWork10.Shell.JsonResponses
{
    public class Workspace
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("workspace_id")]
        public string WorkspaceId { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("custom1")]
        public string Custom1 { get; set; }

        [JsonProperty("custom2")]
        public string Custom2 { get; set; }

        [JsonProperty("default_security")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DefaultAccessLevel DefaultSecurity { get; set; } = DefaultAccessLevel.Private; 

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, WorkspaceId: {WorkspaceId}, Database: {Database}]";
        }
    }

    /// <summary>
    /// iManage Workspace default access level.
    /// </summary>
    public enum DefaultAccessLevel
    {
        [EnumMember(Value = "private")]
        Private,
        [EnumMember(Value ="inherit")]
        Inherit,
        [EnumMember(Value = "public")]
        Public,
        [EnumMember(Value = "view")]
        View
    }
}