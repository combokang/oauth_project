using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiddleOffice.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidMobile(this string mobile)
        {
            try
            {
                return Regex.IsMatch(mobile, @"^09\d{8}$");
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidTaxId(this string value)
        {
            int vatNumber;
            var logic = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };
            int addition, sum = 0, j = 0, x;

            if (value == null || value.Length != 8) { return false; }
            if (!int.TryParse(value, out vatNumber)) { return false; }

            for (x = 0; x < logic.Length; x++)
            {
                var no = Convert.ToInt32(value.Substring(x, 1));
                j = no * logic[x];
                addition = ((j / 10) + (j % 10));
                sum += addition;
            }

            if (sum % 10 == 0) { return true; }

            if (value.Substring(6, 1) == "7")
            {
                if (sum % 10 == 9)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidIdNumber(this string id)
        {
            var format = new Regex(@"^[A-Z]\d{9}$");
            if (!format.IsMatch(id)) { return false; }

            id = id.ToUpper();

            var a = new[]
            {
                10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33
            };

            var b = new int[11];

            b[1] = a[(id[0]) - 65] % 10;

            var c = b[0] = a[(id[0]) - 65] / 10;

            for (var i = 1; i <= 9; i++)
            {
                b[i + 1] = id[i] - 48;
                c += b[i] * (10 - i);
            }

            return ((c % 10) + b[10]) % 10 == 0;
        }
    }   
}
