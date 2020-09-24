using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessManager.Db.Entities;
using Newtonsoft.Json;
using Xunit;

namespace FitnessManager.Tests
{
    public class HallControllerTest : BaseApiTest
    {
        private const string ApiUrl = "/api/hall";

        [Fact]
        public async Task Get_Returns_List_Of_Halls()
        {
            var generatedHalls = Enumerable.Range(0, 5)
                .Select(_ => GetRandomHall())
                .ToList();

            await _context.AddRangeAsync(generatedHalls);
            await _context.SaveChangesAsync();

            var json = await _client.GetStringAsync(ApiUrl);
            var halls = JsonConvert.DeserializeObject<List<Hall>>(json);

            foreach (var hall in generatedHalls)
            {
                Assert.Contains(halls, h =>
                    h.Title == hall.Title
                    && h.Capacity == hall.Capacity);
            }
        }

        private static Hall GetRandomHall() => new Hall
        {
            Title = GenerateRandomString(10),
            Capacity = Random.Next(10, 100)
        };
    }
}
