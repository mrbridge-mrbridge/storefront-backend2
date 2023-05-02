namespace Application.Core
{
	public class ExceptionResult
    {
        public ExceptionResult( int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string  Details { get; set; }
    }
}