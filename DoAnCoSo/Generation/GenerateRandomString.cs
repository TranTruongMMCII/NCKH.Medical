using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Generation
{
    public class GenerateRandomString
    {
        private static Random random = new Random();
        public static string GetRandomString()
        {
            int length = 6;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}