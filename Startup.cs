using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml;
using System;
using DatabaseAPI.Authentication;
using DatabaseAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseAPI
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

            services.AddMvc(x => x.EnableEndpointRouting = false);

            services.AddDbContext<DHBWExpertsdatabaseContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DHEX_DATABASE"), providerOptions => providerOptions.EnableRetryOnFailure()));
            services.AddControllers(options =>
            options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings
            {
                OmitXmlDeclaration = false
            })))
                    .AddXmlDataContractSerializerFormatters()//Adds the necesarry Serializer to return XML objects instead of the default Json objects.
                    .AddXmlSerializerFormatters();
            services.AddCors();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dhbw-experts.eu.auth0.com/";
                options.Audience = "https://dhbw-experts-api.azurewebsites.net/";
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("auth0-api", policy => policy.Requirements.Add(new HasScopeRequirement("auth0-api", "https://dhbw-experts.eu.auth0.com/")));
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
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

            app.UseCors(options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}"
                    );
                });
        }
    }
}
