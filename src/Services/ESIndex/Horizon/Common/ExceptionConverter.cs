namespace Horizon.Common
{
    public static class ExceptionConverter
    {
        public static ExceptionDetails GetExceptionDescription(string errorCode)
        {
            switch (errorCode)
            {
                case "FileNotFound":
                    return new ExceptionDetails(
                        explanation: "The file was not found on the disk",
                        recommendation: "Please check that files are located in a disk");
                case "Blacklist":
                    return new ExceptionDetails(
                        explanation: "The extension is located in the blacklist",
                        recommendation: "The files could be read only after excluding those from blacklist");
                case "TextNotRecognized":
                    return new ExceptionDetails(
                        explanation: "The content of the document was not recognized as a text",
                        recommendation: "The files could not have a textual content. Or it could be an issue with iFilter for that extension");
                case "IndexingError":
                    return new ExceptionDetails(
                        explanation: "Indexing error",
                        recommendation: "The indexing process was failed");
                default:
                    return new ExceptionDetails(
                        explanation: "The unhandled exception",
                        recommendation: "It could be an issue with iFilter for that extension");
            }
        }

        public class ExceptionDetails
        {
            public ExceptionDetails(string explanation, string recommendation)
            {
                Explanation = explanation;
                Recommendation = recommendation;
            }

            public string Explanation { get; set; }
            public string Recommendation { get; set; }
        }
    }
}
