using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Polly;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;

namespace Ocelot.APIGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("orderService", options =>
                {
                    //macϵͳ��docker.for.mac.localhost
                    //linuxϵͳ��docker.for.linux.localhost
                    options.Authority = "http://docker.for.win.localhost:9080";//��Ȩ���ĵ�ַ
                    options.ApiName = "orderApi";
                    options.SupportedTokens = SupportedTokens.Both;
                    options.ApiSecret = "orderApi secret";
                    options.RequireHttpsMetadata = false;
                })
                .AddIdentityServerAuthentication("productService", options =>
                {
                    //macϵͳ��docker.for.mac.localhost
                    //linuxϵͳ��docker.for.linux.localhost
                    options.Authority = "http://docker.for.win.localhost:9080";//��Ȩ���ĵ�ַ
                    options.ApiName = "productApi";
                    options.SupportedTokens = SupportedTokens.Both;
                    options.ApiSecret = "productApi secret";
                    options.RequireHttpsMetadata = false;
                });

            //���ocelot����
            services.AddOcelot()
                //���consul֧��
                .AddConsul()
                //��ӻ���
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                //���Polly
                .AddPolly();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����Ocelot�м��
            app.UseOcelot().Wait();
        }
    }
}
