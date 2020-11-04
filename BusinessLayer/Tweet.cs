
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class Tweet : Message
    {

        public Tweet()
        {
        }

        protected List<String> trending;

        protected List<String> mentions;


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public Tweet(String sender, String text)
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        public bool isTrending()
        {
            // TODO implement here
            return false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void findAbbreviations()
        {
            // TODO implement here
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