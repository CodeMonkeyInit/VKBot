using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkBot.BotApi.Messages;

namespace VkBot.Bot
{
    public class BotTask
    {
        public long? UserId { get; set; }

        public long? ChatId { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Body { get; set; }
        
        /// <summary>
        /// Тело сообщения поделенное на части
        /// </summary>
        public string[] BodySplitted { get; set; }

        public bool WasHandled { get; set; }

        public int Offset { get; set; }

        public string[] BotNames { get; set; }

        public string[] Greetings { get; set; }

        public long PeerId
        {
            get
            {
                if (!ChatId.HasValue)
                {
                    return UserId.Value;
                }

                return ChatId.Value + VkMessage.ChatIdOffset;
            }
        }
    }
}
