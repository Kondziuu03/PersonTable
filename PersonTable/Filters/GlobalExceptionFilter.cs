using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonTable.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception occurred");

            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                context.Result = new JsonResult(new { success = false, message = context.Exception.Message });
            else
                context.Result = new RedirectToActionResult("Error", "Error", new { message = context.Exception.Message });

            context.ExceptionHandled = true;
        }
    }
}
