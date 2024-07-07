using System.ComponentModel.DataAnnotations;

namespace Mango.Service.CouponAPI.Models.DTO
{
    public class CouponDTO
    {
        [Key]
        public int CouponId { get; set; }
        
        [Required]
        public string CouponCode { get; set; }

        [Required]
        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }
    }
}
