using System.Net.Http;
using Flurl.Http;
using m_clippy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace m_clippy
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

            // IF --> concrete class as startup
            //services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            // concrete class as startup
            //services.AddSingleton<AdapterWithErrorHandler>();
            services.AddSingleton<ClippyStorage>();
            services.AddSingleton<MigrosService>();
            services.AddSingleton<AllergyService>();
            services.AddSingleton<ReportingService>();

            // transient, on request
            //services.AddTransient<IBot, EchoBot>();


            // One important note is that this approach will not work with a policy that handles FlurlHttpException.
            // That's because you're intercepting calls at the HttpMessageHandler level here. Flurl converts responses
            // and errors to FlurlHttpExceptions higher up the stack, so those won't get trapped/retried with this approach.
            // The policy in the example above traps HttpRequestException and HttpResponseMessage
            // (with non-2XX status codes), which will work.
            // https://stackoverflow.com/questions/52272374/set-a-default-polly-policy-with-flurl
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(5);

            FlurlHttp.Configure(settings => settings.HttpClientFactory = new PollyFactory(policy));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /* app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            */
            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
