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
        public const int NamePosition = -1;

        public async Task CalledByName(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (NamePosition + botTask.Offset < 0 || botTask.WasHandled || botTask.BodySplitted.Length > 1)
            {
                return;
            }

            foreach (string botName in botTask.BotNames)
            {
                if (botTask.BodySplitted[NamePosition + botTask.Offset] == botName)
                {
                    botTask.WasHandled = true;

                    var botResponse = new BotResponse
                    {
                        PeerId = botTask.PeerId,
                        Response = "Что?"
                    };

                    await Task.Run(() => responseHandler(botResponse));
                    
                    return;
                }
            }
            
        }
    }
}
