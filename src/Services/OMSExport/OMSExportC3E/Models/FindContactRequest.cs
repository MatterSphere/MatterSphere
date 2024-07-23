using System.Collections.Generic;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindContactRequest : FindGenericRequest
    {
        public FindContactRequest(string entityID)
        {
            Init("EntityID", entityID);
        }

        public FindContactRequest(int entityIndex)
        {
            Init("EntIndex", entityIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        private void Init(string attrName, string attrValue)
        {
            Selector = new Select
            {
                Archetype = Archetype.Entity,
                Attributes = new string[] { "EntIndex", "EntityID", "Relate.Site.SiteIndex", "Relate.Site.SiteID", "Relate.RelateID" },
                Where = new Where(WhereOperator.And)
                {
                    Predicates = new List<Predicate>
                    {
                        new Predicate
                        {
                            Attribute = attrName,
                            Operator = PredicatesOperator.IsEqualTo,
                            Value = new string[] { attrValue }
                        },
                        new Predicate
                        {
                            Attribute = "Relate.Site.IsDefault",
                            Operator = PredicatesOperator.IsEqualTo,
                            Value =  new string[] { "1" }
                        }
                    }
                },
                Joins = new List<Join>
                {
                    new Join
                    {
                        From = "Entity.EntIndex",
                        To = "Relate.SbjEntity",
                        IsOuterJoin = true
                    },
                    new Join
                    {
                        From = "Relate.RelIndex",
                        To = "Site.Relate",
                        IsOuterJoin = true
                    }
                }
            };
        }
    }
}
