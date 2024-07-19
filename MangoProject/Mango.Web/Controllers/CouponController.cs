using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        public ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async  Task<IActionResult> CouponIndex()
        {
            List<Coupon>? listCoupons = new();
            var couponResponse = await _couponService.GetCoupons();
            if(couponResponse != null && couponResponse.IsSuccess)
            {
                listCoupons = JsonConvert.DeserializeObject<List<Coupon>>(Convert.ToString(couponResponse.Response));
            }
            return View(listCoupons);
        }
    }
}
