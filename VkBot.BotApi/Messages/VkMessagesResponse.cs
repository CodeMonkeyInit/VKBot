using Newtonsoft.Json;

namespace VkBot.BotApi.Messages
{

    public class VkMessagesResponse
    {
        [JsonProperty("response")]
        public VkMessages Messages { get; set; }

        public VkMessagesResponse()
        {
            
        }
    }
}
