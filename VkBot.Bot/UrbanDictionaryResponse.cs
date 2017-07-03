using System.Collections.Generic;
using Newtonsoft.Json;
using VkBot.BotApi;

namespace VkBot.Bot
{

    public partial class BotFunctionsInstaller : IBotFunctionsInstaller
    {
        public class UrbanDictionaryResponse
        {
            public string[] Tags { get; set; }

            public string ResultType { get; set; }

            [JsonProperty("list")]
            public List<WordDefinition> WordDefinitions { get; set; }
        }
    }
}