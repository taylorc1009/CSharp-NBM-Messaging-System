
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class SMS : Message
    {

        public SMS()
        {
        }


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public SMS(String sender, String text, char header)
        {
            this.sender = sender;
            this.text = text;
            this.header = header;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void findAbbreviations()
        {
            // TODO implement here
        }

    }
}