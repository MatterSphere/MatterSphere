using System.Collections.Generic;
using System.Dynamic;
using IndexingController.Models;

namespace IndexingController.Builders
{
    public class Decorator
    {
        private DocumentBuilder _documentBuilder;
        private EntityBuilder _entityBuilder;
        private MultiBuilder _multiBuilder;

        public Decorator(DocumentBuilder documentBuilder)
        {
            _documentBuilder = documentBuilder;
        }

        public Decorator(EntityBuilder entityBuilder)
        {
            _entityBuilder = entityBuilder;
        }

        public Decorator(MultiBuilder multiBuilder)
        {
            _multiBuilder = multiBuilder;
        }

        public EntityInfo CreateDocument(string objectType, ExpandoObject model)
        {
            return _documentBuilder?.CreateDocument(objectType, model);
        }

        public ExpandoObject CreateEntity(string objectType, ExpandoObject model)
        {
            return _entityBuilder?.CreateEntity(objectType, model);
        }

        public IEnumerable<ExpandoObject> CreateRelationships(ExpandoObject model)
        {
            return _multiBuilder?.CreateEntities(model);
        }

        private void GetContentReadingError(string errorMessage)
        {
           
        }
    }
}
