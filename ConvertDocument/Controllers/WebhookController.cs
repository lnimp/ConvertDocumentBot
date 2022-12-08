using ConvertDocument.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConvertDocument.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
                                           [FromBody] Update update)
        {
            if (update.CallbackQuery == null && (update.Message == null || update.Message?.Entities?.First().Type != MessageEntityType.BotCommand && update.Message?.Type != MessageType.Document)) return Ok();
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        Message message = MessageUpdate(ref update);
                        await handleUpdateService.HandleMessageAsync(message);
                        break;
                    }
                case UpdateType.CallbackQuery:
                    {
                        await handleUpdateService.HandleCallbackQueryAsync(update.CallbackQuery);
                        break;
                    }
                    default:
                    await handleUpdateService.UnknownUpdateHandlerAsync(update);
                    return NoContent();
            }
            return Ok();
        }
        Message MessageUpdate(ref Update update)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Document:
                    update.Message.Text = "Document";
                    return update.Message;
                default:
                    return update.Message;
            }
        }
    }
}
