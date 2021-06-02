using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Models.Keyboards
{
    public class HomeKeyboard : Keyboard
    {
        public override string Message => "✌️ Hello, Glad to see you! How can I help you?\n\nChoose one of the options below. 👇";

        public override ReplyKeyboardMarkup Markup => new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[] {"😎 My Profile"},
                new KeyboardButton[] {"🧐 Analyze Profile"},
                new KeyboardButton[] {"🤼 Compare Profiles"},
            },
            resizeKeyboard: true
        );
    }
}