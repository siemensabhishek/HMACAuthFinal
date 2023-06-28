using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using softaware.Authentication.Hmac;
using softaware.Authentication.Hmac.AspNetCore;
using softaware.Authentication.Hmac.AuthorizationProvider;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerEntities.CustomerEntities>(optionsAction: options =>
                                    options.UseSqlServer(configRoot.GetConnectionString("CustomerDB")));
            services.AddControllers();

            services.AddSwaggerGen();

            services.AddRazorPages();

            var hmacAuthenticatedApps = this.configRoot.GetSection("Authentication").GetSection("HmacAuthenticatedApps").Get<HmacAuthenticationClientConfiguration[]>().ToDictionary(e => e.AppId, e => e.ApiKey);

            // Add HMAC authenticaiton handler as a transient service
            services.AddTransient<IHmacAuthorizationProvider>(_ => new MemoryHmacAuthenticationProvider(hmacAuthenticatedApps));

            services.AddAuthentication(o => { o.DefaultScheme = HmacAuthenticationDefaults.AuthenticationScheme; }).AddHmacAuthentication(HmacAuthenticationDefaults.AuthenticationScheme, "HMAC Authentication", options => { });

            // adding the memory cache
            services.AddMemoryCache();

            // adding the auth filter
            if (Convert.ToBoolean(configRoot["DisabledConfig"]))
            {
                services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));
            }


        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerAPI v1");
            });
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapRazorPages();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.Run();
        }
    }
}
