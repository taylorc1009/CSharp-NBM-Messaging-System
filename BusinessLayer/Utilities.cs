﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Utilities
    {
        public static bool isValidPhoneNumber(string number)
        {
            return number[0] == '+' && int.TryParse(number.Substring(1), out _);
        }

        public static bool isValidEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool isValidTwitter(string twitter)
        {
            //return twitter[0] == '@' && !String.IsNullOrWhiteSpace(twitter.Substring(1)) && !int.TryParse(twitter[1].ToString(), out _) && Regex.IsMatch(twitter.Substring(1), @"^[a-z0-9-_]+$", RegexOptions.IgnoreCase);
            return Regex.IsMatch(twitter, @"^@+[a-z][a-z0-9-_]*$", RegexOptions.IgnoreCase);
        }

        public static bool isValidSortCode(string sortCode)
        {
            return Regex.IsMatch(sortCode, @"^([0-9]{2})+[-]+([0-9]{2})+[-]+[0-9]{2}\z");
        }
    }
}
