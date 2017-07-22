using System.Collections.Generic;

namespace VkBot.Bot
{
    public class HandleTaskArguments
    {
        public BotTaskHandler Handler { get; set; }
        public List<BotTask> BotTasks { get; set; }
        public int CommandsOffset { get; set; }
        public BotTaskHandler UnknownCommandHandler { get; set; }

        public HandleTaskArguments()
        {
        }
    }
}