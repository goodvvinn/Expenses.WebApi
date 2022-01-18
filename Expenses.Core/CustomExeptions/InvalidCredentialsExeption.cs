using System.Runtime.Serialization;

namespace Expenses.Core.CustomExeptions
{
    public class InvalidCredentialsExeption : Exception
    {
        public InvalidCredentialsExeption()
        {
        }

        public InvalidCredentialsExeption(string? message) : base(message)
        {
        }

        public InvalidCredentialsExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCredentialsExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
