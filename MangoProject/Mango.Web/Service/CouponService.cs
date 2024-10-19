using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService service)
        {
            _baseService = service; 
        }
        public async Task<ResponseDTO?> AddCoupon(Coupon coupon)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = coupon,
                Url = SD.CouponAPIBase + "/api/coupon/"
            });
        }

        public async Task<ResponseDTO?> DeleteCoupon(int couponId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/coupon/" + couponId
            });
        }

        public async Task<ResponseDTO?> GetCouponByCouponCode(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDTO?> GetCouponById(int couponId)
        {
            var result = await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + couponId
            });
            return result;
        }

        public async Task<ResponseDTO?> GetCoupons()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> UpdateCoupon(Coupon coupon)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.PUT,
                Data = coupon,
                Url = SD.CouponAPIBase + "/api/coupon/"
            });
        }
    }
}
