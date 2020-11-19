using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    //class used to localize common email attributes/methods
    public class Email : Message
    {
        //attributes are public for serialization purposes
        public String subject { get; set; }
        //quarantined URLs are stored in a dictionary, along with their position in the message body
        public Dictionary<int, URL> urlsQuarantined;

        public void quarantineURLs()
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                //acquires a URL surrounded by special characters
                Tuple<String, int, int> trimmed = trimNonAlphabeticals(tokenized[i]);

                if (trimmed != null)
                {
                    //checks if the trimmed word is a URL
                    if (Regex.IsMatch(trimmed.Item1, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$", RegexOptions.IgnoreCase))
                    {
                        //initialises the URL List only if it is null - this way we won't have a bunch of empty lists for every email with no URL
                        if (urlsQuarantined == null)
                            urlsQuarantined = new Dictionary<int, URL>();
                        urlsQuarantined.Add(urlsQuarantined.Count(), new URL(trimmed.Item1, false));

                        //rebuilds the message token with the trimmed special characters, replacing the quarantined URL
                        StringBuilder merger = new StringBuilder();
                        merger.Append(tokenized[i].Substring(0, trimmed.Item2) + "<URL Quarantined>" + tokenized[i].Substring(trimmed.Item3));
                        tokenized[i] = merger.ToString();
                    }
                }
            }

            //rebuilds the tokenized message
            StringBuilder message = new StringBuilder();
            foreach (String tok in tokenized)
                message.Append(tok + ' ');

            this.text = message.ToString().Trim();
        }
    }
}