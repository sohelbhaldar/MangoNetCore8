namespace Mango.Service.CouponAPI.Models.DTO
{
    public class ResponseDTO
    {
        public object? Response { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = String.Empty;
    }
}
