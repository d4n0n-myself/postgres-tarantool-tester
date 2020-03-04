using System;
using System.IO;
using System.Reflection;
using IntegrationService.Models;
using IntegrationService.PostgreSQL;
using IntegrationService.Tarantool;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

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

			services.ManagePostgresDb();

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

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "v1",
					Version = "v1"
				});

#pragma warning disable 618
				c.DescribeAllEnumsAsStrings();
#pragma warning restore 618

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
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

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
				options.DocExpansion(DocExpansion.None);
			});
			
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