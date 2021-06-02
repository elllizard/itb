using System;
using System.Threading.Tasks;
using itb.Models.Api;
using itb.Models.Commands;
using itb.Services.Chat;
using itb.Services.Statistic;
using itb.Services.Telegram;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace itb.Services.Notifications
{
    public class NotificationsService : INotificationsService
    {
        public Commands Commands { get; }
        private readonly ILogger<NotificationsService> _logger;
        private readonly ITelegramService _telegramService;
        private readonly IChatService _chatService;
        private readonly IStatisticService _statisticService;

        public NotificationsService(
            ILogger<NotificationsService> logger,
            ITelegramService telegramService,
            IChatService chatService,
            IStatisticService statisticService
        )
        {
            _logger = logger;
            _telegramService = telegramService;
            _chatService = chatService;
            _statisticService = statisticService;

            Commands = new Commands();
        }

        public async Task HandleNotification(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            try
            {
                ChatModel _chat = await _chatService.ReadOrCreateAsync(update.Message.Chat.Id);
                
                switch (update.Message.Type)
                {
                    case MessageType.Text:
                        if (_chat.Path == null)
                        {
                            await InitDefault(update);
                        }
                        else
                        {
                            Command _command = Commands.GetCommandByPath(_chat.Path);
                            await ExecuteCommandOrInitDefault(_command, update);
                        }

                        break;
                }
            }
            catch (Exception _readOrCreateException)
            {
                _logger.LogError($"Failed to read or create chat {update.Message.Chat.Id}.", _readOrCreateException);
            }
        }

        public async Task HandleCommand(Update update)
        {
            Command _command = Commands.GetCommandByName(update.Message.Text);
            await InitCommandOrInitDefault(_command, update);
        }
        
        public async Task InitCommandOrInitDefault(Command command, Update update)
        {
            if (command != null)
            {
                await command.Init(update, this);
            }
            else
            {
                await InitDefault(update);
            }
        }

        public async Task ExecuteCommandOrInitDefault(Command command, Update update)
        {
            if (command != null)
            {
                await command.Execute(update, this);
            }
            else
            {
                await InitDefault(update);
            }
        }

        public async Task InitDefault(Update update)
        {
            Command _command = Commands.GetCommandByPath("/home");
            await _command.Init(update, this);
        }

        public ILogger<INotificationsService> GetLogger()
        {
            return _logger;
        }

        public ITelegramService GetTelegramService()
        {
            return _telegramService;
        }
        
        public IChatService GetChatService()
        {
            return _chatService;
        }
        
        public IStatisticService GetStatisticService()
        {
            return _statisticService;
        }
    }
}