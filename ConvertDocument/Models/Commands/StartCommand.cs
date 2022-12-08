using System.Linq.Expressions;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConvertDocument.Models.Commands
{
    public class StartCommand : ICommand <Message>
    {
        public string Name => @"/start";
        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Добро, брат, кидай документ, а я посмотрю, что смогу сделать.");
        }
    }
}
