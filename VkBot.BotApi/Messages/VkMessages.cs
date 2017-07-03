namespace VkBot.BotApi.Messages
{
    public class VkMessages
    {
        public int Count { get; set; }

        public int UnreadDialogs { get; set; }

        public VkMessageItem[] Items { get; set; }
    }
}
