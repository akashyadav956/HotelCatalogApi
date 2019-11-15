using HotelCatalogApi.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace HotelCatalogApi
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
            services.AddScoped<HotelContext>();
            services.AddCors(c =>
            {
                c.AddDefaultPolicy(x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

                c.AddPolicy("AllowAll", x =>
                {
                    x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Hotel Catalog API",
                    Description = "Hotel Catalog management API methods for Hotel Booking application",
                    Version = "1.0",
                    Contact = new Contact
                    {
                        Name = "Harshit Srivastava",
                        Email = "HarshitS2@hexaware.com",
                        Url = "https://Hexaware.com/pune"
                    }
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseSwagger();
            //if (env.IsDevelopment())
            //{
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
                    config.RoutePrefix = "";
                });
            //}
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
