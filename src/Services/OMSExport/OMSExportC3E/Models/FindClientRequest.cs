using System.Collections.Generic;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindClientRequest : FindGenericRequest
    {
        public FindClientRequest(string clientID)
        {
            Init("ClientID", clientID);
        }

        public FindClientRequest(int clientIndex)
        {
            Init("ClientIndex", clientIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        private void Init(string attrName, string attrValue)
        {
            Selector = new Select
            {
                Archetype = Archetype.Client,
                Attributes = new string[] { "ClientIndex", "ClientID", "Client.CliDate.CliDateID", "Client.CliDate.EffStart" },
                Where = new Where(WhereOperator.Or)
                {
                    Predicates = new List<Predicate>
                    {
                        new Predicate
                        {
                            Attribute = attrName,
                            Operator = PredicatesOperator.IsEqualTo,
                            Value = new string[] { attrValue }
                        }
                    }
                }
            };
        }
    }
}
