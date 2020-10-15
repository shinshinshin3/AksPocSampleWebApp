using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PocStubAppo1.Models;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using AzureUtils;
//未実装
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Jwt;
using Microsoft.Extensions.Options;

namespace PocStubAppo1
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
            // DBコンテキストの登録。(テーブルがなければ作成する。) 
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(
                Configuration.GetValue<string>("dbConnectionString")));

            // Application Insightsへのログ出力設定
            //
            // NOTE: https://docs.microsoft.com/ja-jp/azure/azure-monitor/app/asp-net-core#configuration-recommendation-for-microsoftapplicationinsightsaspnetcore-sdk-2150--above
            // 
            services.AddSingleton(typeof(ITelemetryChannel), new ServerTelemetryChannel() { StorageFolder = "/tmp/" });
            var telemetryConfigutaion = new TelemetryClientConfigure(Configuration.GetValue<string>("ApplicationInsights_InstrumentationKey")).aiOptions;
            services.AddApplicationInsightsTelemetry(telemetryConfigutaion);

            // Jwt トークンの検証
            // NOTE: https://qiita.com/sengoku/items/6b54ad52e86de1d0e7b6 
            // 
            //services.AddAuthentication().AddJwtBearer();
            /*
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: null);

            services
                .AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerConfigureOptions>();
            */

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // DBにテーブルがなければ作成する。
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbcontext = serviceScope.ServiceProvider.GetRequiredService<TodoContext>();
                dbcontext.Database.EnsureCreated();
            }
            app.UseRouting();

            app.UseAuthorization();

            /* JWt トークンの検証
            //app.Run(context => context.ChallengeAsync(JwtBearerDefaults.AuthenticationScheme));
            */

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
