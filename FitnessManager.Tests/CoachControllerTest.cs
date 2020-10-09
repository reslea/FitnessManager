using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FitnessManager.Db.Entities;
using Newtonsoft.Json;
using Xunit;

namespace FitnessManager.Tests
{
    public class CoachControllerTest : BaseApiTest
    {
        private const string ApiUrl = "/api/coach";

        [Fact]
        public async Task GetAll_Returns_List_Of_Coaches()
        {
            var generatedCoaches = Enumerable.Range(0, 2)
                .Select(_ => GetRandomCoach())
                .ToList();

            await _context.Coaches.AddRangeAsync(generatedCoaches);
            await _context.SaveChangesAsync();
            
            var coachesJson = await _client.GetStringAsync(ApiUrl);
            var coaches = JsonConvert.DeserializeObject<List<Coach>>(coachesJson);

            foreach (var coach in generatedCoaches)
            {
                Assert.Contains(coaches, c =>
                    c.FirstName == coach.FirstName
                    && c.LastName == coach.LastName
                    && c.Specialty == coach.Specialty);
            }
        }

        [Fact]
        public async Task CreateCoach()
        {
            var coach = GetRandomCoach();

            var coachJson = JsonConvert.SerializeObject(coach);

            var response = await _client.PostAsync("/api/coach", 
                new StringContent(coachJson, Encoding.UTF8, MediaTypeNames.Application.Json));

            Assert.True(response.IsSuccessStatusCode);

            var coachDb = _context.Coaches.FirstOrDefault(c =>
           c.FirstName == coach.FirstName
           && c.LastName == coach.LastName
           && c.Specialty == coach.Specialty);

            Assert.NotNull(coachDb);
        }

        private static Coach GetRandomCoach()
        {
            return new Coach
            {
                FirstName = GenerateRandomString(10),
                LastName = GenerateRandomString(5),
                Specialty = (TrainingType)
                    Random.Next(1, Enum.GetNames(typeof(TrainingType)).Length)
            };
        }
    }
}
