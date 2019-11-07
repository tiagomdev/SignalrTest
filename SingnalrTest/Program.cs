using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SingnalrTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static void Test()
        {
            RedisDB<Models.ClientConnection> redisDB = new RedisDB<Models.ClientConnection>();

            var items = redisDB.GetAll();
        }
    }
}
