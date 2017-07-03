using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.BotApi.Messages;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBot.BotApi
{

    public class VkApi : IVkApi
    {
        private readonly VkNet.VkApi _vkApi;

        private const ulong ApplicatonId = 6095236;

        private const int UnreadMessagesMaxCount = 20;

        private const string GetDialogsMethodUrl = "https://api.vk.com/method/messages.getDialogs";

        private const string SearchMethodUrl = "https://api.vk.com/method/messages.search";

        public IWindsorContainer Container { get; } = new WindsorContainer()
            .Install(new VkApiWindsorIntaller());

        public bool IsAuthorized => _vkApi.IsAuthorized;

        public VkApi()
        {
            _vkApi = Container.Resolve<VkNet.VkApi>();
        }

        public async Task<VkMessagesResponse> GetDialogsAsync(VkDialogsParameters vkDialogsParameters)
        {
            if (!_vkApi.IsAuthorized)
            {
                throw new InvalidOperationException("You are not authorized");
            }

            string dialogsJsonString = await GetDialogsMethodUrl
                .PostUrlEncodedAsync(new
                {
                    count = vkDialogsParameters.Count,
                    unread = vkDialogsParameters.Unread,
                    important = vkDialogsParameters.Important,
                    unanswered = vkDialogsParameters.Unanswered,
                    offset = vkDialogsParameters.Offset,
                    previewLength = vkDialogsParameters.PreviewLength,
                    access_token = _vkApi.Token,
                    v = VkNet.VkApi.VkApiVersion,
                })
                .ReceiveString();

            return JsonConvert.DeserializeObject<VkMessagesResponse>(dialogsJsonString);
        }

        

        private void SetDialogNames(VkMessages messages)
        {
            foreach (VkMessageItem messageItem in messages.Items)
            {
                if (messageItem.Message.ChatId != null)
                {
                    messageItem.DialogName = messageItem.Message.Title;

                    continue;
                }

                long messageUserId = messageItem.Message.UserId;

                User user = _vkApi.Users.Get(messageUserId);

                messageItem.DialogName = $"{user.FirstName} {user.LastName}";
            }
        }

        public User GetUser(long userId)
        {
            return _vkApi.Users.Get(userId);
        }

        public bool Login(string accessToken)
        {
            var apiAuthParams = new ApiAuthParams
            {
                AccessToken = accessToken
            };

            _vkApi.Authorize(apiAuthParams);

            return true;
        }

        public bool Login(string login, string password, Func<string> twoFactorAuthFunc)
        {
            var apiAuthParams = new ApiAuthParams
            {
                Login = login,
                Password = password,
                TwoFactorAuthorization = twoFactorAuthFunc,
                ApplicationId = ApplicatonId,
                Settings = Settings.Messages
            };

            _vkApi.Authorize(apiAuthParams);

            return true;
        }

        public async Task<VkMessages> GetDialogsListAsync(bool setDialogNames = true)
        {
            VkMessages messages = (await GetDialogsAsync(new VkDialogsParameters())).Messages;

            if (setDialogNames)
            {
                SetDialogNames(messages);
            }

            return messages;
        }

        public async Task<ICollection<Message>> GetUnreadMessagesAsync()
        {
            List<Message> unreadMessages = new List<Message>();

            VkMessages messages = (await GetDialogsAsync(new VkDialogsParameters
            {
                Count = VkDialogsParameters.MaxMessagesCount,
                Unread = true
            })).Messages;

            foreach (VkMessageItem vkMessageItem in messages.Items)
            {
                List<Message> historyMessages = _vkApi.Messages.GetHistory(new MessagesGetHistoryParams
                    {
                        Count = UnreadMessagesMaxCount,
                        PeerId = vkMessageItem.Message.DialogId
                    })
                    .Messages
                    .Where(m => m.ReadState == MessageReadState.Unreaded)
                    .ToList();

                unreadMessages.AddRange(historyMessages);
            }

            return unreadMessages;
        }

        public async Task<long> GetWordCountAsync(string word, long? dialogId = null)
        {
            string messagesJson = await SearchMethodUrl.PostUrlEncodedAsync(new
                {
                    access_token = _vkApi.Token,
                    v = VkNet.VkApi.VkApiVersion,
                    q = word,
                    count = 100,
                    peer_id = dialogId
                })
                .ReceiveString();

            var messages = JsonConvert.DeserializeObject<VkMessagesResponse>(messagesJson).Messages;

            return messages.Count;
        }

        public long PostMessage(MessagesSendParams messageParams)
        {
            return _vkApi.Messages.Send(messageParams);
        }
    }
}