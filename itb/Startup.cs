using System;
using itb.Models.Configurtations;
using itb.Services.Chat;
using itb.Services.Notifications;
using itb.Services.Statistic;
using itb.Services.Telegram;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace itb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationConfiguration>(Configuration.GetSection("Application"));
            services.Configure<TelegramConfiguration>(Configuration.GetSection("Telegram"));

            services.AddSingleton<ITelegramService, TelegramService>();
            services.AddSingleton<INotificationsService, NotificationsService>();
            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IStatisticService, StatisticService>();

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}