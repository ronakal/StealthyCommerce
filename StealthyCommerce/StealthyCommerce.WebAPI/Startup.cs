using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageOffer;
using StealthyCommerce.Service.ManageOrder;
using StealthyCommerce.Service.ManageProduct;
using StealthyCommerce.Service.StealthyCommerceDBEntities;
using Swashbuckle.AspNetCore.Swagger;

namespace StealthyCommerce.WebAPI
{
    public class Startup
    {
        private readonly IHostingEnvironment Environment;
        private readonly ILoggerFactory Logger;

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory logger)
        {
            Configuration = configuration;
            Environment = environment;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            Logger.AddLog4Net();
            //services.AddLogging(l => l.AddLog4Net());
            services.AddSingleton<ILogger, Log4NetLogger>();
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddScoped(_ => new StealthyCommerceDBContext());

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IOrderService, OrderService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Stealthy Commerce API",
                    Version = "v1",
                    Description = "Stealthy Commerce was created to support your commerce site, completely behind the scenes. This low stress product will help with managing subscriptions for new and existing customers, giving your business that silicon valley edge it needs. What are you waiting for, scroll down to see how it works!",
                    Contact = new Contact
                    {
                        Name = "Ronak Alian",
                        Email = string.Empty,
                        Url = "https://www.linkedin.com/in/ronak-alian-809a4588/"
                    },
                    License = new License
                    {
                        Name = "MIT",
                        Url = "https://en.wikipedia.org/wiki/MIT_License"
                    }
                });
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stealthy Commerce API");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
