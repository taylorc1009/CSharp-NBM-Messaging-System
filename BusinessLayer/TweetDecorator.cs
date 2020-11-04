
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class TweetDecorator : ValidateDecorator
    {

        public TweetDecorator()
        {
        }

        /// <summary>
        /// @return
        /// </summary>
        public override bool validate()
        {
            // TODO implement here
            return false;
        }

    }
}