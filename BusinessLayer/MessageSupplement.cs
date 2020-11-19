using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
    //class used to localize 'findAbbreviations' for both SMS and Tweet
    public class MessageSupplement : Message
    {
        public void findAbbreviations(Dictionary<String, String> abbreviations)
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                //acquire any abbreviation surrounded by special characters
                Tuple<String, int, int> trimmed = trimNonAlphabeticals(tokenized[i]);

                if (trimmed != null)
                {
                    //checks if the trimmed string is present in the dictionary of abbreviations
                    if (abbreviations.ContainsKey(trimmed.Item1.ToUpper()))
                    {
                        //rebuilds the message token with the abbreviation, followed by the expansion and surrounds them with any previously existing special characters
                        StringBuilder merger = new StringBuilder();
                        merger.Append(tokenized[i].Substring(0, trimmed.Item2 + trimmed.Item1.Length) + " <" + abbreviations[trimmed.Item1.ToUpper()] + ">" + tokenized[i].Substring(trimmed.Item3));
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
