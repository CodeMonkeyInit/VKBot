using System;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.Bot;
using VkBot.BotApi;
using VkBot.Functions.JokeApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
        public async Task Joke(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (botTask.Body.Contains("анекдот"))
            {
                botTask.WasHandled = true;
                string jsonQuote = await JokeApi.PostUrlEncodedAsync(new { CType = 11 })
                    .ReceiveString();

                Joke quote;

                BotResponse botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId
                };


                try
                {
                    quote = JsonConvert.DeserializeObject<Joke>(jsonQuote);
                }
                catch (Exception)
                {
                    await PrintRandomError(botResponse, responseHandler);

                    return;
                }


                botResponse.Response = $"Держи анекдот: \"{quote.Content}\"";

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}
