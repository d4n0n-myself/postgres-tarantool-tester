using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IntegrationService.Models.Entities;
using IntegrationService.Tarantool;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using IntegrationService.Models;
using Microsoft.Extensions.DependencyInjection;
using IntegrationService.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationService.Web
{
	public class Program
	{
		public static void Main1(string[] args)
		{
			Console.WriteLine("Погнали наххххх");
			Startup.Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			ConnectionStrings.Current = new ConnectionStrings(
				Startup.Configuration.GetValue<string>("Postgres"),
				Startup.Configuration.GetValue<string>("Tarantool"));
			Console.WriteLine("строки загнал");
			var stopwatch = new Stopwatch();
			var tasks = new List<Task>();
			Console.WriteLine("погнали");
			stopwatch.Start();
			
			Parallel.For(0, 30000, i =>
			{
				var tarantoolRepository = new TarantoolRepository();
				tasks.Add(tarantoolRepository.SaveAsync(new Author()
				{
					Age = 11,
					Name = "123",
					Nationality = "string",
					BirthCountry = "asd",
					BirthCity = "123",
					BooksCount = 123,
					FamilyName = "123",
					MiddleName = "123"
				}));
				Console.WriteLine(i);
			});

			Task.WaitAll(tasks.Where(x => x != null).ToArray());
			stopwatch.Stop();
			Console.WriteLine($"Пизданулся: {tasks.Count(x => x == null)} раз");
			Console.WriteLine("Втащил этой твари за " + stopwatch.ElapsedMilliseconds);
		}

		public static void Main(string[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				logger.Info("init main");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception exception)
			{
				//NLog: catch setup errors
				logger.Error(exception, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
				.ConfigureLogging(log =>
				{
					log.ClearProviders();
					log.AddConsole();
					log.SetMinimumLevel(LogLevel.Warning);
				})
				.UseNLog();
	}
}