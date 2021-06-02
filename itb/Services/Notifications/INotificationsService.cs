using System;
using System.Threading.Tasks;
using itb.Models.Commands;
using itb.Services.Chat;
using itb.Services.Statistic;
using itb.Services.Telegram;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace itb.Services.Notifications
{
    public interface INotificationsService
    {
        public Commands Commands { get; }
        public Task HandleNotification(Update update);
        public Task HandleCommand(Update update);
        public Task InitCommandOrInitDefault(Command command, Update update);
        public Task ExecuteCommandOrInitDefault(Command command, Update update);
        public Task InitDefault(Update update);
        
        public ILogger<INotificationsService> GetLogger();
        public ITelegramService GetTelegramService();
        public IChatService GetChatService();
        public IStatisticService GetStatisticService();
    }
}