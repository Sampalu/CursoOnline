using CursoOnline.Dominio.Base;
using CursoOnline.Ioc;
using CursoOnline.Web.Filters;
using Microsoft.Extensions.Configuration;
using Polly.Caching;
using Polly.Registry;
using Polly;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            StartupIoc.ConfigureServices(services, Configuration);

            services.AddRazorPages();
            services.AddMvc(opt => {
                opt.EnableEndpointRouting = false;
                opt.Filters.Add(typeof(CustomExceptionFilter));
            });

            services.AddMemoryCache();
            services.AddSingleton<Polly.Caching.IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
            services.AddSingleton<Polly.Registry.IReadOnlyPolicyRegistry<string>, Polly.Registry.PolicyRegistry>((serviceProvider) =>
            {
                PolicyRegistry registry = new PolicyRegistry();

                var cachePolicy = Policy.CacheAsync<List<Curso>>(serviceProvider
                            .GetRequiredService<IAsyncCacheProvider>()
                            .AsyncFor<List<Curso>>(),
                         TimeSpan.FromMinutes(2));

                registry.Add("CachingPolicy2", cachePolicy);

                return registry;
            });

            //services.AddSingleton<Polly.Registry.IReadOnlyPolicyRegistry<string>, Polly.Registry.PolicyRegistry>((serviceProvider) =>
            //{
            //    PolicyRegistry registry = new PolicyRegistry();

            //    registry.Add("CachingPolicy2",
            //        Policy.CacheAsync<List<Curso>>(
            //            serviceProvider
            //                .GetRequiredService<IAsyncCacheProvider>()
            //                .AsyncFor<List<Curso>>(),
            //            TimeSpan.FromMinutes(2)));
            //    return registry;
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                await next.Invoke();

                IUnitOfWork? unitOfWork = (IUnitOfWork)context.RequestServices.GetService(typeof(IUnitOfWork));
                unitOfWork.Commit();
            });

            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseMvcWithDefaultRoute();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
