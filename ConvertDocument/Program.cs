using ConvertDocument.Controllers;
using ConvertDocument.Models;
using ConvertDocument.Models.Services;
using Telegram.Bot;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

        builder.Services.AddHostedService<ConfigureWebhook>();

        builder.Services.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));

        builder.Services.AddScoped<HandleUpdateService>();

        builder.Services.AddControllers().AddNewtonsoftJson();

        var app = builder.Build();

        app.UseRouting();
        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            var token = botConfig.BotToken;
            endpoints.MapControllerRoute(name: "tgwebhook",
                                         pattern: $"api/bot",
                                         new { controller = "Webhook", action = "Post" });
            endpoints.MapControllers();
        });

        app.Run();
    }
}