using ConvertDocument.Models.Convert;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConvertDocument.Models.Commands
{
    public class SendDocumentCommand : ICommand<Message>
    {
        public string Name => "Document";
        public async Task Execute(Message message, TelegramBotClient client)
        {
            string documentName = message.Document?.FileName;
            if (!ConvertInfo.CheckType(documentName.ToUpper())) 
            {
                await client.SendTextMessageAsync(message.Chat.Id, "Ничего не смогу сделать,такой формат не поддерживается"); 
                return;
            } 
            string[] ConvertibleTypes = ConvertInfo.GetConvertibleTypes(documentName);
            var inlineKeyboard = new InlineKeyboardButton[ConvertibleTypes.Length];
            for (int count = 0; count < ConvertibleTypes.Length; count++)
            {
                inlineKeyboard[count] = new InlineKeyboardButton(ConvertibleTypes[count])
                {
                    CallbackData = ConvertibleTypes[count].ToUpper(),
                };
            }
            InlineKeyboardMarkup keyboardMarkup = new InlineKeyboardMarkup(inlineKeyboard);
            await client.SendTextMessageAsync(message.Chat.Id, $"Так,выбери во что преобразовать:", replyMarkup: keyboardMarkup, replyToMessageId: message.MessageId);
        }
    }
}
