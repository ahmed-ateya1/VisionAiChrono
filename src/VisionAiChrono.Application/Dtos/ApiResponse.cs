using System.Net;

namespace VisionAiChrono.Application.Dtos
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
    }
}
