using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.Bot;
using VkBot.BotApi;
using VkNet.Model;
using VkBot.Functions.UrbanDictionaryApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller : IBotFunctionsInstaller
    {
        private const string QuoteApi = "http://api.forismatic.com/api/1.0/";
        private const string JokeApi = "http://rzhunemogu.ru/RandJSON.aspx";
        private const string UrbanDictionaryApi = "https://mashape-community-urban-dictionary.p.mashape.com/define";
        private const string GeniousSearchApi = "https://api.genius.com/";

        private const int CommandPosition = 0;
        private const int CommandContinuation = 1;
        private const int ArgumentsStart = 1;

        private const string HelpStringsFilepath = "txt/help.txt";

        private const string CommandUnknownResponsesFilepath = "txt/command_unknown.txt";

        private const string ErrorOccuredResponsesFilepath = "txt/error_occured.txt";

        private string[] _helpResponses =  {"Команды помощи не подгрузились мне очень жаль((("};

        private const string UseCommandHelpString = "Воспользуйтесь командой help.";

        private string[] _commandUnknownResponses =
            {"Извини, но я пока тебя не понимаю(((", "Я пока не знаю такой команды(((", "Я тебя не понял, прости((("};

        private string[] _errorOccuredResponses =
        {
            "Произошла ошибка, мне очень жаль(((", "Что-то пошло не так, этот пидор ща пофиксит(или нет)))"
        };

        private void LoadTextFiles()
        {
            try
            {
                //TODO Handle Correctly
                _helpResponses = File.ReadAllLines(HelpStringsFilepath);
                _commandUnknownResponses = File.ReadAllLines(CommandUnknownResponsesFilepath);

                _errorOccuredResponses = File.ReadAllLines(ErrorOccuredResponsesFilepath);
            }
            catch (Exception)
            {
            }
        }

        private async Task PrintRandomError(BotResponse botResponse, ResponseHandler responseHandler)
        {
            botResponse.Response = _errorOccuredResponses.RandomElement();

            await Task.Run(() => responseHandler(botResponse));
        }

        public async Task WhatIsLove(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (botTask.Offset + CommandPosition < botTask.Body.Length &&
                botTask.BodySplitted[CommandPosition + botTask.Offset].Contains("вячеслав"))
            {
                botTask.WasHandled = true;

                var botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId,
                    Response = "Baby don\'t hurt me\nDon\'t hurt me\nNo more"
                };
                
                await Task.Run(() => responseHandler(botResponse));
            }
            
        }

        public async Task CommandUnknown(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (!botTask.WasHandled)
            {

                var botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId,
                    Response = _commandUnknownResponses.RandomElement()
                };

                botResponse.Response += $"\n{UseCommandHelpString} Или попробуйте:\n {_helpResponses.RandomElement()}";

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
            bot.RegisterTaskHandler(Help);
            bot.RegisterTaskHandler(CalledByName);
            bot.RegisterTaskHandler(WhatIsLove);
            bot.RegisterTaskHandler(Calculate);

            bot.RegisterGreetingTaskHandler(Greeting);

            bot.RegisterOnUnknownCommandHandler(CommandUnknown);
        }

        public BotFunctionsInstaller()
        {
            LoadTextFiles();
        }
    }
}