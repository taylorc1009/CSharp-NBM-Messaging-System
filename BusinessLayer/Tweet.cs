
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Tweet : MessageSupplement
    {
        public Tweet() { }

        private List<String> hashtags;
        private List<String> mentions;

        public Tweet(String sender, String text)
        {
            this.sender = sender;
            this.text = text;
            this.decorate(2);
        }

        public List<String> getHashtags()
        {
            return hashtags;
        }

        public List<String> getMentions()
        {
            return mentions;
        }

        public void findHashtags(Dictionary<String, int> trending)
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                if (tokenized[i].Contains('#'))
                {
                    int s = 0, e = 1;
                    while (tokenized[i][s] != '#' && s < tokenized[i].Length)
                        s++;
                    while (Regex.IsMatch(tokenized[i][s + e].ToString(), @"[a-z0-9]", RegexOptions.IgnoreCase) && e < tokenized[i].Length - 1)
                        e++;

                    String temp = tokenized[i].Substring(s, e).ToLower();
                    if (Regex.IsMatch(temp, @"#([a-z0-9]+)", RegexOptions.IgnoreCase))
                    {
                        if (hashtags == null)
                            hashtags = new List<String>();
                        hashtags.Add(temp);
                        if (!trending.ContainsKey(temp))
                            trending.Add(temp, 0);
                        trending[temp]++;
                    }
                }
            }
        }

        public void findMentions()
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                if (tokenized[i].Contains('@'))
                {
                    int s = 0, e = 1;
                    while (tokenized[i][s] != '@' && s < tokenized[i].Length)
                        s++;
                    while (Regex.IsMatch(tokenized[i][s + e].ToString(), @"[a-z0-9]", RegexOptions.IgnoreCase) && e < tokenized[i].Length - 1)
                        e++;

                    String temp = tokenized[i].Substring(s, e).ToLower();
                    if (Regex.IsMatch(tokenized[i].Substring(s, e), @"@([a-z0-9]+)", RegexOptions.IgnoreCase))
                    {
                        if (mentions == null)
                            mentions = new List<String>();
                        mentions.Add(temp);
                    }
                }
            }
        }
    }
}