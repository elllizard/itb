using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using itb.Models.Api;
using itb.Models.Keyboards;
using itb.Services.Notifications;
using itb.Services.Telegram;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace itb.Models.Commands
{
    public class CompareProfilesCommand : Command
    {
        public override string Name => "🤼 Compare Profiles";
        public override string Path => "/home/compare";

        public override async Task Init(Update update, INotificationsService notificationsService)
        {
            ITelegramService _telegramService = notificationsService.GetTelegramService();

            await _telegramService.StartTypingAsync(update.Message.Chat.Id);

            try
            {
                ChatModel _chat = await notificationsService.GetChatService()
                    .UpdatePathAsync(update.Message.Chat.Id, Path);

                Keyboard _keyboard;

                if (_chat.Username != null && (_chat.State == null || _chat.State != _chat.Username))
                {
                    _keyboard = new CompareAuthorizedKeyboard();
                }
                else
                {
                    _keyboard = new BackToHomeKeyboard();
                }

                await _telegramService.SendReplyKeyboardAsync(
                    update.Message.Chat.Id,
                    _keyboard.Message,
                    _keyboard.Markup
                );
            }
            catch (Exception _updateException)
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
                    ChatModel _chat = await notificationsService.GetChatService().ReadAsync(update.Message.Chat.Id);
                    string _username = update.Message.Text.Equals("🙋‍ Compare with me")
                        ? _chat.Username
                        : update.Message.Text;

                    if (_chat.State is null)
                    {
                        await notificationsService.GetChatService()
                            .UpdateStateAsync(update.Message.Chat.Id, _username);
                        await Init(update, notificationsService);
                    }
                    else
                    {
                        List<StatisticModel> _statistics =
                            await notificationsService.GetStatisticService().ReadAsync(_chat.State, _username);

                        List<ExtendedStatisticModel> _extendedStatistics =
                            notificationsService.GetStatisticService().CalculateStatistic(_statistics);

                        if (_statistics.Count > 0)
                        {
                            StringBuilder _message = new StringBuilder();
                            _message.Append("Profiles comparison is ready! 👍\n\n");
                            _message.Append($"<b>Follows:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.Follows);
                                _message.Append((statistic.Follows == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.Follows}</code>\n");
                            });

                            _message.Append($"\n<b>Followed By:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.FollowedBy);
                                _message.Append((statistic.FollowedBy == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.FollowedBy}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Posts:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.PostsCount);
                                _message.Append((statistic.PostsCount == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.PostsCount}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Total Likes:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.TotalLikes);
                                _message.Append((statistic.TotalLikes == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.TotalLikes}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Max Likes:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.MaxLikes);
                                _message.Append((statistic.MaxLikes == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.MaxLikes}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Min Likes:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.MinLikes);
                                _message.Append((statistic.MinLikes == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.MinLikes}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Total Comments:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.TotalComments);
                                _message.Append((statistic.TotalComments == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.TotalComments}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Max Comments:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.MaxComments);
                                _message.Append((statistic.MaxComments == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.MaxComments}</code>\n");
                            });
                            
                            _message.Append($"\n<b>Min Comments:</b>\n");
                            _extendedStatistics.ForEach(statistic =>
                            {
                                int _max = _extendedStatistics.Max(s => s.MinComments);
                                _message.Append((statistic.MinComments == _max ? "🥇" : "🥈") + $"{statistic.Username}: <code>{statistic.MinComments}</code>\n");
                            });

                            await _telegramService.SendTextMessageAsync(
                                update.Message.Chat.Id,
                                _message.ToString(),
                                ParseMode.Html
                            );

                            await notificationsService.InitDefault(update);
                        }
                        else
                        {
                            await Error(update, notificationsService);
                            await notificationsService.InitDefault(update);
                        }
                    }
                }
                catch (Exception _updateException)
                {
                    await Error(update, notificationsService);
                    await notificationsService.InitDefault(update);
                }
            }
        }
    }
}