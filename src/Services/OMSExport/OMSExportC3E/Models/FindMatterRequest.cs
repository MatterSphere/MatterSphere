using System.Collections.Generic;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindMatterRequest : FindGenericRequest
    {
        public FindMatterRequest(string matterID)
        {
            Selector = new Select
            {
                Archetype = Archetype.Matter,
                Attributes = new string[] { "MatterID", "MattIndex", "Matter.MattDate.MattDateID", "Matter.MattDate.EffStart" },
                Where = new Where(WhereOperator.Or)
                {
                    Predicates = new List<Predicate>
                    {
                        new Predicate
                        {
                            Attribute = "MatterID",
                            Operator = PredicatesOperator.IsEqualTo,
                            Value = new string[] { matterID }
                        }
                    }
                }
            };
        }
    }
}
