using ConaviWeb.Data;
using ConaviWeb.Data.Repositories;
using ConaviWeb.Data.Shell;
using ConaviWeb.Data.Sisevive;
using ConaviWeb.Model.Common;
using ConaviWeb.Services;
using ConaviWeb.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Text;

namespace ConaviWeb
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
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddRazorPages();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("172.16.250.2"));
            });

            //services.Configure<FormOptions>(options =>
            //{
            //    // Set the limit to 256 MB
            //    options.MultipartBodyLengthLimit = 268435456;
            //});

            //Sesion
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
            });

            //Conexion DataBase
            var ConnectionConfig = new MySQLConfiguration(Configuration.GetConnectionString("SiseviveConnection"), 
                Configuration.GetConnectionString("UserConnection"),
                Configuration.GetConnectionString("EDConnection")
                );
            services.AddSingleton(ConnectionConfig);

            //Mail Service
            services.Configure<MailSetting>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            //Dependency
            services.AddScoped<ISiseviveRepository, SiseviveRepository>();
            services.AddScoped<IProcessEDRepository, ProcessEDRepository>();
            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ISourceFileRepository, SourceFileRepository>();
            services.AddScoped<ISecurityTools, SecurityTools>();
            services.AddScoped<IUserRepository, UserRepository>();
            var appSettingSection = Configuration.GetSection("AppSettings");
            //services.AddSingleton<HttpClient>();  Revisar el uso de esta inyección
            services.AddScoped<IProcessSignRepository, ProcessSignRepository>();
            services.AddScoped<IProcessSigningService, ProcessSigningService>();
            services.AddScoped<IProcessCancelService, ProcessCancelService>();
            //JWT
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(appSettingsSection);

            var appSetting = appSettingsSection.Get<AppSetting>();
            var llave = Encoding.ASCII.GetBytes(appSetting.SecretJWT);
            services.AddAuthentication(d =>
            {
                d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(d =>
                {
                    d.RequireHttpsMetadata = false;
                    d.SaveToken = true;
                    d.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(llave),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

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

            app.UseSession();
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });

            app.UseHttpsRedirection();
            //Se requiere para acceder a los archivoscargados
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Inicio}/{action=Index}/{id?}");
            });
        }
    }
}
