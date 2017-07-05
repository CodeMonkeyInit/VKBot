using Newtonsoft.Json;

namespace VkBot.Functions.GeniousApi
{
    public class Song
    {
        [JsonProperty("full_title")]
        public string FullTitle { get; set; }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"�����: {FullTitle}\n������ �� �����: {Url}";
        }
    }
}