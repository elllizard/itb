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
    public class ProfileDeleteUsernameCommand : Command
    {
        public override string Name => "🙈 Delete My Username";

        public override string Path => "/home/profile/delete";

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

                ProfileUsernameDeleteKeyboard _keyboard = new ProfileUsernameDeleteKeyboard();

                await _telegramService.SendReplyKeyboardAsync(
                    update.Message.Chat.Id,
                    _keyboard.Message,
                    _keyboard.Markup
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
                try
                {
                    if (update.Message.Text.Equals("😔‍ Confirm Delete"))
                    {
                        await notificationsService.GetChatService()
                            .UpdateUsernameAsync(update.Message.Chat.Id, null);
                    
                        await _telegramService.SendTextMessageAsync(update.Message.Chat.Id,
                            "🙈 Username successfully deleted!"
                        );
                        await notificationsService.InitDefault(update);
                    }
                    else
                    {
                        await Error(update, notificationsService);
                        await Init(update, notificationsService);
                    }
                }
                catch(Exception _updateException)
                {
                    notificationsService.GetLogger().LogError($"Failed command: '{Name}'", _updateException);
                    await Error(update, notificationsService);
                    await notificationsService.InitDefault(update);
                }
            }
        }
    }
}