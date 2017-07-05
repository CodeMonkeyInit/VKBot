using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.Bot;
using VkBot.BotApi;
using VkBot.Functions.GeniousApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
        public async Task Lyrics(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            string[] botTaskSplitted = botTask.BodySplitted;

            int lyricsRequiredCountPostion = botTaskSplitted.Length - 1;

            int firstCommand = CommandPosition + botTask.Offset;
            if (botTaskSplitted[firstCommand].Contains("слова") || botTaskSplitted[firstCommand].Contains("что"))
            {
                int? songsPredictionsRequired;

                BotResponse botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId
                };

                try
                {
                    songsPredictionsRequired = int.Parse(botTaskSplitted[lyricsRequiredCountPostion]);
                }
                catch (FormatException)
                {
                    songsPredictionsRequired = null;
                }

                int requestLength;

                if (songsPredictionsRequired == null)
                {
                    requestLength = botTaskSplitted.Length;
                    songsPredictionsRequired = 1;
                }
                else
                {
                    requestLength = lyricsRequiredCountPostion;
                }

                for (int offset = botTask.Offset + ArgumentsStart; offset < botTaskSplitted.Length; offset++)
                {
                    if (botTaskSplitted[offset].Contains("песн"))
                    {
                        offset++;

                        if (offset > botTaskSplitted.Length - 1)
                        {
                            return;
                        }

                        string requiest = string.Join(" ", botTaskSplitted, offset, requestLength - offset);

                        botTask.WasHandled = true;

                        string geniousSearchJson = await $"{GeniousSearchApi}search?q={requiest}"
                            .WithHeader("Authorization",
                                "Bearer REbaZchpieWl_h4skTaMSOm0r68rMSm2WQ3mInEMcKDnMWHDSUs0z2L_XM6s3K3N")
                            .GetStringAsync();

                        List<Hit> hits;
                        try
                        {
                            hits = JsonConvert.DeserializeObject<GeniousApiResponse>(geniousSearchJson).Response.Hits;

                            StringBuilder response = new StringBuilder();

                            response.Append("Слова: \n");

                            for (int i = 0; i < hits.Count && i < songsPredictionsRequired; i++)
                            {
                                response.Append($"{i + 1}.{hits[i].Result}\n");
                            }

                            botResponse.Response = response.ToString();
                        }
                        catch (Exception)
                        {
                            botResponse.Response = _errorOccuredResponses.RandomElement();

                            await Task.Run(() => responseHandler(botResponse));

                            return;
                        }
                    }
                }

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}
