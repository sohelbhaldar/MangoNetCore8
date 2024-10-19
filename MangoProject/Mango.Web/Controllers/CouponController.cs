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
        public async Task<IActionResult> CouponIndex()
        {
            List<Coupon>? listCoupons = new();
            var response = await _couponService.GetCoupons();
            if(response != null && response.IsSuccess)
            {
                listCoupons = JsonConvert.DeserializeObject<List<Coupon>>(Convert.ToString(response?.Response));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listCoupons);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(Coupon coupon) 
        {
            if (coupon != null && ModelState.IsValid)
            {
                var response = await _couponService.AddCoupon(coupon);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon Created Successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            if (couponId > 0)
            {
                var response = await _couponService.GetCouponById(couponId);
				if (response != null && response.IsSuccess)
				{
					var coupon = JsonConvert.DeserializeObject<Coupon>(Convert.ToString(response.Response));
                    return View(coupon);
				}
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return NotFound();
		}

        [HttpPost]
        public async Task<IActionResult> CouponDelete(Coupon coupon)
        {
            if (coupon != null)
            {
                var response = await _couponService.DeleteCoupon(coupon.CouponId);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon Deleted Successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(coupon);
        }
    }
}
