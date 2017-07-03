using System;
using System.Linq;
using System.Threading.Tasks;
using VkBot.BotApi;
using VkBot.BotApi.Messages;
using VkNet.Model.RequestParams;

namespace VkBot.ConsoleTestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var vkWordCounter = new VkApi();

            string authToken = "2442e13e5392415bd8a51fe92ac19b12bde363f7532136ba91c952948af92de9ab2fa98c0ab01ce88b3ac";

            vkWordCounter.Login(authToken);

            using (Task<VkMessages> vkMessages = vkWordCounter.GetDialogsListAsync())
            {
                vkMessages.Wait();

                foreach (VkMessageItem message in vkMessages.Result.Items)
                {
                    VkMessage vkMessage = message.Message;
                    Console.WriteLine($"id = {vkMessage.ChatId ?? vkMessage.UserId} {message.DialogName}");
                }

                Console.Write("Введите id диалога: ");
                long dialogId = long.Parse(Console.ReadLine());

                VkMessageItem messageItem = vkMessages.Result.Items.FirstOrDefault(
                    m => m.Message.ChatId == dialogId || m.Message.UserId == dialogId);

                long messageDialogId = messageItem.Message.DialogId;

                string zryaCountString = $"Кол-во \"зря\" в диалоге: {vkWordCounter.GetWordCountAsync("зря", messageDialogId).Result} " +
                                         "\n P.S С нелюбовью, бот";

                vkWordCounter.PostMessage(new MessagesSendParams
                {
                    Message = zryaCountString,
                    PeerId = messageDialogId
                });

                Console.WriteLine(zryaCountString);
            }

            Console.ReadKey();
        }
    }
}