namespace VkBot.Bot
{
    public class BotResponse
    {
        public long PeerId { get; set; }

        public string Response { get; set; }
    }

    public delegate void ResponseHandler(BotResponse response);
}