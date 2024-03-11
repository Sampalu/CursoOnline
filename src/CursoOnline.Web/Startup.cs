using CursoOnline.Dominio.Base;
using CursoOnline.Ioc;
using CursoOnline.Web.Filters;
using Microsoft.Extensions.Configuration;

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
