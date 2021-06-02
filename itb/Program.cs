using System;
using itb.Services.Telegram;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace itb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost _host = CreateHostBuilder(args).Build();

            using (IServiceScope _serviceScope = _host.Services.CreateScope())
            {
                IServiceProvider _services = _serviceScope.ServiceProvider;

                try
                {
                    ITelegramService _telegramService = _services.GetRequiredService<ITelegramService>();
                    _telegramService.SetWebhookAsync();
                }
                catch (Exception _webhookException)
                {
                    ILogger<Program> _logger = _services.GetRequiredService<ILogger<Program>>();
                    _logger.LogError(_webhookException, "An error occurred while trying to set Web Hook.");
                }
            }

            _host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables(prefix: "ITB_");
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}