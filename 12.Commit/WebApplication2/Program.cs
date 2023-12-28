using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Multilanguage iþlemi
            #region Localizer
            builder.Services.AddSingleton<LanguageService>();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                    return factory.Create(nameof(SharedResource), assemblyName.Name);
                });
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR"),
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportCultures;
                options.SupportedUICultures = supportCultures;
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            });
            #endregion


            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();//RazorRuntimeCompilation komutu ile birlikte google ekraný kapanmadan deðiþikliklerimiz görüntüleyebiliyoruz.
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddDbContext<DatabaseContext>(opts =>

			{
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts =>
                {
                    opts.Cookie.Name = ".WebApplication2.auth";
                    opts.ExpireTimeSpan =TimeSpan.FromDays(7);
                    opts.SlidingExpiration = false;
                    opts.LoginPath = "/Account/Login";
                    opts.LogoutPath = "/Account/Logout";
                    opts.AccessDeniedPath = "/Home/AccessDenied";
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); 
            app.UseStaticFiles();

            //Multilanguage
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}