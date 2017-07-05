using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkBot.Functions
{
    public static class RandomArrayElement
    {
        public static string RandomElement(this String[] strings)
        {
            var random = new Random();

            return strings[random.Next(strings.Length)];
        }
    }
}
