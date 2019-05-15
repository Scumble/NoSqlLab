using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoSqlLab.Services;
using NoSqlLab.Services.Repositories;

namespace NoSqlLab
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
            services.AddScoped<UserRepository>();
            services.AddScoped<NoteRepository>();
            services.AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions,SimpleAuthHandler> ("BasicAuthentication", null);

            // Создание индексов в коллекции User
            services.BuildServiceProvider().GetRequiredService<UserRepository>()
                  .CreateIndexes();

            // Создание индексов в коллекции Note
            services.BuildServiceProvider().GetRequiredService<NoteRepository>()
                  .CreateIndexes();
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
