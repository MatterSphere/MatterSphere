using System;
using System.Collections.Generic;
using System.Dynamic;

namespace IndexingController.Builders
{
    public class EntityBuilder : SingleBuilder
    {
        public ExpandoObject CreateEntity(string objectType, ExpandoObject model)
        {
            var modelPropertyDictionary = model as IDictionary<string, object>;
            var entity = new ExpandoObject() as IDictionary<string, object>;
            entity.Add(_objectTypeFieldName, objectType);
            bool deleted = false;
            entity.Add(_entityDeleted, deleted);
            foreach (var modelProperty in modelPropertyDictionary)
            {
                if (modelProperty.Key == _entityDeleted)
                {
                    bool.TryParse(modelProperty.Value.ToString(), out deleted);
                    entity[_entityDeleted] = deleted;
                }
                else
                {
                    entity.Add(modelProperty.Key, modelProperty.Value);
                }
            }

            return (dynamic)entity;
        }
    }
}
