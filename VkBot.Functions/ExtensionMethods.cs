using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkBot.Functions
{
    public static class ExtensionMethods
    {
        public static T RandomElement<T>(this T[] array)
        {
            var random = new Random();

            return array[random.Next(array.Length)];
        }

        public static bool ContainsAny(this string haystack, string[] needles)
        {
            return needles.Any(haystack.Contains);
        }
    }
}
