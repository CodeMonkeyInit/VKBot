using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.BotApi;
using VkNet.Model;

namespace VkBot.Bot
{
    public class GeniousApiResponse
    {
        public GeniusSearchResponse Response { get; set; }
    }

    public class GeniusSearchResponse
    {
        public List<Hit> Hits { get; set; }
    }

    public class Hit
    {
        public Song Result { get; set; }
    }

    public class Song
    {
        [JsonProperty("full_title")]
        public string FullTitle { get; set; }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"Песня: {FullTitle}\nСсылка на текст: {Url}";
        }
    }

    public class Quote
    {
        public string QuoteText { get; set; }

        public string QuoteAuthor { get; set; }

        public string QuoteLink { get; set; }
    }

    public class Joke
    {
        public string Content { get; set; }
    }

    public partial class BotFunctionsInstaller : IBotFunctionsInstaller
    {
        private const string QuoteApi = "http://api.forismatic.com/api/1.0/";
        private const string JokeApi = "http://rzhunemogu.ru/RandJSON.aspx";
        private const string UrbanDictionaryApi = "https://mashape-community-urban-dictionary.p.mashape.com/define";
        private const string GeniousSearchApi = "https://api.genius.com/";

        private const int CommandPosition = 0;
        private const int CommandContinuation = 1;
        private const int ArgumentsStart = 1;

        private readonly string[] _commandUnknownResponses =
            {"Извини, но я пока тебя не понимаю(((", "Я пока не знаю такой команды((("};

        private readonly string[] _errorOccuredResponses =
        {
            "Произошла ошибка, мне очень жаль(((", "Что-то пошло не так, этот пидор ща пофиксит(или нет)))"
        };

        public async Task Lyrics(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            string[] botTaskSplitted = botTask.BodySplitted;

            int lyricsRequiredCountPostion = botTaskSplitted.Length - 1;
            
            if (botTaskSplitted[CommandPosition + botTask.Offset].Contains("слова"))
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
                        catch (Exception e)
                        {
                            var random = new Random();

                            botResponse.Response = _errorOccuredResponses[random.Next(_errorOccuredResponses.Length - 1)];

                            await Task.Run(() => responseHandler(botResponse));

                            return;
                        }
                    }
                }
                
                await Task.Run(() => responseHandler(botResponse));
            }
        }

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
                catch (Exception e)
                {
                    var random = new Random();

                    botResponse.Response = _errorOccuredResponses[random.Next(_errorOccuredResponses.Length - 1)];

                    await Task.Run(() => responseHandler(botResponse));

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

        public async Task Joke(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (botTask.Body.Contains("анекдот"))
            {
                botTask.WasHandled = true;
                string jsonQuote = await JokeApi.PostUrlEncodedAsync(new {CType = 11})
                    .ReceiveString();

                Joke quote;

                try
                {
                    quote = JsonConvert.DeserializeObject<Joke>(jsonQuote);
                }
                catch (Exception)
                {
                    return;
                }

                BotResponse botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId,
                    Response = $"Держи анекдот: \"{quote.Content}\""
                };

                await Task.Run(() => responseHandler(botResponse));
            }
        }

        public async Task Find(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            const int findRequiestMinimalLength = 3;

            string[] taskBodySplitted = botTask.BodySplitted;

            if (taskBodySplitted.Length < findRequiestMinimalLength)
            {
                return;
            }

            if (taskBodySplitted[CommandPosition + botTask.Offset].Contains("скольк"))
            {
                botTask.WasHandled = true;
                for (int i = botTask.Offset + ArgumentsStart; i < taskBodySplitted.Length; i++)
                {
                    if (taskBodySplitted[i].Contains("слов") || taskBodySplitted[i].Contains("предложен"))
                    {
                        i++;

                        if (i > taskBodySplitted.Length - 1)
                        {
                            return;
                        }

                        int requiestLength = taskBodySplitted.Length - i;

                        string joinedRequiest = string.Join(" ", taskBodySplitted, i, requiestLength);

                        long wordCount = await vkApi.GetWordCountAsync(joinedRequiest, botTask.PeerId);

                        BotResponse botResponse = new BotResponse
                        {
                            PeerId = botTask.PeerId,
                            Response = $"Слово/предложение \"{joinedRequiest}\" повторялось {wordCount} раз(а)"
                        };

                        await Task.Run(() => responseHandler(botResponse));

                        return;
                    }
                }
            }
        }

        public async Task GlebSpecial(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (botTask.Body.Contains("ave maria"))
            {
                botTask.WasHandled = true;

                var botResponse = new BotResponse()
                {
                    PeerId = botTask.PeerId,
                    Response = "Deus Vult!"
                };

                await Task.Run(() => responseHandler(botResponse));
            }
        }

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

        public async Task Greeting(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            var botTaskBodySplitted = botTask.BodySplitted;

            foreach (string greeting in botTask.Greetings)
            {
                string message = botTaskBodySplitted[CommandPosition + botTask.Offset];
                if (message.Contains(greeting))
                {
                    botTask.WasHandled = true;
                    User user = new User
                    {
                        FirstName = "человек"
                    };

                    if (botTask.UserId != null)
                    {
                        user = vkApi.GetUser((long) botTask.UserId);
                    }

                    BotResponse botResponse = new BotResponse
                    {
                        PeerId = botTask.PeerId,
                        Response = $"И тебе {message}, {user.FirstName}!"
                    };

                    await Task.Run(() => responseHandler(botResponse));

                    return;
                }
            }
        }

        public async Task CommandUnknown(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (!botTask.WasHandled)
            {
                var random = new Random();

                var botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId,
                    Response = _commandUnknownResponses[random.Next(_commandUnknownResponses.Length - 1)]
                };

                await Task.Run(() => responseHandler(botResponse));
            }
        }

        public void Install(IVkBot bot)
        {
            bot.RegisterTaskHandler(Greeting);
            bot.RegisterTaskHandler(Quote);
            bot.RegisterTaskHandler(Find);
            bot.RegisterTaskHandler(Joke);
            bot.RegisterTaskHandler(UrbanDictionary);
            bot.RegisterTaskHandler(Lyrics);
            bot.RegisterTaskHandler(GlebSpecial);

            bot.RegisterGreetingTaskHandler(Greeting);

            bot.RegisterOnUnknownCommandHandler(CommandUnknown);
        }

        public BotFunctionsInstaller()
        {
        }
    }
}