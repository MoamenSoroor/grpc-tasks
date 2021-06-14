using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Proto;
using Payment.Proto;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gRPCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public ILogger<OrdersController> Logger { get; }
        public InventoryService.InventoryServiceClient InventoryClient{get; }
        public PaymentService.PaymentServiceClient PaymentClient{get; }
       
        
        //ctor
        public OrdersController(ILogger<OrdersController> logger,
            InventoryService.InventoryServiceClient inv, 
            PaymentService.PaymentServiceClient pay)
        {
            Logger = logger;
            InventoryClient = inv;
            PaymentClient = pay;
            logger.LogInformation("start orders controller.");

        }
        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Order value)
        {
            var msg = new InventoryMessage();
            msg.Items.AddRange(value.Items.Select(i =>
            new Inventory.Proto.Item() { Id = i.Id, Price = i.Price, Quantity = i.Quantity }));
            var inv = await InventoryClient.DeduceInventoryAsync(msg);
            Logger.LogInformation("info:");

            var payMsg = new PaymentMessage()
            {
                UserId = value.UserId,
                TotalPrice = value.TotalPrice
            };
            
            var pay = await PaymentClient.DeduceBalanceAsync(payMsg);

            if (inv.Status == Inventory.Proto.ServiceStatus.Sucess &&
                pay.Status == Payment.Proto.ServiceStatus.Sucess)
                return Ok();
            else
                return BadRequest();
        }

    }
}
