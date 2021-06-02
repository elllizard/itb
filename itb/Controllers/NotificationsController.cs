using System;
using System.Threading.Tasks;
using itb.Services.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace itb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly ILogger<NotificationsController> _logger;
        private readonly INotificationsService _notificationsService;

        public NotificationsController(
            ILogger<NotificationsController> logger,
            INotificationsService notificationsService
        )
        {
            _logger = logger;
            _notificationsService = notificationsService;
        }

        [HttpPost]
        public async Task<IActionResult> Notify([FromBody] Update update)
        {
            _logger.LogInformation($"Received message '{update.Message.Text}' from '{update.Message.Chat.Id}'.");
            await _notificationsService.HandleNotification(update);
            return Ok();
        }
    }
}