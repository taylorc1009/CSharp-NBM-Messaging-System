using System;

namespace BusinessLayer
{
    public abstract class ValidateComponent
    {
        //the method that will be decorated to validate each type of method appropriate to their format
        public abstract bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature);
    }
}