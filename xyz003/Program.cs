using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace xyz003
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
           .ConfigureServices((context, services) =>
           {
               HostConfig.CertPath = context.Configuration["CertPath"];
               HostConfig.CertPassword = context.Configuration["CertPassword"];
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               //version 1
               // webBuilder.ConfigureKestrel(option =>
               //{
               //    option.ListenAnyIP(5000);
               //    option.ListenAnyIP(5001, listoption =>
               //    {
               //        listoption.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
               //    });
               //});
               // webBuilder.UseStartup<Startup>();

               var host = Dns.GetHostEntry("xyz003.dk");
               webBuilder.ConfigureKestrel(option =>
               {
                   option.Listen(host.AddressList[0], 8080);
                   option.Listen(host.AddressList[0], 443, listoption =>
                   {
                       listoption.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
                   });
               });
               webBuilder.UseStartup<Startup>();
           });
    }

    public static class HostConfig
    {
        public static string CertPath { get; set; }
        public static string CertPassword { get; set; }
    }
}
