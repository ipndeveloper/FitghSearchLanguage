using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tranzact.Search.Managers;
using Tranzact.Search.Models;

namespace Tranzact.Search
{

    public class Startup
    {
        public  static  IHost Host { get; set; }
        private static  IConfigurationRoot configuration;
        private readonly IServiceProvider provider;
        ServiceCollection services = new ServiceCollection();
        public IServiceProvider Provider => provider;
        public static void Start()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => { ConfigureServices(services); })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Trace);
                    logBuilder.AddLog4Net("log4net.config");

                }).UseConsoleLifetime();

            Host = builder.Build();
        }

        public static  void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(configuration);
            services.AddHttpClient<ISearch<SearchResponse>, BingSearch>(client =>
            {
            });
            services.AddHttpClient<ISearch<SearchResponse>, GoogleSearch>(client =>
            {
            });
            services.AddScoped<ISearchManager<SearchResponse>, SearchManager<SearchResponse>>();
            //services.TryAddScoped<SearchManager>();
            services.TryAddScoped<FigthManager<FigthResponse,SearchResponse>>();
        }
    }
}
