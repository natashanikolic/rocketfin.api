using Microsoft.AspNetCore.Mvc.Filters;

namespace RocketFinApi.ActionFilters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executing endpoint: {ActionName} with arguments: {Arguments}", context.ActionDescriptor.DisplayName, context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executed endpoint: {ActionName} with result: {Result}", context.ActionDescriptor.DisplayName, context.Result);
        }
    }
}
