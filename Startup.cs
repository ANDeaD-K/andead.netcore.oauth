using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using andead.netcore.oauth.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace andead.netcore.oauth
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigurationManager configurationManager = new ConfigurationManager(_configuration);
            // JwtManager jwtManager = new JwtManager(configurationManager);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                // .AddCors(options =>
                // {
                //     options.AddPolicy("AllowAll",
                //         builder =>
                //         {
                //             builder
                //             .AllowAnyOrigin() 
                //             .AllowAnyMethod()
                //             .AllowAnyHeader()
                //             .AllowCredentials();
                //         });
                // })
                .AddSingleton(configurationManager)
                .AddSingleton<JwtManager>()
                .AddAuthentication(options => 
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config => 
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configurationManager.GetValue(ConfigurationKey.ISSUER),
                        ValidAudience = configurationManager.GetValue(ConfigurationKey.AUDIENCE),
                        IssuerSigningKey = configurationManager.GetSymmetricSecurityKey(ConfigurationKey.SIGNING_KEY),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // app.UseDefaultFiles();
            // app.UseStaticFiles();

            app.UseAuthentication();

            // app.UseCors("AllowAll");
            app.UseMvcWithDefaultRoute();
        }
    }
}
