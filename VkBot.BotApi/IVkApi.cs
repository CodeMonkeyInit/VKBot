using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkBot.BotApi.Messages;
using VkBot.IocContainer;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBot.BotApi
{
    public interface IVkApi
    {
        IContainer Container { get; }

        bool IsAuthorized { get; }

        Task<VkMessagesResponse> GetDialogsAsync(VkDialogsParameters vkDialogsParameters);
        Task<VkMessages> GetDialogsListAsync(bool setDialogsNames = true);
        Task<ICollection<Message>> GetUnreadMessagesAsync();
        Task<long> GetWordCountAsync(string word, long? dialogId = default(long?));
        bool Login(string accessToken);
        bool Login(string login, string password, Func<string> twoFactorAuthFunc);
        long PostMessage(MessagesSendParams messageParams);
        User GetUser(long userId);
        void AddEveryOneToFriendsList();
    }
}