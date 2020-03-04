using System;
using IntegrationService.Models;
using IntegrationService.PostgreSQL;
using IntegrationService.Tarantool;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntegrationService.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public static Type CurrentDatabase { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.RegisterPgContext();

			CurrentDatabase = typeof(PostgresRepository);
			
			services.AddScoped<IRepository>(provider =>
			{
				if (CurrentDatabase == typeof(TarantoolRepository))
				{
					return new TarantoolRepository();
				}

				var context = provider.GetService<ApplicationDbContext>();
				return new PostgresRepository(context);
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					"default",
					"{controller}/{action=Index}/{id?}");
			});
		}
	}
}