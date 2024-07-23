using System;

namespace Models.DbModels
{
    public class BlacklistCriterion
    {
        public BlacklistCriterion(string extension, long maxSize, string encoding, string content)
        {
            Extension = extension;
            MaxSize = maxSize;
            Encoding = encoding;
            Content = content;

            Criterion = CriterionFlags.Extension;
            if (maxSize > 0)
            {
                Criterion |= CriterionFlags.MaxSize;
            }

            if (!string.IsNullOrWhiteSpace(encoding))
            {
                Criterion |= CriterionFlags.Encoding;
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                Criterion |= CriterionFlags.Content;
            }
        }

        public string Extension { get; }
        public long MaxSize { get; }
        public string Encoding { get; }
        public string Content { get; }
        public CriterionFlags Criterion { get; private set; }

        public bool HasContains
        {
            get { return !string.IsNullOrWhiteSpace(Content); }
        }
        
        [Flags]
        public enum CriterionFlags
        {
            Extension = 1,
            MaxSize = 2,
            Encoding = 4,
            Content = 8
        }
    }
}
