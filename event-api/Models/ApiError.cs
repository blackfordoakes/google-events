using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace EventApi.Models
{
    public class ApiError
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public string Detail { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? ErrorCode { get; set; }

        /// <summary>
        /// Translate an error code to an error message.
        /// </summary>
        public static ReadOnlyDictionary<long, string> ErrorLookups { get; } = new ReadOnlyDictionary<long, string>(new Dictionary<long, string>
        {
            {-1, "An unexpected error has occurred."}
        });

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        public ApiError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ApiError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Uses the error code to find the mapped message.
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="detail">additional message</param>
        public ApiError(long? errorCode, string detail = null)
        {
            ErrorCode = errorCode;
            // If error code can’t be found, returns the built-in generic error message.
            if (errorCode.HasValue)
                Message = ErrorLookups.TryGetValue(errorCode.Value, out string value) ? value : ErrorLookups[-1];
            else
                Message = ErrorLookups[-1];

            if (!string.IsNullOrWhiteSpace(detail))
                Detail = detail;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        public ApiError(ModelStateDictionary modelState)
        {
            Message = "Invalid parameters.";
            Detail = modelState?
                .FirstOrDefault(x => x.Value.Errors.Any()).Value?.Errors?
                .FirstOrDefault()?.ErrorMessage;
        }
    }
}
