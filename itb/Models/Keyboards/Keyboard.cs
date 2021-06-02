using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace itb.Models.Keyboards
{
    public abstract class Keyboard
    {
        public abstract string Message { get; }

        public abstract ReplyKeyboardMarkup Markup { get; }
    }
}