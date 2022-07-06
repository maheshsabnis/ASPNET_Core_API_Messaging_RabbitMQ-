using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core_SenderAPI.Models;
using Core_SenderAPI.Logic;
namespace Core_SenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        ProcessOrder process;

        public OrdersController()
        {
            process = new ProcessOrder();
        }

        [HttpPost]
        public IActionResult Post(Order order)
        {
            var response = new ResponseObject();
            response = process.ManageOrder(order);
            return Ok(response);
        }
    }
}
