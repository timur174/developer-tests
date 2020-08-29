using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HockeyApi {
	public class Program {
		private static readonly IConfiguration _configuration = InitializeConfiguration();

		public static int Main(string[] args) {
			try {
				var host = WebHost.CreateDefaultBuilder<Startup>(args)
				  .UseKestrel(k => k.AddServerHeader = false)
				  .UseContentRoot(Directory.GetCurrentDirectory())
				  .UseConfiguration(_configuration)
				  .Build();

				host.Run();

				return 0;
			} catch {
				return -1;
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<Startup>();
				});

		private static IConfigurationRoot InitializeConfiguration() {
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
		}
	}
}
