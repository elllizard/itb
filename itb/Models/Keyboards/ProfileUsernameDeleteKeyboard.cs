using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Models.Keyboards
{
    public class ProfileUsernameDeleteKeyboard : Keyboard
    {
        public override string Message => "🙈 Delete My Username\nAre you sure you want to delete your username?\n\nChoose one of the options below. 🤨";

        public override ReplyKeyboardMarkup Markup => new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[] {"😔‍ Confirm Delete"},
                new KeyboardButton[] {"🏡 Home"},
            },
            resizeKeyboard: true
        );
    }
}