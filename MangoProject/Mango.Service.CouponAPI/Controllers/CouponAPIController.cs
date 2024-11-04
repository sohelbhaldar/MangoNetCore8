using AutoMapper;
using Mango.Service.CouponAPI.Data;
using Mango.Service.CouponAPI.Models;
using Mango.Service.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private ResponseDTO _responseDTO;
        private IMapper _mapper;
        public CouponAPIController(AppDBContext dBContext, IMapper mapper) {
            _dbContext = dBContext;
            _mapper = mapper;
            _responseDTO = new ResponseDTO();
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var coupons = _dbContext.Coupons.ToList();
                if (coupons.Any())
                {
                    var result = _mapper.Map<IEnumerable<CouponDTO>>(coupons);
                    _responseDTO.Response = result;
                    return Ok(_responseDTO);
                }
                else
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "No Coupon Found";
                    return NotFound(_responseDTO);
                }
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return BadRequest(_responseDTO);
            }
        }

        [HttpGet]
        [Route("{couponId:int}")]
        public IActionResult Get(int couponId)
        {
            try
            {
                var coupons = _dbContext.Coupons.FirstOrDefault(x => x.CouponId == couponId);
                if(coupons != null)
                {
                    var result = _mapper.Map<CouponDTO>(coupons);
                    _responseDTO.Response = result;
                    return Ok(_responseDTO);
                }
                else
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "No Coupon Found";
                    return NotFound(_responseDTO);
                }
            }
            catch(Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return BadRequest(_responseDTO);
            }
        }

        [HttpGet]
        [Route("GetByCode/{couponCode}")]
        public IActionResult Get(string couponCode)
        {
            try
            {
                var coupons = _dbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower().Equals(couponCode.ToLower()));
                if (coupons != null)
                {
                    var result = _mapper.Map<CouponDTO>(coupons);
                    _responseDTO.Response = result;
                    return Ok(_responseDTO);
                }
                else
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "No Coupon Found";
                    return NotFound(_responseDTO);
                }
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return BadRequest(_responseDTO);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CouponDTO couponDTO)
        {
            try
            {
                if(couponDTO != null)
                {
                    var coupon = _mapper.Map<Coupon>(couponDTO);
                    _dbContext.Coupons.Add(coupon);
                    _dbContext.SaveChanges();
                    _responseDTO.Response = coupon;
                    _responseDTO.Message = "Coupon Added Successfully";
                    return Ok(_responseDTO);
                }
                else
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "No Coupon Found";
                    return NotFound(_responseDTO);
                }
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                if(ex.InnerException != null)
                {
                    _responseDTO.Message = ex.Message + ' ' + ex.InnerException.Message;
                }
                return BadRequest(_responseDTO);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                if (couponDTO != null)
                {
                    var coupon = _mapper.Map<Coupon>(couponDTO);
                    var couponToUpdate = _dbContext.Coupons.FirstOrDefault(x => x.CouponId == coupon.CouponId);
                    if(couponToUpdate != null)
                    {
                        _dbContext.Coupons.Update(coupon);
                    }
                    else
                    {
                        _responseDTO.IsSuccess = false;
                        _responseDTO.Message = "No Coupon Found";
                        return NotFound(_responseDTO);
                    }
                    _dbContext.SaveChanges();
                    _responseDTO.Response = coupon;
                    _responseDTO.Message = "Coupon Updated Successfully";
                    return Ok(_responseDTO);
                }
                else
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "No Coupon Found";
                    return Ok(_responseDTO);
                }
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    _responseDTO.Message = ex.Message + ' ' + ex.InnerException.Message;
                }
                return BadRequest(_responseDTO);
            }
        }

        [HttpDelete]
        [Route("{couponId:int}")]
        public IActionResult Delete(int couponId)
        {
            try
            {
                if (couponId > 0)
                {
                    var couponToDelete = _dbContext.Coupons.FirstOrDefault(x => x.CouponId == couponId);
                    if (couponToDelete != null)
                    {
                        _dbContext.Coupons.Remove(couponToDelete);
                    }
                    else
                    {
                        _responseDTO.IsSuccess = false;
                        _responseDTO.Message = "No Coupon Found";
                        return NotFound(_responseDTO);
                    }
                    _dbContext.SaveChanges();
                    _responseDTO.Response = couponToDelete;
                    _responseDTO.Message = "Coupon Deleted Successfully";
                    return Ok(_responseDTO);
                }
                else
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "No Coupon Found";
                    return Ok(_responseDTO);
                }
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    _responseDTO.Message = ex.Message + ' ' + ex.InnerException.Message;
                }
                return BadRequest(_responseDTO);
            }
        }

    }
}
