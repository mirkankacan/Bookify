using Bookify.Application.Behaviours;
using Bookify.Domain.Bookings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddTransient<PricingService>();
            //services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            //services.AddTransient<IRequestHandler<CreateBookCommand, Guid>, CreateBookCommandHandler>();
            //services.AddTransient<IRequestHandler<UpdateBookCommand, Guid>, UpdateBookCommandHandler>();
            //services.AddTransient<IRequestHandler<DeleteBookCommand, Guid>, DeleteBookCommandHandler>();
            //services.AddTransient<IRequestHandler<GetBookIdQuery, BookDto>, GetBookByIdQueryHandler>();
            //services.AddTransient<IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>, GetBooksQueryHandler>();
            return services;
        }
    }
}