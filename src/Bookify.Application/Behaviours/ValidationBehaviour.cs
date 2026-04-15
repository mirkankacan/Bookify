using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);
            var validationErrors = validators
                .Select(v => v.Validate(context))
                .Where(v => v.Errors.Any())
                .SelectMany(result => result.Errors)
                .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
                .ToList();

            if (validationErrors.Any())
            {
                throw new Exceptions.ValidationException(validationErrors);
            }
            return await next();
        }
    }
}