using System;
using System.Collections.Generic;
using System.Linq;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventApi.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("events")]
        [ProducesResponseType(200)]
        public IActionResult GetAllEvents()
        {
            var events = new List<SubscriptionChangeEvent>();
            return Ok(events);
        }
    }
}
