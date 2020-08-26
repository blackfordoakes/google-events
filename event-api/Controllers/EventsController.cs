using System;
using System.Threading.Tasks;
using Events.Provider.Interfaces;
using Events.Provider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventApi.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet("events")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEvents();
            return Ok(events);
        }

        [HttpPost("events")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddEvent(SubscriptionChangeEvent changeEvent)
        {
            await _eventService.AddEvent(changeEvent);
            return Ok();
        }
    }
}
