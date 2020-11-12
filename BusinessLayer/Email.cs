using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Email : Message
    {

        public Email()
        {
        }

        private Dictionary<int, URL> urlsQuarantined;


        /// <summary>
        /// @return
        /// </summary>
        public void quarantineURLs()
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++) {

                //
                //
                // TODO currently not working
                //
                //

                //this is used to work around non-alphabetical chars, for rexmple if we had a URL in the form ",(http://example.com)."
                int s = 0, e = tokenized[i].Length;
                while (!Regex.IsMatch(tokenized[i][s].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && s < tokenized[i].Length)
                    s++;
                while (!Regex.IsMatch(tokenized[i][e - 1].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && e > 0)
                    e--;

                if (Regex.IsMatch(tokenized[i].Substring(s, e - s), @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$", RegexOptions.IgnoreCase))
                {
                    if (urlsQuarantined == null)
                        urlsQuarantined = new Dictionary<int, URL>();
                    urlsQuarantined.Add(urlsQuarantined.Count(), new URL(tokenized[i].Substring(s, e), false));

                    StringBuilder merger = new StringBuilder();
                    merger.Append(tokenized[i].Substring(0, s) + "<URL Quarantined>" + tokenized[i].Substring(e));
                    tokenized[i] = merger.ToString();
                }
            }

            StringBuilder message = new StringBuilder();
            foreach (String tok in tokenized)
                message.Append(tok + ' ');

            this.text = message.ToString().Trim();
        }

    }
}