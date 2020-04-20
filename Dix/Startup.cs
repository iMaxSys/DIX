using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dix
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
            services.AddControllers();

            services.AddSingleton<ISingleton, Singleton>();
            services.AddScoped<IScope, Scope>();
            services.AddScoped<IScope, Scope1>();
            services.AddScoped<IScope, Scope2>();
            services.AddScoped<IScope, Scope3>();
            services.AddScoped<IScope, Scope4>();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IAllScope, AllScope>();
            services.AddTransient<ITransit, Transit>();
            services.AddTransient<ITransit, Transit1>();

            services.AddScoped<Scope3>();
            services.AddScoped<Scope4>();

            services.AddScoped(provider =>
            {
                Func<int, IScope> func = n =>
                {
                    switch (n)
                    {
                        case 3:
                            return provider.GetService<Scope3>();
                        case 4:
                            return provider.GetService<Scope4>();
                        default:
                            throw new NotSupportedException();
                    }
                };
                return func;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
