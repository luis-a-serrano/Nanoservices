using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ObjectAPI {
   public class Startup {
      public Startup(IConfiguration configuration) {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services) {
         services.AddMvc();
         services.AddSwaggerGen(gen => {
            gen.SwaggerDoc("v1", new Info { Title = "Nanoservice API", Version = "1.0" });
         });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
         if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
         }

         app.UseMvc();
         app.UseSwagger();
         app.UseSwaggerUI(ui => {
            ui.SwaggerEndpoint("/swagger/v1/swagger.json", "v1.0");
         });
      }
   }
}
