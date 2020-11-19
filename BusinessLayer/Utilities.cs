using System;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Utilities
    {
        public static bool isValidPhoneNumber(string number)
        {
            //checks if the phone number is international (contains a '+') and is numerical, containing between 3 and 11 numbers
            String sub = number.Substring(1);
            return number[0] == '+' && Regex.IsMatch(sub, @"^[0-9]{3,11}$");
        }

        public static bool isValidEmail(string email)
        {
            //checks in the email follows the format on an entire email address
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool isValidTwitter(string twitter)
        {
            //checks if the ID starts with an '@', followed by only a letter, then any letter, number, and/or a hyphen/underscore (max of 16 characters)
            return Regex.IsMatch(twitter, @"^@+[a-z]([a-z0-9-_]{0,14})$", RegexOptions.IgnoreCase);
        }

        public static bool isValidSortCode(string sortCode)
        {
            //checks if the sort code follows the format 'XX-XX-XX'
            return Regex.IsMatch(sortCode, @"^([0-9]{2})+[-]+([0-9]{2})+[-]+[0-9]{2}$");
        }
    }
}
