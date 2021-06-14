using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using gRPCAPI.ProtosAPI;
using Grpc.Core;
using Inventory.Proto;
using Payment.Proto;

namespace gRPCAPI.Services
{
    public class OrderService : ProtosAPI.OrderProtoService.OrderProtoServiceBase
    {
        private readonly ILogger<OrderService> logger;

        public InventoryService.InventoryServiceClient InventoryClient { get; }
        public PaymentService.PaymentServiceClient PaymentClient { get; }


        public OrderService( ILogger<OrderService> logger,
            InventoryService.InventoryServiceClient inv,
            PaymentService.PaymentServiceClient pay)
        {
            InventoryClient = inv;
            PaymentClient = pay;
            this.logger = logger;
            logger.LogInformation("start the order service.");
        }


        public override async Task<OrderResult> PostOrder(OrderMessage request, ServerCallContext context)
        {
            try
            {

                var order = new Order()
                {
                    Id = 0,
                    Items = request.Items.Select(it => new gRPCAPI.Item()
                    {
                        Id = it.Id,
                        Price = it.Price,
                        Quantity = it.Quantity
                    }).ToList(),
                    UserId = request.UserId
                };


                // inventory service 
                var msg = new InventoryMessage();
                msg.Items.AddRange(order.Items.Select(i =>
                new Inventory.Proto.Item() { Id = i.Id, Price = i.Price, Quantity = i.Quantity }));
                
                var inv = await InventoryClient.DeduceInventoryAsync(msg);
                
                // payment service 
                var payMsg = new PaymentMessage()
                {
                    UserId = order.UserId,
                    TotalPrice = order.TotalPrice
                };

                var pay = await PaymentClient.DeduceBalanceAsync(payMsg);


                if (pay.Status == Payment.Proto.ServiceStatus.Sucess
                    && inv.Status == Inventory.Proto.ServiceStatus.Sucess)
                {

                    logger.LogInformation($"Success: Payment Service: {pay.Status}");
                    logger.LogInformation($"Success: Invnetory Service : {inv.Status}");
                    logger.LogInformation($"Success: user id : {order.UserId}, items: {string.Join(", ", order.Items)}");
                    return new OrderResult() { Result = OrderStatus.Success };
                }
                else
                {
                    if(pay.Status == Payment.Proto.ServiceStatus.Fail)
                        logger.LogInformation($"Failed: Payment Service: {pay.Status}");

                    if (inv.Status == Inventory.Proto.ServiceStatus.Fail)
                        logger.LogInformation($"Failed: Invnetory Service : {inv.Status}");
                    
                    logger.LogInformation($"Failed: user id : {order.UserId}, items: {string.Join(", ", order.Items)}");
                    return new OrderResult() { Result = OrderStatus.Failed };
                }

            }
            catch (RpcException ex)
            {

                logger.LogError(ex.Message);
                return new OrderResult() { Result = OrderStatus.Failed };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new OrderResult() { Result = OrderStatus.Failed };
            }
        }









    }
}