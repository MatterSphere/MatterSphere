using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading;
using Common.Enums;
using DocumentReader;
using IndexingController.Models;
using Models.Interfaces;
using ILogger = Models.Interfaces.ILogger;

namespace IndexingController.Builders
{
    public class DocumentBuilder : SingleBuilder
    {
        private readonly IBlacklistValidator _blacklistValidator;
        private readonly ILogger _logger;
        private readonly int _timeout;
        private readonly bool _useOcrIndexing;
        private readonly int _ocrTimeout;

        public DocumentBuilder(IBlacklistValidator blacklistValidator, ILogger logger, int timeout, bool useOcrIndexing, int ocrTimeout)
        {
            _blacklistValidator = blacklistValidator;
            _logger = logger;
            _timeout = timeout;
            _useOcrIndexing = useOcrIndexing;
            _ocrTimeout = ocrTimeout;
        }

        public EntityInfo CreateDocument(string objectType, ExpandoObject model)
        {
            long entityId = 0;
            string path = null;
            long size = 0;
            var key = Guid.Empty;
            var isDeleted = false;
            var skipContent = false;

            try
            {
                var propertyDictionary = model as IDictionary<string, object>;
                var document = new ExpandoObject() as IDictionary<string, object>;
                document.Add(_objectTypeFieldName, objectType);

                string content = null;
                string contentReadingError = null;
                DocumentProcessErrorTypeEnum? documentProcessErrorType = null;

                foreach (var modelProperty in propertyDictionary)
                {
                    switch (modelProperty.Key)
                    {
                        case _documentPathFieldName:
                            path = modelProperty.Value.ToString();
                            break;
                        case _documentContentFieldName:
                            break;
                        case _id:
                            key = new Guid(modelProperty.Value.ToString());
                            document.Add(modelProperty.Key, modelProperty.Value);
                            break;
                        case _docDeleted:
                        case _precDeleted:
                            Boolean.TryParse(modelProperty.Value.ToString(), out isDeleted);
                            document.Add(modelProperty.Key, modelProperty.Value);
                            document.Add(_entityDeleted, isDeleted);
                            break;
                        case _entityId:
                            long id;
                            Int64.TryParse(modelProperty.Value.ToString(), out id);
                            entityId = id;
                            document.Add(modelProperty.Key, modelProperty.Value);
                            break;
                        case _entityDeleted:
                            Boolean.TryParse(modelProperty.Value.ToString(), out isDeleted);
                            document.Add(_entityDeleted, isDeleted);
                            break;
                        case _skipContent:
                            bool.TryParse(modelProperty.Value.ToString(), out skipContent);
                            break;
                        default:
                            document.Add(modelProperty.Key, modelProperty.Value);
                            break;
                    }
                }

                if (!skipContent && path != null && !isDeleted)
                {
                    if (!System.IO.File.Exists(path))
                    {
                        contentReadingError = "File was not found";
                        documentProcessErrorType = DocumentProcessErrorTypeEnum.FileNotFound;
                        _logger.Error($"The document {key} was not found");
                    }
                    else
                    {
                        var fileInfo = new FileInfo(path);
                        size = fileInfo.Length;

                        if (_blacklistValidator != null && _blacklistValidator.FindFile(fileInfo))
                        {
                            contentReadingError = "File is in blacklist";
                            documentProcessErrorType = DocumentProcessErrorTypeEnum.Blacklist;
                            _logger.Error($"The document {key} is in blacklist");
                        }
                        else
                        {
                            DocumentProcessErrorTypeEnum? errorCode;
                            string errorMessage;
                            var contentReadResult = ReadContent(path, out errorCode, out errorMessage);
                            if (contentReadResult.Key)
                            {
                                content = contentReadResult.Value;
                            }
                            else
                            {
                                documentProcessErrorType = errorCode;
                                contentReadingError = errorMessage;
                                _logger.Error($"The document {key} was not read. The Error message is {errorMessage}");
                            }
                        }
                    }
                }

                if (content != null)
                {
                    document.Add(_documentContentFieldName, content);
                }

                var info = new EntityInfo((dynamic)document, entityId, key, path, size, isDeleted);
                if (documentProcessErrorType.HasValue)
                {
                    info.SetError(documentProcessErrorType.Value, contentReadingError, true);
                }

                return info;
            }
            catch (Exception e)
            {
                var info = new EntityInfo(null, entityId, key, path, size, isDeleted);
                info.SetError(DocumentProcessErrorTypeEnum.UnhandledException, e.Message);
                info.ProcessError = true;
                return info;
            }
        }
        
        private KeyValuePair<bool, string> ReadContent(string path, out DocumentProcessErrorTypeEnum? errorCode, out string errorMessage)
        {
            errorCode = null;
            errorMessage = null;

            try
            {
                var reader = new ContentReader(_timeout);
                var content = reader.GetContent(path);
                if (_useOcrIndexing && string.IsNullOrWhiteSpace(content))
                {
                    var ocrReader = new OcrContentReader(_ocrTimeout);
                    content = ocrReader.GetContent(path);
                }

                return new KeyValuePair<bool, string>(true, content);
            }
            catch (ThreadAbortException)
            {
                errorCode = DocumentProcessErrorTypeEnum.UnhandledException;
                errorMessage = "The document was not read in time";
                return new KeyValuePair<bool, string>(false, null);
            }
            catch (Exception e)
            {
                errorCode = DocumentProcessErrorTypeEnum.TextNotRecognized;
                errorMessage = e.Message;
                return new KeyValuePair<bool, string>(false, null);
            }
        }
    }
}
