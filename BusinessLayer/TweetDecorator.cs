using System;

namespace BusinessLayer
{
    public class TweetDecorator : ValidateDecorator
    {
        public TweetDecorator() { }

        //used to decorate the method 'validate' with the Tweet validations
        public override bool validate(String sender, String subject, String message, DateTime SIRDate, String sortCode, String nature)
        {
            return !String.IsNullOrEmpty(sender)
                && Utilities.isValidTwitter(sender)
                && !String.IsNullOrEmpty(message)
                && message.Length <= 140;
        }
    }
}