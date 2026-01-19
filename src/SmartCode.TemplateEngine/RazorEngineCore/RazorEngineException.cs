using System;
using System.Runtime.Serialization;

namespace RazorEngineCore
{
    public class RazorEngineException : Exception
    {
        public RazorEngineException()
        {
        }

        public RazorEngineException(string message) : base(message)
        {
        }

        public RazorEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
