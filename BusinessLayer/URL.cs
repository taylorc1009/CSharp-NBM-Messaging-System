
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class URL
    {
        public URL() { }

        public String intent { get; set; }

        public bool safe { get; set; }

        public URL(String intent, bool safe)
        {
            this.intent = intent;
            this.safe = safe;
        }
    }
}