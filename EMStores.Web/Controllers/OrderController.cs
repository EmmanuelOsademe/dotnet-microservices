using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace EMStores.Web.Controllers
{
    public class OrderController(IOrderService orderService) : Controller
    {
        private readonly IOrderService _orderService = orderService;
        public IActionResult OrderIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<OrderHeaderDto> ordersHeaders = [];
            string userId = "";
            if (!User.IsInRole(StaticDetails.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }

            ResponseDto response = await _orderService.GetAllOrders(userId);
            if(response != null && response.IsSuccess)
            {
                var orders = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result));
                foreach(var item in orders)
                {
                    ordersHeaders.Append(item.OrderHeader);
                }
            }
            else
            {
                ordersHeaders = new List<OrderHeaderDto>();
            }

            return Json(new { data = ordersHeaders });
        }
    }
}
