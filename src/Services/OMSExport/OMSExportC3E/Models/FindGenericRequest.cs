using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindGenericRequest
    {
        [JsonProperty("select")]
        public Select Selector { get; set; }

        [JsonProperty("filterNullValues")]
        public bool FilterNullValues => false;

        public class Select
        {
            [JsonProperty("archetype")]
            [JsonConverter(typeof(StringEnumConverter))]
            public Archetype Archetype { get; set; }

            [JsonProperty("archetypeType")]
            public int ArchetypeType => 1;

            [JsonProperty("attributes")]
            public string[] Attributes { get; set; }

            [JsonProperty("where")]
            public Where Where { get; set; }

            [JsonProperty("joins", NullValueHandling = NullValueHandling.Ignore)]
            public List<Join> Joins { get; set; }
        }

        public class Where
        {
            public Where(WhereOperator whereOperator) { Operator = whereOperator; }

            [JsonProperty("operator")]
            [JsonConverter(typeof(StringEnumConverter))]
            public WhereOperator Operator { get; private set; }

            [JsonProperty("predicates")]
            public List<Predicate> Predicates { get; set; }

            [JsonProperty("groups", NullValueHandling = NullValueHandling.Ignore)]
            public List<Where> Groups { get; set; }
        }

        public class Predicate
        {
            [JsonProperty("attribute")]
            public string Attribute { get; set; }

            [JsonProperty("operator")]
            [JsonConverter(typeof(StringEnumConverter))]
            public PredicatesOperator Operator { get; set; }

            [JsonProperty("value")]
            public string[] Value { get; set; }
        }

        public class Join
        {
            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("isOuterJoin")]
            public bool IsOuterJoin { get; set; }
        }

        public enum Archetype
        {
            Client,
            Entity,
            Matter,
            NxAttachment
        }

        public enum WhereOperator
        {
            And,
            Or
        }

        public enum PredicatesOperator
        {
            IsEqualTo,
            IsNotEqualTo,
            IsGreaterThan,
            IsGreaterOrEqualTo,
            IsLessThan,
            IsLessOrEqualTo,
            IsIn,
            IsNotIn,
            IsBetween,
            Contains,
            DoesNotContain,
            BeginsWith,
            DoesNotBeginWith,
            EndsWith,
            DoesNotEndWith,
            IsNull,
            IsNotNull
        }
    }
}
