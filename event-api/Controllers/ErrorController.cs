using System;
using EventApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly DiagnosticSettings _settings;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ErrorController(IOptions<DiagnosticSettings> diagnosticSettings)
        {
            _settings = diagnosticSettings.Value;
        }

        [HttpGet("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            ApiError err = new ApiError();

            if (_settings.ShowStackTrace)
            {
                err.Message = context.Error.Message;
                err.Detail = context.Error.StackTrace;
            }
            else
            {
                err.Message = "A server error occurred.";
                err.Detail = context.Error.Message;
            }

            // TODO: log or notify
            _logger.Error(context.Error);

            return Problem(
                detail: err.Detail,
                title: err.Message);
        }
    }
}
