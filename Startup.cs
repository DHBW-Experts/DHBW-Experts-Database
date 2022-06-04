using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml;
using System;
using System.Collections.Generic;
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
                options.Authority = Configuration["Authentication:Authority"];
                options.Audience = Configuration["Authentication:Audience"];
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:auth0-api", policy => policy.Requirements.Add(new HasScopeRequirement("read:auth0-api", Configuration["Authentication:Authority"])));
                options.AddPolicy("write:auth0-api", policy => policy.Requirements.Add(new HasScopeRequirement("write:auth0-api", Configuration["Authentication:Authority"])));
                options.AddPolicy("read:profile", policy => policy.Requirements.Add(new HasScopeRequirement("read:profile", Configuration["Authentication:Authority"])));
                options.AddPolicy("write:profile", policy => policy.Requirements.Add(new HasScopeRequirement("write:profile", Configuration["Authentication:Authority"])));
                
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "DHBW-Experts API" });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Open Id"},
                                { "read:profile", "Read Profile"},
                                { "write:profile", "Write Profile"}
                            },
                            AuthorizationUrl = new Uri(Configuration["Authentication:Authority"] + "authorize?audience=" + Configuration["Authentication:Audience"])
                        }
                    }
                });
                swagger.OperationFilter<SecurityRequirementsOperationFilter>();
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
                c.OAuthClientId(Configuration["Authentication:ClientId"]);
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
