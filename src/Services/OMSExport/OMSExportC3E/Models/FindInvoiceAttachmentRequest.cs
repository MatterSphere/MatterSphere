using System;
using System.Collections.Generic;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindInvoiceAttachmentRequest : FindGenericRequest
    {
        public FindInvoiceAttachmentRequest(DateTime timeStamp)
        {
            Selector = new Select
            {
                Archetype = Archetype.NxAttachment,
                Attributes = new string[]
                {
                    "InvMaster.InvIndex",
                    "InvMaster.InvNumber",
                    "InvMaster.IsReversed",
                    "NxAttachmentID",
                    "FileName",
                    "Description",
                    "Matter.MatterID",
                    "Matter.MattIndex"
                },
                Where = new Where(WhereOperator.And)
                {
                    Predicates = new List<Predicate>
                    {
                        new Predicate
                        {
                            Attribute = "NxAttachment.ParentArchetypeCode",
                            Operator = PredicatesOperator.IsEqualTo,
                            Value = new string[] { "InvMaster" }
                        },
                        new Predicate
                        {
                            Attribute = "NxAttachment.TypeCode",
                            Operator = PredicatesOperator.IsEqualTo,
                            Value = new string[] { "FILE" }
                        }
                    },
                    Groups = new List<Where>
                    {
                        new Where(WhereOperator.Or)
                        {
                            Predicates = new List<Predicate>
                            {
                                new Predicate
                                {
                                    Attribute = "NxAttachment.TimeStamp",
                                    Operator = PredicatesOperator.IsGreaterOrEqualTo,
                                    Value = new string[] { timeStamp.ToString("yyyy-MM-dd HH:mm:ss") }
                                }
                            },
                            Groups = new List<Where>
                            {
                                new Where(WhereOperator.And)
                                {
                                    Predicates = new List<Predicate>
                                    {
                                        new Predicate
                                        {
                                            Attribute = "InvMaster.TimeStamp",
                                            Operator = PredicatesOperator.IsGreaterOrEqualTo,
                                            Value = new string[] { timeStamp.ToString("yyyy-MM-dd HH:mm:ss") }
                                        },
                                        new Predicate
                                        {
                                            Attribute = "InvMaster.IsReversed",
                                            Operator = PredicatesOperator.IsEqualTo,
                                            Value = new string[] { "1" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Joins = new List<Join>
                {
                    new Join
                    {
                        From = "NxAttachment.ParentItemID",
                        To = "InvMaster.InvMasterId"
                    },
                    new Join
                    {
                        From = "InvMaster.LeadMatter",
                        To = "Matter.MattIndex"
                    }
                }
            };
        }
    }
}
