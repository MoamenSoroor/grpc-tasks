using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Payment.Proto;

namespace Server.Payment.Services
{
    public class MyPaymentService : PaymentService.PaymentServiceBase
    {

        public MyPaymentService(ILogger<MyPaymentService> logger)
        {
            Logger = logger;
        }
        public static List<User> Users { get; } = new List<User>()
        {
            new User(){ Id=1,Name="Ahmed", Balance=2000},
            new User(){ Id=2,Name="Mohammed", Balance=150000},
            new User(){ Id=3,Name="Gamal", Balance=123000},
            new User(){ Id=4,Name="Samir", Balance=2000},
            new User(){ Id=5,Name="Waleed", Balance=10},
            new User(){ Id=6,Name="Hossam", Balance=34000}


        };
        public ILogger<MyPaymentService> Logger { get; }

        public override Task<ServiceResult> DeduceBalance(PaymentMessage request, ServerCallContext context)
        {
            var user = Users.FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                Logger.LogInformation($"user not found");
                return Task.FromResult(new ServiceResult() { Status = ServiceStatus.Fail });
            }

            if (user.Balance >= request.TotalPrice)
            {
                user.Balance -= request.TotalPrice;

                Logger.LogInformation($"{user.Id} {user.Name} balance deduced by {request.TotalPrice}");
                return Task.FromResult(new ServiceResult() { Status = ServiceStatus.Sucess });
            }
            else
            {
                Logger.LogInformation($"{user.Id} {user.Name} balance can't be deduced by {request.TotalPrice}");
                return Task.FromResult(new ServiceResult() { Status = ServiceStatus.Fail });
            }
        }
    }
}
