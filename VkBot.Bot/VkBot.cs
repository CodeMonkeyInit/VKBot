﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VkBot.BotApi;
using VkBot.IocContainer;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkBot.Bot
{
    public class VkBot : IVkBot, IDisposable
    {
        private readonly IContainer _container;

        private readonly IMapper _mapper;

        private BotTaskHandler _taskHandler;
        private BotTaskHandler _onUnknownCommandHandler;
        private BotTaskHandler _greetingTaskHandler;

        private readonly ILogger _logger;

        private Task _botMessagesCheckTask;

        private readonly char[] _wordSeparators = {' ', ',', '.', '!', '?'};

        private const int NameLength = 1;
        private const int NoOffset = 0;
        
        private CancellationTokenSource _cancellationTokenSource;

        public readonly string[] Greetings = { "привет", "здравствуйте", "прив" , "здравствуй", "хай", "hello", "hi", "ку", "дорова", "дратути", "спокойной", "пока"};
        public readonly string[] Names = { "бот", "алеша", "алёша", "алексей", "олеша", "alosha", "ололоша", "bot" };

        public readonly IVkApi Api;
        

        public bool IsAuthorized => Api.IsAuthorized;

        public bool BotWorking => _botMessagesCheckTask != null;

        public VkBot()
        {
            _container = new Container().Install(new BotContainerInstaller());
            Api = _container.Resolve<IVkApi>();
            _logger = _container.Resolve<ILogger>();
            _mapper = new Mapper(new MapperConfiguration(config => config
                .CreateMap<Message, BotTask>()));
        }
        
        public VkBot(string accessToken): this()
        {
            SetAccessToken(accessToken);
        }

        private bool FindWordsStartingWithSubstringsInFirstWord(Message message, string[] substrings)
        {
            var messageLowercase = message.Body.Trim().Split(' ')[0].ToLower();

            foreach (string substring in substrings)
            {
                bool contains = messageLowercase.StartsWith(substring);

                if (contains)
                {
                    return true;
                }
            }

            return false;
        }
        
        private void CheckIfAuthorized()
        {
            if (!IsAuthorized)
            {
                throw new InvalidOperationException("Bot is not authorized. "
                                                    + "Consider calling Authorize function first.");
            }
        }

        private bool BotWasCalledInChat(Message message)
        {
            if (FindWordsStartingWithSubstringsInFirstWord(message, Names) && message.ChatId != null)
            {
                return true;
            }

            return false;
        }

        private bool SomeoneInChatGreeted(Message message)
        {
            if (FindWordsStartingWithSubstringsInFirstWord(message, Greetings) && message.ChatId != null)
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
                        await handleTaskArguments.Handler(botTask, SendResponse, Api);
                    }
                    catch (Exception e)
                    {
                        _logger.LogException(e);
                    }

                    handleTaskArguments.UnknownCommandHandler?.Invoke(botTask, SendResponse, Api);
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
                    ICollection<Message> unreadMessages = await Api.GetUnreadMessagesAsync();

                    List<Message> directMessages = unreadMessages.Where(RecievedDirectMessage).ToList();

                    List<Message> messagesWhereBotWasCalled = unreadMessages.Where(BotWasCalledInChat).ToList();

                    List<Message> messagesWhereSomeoneInChatGreeted = unreadMessages.Where(SomeoneInChatGreeted).ToList();

                    List<BotTask> botTasks = _mapper.Map<List<BotTask>>(messagesWhereBotWasCalled);

                    List<BotTask> greetingTasks = _mapper.Map<List<BotTask>>(messagesWhereSomeoneInChatGreeted);

                    List<BotTask> botDirectTasks = _mapper.Map<List<BotTask>>(directMessages);

                    Task directMessagesTasks = HandleTasksAsync(new HandleTaskArguments
                    {
                        Handler = _taskHandler,
                        BotTasks = botDirectTasks,
                        CommandsOffset = NoOffset,
                        UnknownCommandHandler = _onUnknownCommandHandler
                    });
                    
                    Task groupChatsTasks = HandleTasksAsync(new HandleTaskArguments
                    {
                        Handler = _taskHandler,
                        BotTasks = botTasks,
                        CommandsOffset = NameLength,
                        UnknownCommandHandler = _onUnknownCommandHandler
                    });
                    
                    Task grettingTasks = HandleTasksAsync(new HandleTaskArguments
                    {
                        Handler = _greetingTaskHandler,
                        BotTasks = greetingTasks,
                        CommandsOffset = NoOffset
                    });

                    await directMessagesTasks;
                    await groupChatsTasks;
                    await grettingTasks;

                }
                catch (Exception e)
                {
                    //VkApi Fucked
                    _logger.LogException(e);
                }
                
                await Task.Delay(timeSpanBetweenChecks, cancelationToken);
            }
        }
        
        public async void SendResponse(BotResponse response)
        {
            if (string.IsNullOrEmpty(response.Response))
            {
                var exception = new ArgumentException(nameof(response.Response) + "Message is empty");

                _logger.LogException(exception);
                
                return;
            }

            await Task.Run(() => Api.PostMessage(new MessagesSendParams
            {
                PeerId = response.PeerId,
                Message = response.Response
            }));
        }

        public void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException($"{nameof(accessToken)} invalid");
            }

            Api.Login(accessToken);
            
            if (!IsAuthorized)
            {
                throw new ArgumentException($"{nameof(accessToken)} invalid");
            }
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
            CheckIfAuthorized();
            
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

        public void Dispose()
        {
            _botMessagesCheckTask?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}