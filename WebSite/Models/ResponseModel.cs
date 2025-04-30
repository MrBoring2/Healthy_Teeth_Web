using System.Net;

namespace WebSite.Models
{
    public class ResponseModel<T>
    {
        public ResponseModel(HttpStatusCode statusCode, T cotnent, string message = "")
        {
            StatusCode = statusCode;
            Content = cotnent;
            this.Message = message;
        }

        public HttpStatusCode StatusCode { get; }
        public T Content { get; }
        public string Message { get; }
    }
}
