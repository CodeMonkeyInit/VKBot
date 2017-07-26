using System;
using System.Collections;
using System.IO;
using System.Text;
using VkBot.BotApi.Messages;

namespace VkBot.Bot
{
    public class BotLogger : ILogger
    {
        private const string LogName = "log.txt";
        private const string LoggerFolder = "logs";
        private readonly object _logger = new object();

        public void LogException(Exception e)
        {
            string loggerPath = Path.Combine(LoggerFolder, LogName);

            lock (_logger)
            {
                File.AppendAllText(loggerPath,
                    $"\n{DateTime.Now}:\n Exception :{e.Message}\n StackTrace: \n{e.StackTrace}\n");
            }
        }

        public BotLogger()
        {
            Directory.CreateDirectory(LoggerFolder);
        }
    }
}