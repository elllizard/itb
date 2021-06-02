using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Models.Keyboards
{
    public class CompareAuthorizedKeyboard : Keyboard
    {
        public override string Message => "🤼 Compare Profiles\n\nType username or choose one of the options below. 👇";

        public override ReplyKeyboardMarkup Markup => new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[] {"🙋‍ Compare with me"},
                new KeyboardButton[] {"🏡 Home"},
            },
            resizeKeyboard: true
        );
    }
}