﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeopleIKnow.Configuration;
using PeopleIKnow.DataAccess;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Services;

namespace PeopleIKnow
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
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddHttpClient<IMessagingService, TelegramMessagingService>();
            services.AddTransient<IReminderService, ReminderService>();
            services.Configure<NotificationSettings>(Configuration.GetSection("Notifications"));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("en");
                options.AddSupportedUICultures("en", "de");
                options.FallBackToParentUICultures = true;
            });
            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddRazorPages();
            services.AddDbContext<ContactContext>(options => options.UseSqlite("Data Source=people.db"));
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ContactContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddHostedService<NotificationHostedService>();
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
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}