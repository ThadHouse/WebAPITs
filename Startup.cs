using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using WebAPITs.Models;
using System.Linq;
using System.Threading;

namespace WebAPITs
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;
        private Timer _saveTimer;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _hostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
            services.AddSingleton<IFileProvider>(physicalProvider);

            services.AddDbContext<AccountContext>(opt => opt.UseInMemoryDatabase());
            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "WebAPITs.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "help";
            });

            _saveTimer = new Timer((o) => {
                try {
                    var db = app.ApplicationServices.GetService<AccountContext>();
                    var accounts = db.Accounts.ToList();
                    var file = app.ApplicationServices.GetService<IFileProvider>().GetFileInfo("accounts.json");
                    File.WriteAllText(file.PhysicalPath, JsonConvert.SerializeObject(accounts.Select(x => new {x.Username, x.Password, x.Money})));
                } catch {

                }
            }, null, 5000, 5000);

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                var file = app.ApplicationServices.GetService<IFileProvider>().GetFileInfo("accounts.json");
                if (file.Exists)
                {
                    var db = app.ApplicationServices.GetService<AccountContext>();
                    var json = File.ReadAllText(file.PhysicalPath);
                    var accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                    db.Accounts.AddRange(accounts);
                    db.SaveChanges();
                }
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                _saveTimer.Dispose();
                try {
                    var db = app.ApplicationServices.GetService<AccountContext>();
                    var accounts = db.Accounts.ToList();
                    var file = app.ApplicationServices.GetService<IFileProvider>().GetFileInfo("accounts.json");
                    File.WriteAllText(file.PhysicalPath, JsonConvert.SerializeObject(accounts.Select(x => new {x.Username, x.Password, x.Money})));
                } catch {

                }
            });
        }
    }
}
