using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Windsor;
using VkBot.BotApi;
using VkBot.BotApi.Messages;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBot.Bot
{
    public class VkBot : IVkBot
    {
        private readonly IWindsorContainer _container;

        private readonly IMapper _mapper;

        private BotTaskHandler _taskHandler;

        private BotTaskHandler _onUnknownCommandHandler;

        private BotTaskHandler _greetingTaskHandler;

        private Task _botMessagesCheckTask;

        private readonly char[] _wordSeparators = {' ', ',', '.', '!', '?'};

        private const string LoggerPath = "logs/log.txt";

        private readonly object _logger = new object();

        private const int NameLength = 1;

        private const int NoOffset = 0;
        
        private CancellationTokenSource _cancellationTokenSource;

        public readonly string[] Greetings = { "привет", "здравствуйте", "прив" , "здравствуй", "хай", "hello", "hi", "ку", "дорова", "дратути", "спокойной", "пока"};

        public readonly string[] Names = { "бот", "алеша", "алёша", "алексей", "олеша", "alosha", "ололоша", "bot" };

        public readonly IVkApi _api;

        public bool IsAuthorized => _api.IsAuthorized;

        public bool BotWorking => _botMessagesCheckTask != null;

        
        public VkBot(string accessToken)
        {
            _container = new WindsorContainer().Install(new VkApiWindsorIntaller());
            _api = _container.Resolve<IVkApi>();
            _api.Login(accessToken);
            
            _mapper = new Mapper(new MapperConfiguration(config => config.CreateMap<Message, BotTask>()));

            if (!IsAuthorized)
            {
                throw new ArgumentException($"{nameof(accessToken)} invalid");
            }
        }

        private bool FindSubstringsInMessageFirstWord(Message message, string[] substrings)
        {
            var messageLowercase = message.Body.Trim().Split(' ')[0].ToLower();

            foreach (string substring in substrings)
            {
                bool contains = messageLowercase.Contains(substring);

                if (contains)
                {
                    return true;
                }
            }

            return false;
        }

        private bool BotWasCalledInChat(Message message)
        {
            if (FindSubstringsInMessageFirstWord(message, Names) && message.ChatId != null)
            {
                return true;
            }

            return false;
        }

        private bool SomeoneInChatGreeted(Message message)
        {
            if (FindSubstringsInMessageFirstWord(message, Greetings) && message.ChatId != null)
            {
                return true;
            }

            return false;
        }

        private bool RecievedDirectMessage(Message m)
        {
            if (m.ChatId == null && m.UserId != null)
            {
                return true;
            }
            return false;
        }

        void LogException(Exception e)
        {
            lock (_logger)
            {
                File.AppendAllText(LoggerPath, $"{DateTime.Now}:\n Exception :{e.Message}\n StackTrace: \n{e.StackTrace}\n");
            }
        }

        private async Task HandleTasksAsync(HandleTaskArguments handleTaskArguments)
        {
            if (handleTaskArguments.Handler != null)
            {
                foreach (BotTask botTask in handleTaskArguments.BotTasks)
                {
                    botTask.Body = botTask.Body.Trim().ToLower();

                    botTask.BodySplitted =
                        botTask.Body.Split(_wordSeparators, StringSplitOptions.RemoveEmptyEntries);

                    botTask.Greetings = Greetings;
                    botTask.Offset = handleTaskArguments.CommandsOffset;
                    botTask.BotNames = Names;

                    try
                    {
                        await handleTaskArguments.Handler(botTask, SendResponse, _api);
                    }
                    catch (Exception e)
                    {
                        LogException(e);
                    }

                    handleTaskArguments.UnknownCommandHandler?.Invoke(botTask, SendResponse, _api);
                }
            }
        }

        
        private async Task PerformMessageChecksAsync(CancellationToken cancelationToken, TimeSpan timeSpanBetweenChecks)
        {
            while (true)
            {
                cancelationToken.ThrowIfCancellationRequested();

                try
                {
                    ICollection<Message> unreadMessages = await _api.GetUnreadMessagesAsync();

                    List<Message> directMessages = unreadMessages.Where(RecievedDirectMessage).ToList();

                    List<Message> messagesWhereBotWasCalled = unreadMessages.Where(BotWasCalledInChat).ToList();

                    List<Message> messagesWhereSomeoneInChatGreeted = unreadMessages.Where(SomeoneInChatGreeted).ToList();

                    List<BotTask> botTasks = _mapper.Map<List<BotTask>>(messagesWhereBotWasCalled);

                    List<BotTask> greetingTasks = _mapper.Map<List<BotTask>>(messagesWhereSomeoneInChatGreeted);

                    List<BotTask> botDirectTasks = _mapper.Map<List<BotTask>>(directMessages);

                    await HandleTasksAsync(new HandleTaskArguments
                    {
                        Handler = _taskHandler,
                        BotTasks = botDirectTasks,
                        CommandsOffset = NoOffset,
                        UnknownCommandHandler = _onUnknownCommandHandler
                    });

                    await HandleTasksAsync(new HandleTaskArguments
                    {
                        Handler = _taskHandler,
                        BotTasks = botTasks,
                        CommandsOffset = NameLength,
                        UnknownCommandHandler = _onUnknownCommandHandler
                    });

                    await HandleTasksAsync(new HandleTaskArguments
                    {
                        Handler = _greetingTaskHandler,
                        BotTasks = greetingTasks,
                        CommandsOffset = NoOffset
                    });

                }
                catch (Exception e)
                {
                    //VkApi Fucked

                    LogException(e);
                }
                
                await Task.Delay(timeSpanBetweenChecks, cancelationToken);
            }
        }
        

        public async void SendResponse(BotResponse response)
        {
            await Task.Run(() => _api.PostMessage(new MessagesSendParams
            {
                PeerId = response.PeerId,
                Message = response.Response
            }));
        }

        public void RegisterTaskHandler(BotTaskHandler taskHandler)
        {
            _taskHandler += taskHandler;
        }

        public void RegisterOnUnknownCommandHandler(BotTaskHandler taskHandler)
        {
            _onUnknownCommandHandler = taskHandler;
        }

        public void RegisterGreetingTaskHandler(BotTaskHandler taskHandler)
        {
            _greetingTaskHandler = taskHandler;
        }

        public void Install(IBotFunctionsInstaller installer)
        {
            installer.Install(this);
        }

        public void Start(TimeSpan timeBetweenChecks)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("Bot already started");
            }

            _cancellationTokenSource = new CancellationTokenSource();

            CancellationToken cancelationToken = _cancellationTokenSource.Token;

            _botMessagesCheckTask = new Task(
                async () =>
                {
                    try
                    {
                        await PerformMessageChecksAsync(cancelationToken, timeBetweenChecks);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    
                },
                cancelationToken);

            _botMessagesCheckTask.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();

            _botMessagesCheckTask.Dispose();
            _botMessagesCheckTask = null;
        }
    }
}