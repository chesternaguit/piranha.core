﻿using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piranha;
using Piranha.Data;

namespace AspNetCoreWeb
{
    public class Startup
    {
        #region Properties
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="env">The current hosting environment</param>
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddPiranhaDb(options => {
                options.Connection = new SqlConnection("data source=(localdb)\\MSSQLLocalDB;initial catalog=piranha.aspnetcore;integrated security=true");
                options.Migrate = true;

            });
            services.AddScoped<Api, Api>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, Api api) {
            loggerFactory.AddConsole();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // Initialize Piranha
            App.Init(api);

            // Build types
            var pageTypeBuilder = new Piranha.AttributeBuilder.PageTypeBuilder(api)
                .AddType(typeof(Models.MarkdownPage));
            pageTypeBuilder.Build();

            // Register middleware
            app.UseStaticFiles();
            app.UsePiranha();

            app.UseMvc(routes => {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });
            Seed(api);
        }

        /// <summary>
        /// Seeds some test data.
        /// </summary>
        /// <param name="db"></param>
        private void Seed(Api api) {
            if (api.Sites.GetAll().Count() == 0) {
                // Add the main site
                var siteId = Guid.NewGuid();
                var site = new Site() {
                    Id = siteId,
                    Title = "Default site",
                    InternalId = "DefaultSite",
                    IsDefault = true
                };
                api.Sites.Save(site);

                // Add the startpage
                using (var stream = File.OpenRead("assets/seed/startpage.md")) {
                    using (var reader = new StreamReader(stream)) {
                        var startPage = Models.MarkdownPage.Create(api);
                        startPage.SiteId = siteId;
                        startPage.Title = "Welcome to Piranha CMS";
                        startPage.Ingress = "The CMS framework with an extra bite";
                        startPage.Body = reader.ReadToEnd();
                        startPage.Published = DateTime.Now;

                        api.Pages.Save(startPage);
                    }
                }
            }
        }
    }
}