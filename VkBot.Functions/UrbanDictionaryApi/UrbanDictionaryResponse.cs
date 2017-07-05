﻿using System.Collections.Generic;
using Newtonsoft.Json;
using VkBot.BotApi;

namespace VkBot.Functions.UrbanDictionaryApi
{
    public class UrbanDictionaryResponse
    {
        public string[] Tags { get; set; }

        public string ResultType { get; set; }

        [JsonProperty("list")]
        public List<WordDefinition> WordDefinitions { get; set; }
    }
}