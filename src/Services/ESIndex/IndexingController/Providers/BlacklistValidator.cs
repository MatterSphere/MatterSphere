using System;
using System.IO;
using System.Linq;
using System.Text;
using Models.DbModels;
using Models.Interfaces;

namespace IndexingController.Providers
{
    public class BlacklistValidator : IBlacklistValidator
    {
        private readonly BlacklistCriterion[] _blacklistCriteria;
        private const int _bufferSize = 16384;

        public BlacklistValidator(BlacklistCriterion[] blacklistCriteria)
        {
            _blacklistCriteria = blacklistCriteria;
        }
        
        public bool FindFile(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                return false;
            }

            var extension = fileInfo.Extension.Substring(1);
            var blacklist = _blacklistCriteria.Where(item => extension.Equals(item.Extension, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (!blacklist.Any())
            {
                return false;
            }

            blacklist = blacklist.Where(item => (item.Criterion & BlacklistCriterion.CriterionFlags.MaxSize) == 0 ||
                                                   ((item.Criterion & BlacklistCriterion.CriterionFlags.MaxSize) != 0 &&
                                                    item.MaxSize <= fileInfo.Length)).ToList();
            if (!blacklist.Any())
            {
                return false;
            }

            var fileEncoding = GetEncoding(fileInfo.FullName).EncodingName;
            blacklist = blacklist.Where(item => (item.Criterion & BlacklistCriterion.CriterionFlags.Encoding) == 0 ||
                                               ((item.Criterion & BlacklistCriterion.CriterionFlags.Encoding) != 0 &&
                                                fileEncoding.Equals(item.Encoding, StringComparison.InvariantCultureIgnoreCase))).ToList();
            if (!blacklist.Any())
            {
                return false;
            }

            if (blacklist.Any(item => !item.HasContains))
            {
                return true;
            }

            var criteriaGroups = blacklist.GroupBy(item => item.Content).ToList();
            foreach (var criterionGroup in criteriaGroups)
            {
                Encoding encoding;
                try
                {
                    var encodingName = criterionGroup.First().Encoding;
                    if (encodingName == "Unicode (UTF-8)")
                    {
                        encodingName = "UTF-8";
                    }

                    encoding = Encoding.GetEncoding(encodingName);
                }
                catch
                {
                    encoding = Encoding.Unicode;
                }
                
                using (var streamReader = new StreamReader(new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read), encoding))
                {
                    var charPart1 = new char[_bufferSize];
                    var charPart2 = new char[_bufferSize];
                    var charsRead = streamReader.Read(charPart1, 0, _bufferSize);

                    if (charsRead == 0)
                    {
                        continue;
                    }

                    while (charsRead > 0)
                    {
                        var part1 = new string(charPart1);
                        var part2 = new string(charPart2);
                        var combined = part1 + part2;
                        if (combined.Contains(criterionGroup.Key))
                        {
                            return true;
                        }

                        charPart2 = charPart1;
                        charsRead = streamReader.Read(charPart1, 0, _bufferSize);
                    }
                }
            }

            return false;
        }

        private Encoding GetEncoding(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName, true))
            {
                return reader.CurrentEncoding;
            }
        }
    }
}
