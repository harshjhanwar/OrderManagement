namespace OrderManagement.Models
{
    public class Result(bool Success, string? Message = null)
    {
        public bool Success { get; set; } = Success;
        public string? Message { get; set; } = Message;
    }
}
