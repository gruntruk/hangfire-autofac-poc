using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Hangfire;
using Hangfire.Redis;
using HangfireAutofacDemo.Jobs;
using HangfireAutofacDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HangfireAutofacDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public static MultitenantContainer ApplicationContainer { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHangfire(config =>
                config.UseRedisStorage("localhost:6379", new RedisStorageOptions {Prefix = "HangfireAutofacDemo"}));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<CountService>().InstancePerLifetimeScope();
            builder.RegisterType<OtherService>().InstancePerDependency();
            builder.RegisterType<CountJob>();

            var container = builder.Build();

            // setup multitenant container
            var mtc = new MultitenantContainer(new QueryStringTenantIdentificationStrategy(container.Resolve<IHttpContextAccessor>()), container);
            mtc.ConfigureTenant("other", b =>
            {
                b.RegisterType<CountService>().WithParameter("seed", 100).InstancePerLifetimeScope();
            });

            Startup.ApplicationContainer = mtc;

            return new AutofacServiceProvider(mtc);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
