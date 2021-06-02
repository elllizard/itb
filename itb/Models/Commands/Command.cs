using System;
using System.Threading.Tasks;
using itb.Services.Notifications;
using itb.Services.Telegram;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace itb.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract string Path { get; }

        public abstract Task Init(Update update, INotificationsService notificationsService);

        public abstract Task Execute(Update update, INotificationsService notificationsService);

        public async Task CheckBeforeHandle(Update update, INotificationsService notificationsService)
        {
            Command _nextCommand = notificationsService.Commands.GetCommandByName(update.Message.Text);
            if (_nextCommand != null)
            {
                await notificationsService.HandleCommand(update);
            }
            else
            {
                string _exception = $"Failed command: '{Name}': Can not find command with name '{update.Message.Text}'";
                notificationsService.GetLogger().LogWarning(_exception);
                throw new Exception(_exception);
            }
        }
        public async Task Error(Update update, INotificationsService notificationsService)
        {
            ITelegramService _telegramService = notificationsService.GetTelegramService();
            await _telegramService.SendTextMessageAsync(update.Message.Chat.Id,
                "😢 Sorry, something went wrong.\n\nPlease try again."
            );
        }

        public bool NameEquals(string name)
        {
            return name.Equals(this.Name);
        }

        public bool PathEquals(string path)
        {
            return path.Equals(this.Path);
        }
    }
}