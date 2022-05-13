using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DatabaseAPI.Authentication;
using DatabaseAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
                options.AddPolicy("read:auth0-api", policy => policy.Requirements.Add(new HasScopeRequirement("read:auth0-api", "https://dhbw-experts.eu.auth0.com/")));
                options.AddPolicy("write:auth0-api", policy => policy.Requirements.Add(new HasScopeRequirement("write:auth0-api", "https://dhbw-experts.eu.auth0.com/")));
                options.AddPolicy("read:profile", policy => policy.Requirements.Add(new HasScopeRequirement("read:profile", "https://dhbw-experts.eu.auth0.com/")));
                options.AddPolicy("write:profile", policy => policy.Requirements.Add(new HasScopeRequirement("write:profile", "https://dhbw-experts.eu.auth0.com/")));
                
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "DHBW-Experts API" });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            
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
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DHBW-Experts API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("swagger/index.html", permanent: true);
                    return Task.CompletedTask;
                });

                endpoints.MapControllers();
            });
            
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
