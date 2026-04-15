using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest

    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                logger.LogInformation("Executing command {Command}", name);
                var result = await next();
                logger.LogInformation("Command {Command} executed successfully", name);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Command {Command} failed", name);
                throw;
            }
        }
    }
}