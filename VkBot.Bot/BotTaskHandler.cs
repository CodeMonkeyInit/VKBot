using System.Collections.Generic;
using System.Threading.Tasks;
using VkBot.BotApi;
using VkNet.Model;

namespace VkBot.Bot
{
    public delegate Task BotTaskHandler(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi);
}