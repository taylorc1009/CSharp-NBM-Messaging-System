using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Tweet : MessageSupplement
    {
        //attributes are public for serialization purposes
        public List<String> hashtags;
        public List<String> mentions;

        public Tweet(String sender, String text)
        {
            this.sender = sender;
            this.text = text;

            //tells the decorator we want to decorate 'validate' for this object using the 'TweetDecorator'
            this.decorate(2);
        }

        public void findHashtags(Dictionary<String, int> trending)
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                //acquires any embedded hashtag only if one is present
                if (tokenized[i].Contains('#'))
                {
                    int s = 0, e = 1;

                    //while the increment is not at the point of the hashtag and not at the end of the string
                    while (s < tokenized[i].Length && tokenized[i][s] != '#')
                        s++;

                    //while the increment is not exceeding the string length and the char at the increment is alphabetical
                    while (e < tokenized[i].Length && Regex.IsMatch(tokenized[i][s + e].ToString(), @"[a-z0-9]", RegexOptions.IgnoreCase))
                        e++;

                    //acquires the substring containing the hashtag
                    String hashtag = tokenized[i].Substring(s, e).ToLower();
                    if (Regex.IsMatch(hashtag, @"#([a-z0-9]+)", RegexOptions.IgnoreCase))
                    {
                        //initialises the hashtag List only if it is null - this way we won't have a bunch of empty lists for every Tweet with no hashtag
                        if (hashtags == null)
                            hashtags = new List<String>();
                        hashtags.Add(hashtag);

                        //add the hashtag to the list of trending hashtags, if it doesn't already exists, and increment its count of occurancies
                        if (!trending.ContainsKey(hashtag))
                            trending.Add(hashtag, 0);
                        trending[hashtag]++;
                    }
                }
            }
        }

        public void findMentions()
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                //acquires any embedded mention only if one is present
                if (tokenized[i].Contains('@'))
                {
                    int s = 0, e = 1;

                    //same increments as for the hashtag, but we look for an '@' instead
                    while (s < tokenized[i].Length && tokenized[i][s] != '@')
                        s++;
                    while (e < tokenized[i].Length && Regex.IsMatch(tokenized[i][s + e].ToString(), @"[a-z0-9]", RegexOptions.IgnoreCase))
                        e++;

                    String mention = tokenized[i].Substring(s, e).ToLower();
                    if (Regex.IsMatch(mention, @"@([a-z0-9]+)", RegexOptions.IgnoreCase))
                    {
                        //again, initialises the mentions List only if it is null
                        if (mentions == null)
                            mentions = new List<String>();
                        mentions.Add(mention);
                    }
                }
            }
        }
    }
}