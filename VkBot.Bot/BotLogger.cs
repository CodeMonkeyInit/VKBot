using System;
using System.Collections;
using System.IO;
using System.Text;
using VkBot.BotApi.Messages;

namespace VkBot.Bot
{
    public class BotLogger : ILogger
    {
        private const string LoggerPath = "logs/log.txt";
        private readonly object _logger = new object();

        public void LogException(Exception e)
        {
            lock (_logger)
            {
                File.AppendAllText(LoggerPath,
                    $"{DateTime.Now}:\n Exception :{e.Message}\n StackTrace: \n{e.StackTrace}\n");
            }
        }

        public BotLogger()
        {
            Directory.CreateDirectory(LoggerPath);
        }
    }
}