using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options; // Add this line
using Films.Services;
using Films.Models;
using MongoDB.Driver;



namespace HonoursProjectNoSQL
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
            services.Configure<FilmsDatabaseSettings>(
                Configuration.GetSection(nameof(FilmsDatabaseSettings)));

            services.AddSingleton<IFilmsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<FilmsDatabaseSettings>>().Value);

            // Register the IMongoDatabase service
            services.AddSingleton<IMongoDatabase>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IFilmsDatabaseSettings>();
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });

                services.AddSingleton<FilmsService>();
                services.AddSingleton<ActorsService>();
                services.AddRazorPages();
                services.AddAuthorization();

    // Other service configurations...
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
