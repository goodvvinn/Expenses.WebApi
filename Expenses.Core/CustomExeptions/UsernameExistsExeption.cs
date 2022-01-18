using System.Runtime.Serialization;

namespace Expenses.Core.CustomExeptions
{
    public class UsernameExistsExeption : Exception
    {
        public UsernameExistsExeption()
        {
        }

        public UsernameExistsExeption(string? message) : base(message)
        {
        }

        public UsernameExistsExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UsernameExistsExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
