using System;
using System.Linq;
using System.Net.Http;
using FitnessManager.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessManager.Tests
{
    public class BaseApiTest
    {
        protected static Random Random = new Random();
        protected readonly HttpClient _client;
        protected FitnessDbContext _context;

        public BaseApiTest()
        {
            var webHostBuilder = new WebHostBuilder()
                .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json"))
                .UseStartup<Startup>();

            var server = new TestServer(webHostBuilder);

            _client = server.CreateClient();
            _context = server.Services.GetService<FitnessDbContext>();
        }

        protected static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(
                Enumerable
                    .Repeat(chars, length)
                    .Select(s => s[Random.Next(s.Length)])
                    .ToArray());
        }
    }
}
