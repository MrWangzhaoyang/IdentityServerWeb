using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using IdentityServerWeb.Data;
using IdentityServerWeb.Models;
using IdentityServer4.Services;
using IdentityServerWeb.Service;
using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServerWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string IdentityServerWeb = Configuration.GetConnectionString("IdentityServerWeb");
            string IdentityServerConfigure = Configuration.GetConnectionString("IdentityServerConfigure");
            var migretionAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(IdentityServerWeb));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryApiResources(Config.GetApiResource())
                //.AddInMemoryClients(Config.GetClients())
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(option =>
                {
                    option.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(IdentityServerConfigure, sql => sql.MigrationsAssembly(migretionAssembly));
                    };
                })
                .AddOperationalStore(option =>
                {
                    option.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(IdentityServerConfigure, sql => sql.MigrationsAssembly(migretionAssembly));
                    };
                });
            //.Services.AddScoped<IProfileService,ProfileService>();

            //services.Configure<IdentityOptions>(option => {
            //    option.Password.RequireLowercase = false;//小写
            //    option.Password.RequireNonAlphanumeric = true;//特殊符号
            //    option.Password.RequireUppercase = false;//大写
            //    option.Password.RequiredLength = 6;//长度
            //});

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            InitIdentityServerDataBase(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void InitIdentityServerDataBase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!configurationDbContext.Clients.Any())
                {
                    Console.WriteLine("Clients being populated");
                    foreach (var client in Config.GetClients().ToList())
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Clients already populated");
                }

                if (!configurationDbContext.IdentityResources.Any())
                {
                    Console.WriteLine("IdentityResources being populated");
                    foreach (var resource in Config.GetIdentityResources().ToList())
                    {
                        configurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                else
                {
                    Console.WriteLine("IdentityResources already populated");
                }

                if (!configurationDbContext.ApiResources.Any())
                {
                    Console.WriteLine("ApiResources being populated");
                    foreach (var resource in Config.GetApiResource().ToList())
                    {
                        configurationDbContext.ApiResources.Add(resource.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                else
                {
                    Console.WriteLine("ApiResources already populated");
                }
            }
        }
    }
}
