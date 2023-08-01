using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading.Tasks;

namespace YourAppName
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    string responseMessage = "Hello, World!";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/plain";
                    await context.Response.BodyWriter.WriteAsync(responseBytes);

                });

                // Startup Probe - Returns 200 status code to indicate that the application has completed its initialization
                // For simplicity, we will assume the application is ready immediately after startup
                endpoints.MapGet("/health/startup", async context =>
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Startup Probe: OK");
                });

                // Liveness Probe - Returns 200 status code when the application is alive
                endpoints.MapGet("/health/liveness", async context =>
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Liveness Probe: OK");
                });

                // Readiness Probe - Returns 200 status code when the application is ready to serve requests
                endpoints.MapGet("/health/readiness", async context =>
                {
                    // Add your own readiness check logic here.
                    // For example, if your application depends on a database or other services,
                    // check if the required dependencies are available and ready to serve requests.

                    // If your application is ready, return 200 status code.
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Readiness Probe: OK");
                });


            });
        }
    }
}

