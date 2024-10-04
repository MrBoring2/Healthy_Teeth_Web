using System.Net;

namespace WebSite.Models
{
    public class ResponseModel
    {
        public ResponseModel(HttpStatusCode statusCode, string cotnent)
        {
            StatusCode = statusCode;
            Content = cotnent;
        }

        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
    }
}
