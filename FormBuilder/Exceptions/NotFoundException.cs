using System.Net;

namespace FormBuilder.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(int id) : base($"Form with id: {id} not found", HttpStatusCode.NotFound) { }
    }
}
