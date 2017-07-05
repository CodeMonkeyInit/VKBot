using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Bot;
using VkBot.BotApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
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
    }
}
