﻿using System.Text;

namespace MallDomain.utils
{
    public class NumUtils
    {




        public static string GenOrderNo()
        {
            byte[] numeric = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int r = numeric.Length;
            Random rand = new();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                sb.Append(numeric[rand.Next(r)]);
            }
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            string result = timestamp + sb.ToString();
            return result;

        }

        // '2,3' 转换为[2,3]
        public static List<long> StrToInt(string str)
        {
            var list = new List<long>();
            foreach (var item in str.Split(","))
            {
                list.Add(long.Parse(item));
            }
            return list;
        }
    }
}
