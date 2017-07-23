﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

//                string expression = botTask.BodySplitted
//                    .Skip(expressionPostion)
//                    .Aggregate((previous, current) => $"{previous}{current}");
//
//                string expression = string
//                    .Join(String.Empty, 
//                        botTask.BodySplitted, 
//                        expressionPostion, 
//                        botTask.BodySplitted.Length - expressionPostion);
                string expression = String.Empty;

                for (int i = expressionPostion; i < botTask.BodySplitted.Length; i++)
                {
                    expression += botTask.BodySplitted[i];
                }

                var dataTable = new DataTable();

                //var mathExpression = new Expression(expression);
                string result;
                
                try
                {
                    result = dataTable.Compute(expression, String.Empty).ToString();
                }
                catch (Exception e)
                {
                    result = "Неверное выражение";
                }

                botResponse.Response = $"Ответ: {result}";

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}