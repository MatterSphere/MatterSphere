using Models.Common;
using Models.Interfaces;

namespace IndexingController.Builders
{
    public class BuilderFactory
    {
        private readonly IBlacklistValidator _blacklistValidator;
        private readonly ILogger _logger;
        private readonly int _timeout;
        private readonly bool _useOcrIndexing;
        private readonly int _ocrTimeout;

        public BuilderFactory(IBlacklistValidator blacklistValidator, ILogger logger, int timeout, bool useOcrIndexing, int ocrTimeout)
        {
            _blacklistValidator = blacklistValidator;
            _logger = logger;
            _timeout = timeout;
            _useOcrIndexing = useOcrIndexing;
            _ocrTimeout = ocrTimeout;
        }

        public Decorator CreateBuilder(MessageTypeEnum entityType)
        {
            switch (entityType)
            {
                case MessageTypeEnum.Contentable:
                    return new Decorator(new DocumentBuilder(_blacklistValidator, _logger, _timeout, _useOcrIndexing, _ocrTimeout));
                case MessageTypeEnum.Relation:
                    return new Decorator(new RelationBuilder());
                default:
                    return new Decorator(new EntityBuilder());
            }
        }
    }
}
