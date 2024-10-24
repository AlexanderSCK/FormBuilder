using System.Net;

namespace FormBuilder.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode;
        public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode; 
        }
    }
}
