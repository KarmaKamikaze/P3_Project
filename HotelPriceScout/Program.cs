using HotelPriceScout.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace HotelPriceScout
{
    public class Program
    {
        public static ScoutSharedService ScoutSharedService { get; set; }
        public static void Main(string[] args)
        {
            ScoutSharedService = new ScoutSharedService();
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}