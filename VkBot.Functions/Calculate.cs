﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
 using Jace;
 using VkBot.Bot;
using VkBot.BotApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
        private string[] calculateComands = {"считай", "реши", "вычисли"};

        public async Task Calculate(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            int taskCommandPostion = botTask.Offset + CommandPosition;
            int expressionPostion = taskCommandPostion + 1;

            if (expressionPostion < botTask.Body.Length &&
                botTask.BodySplitted[taskCommandPostion].ContainsAny(calculateComands))
            {
                var botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId
                };

                botTask.WasHandled = true;

                string expression = botTask.BodySplitted
                    .Skip(expressionPostion)
                    .Aggregate((previous, current) => $"{previous}{current}");

                var calculationEngine = new CalculationEngine();

                try
                {
                    double result = calculationEngine.Calculate(expression);

                    botResponse.Response = $"Ответ: {result}";
                }
                catch (Exception)
                {
                    botResponse.Response = "Неверное выражение";
                }

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}