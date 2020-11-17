using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Email : Message
    {
        public Email() { }

        public String subject { get; set; }

        //attribute is public for serialization purposes
        public Dictionary<int, URL> urlsQuarantined;

        public void quarantineURLs()
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                Tuple<String, int, int> trimmed = trimNonAlphabeticals(tokenized[i]);

                if (trimmed != null)
                {
                    if (Regex.IsMatch(trimmed.Item1, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$", RegexOptions.IgnoreCase))
                    {
                        if (urlsQuarantined == null)
                            urlsQuarantined = new Dictionary<int, URL>();
                        urlsQuarantined.Add(urlsQuarantined.Count(), new URL(trimmed.Item1, false));

                        StringBuilder merger = new StringBuilder();
                        merger.Append(tokenized[i].Substring(0, trimmed.Item2) + "<URL Quarantined>" + tokenized[i].Substring(trimmed.Item3));
                        tokenized[i] = merger.ToString();
                    }
                }
            }

            StringBuilder message = new StringBuilder();
            foreach (String tok in tokenized)
                message.Append(tok + ' ');

            this.text = message.ToString().Trim();
        }
    }
}