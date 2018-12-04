using System;

namespace Application.Services
{
    public class AppException : Exception
    {
        public AppException()
        {
        }

        public AppException(string name) : base(name)
        {
        }

        public AppException(string name, Exception innerException) : base(name, innerException)
        {
        }
    }
}