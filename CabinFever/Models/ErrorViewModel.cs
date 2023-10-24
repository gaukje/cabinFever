namespace CabinFever.Models
{
    // This class represents a model for handling errors in the application.
    public class ErrorViewModel
    {
        // Gets or sets the request ID associated with the error.
        public string? RequestId { get; set; }

        // Determines whether to show the request ID. It's true if RequestId is not empty.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}