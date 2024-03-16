using DealRept.CustomTokenProvider;
using DealRept.CustomValidators;
using DealRept.Data;
using DealRept.Models;
using DealRept.Models.ViewModel.Factory;
using DealRept.Services;
using DealRept.Services.AdminEmailsService;
using DealRept.Services.EmailService;
using DealRept.Services.RazorRenderService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DealRept
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment environment)
        {
            Configuration = config;
            Environment = environment;
        }

        private IConfiguration Configuration { get;}

        private IWebHostEnvironment Environment { get;}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRazorRendererHelper, RazorRendererHelper>();

            var builder = services.AddControllersWithViews().AddControllersAsServices();
            if (Environment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }

            services.AddDbContext<DealDbContext>(opts =>
            {
                opts.UseMySQL(Configuration["ConnectionStrings:DealReptConnection"]);
            });

            services.AddDbContext<IdentityContext>(opts =>
            {
                opts.UseMySQL(Configuration["ConnectionStrings:IdentityConnection"]);
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation")
                .AddPasswordValidator<CustomPasswordValidator<User>>();

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddJavaScriptBundle("js/cite.js", "js/dirtyCheck.js", "js/mainFunctions.js");
                pipeline.AddCssBundle("css/cite.css", "css/**/*.css");
            });

            services.Configure<MvcOptions>(opts => opts.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value => $"Please enter a value."));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5).Add(TimeSpan.FromHours(Configuration.GetValue<int>("TimeZoneUTCDifference")));
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //SignIn settings.
                options.SignIn.RequireConfirmedEmail = true;

                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            });

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsFactory>();

            services.AddScoped<ITooltipService, ToolTipService>();

            var emailConfig = Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<IEmailsGetter, EmailsGetter>();

            services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
     opt.TokenLifespan = TimeSpan.FromDays(3));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Errors/Error");
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors/Error");
                app.UseHsts();
            }

            app.UseWebOptimizer();

            app.UseStaticFiles();
            //app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/Errors/StatusCode", "?statusCode={0}");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

            SeedData.EnsurePopulated(app);
            IdentitySeedData.CreateAdminAccount(app.ApplicationServices, Configuration);
        }
    }
}
