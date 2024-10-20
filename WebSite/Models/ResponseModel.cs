using System.Net;

namespace WebSite.Models
{
    public class ResponseModel<T>
    {
        public ResponseModel(HttpStatusCode statusCode, T cotnent)
        {
            StatusCode = statusCode;
            Content = cotnent;
        }

        public HttpStatusCode StatusCode { get; }
        public T Content { get; }
    }
}
