using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itb.Models.Api;
using itb.Services.Notifications;
using itb.Services.Telegram;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace itb.Models.Commands
{
    public class ProfileAnalyzeMyProfileCommand : Command
    {
        public override string Name => "🧐 Analyze My Profile";
        public override string Path => "/home/profile/analyze";
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

                List<StatisticModel> _statistics =
                    await notificationsService.GetStatisticService().ReadAsync(_chat.Username);
                
                List<ExtendedStatisticModel> _extendedStatistics =
                    notificationsService.GetStatisticService().CalculateStatistic(_statistics);
                
                if (_statistics.Count > 0)
                {
                    ExtendedStatisticModel _statistic = _extendedStatistics.First();

                    StringBuilder _message = new StringBuilder();
                    _message.Append($"{_statistic.AvatarUrl}\n\n");
                    _message.Append("Profile analyze is ready! 👍\n\n");
                    _message.Append($"<b>Username:</b> <code>{_statistic.Username}</code>\n");
                    _message.Append($"<b>Follows:</b> <code>{_statistic.Follows}</code>\n");
                    _message.Append($"<b>Followed By:</b> <code>{_statistic.FollowedBy}</code>\n");
                    _message.Append($"<b>Posts:</b> <code>{_statistic.PostsCount}</code>\n");
                    _message.Append($"<b>Total Likes:</b> <code>{_statistic.TotalLikes}</code>\n");
                    _message.Append($"<b>Max Likes:</b> <code>{_statistic.MaxLikes}</code>\n");
                    _message.Append($"<b>Min Likes:</b> <code>{_statistic.MinLikes}</code>\n");
                    _message.Append($"<b>Total Comments:</b> <code>{_statistic.TotalComments}</code>\n");
                    _message.Append($"<b>Max Comments:</b> <code>{_statistic.MaxComments}</code>\n");
                    _message.Append($"<b>Min Comments:</b> <code>{_statistic.MinComments}</code>");
                    
                    await _telegramService.SendTextMessageAsync(update.Message.Chat.Id, _message.ToString(), ParseMode.Html);
                    
                    await Execute(update, notificationsService);
                }
                else
                {
                    await Error(update, notificationsService);
                    await notificationsService.InitDefault(update);
                }
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
            
            await notificationsService.InitDefault(update);
        }
    }
}