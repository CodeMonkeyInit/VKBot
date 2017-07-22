using System.Text;
using System.Threading.Tasks;
using VkBot.Bot;
using VkBot.BotApi;

namespace VkBot.Functions
{
    public partial class BotFunctionsInstaller
    {
        public async Task Help(BotTask botTask, ResponseHandler responseHandler, IVkApi vkApi)
        {
            string[] botTaskBodySplitted = botTask.BodySplitted;

            if (botTaskBodySplitted[CommandPosition + botTask.Offset].Contains("помо") ||
                botTaskBodySplitted[CommandPosition + botTask.Offset].Contains("help"))
            {
                var botResponse = new BotResponse()
                {
                    PeerId = botTask.PeerId
                };

                botTask.WasHandled = true;

                var helpStringBuilder = new StringBuilder();

                helpStringBuilder.Append(
                    "К боту можно обращатся без имени в лс и с именем в диалогах (например: \"Алеша, анекдот\")\nИли:\n");

                foreach (string helpHint in _helpResponses)
                {
                    helpStringBuilder.Append($"{helpHint}\n");
                }

                botResponse.Response = helpStringBuilder.ToString();

                await Task.Run(() => responseHandler(botResponse));
            }
        }
    }
}
