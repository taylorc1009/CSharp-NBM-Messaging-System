using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public abstract class ValidateComponent
    {
        public abstract bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature);
    }
}