using System;
using System.Text;
using AutoMapper;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FitnessManager.Db;
using FitnessManager.Db.Repositories;
using FitnessManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FitnessManager
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
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "localhost",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtTokenSecret"]))
                    };
                });

            var dbConnectionString = Configuration["DbConnectionString"];
            services.AddDbContext<FitnessDbContext>(
                builder => builder.UseSqlServer(dbConnectionString));

            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<ICoachRepository, CoachRepository>();
            services.AddScoped<ITrainingService, TrainingService>();

            services.AddSingleton<IEventStoreConnection>(serviceProvider =>
            {
                var connectionString = Configuration["EventStoreConnectionString"];
                var conn = EventStoreConnection.Create(new Uri(connectionString));
                conn.ConnectAsync().Wait();
                return conn;
            });

            services.AddSingleton<UserCredentials>(serviceProvider => serviceProvider
                .GetService<IEventStoreConnection>()
                .Settings
                .DefaultUserCredentials);

            services.AddAutoMapper(GetType());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("localhost");
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            EnsureDbCreated(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void EnsureDbCreated(IApplicationBuilder app)
        {
            using var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();

            var context = serviceScope.ServiceProvider
                .GetRequiredService<FitnessDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
