using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VkBot.Functions
{
    public static class RandomArrayElement
    {
        public static T RandomElement<T>(this T[] array)
        {
            var random = new Random();

            return array[random.Next(array.Length)];
        }
    }
}
