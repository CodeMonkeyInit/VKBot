using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.Bot;
using VkBot.BotApi;

namespace VkBot.Functions
{
    public  partial class BotFunctionsInstaller
    {
        public async Task Quote(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (botTask.Body.Contains("цитат"))
            {
                botTask.WasHandled = true;
                string jsonQuote = await QuoteApi.PostUrlEncodedAsync(new {method = "getQuote", format = "json"})
                    .ReceiveString();

                Quote quote = JsonConvert.DeserializeObject<Quote>(jsonQuote);

                BotResponse botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId,
                    Response = $"Держи: \"{quote.QuoteText}\"\n{quote.QuoteAuthor}"
                };

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}
