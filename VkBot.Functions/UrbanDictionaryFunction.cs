using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.Bot;
using VkBot.BotApi;
using VkBot.Functions.UrbanDictionaryApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
        public async Task UrbanDictionary(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            string[] botTaskSplitted = botTask.BodySplitted;

            int definitionsRequiredCountPostion = botTaskSplitted.Length - 1;

            if (botTaskSplitted[CommandPosition + botTask.Offset].Contains("ud"))
            {
                botTask.WasHandled = true;

                int? definitionsRequired;

                BotResponse botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId
                };

                int offset = botTask.Offset + ArgumentsStart;

                try
                {
                    definitionsRequired = int.Parse(botTaskSplitted[definitionsRequiredCountPostion]);
                }
                catch (FormatException)
                {
                    definitionsRequired = null;
                }

                int requestLength;

                if (definitionsRequired == null)
                {
                    requestLength = botTaskSplitted.Length;
                    definitionsRequired = 1;
                }
                else
                {
                    requestLength = definitionsRequiredCountPostion;
                }

                string requiest = string.Join(" ", botTaskSplitted, offset, requestLength - offset);

                string jsonUrbanDicitionaryDefinitions =
                    await $"{UrbanDictionaryApi}?term={requiest}"
                        .WithHeader("X-Mashape-Key", "6WWfQL9qzRmsh7oFFBVNupXMinn1p1Zpvyvjsn3eJAyoWopTUT")
                        .GetStringAsync();
                UrbanDictionaryResponse urbanDictionaryResponse;

                try
                {
                    urbanDictionaryResponse =
                        JsonConvert.DeserializeObject<UrbanDictionaryResponse>(jsonUrbanDicitionaryDefinitions);
                }
                catch (Exception)
                {
                    await PrintRandomError(botResponse, responseHandler);

                    return;
                }

                if (urbanDictionaryResponse.WordDefinitions.Count == 0)
                {
                    botResponse.Response = "Definitions not found(((";
                }
                else
                {
                    StringBuilder response = new StringBuilder();

                    response.Append("Urban dictionary definition");

                    for (int i = 0; i < urbanDictionaryResponse.WordDefinitions.Count && i < definitionsRequired; i++)
                    {
                        response.Append($"\nVariant {i + 1}\n{urbanDictionaryResponse.WordDefinitions[i]}\n");
                    }

                    botResponse.Response = response.ToString();
                }

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}
