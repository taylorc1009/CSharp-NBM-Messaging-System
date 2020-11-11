
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class Message
    {

        public Message()
        {
        }

        protected String sender;

        protected String text;

        protected char header;


        /// <summary>
        /// @return
        /// </summary>
        public String getSender()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @return
        /// </summary>
        public String getText()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @return
        /// </summary>
        public bool validate()
        {
            // TODO implement here
            return false;
        }

    }
}