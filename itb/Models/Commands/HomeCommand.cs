using System;
using System.Threading.Tasks;
using itb.Models.Api;
using itb.Models.Keyboards;
using itb.Services.Notifications;
using itb.Services.Telegram;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace itb.Models.Commands
{
    public class HomeCommand : Command
    {
        public override string Name => "🏡 Home";

        public override string Path => "/home";

        public override async Task Init(Update update, INotificationsService notificationsService)
        {
            ITelegramService _telegramService = notificationsService.GetTelegramService();

            await _telegramService.StartTypingAsync(update.Message.Chat.Id);
            
            try
            {
                await notificationsService.GetChatService()
                    .UpdateStateAsync(update.Message.Chat.Id, null);
                ChatModel _chat = await notificationsService.GetChatService()
                    .UpdatePathAsync(update.Message.Chat.Id, Path);
            
                HomeKeyboard _homeKeyboard = new HomeKeyboard();

                await _telegramService.SendReplyKeyboardAsync(
                    update.Message.Chat.Id,
                    _homeKeyboard.Message,
                    _homeKeyboard.Markup
                );
            }
            catch(Exception _updateException)
            {
                notificationsService.GetLogger().LogError($"Failed command: '{Name}'", _updateException);
                await Error(update, notificationsService);
                await notificationsService.InitDefault(update);
            }
        }

        public override async Task Execute(Update update, INotificationsService notificationsService)
        {
            ITelegramService _telegramService = notificationsService.GetTelegramService();

            await _telegramService.StartTypingAsync(update.Message.Chat.Id);
            
            try
            {
                await CheckBeforeHandle(update, notificationsService);
            }
            catch (Exception _checkException)
            {
                notificationsService.GetLogger().LogError($"Failed command: '{Name}'", _checkException);
                await Error(update, notificationsService);
                await Init(update, notificationsService);
            }
        }
    }
}