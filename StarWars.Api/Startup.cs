using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWars.Api.Services;
using StarWars.DAL.Entities;
using StarWars.Services;

namespace StarWars
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                             .AddJsonOptions(options =>
                             {
                                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                 options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                             });

            services.AddDbContext<StarWarsDbContext>(item => item.UseSqlServer
                     (Configuration.GetConnectionString("StarWarsDbConnection")));

            services.AddScoped<ICharacterServices, CharacterServices>();
            services.AddScoped<IPlanetServices, PlanetServices>();
            services.AddScoped<IEpisodeServices, EpisodeServices>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
