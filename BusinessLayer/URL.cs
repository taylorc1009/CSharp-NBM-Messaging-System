
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class URL
    {

        public URL()
        {
        }

        private String intent;

        private bool safe;

        public URL(String intent, bool safe)
        {
            this.intent = intent;
            this.safe = safe;
        }

        /// <summary>
        /// @return
        /// </summary>
        public String getIntent()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @return
        /// </summary>
        public bool isSafe()
        {
            // TODO implement here
            return false;
        }

    }
}