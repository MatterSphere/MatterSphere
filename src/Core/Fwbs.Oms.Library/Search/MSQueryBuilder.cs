using System;
using System.Collections.Generic;

namespace FWBS.OMS.Search
{
    public class MSQueryBuilder
    {
        public string Build(string query)
        {
            query = query.Trim();

            // Search all '"*"'
            if (query == "" || query == "*" || query == "\"\"")
            {
                return "\"*\"";
            }

            // Search whole term, e.g. '"computer failure"', pass query as is.
            // The user should be responsible for valid Full-Text query syntax.
            // For example, the query ["F1" OR "F2"] will work, whereas ["F1" qwerty "F2"] will fail with an error.
            if (query.Length > 2 && query[0] == '"' && query[query.Length - 1] == '"')
            {
                return query;
            }

            // Search by individual words or prefixes, e.g. '"comp*" AND "fail*"' or '"computer" AND "failure"'
            return GetCustomQuery(query);
        }

        public string[] GetQueryWords(string query)
        {
            return query.Split(new string[] { "\" AND \"", "\"", " ", "*" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetCustomQuery(string query)
        {
            List<string> words = new List<string>();
            foreach (string token in query.Split(new char[0], StringSplitOptions.RemoveEmptyEntries))
            {
                string word = TrimPunctuation(token);
                if (word.Length > 0 && !words.Contains(word))
                    words.Add(word);
            }

            query = string.Concat("\"", string.Join("\" AND \"", words), "\"");
            return query;
        }

        private string TrimPunctuation(string value)
        {
            int trimStart = 0, trimEnd = value.Length - 1;

            while (trimStart <= trimEnd && char.IsPunctuation(value[trimStart]))
                trimStart++;

            while (trimEnd > trimStart && char.IsPunctuation(value[trimEnd]) && value[trimEnd] != '*')
                trimEnd--;

            if (trimStart == value.Length)
                value = string.Empty;
            else if (trimStart > 0 || trimEnd < value.Length - 1)
                value = value.Substring(trimStart, trimEnd - trimStart + 1);

            return value;
        }
    }
}
