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
        public async Task CalledByName(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            foreach (string botName in botTask.BotNames)
            {
                if (botTask.BodySplitted[CommandPosition] == botName)
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
