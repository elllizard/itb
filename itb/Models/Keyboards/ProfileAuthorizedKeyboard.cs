using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Models.Keyboards
{
    public class ProfileAuthorizedKeyboard : Keyboard
    {
        public override string Message => "😎 My Profile\nHere you can manage your profile.\n\nChoose one of the options below. 👇";

        public override ReplyKeyboardMarkup Markup => new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[] {"🧐 Analyze My Profile"},
                new KeyboardButton[] {"🐣 Update My Username", "🙈 Delete My Username"},
                new KeyboardButton[] {"🏡 Home"},
            },
            resizeKeyboard: true
        );
    }
}