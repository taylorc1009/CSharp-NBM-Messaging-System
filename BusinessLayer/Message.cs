
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

        public String sender { get; set; }

        public String text { get; set; }

        public char header { get; set; }
        public DateTime sentAt { get; set; }

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