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

		public async Task<IActionResult> OrderDetail(int orderId)
		{
            OrderDto orderDto = new();
            string userId = userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _orderService.GetOrder(orderId);

			if (response != null && response.IsSuccess)
			{
                orderDto = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(response.Result));
			}

            if (!User.IsInRole(StaticDetails.RoleAdmin) && userId != orderDto.OrderHeader.UserId)
            {
                return NotFound();
            }

			return View(orderDto);
		}

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup (int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_ReadyForPickup);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_Cancelled);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_Completed);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string status)
        {
            List<OrderHeaderDto> ordersHeaders = [];
            string userId = "";
            if (!User.IsInRole(StaticDetails.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }

            ResponseDto response = await _orderService.GetAllOrders(userId);
            if(response != null && response.IsSuccess)
            {
                var orders = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result));
                switch (status)
                {
                    case "approved":
                        orders = orders.Where(order => order.OrderHeader.Status == StaticDetails.Status_Approved).ToList();
                        break;
					case "readyforpickup":
						orders = orders.Where(order => order.OrderHeader.Status == StaticDetails.Status_ReadyForPickup).ToList();
						break;
					case "cancelled":
						orders = orders.Where(order => order.OrderHeader.Status == StaticDetails.Status_Cancelled).ToList();
						break;
                    default:
                        break;  

				}
                foreach(var item in orders)
                {
                    ordersHeaders.Add(item.OrderHeader);
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
