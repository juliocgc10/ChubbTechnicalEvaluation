namespace Chubb.Domain.Common
{
    public class ResponseServices
    {
        public bool IsSuccess { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }
        public string MessageException { get; set; }
    }

    
}
