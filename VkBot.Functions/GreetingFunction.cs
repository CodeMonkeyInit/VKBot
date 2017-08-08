using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkBot.Bot;
using VkBot.BotApi;
using VkNet.Model;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
        public async Task Greeting(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            var botTaskBodySplitted = botTask.BodySplitted;

            foreach (string greeting in botTask.Greetings)
            {
                string message = botTaskBodySplitted[CommandPosition + botTask.Offset];

                if (message == greeting)
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
                        Response = $"И тебе {greeting}, {user.FirstName}!"
                    };

                    await Task.Run(() => responseHandler(botResponse));
                }
            }
        }
    }
}
