using System.Text;
using System.Threading.Tasks;
using itb.Models.Configurtations;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramConfiguration _telegramConfig;
        private readonly ApplicationConfiguration _applicationConfig;
        private readonly TelegramBotClient _client;

        public TelegramService(
            IOptions<TelegramConfiguration> telegramConfig,
            IOptions<ApplicationConfiguration> applicationConfig
        )
        {
            _telegramConfig = telegramConfig.Value;
            _applicationConfig = applicationConfig.Value;

            _client = new TelegramBotClient(_telegramConfig.Token);
        }

        public async Task SetWebhookAsync()
        {
            StringBuilder _url = new StringBuilder();
            _url.Append(_applicationConfig.Url);
            _url.Append(_telegramConfig.WebhookPath);

            await _client.SetWebhookAsync(_url.ToString());
        }

        public async Task<Message> SendTextMessageAsync(long id, string text, ParseMode parseMode = ParseMode.Default)
        {
            return await _client.SendTextMessageAsync(id, text, parseMode);
        }

        public async Task<Message> SendReplyKeyboardAsync(long id, string text, ReplyKeyboardMarkup replyKeyboard, ParseMode parseMode = ParseMode.Default)
        {
            return await _client.SendTextMessageAsync(
                chatId: id,
                text: text,
                parseMode: parseMode,
                replyMarkup: replyKeyboard
            );
        }

        public async Task StartTypingAsync(long id)
        {
            await _client.SendChatActionAsync(
                chatId: id,
                chatAction: ChatAction.Typing
            );
        }
    }
}