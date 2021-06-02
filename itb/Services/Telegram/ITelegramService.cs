using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Services.Telegram
{
    public interface ITelegramService
    {
        public Task SetWebhookAsync();

        public Task<Message> SendTextMessageAsync(long id, string text, ParseMode parseMode = ParseMode.Default);

        public Task<Message> SendReplyKeyboardAsync(long id, string text, ReplyKeyboardMarkup replyKeyboard, ParseMode parseMode = ParseMode.Default);

        public Task StartTypingAsync(long id);
    }
}