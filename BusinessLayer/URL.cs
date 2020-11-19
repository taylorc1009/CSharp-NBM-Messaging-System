using System;

namespace BusinessLayer
{
    public class URL
    {
        public URL() { }

        public String intent { get; set; }
        public bool safe { get; set; }

        public URL(String intent, bool safe)
        {
            //stores the URL, along with a boolean determining if it's safe
            this.intent = intent;
            this.safe = safe;
        }
    }
}