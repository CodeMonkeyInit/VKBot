using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.Bot;
using VkBot.BotApi;

namespace VkBot.Functions
{
    public  partial class BotFunctionsInstaller
    {
        public async Task GlebSpecial(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            if (botTask.Body.Contains("ave maria"))
            {
                botTask.WasHandled = true;

                var botResponse = new BotResponse
                {
                    PeerId = botTask.PeerId,
                    Response = "Deus Vult!"
                };

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}
