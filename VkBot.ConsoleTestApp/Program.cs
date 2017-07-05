using System;
using System.Linq;
using System.Threading.Tasks;
using VkBot.BotApi;
using VkBot.BotApi.Messages;
using VkBot.Functions;
using VkNet.Model.RequestParams;

namespace VkBot.ConsoleTestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            new BotFunctionsInstaller();

        }
    }
}