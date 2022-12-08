using ConvertDocument.Models.Commands;
using ConvertDocument.Models.Convert;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ConvertDocument.Models.Services
{
    public class HandleUpdateService
    {
        private readonly TelegramBotClient botClient;
        private readonly ILogger<HandleUpdateService> logger;
        private List<ICommand<Message>> commandsMessage = new List<ICommand<Message>>() { new StartCommand(), new SendDocumentCommand(), };
        private List<ICommand<CallbackQuery>> commandsCallbackQuery = new List<ICommand<CallbackQuery>>() { new ConvertCommand() };
        public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger)
        {
            this.botClient = (TelegramBotClient?)botClient;
            this.logger = logger;
        }
        public async Task HandleMessageAsync(Message message)
        {
            try
            {
                if (commandsMessage.All(i => i.Name != message.Text)) return;
                await commandsMessage.FirstOrDefault(i => i.Name == message.Text).Execute(message, botClient);
            }
#pragma warning disable CA1031
            catch (Exception exception)
#pragma warning restore CA1031
            {
                await HandleErrorAsync(exception);
            }
        }
        public async Task HandleCallbackQueryAsync(CallbackQuery callback)
        {
            try
            { 
                await commandsCallbackQuery.First().Execute(callback, botClient);//.Execute(callback, botClient);
            }
#pragma warning disable CA1031
            catch (Exception exception)
#pragma warning restore CA1031
            {
                await HandleErrorAsync(exception);
            }
        }
        public Task UnknownUpdateHandlerAsync(Update update)
        {
            logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }
        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
