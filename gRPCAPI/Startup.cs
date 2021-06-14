using gRPCAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(o => o.AddPolicy("all",plc =>
            {
                plc.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));

            services.AddGrpc();
            services.AddControllers();
            services.AddGrpcClient<Inventory.Proto.InventoryService.InventoryServiceClient>(op=> 
            op.Address = new Uri("https://localhost:5003"));
            services.AddGrpcClient<Payment.Proto.PaymentService.PaymentServiceClient>(op =>
            op.Address = new Uri("https://localhost:5004"));
            //services.AddScoped<Inventory.Proto.InventoryService.InventoryServiceClient>();
            //services.AddScoped<Payment.Proto.PaymentService.PaymentServiceClient>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPCAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPCAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();
            app.UseGrpcWeb();
            //app.UseGrpcWeb(new GrpcWebOptions() { DefaultEnabled = true });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrderService>().EnableGrpcWeb().RequireCors("all");
                endpoints.MapControllers();
            });
        }
    }
}
