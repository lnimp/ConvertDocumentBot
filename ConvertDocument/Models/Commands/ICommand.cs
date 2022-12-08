using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConvertDocument.Models.Commands
{
    public interface ICommand <T>
    {
        public string Name { get; }
        public  Task Execute(T update, TelegramBotClient botClient);
    }
}
