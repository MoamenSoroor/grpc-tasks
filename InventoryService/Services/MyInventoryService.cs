using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Inventory.Proto;
using Microsoft.Extensions.Logging;

namespace Server.Inventory.Services
{
    public class MyInventoryService : InventoryService.InventoryServiceBase
    {

        public MyInventoryService(ILogger<MyInventoryService> logger)
        {
            Logger = logger;
        }

        public static List<InventoryItem> Items { get; } = new List<InventoryItem>()
        {
            new InventoryItem(){ Id=1,Name="Cheese", Price=10.0, Quantity=300},
            new InventoryItem(){ Id=2,Name="Shoes", Price=24.0, Quantity=120},
            new InventoryItem(){ Id=3,Name="Tomato", Price=30.0, Quantity=200},
            new InventoryItem(){ Id=4,Name="Cocacola", Price=15.0, Quantity=1000},
            new InventoryItem(){ Id=5,Name="TV", Price=2000.0, Quantity=150},
            new InventoryItem(){ Id=6,Name="Labtop", Price=5000.0, Quantity=50}
        };
        public ILogger<MyInventoryService> Logger { get; }

        public override Task<ServiceResult> DeduceInventory(InventoryMessage request, ServerCallContext context)
        {
            var success = !Items.Where(it => request.Items.Any(req => req.Id == it.Id))
                            .Select(s =>
                                new
                                {
                                    InvItem = s,
                                    ReqItem = request.Items.FirstOrDefault(i => i.Id == s.Id)
                                })
                            .Select(s =>
                            {

                                if (s.InvItem.Quantity >= s.ReqItem.Quantity)
                                {
                                    s.InvItem.Quantity -= s.ReqItem.Quantity;

                                    Logger.LogInformation($"{s.InvItem.Id} deduced by {s.ReqItem.Quantity}");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            })
                            .Any(a => a == false);

            Logger.LogInformation("Deduce Inventory: "+ success);

            return Task.FromResult<ServiceResult>(new ServiceResult() {  Status= success?ServiceStatus.Sucess:ServiceStatus.Fail});
        }
    }
}
