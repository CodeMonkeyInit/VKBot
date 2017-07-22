namespace VkBot.BotApi.Messages
{
    public class VkMessageItem
    {
        public string DialogName { get; internal set; }

        public int? InRead { get; set; }

        public int? OutRead { get; set; }

        public VkMessage Message { get; set; }

        public override string ToString()
        {
            return DialogName;
        }
    }
}