namespace Bookify.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(IReadOnlyList<ValidationError> errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}