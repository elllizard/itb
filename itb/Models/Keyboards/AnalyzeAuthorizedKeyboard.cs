using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Models.Keyboards
{
    public class AnalyzeAuthorizedKeyboard : Keyboard
    {
        public override string Message => "🧐 Analyze Profile\n\nType username or choose one of the options below. 👇";

        public override ReplyKeyboardMarkup Markup => new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[] {"🧐 Analyze My Profile"},
                new KeyboardButton[] {"🏡 Home"},
            },
            resizeKeyboard: true
        );
    }
}