using Newtonsoft.Json;

namespace VkBot.BotApi.Messages
{
    public class VkMessage
    {
        public const long ChatIdOffset = 2000000000;

        public long Id { get; set; }

        public long Date { get; set; }

        public long Out { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        public int ReadState { get; set; }

        public string Title { get; set; }

        [JsonProperty("chat_id")]
        public long? ChatId { get; set; }

        public long DialogId
        {
            get
            {
                if (ChatId == null)
                {
                    return UserId;
                }

                return (long) ChatId + ChatIdOffset;
            }
        }

        public VkMessage()
        {
        }
    }
}