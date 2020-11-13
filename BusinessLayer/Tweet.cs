
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class Tweet : MessageSupplement
    {

        public Tweet()
        {
        }

        private List<String> trending;

        private List<String> mentions;

        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public Tweet(String sender, String text, DateTime sentAt, char header)
        {
            this.sender = sender;
            this.text = text;
            this.sentAt = sentAt;
            this.header = header;
        }

        /// <summary>
        /// @return
        /// </summary>
        public String findHashtags()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @return
        /// </summary>
        public String findMentions()
        {
            // TODO implement here
            return null;
        }

    }
}