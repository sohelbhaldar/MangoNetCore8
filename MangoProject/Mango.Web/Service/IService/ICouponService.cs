using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetCoupons();
        Task<ResponseDTO?> GetCouponById(int couponId);
        Task<ResponseDTO?> GetCouponByCouponCode(string couponCode);
        Task<ResponseDTO?> AddCoupon(Coupon coupon);
        Task<ResponseDTO?> UpdateCoupon(Coupon coupon);
        Task<ResponseDTO?> DeleteCoupon(int couponId);
    }
}
