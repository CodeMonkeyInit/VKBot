namespace VkBot.Functions.UrbanDictionaryApi
{
    public class WordDefinition
    {
        public string Definition { get; set; }

        public string Word { get; set; }

        public string Example { get; set; }

        public string Permalink { get; set; }

        public override string ToString()
        {
            return $"Word: {Word}\nDefinition: {Definition}\nExample: \"{Example}\"";
        }
    }
}