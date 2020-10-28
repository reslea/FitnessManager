using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FitnessManager.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FitnessManager.Services
{
    public class TrainingService : ITrainingService
    {
        private const string TrainingStreamName = "Trainings";
        private readonly IEventStoreConnection _connection;
        private readonly UserCredentials _creds;

        public TrainingService(IEventStoreConnection connection, UserCredentials creds)
        {
            _connection = connection;
            _creds = creds;
        }

        public async Task<bool> HasConflictingTraining(TrainingAddModel training)
        {
            var dateFrom = training.StartTime.AddHours(-1);
            var dateTo = training.StartTime.AddHours(1);

            var coachConflict = await HasConflictingDateInStream($"coachTrainings-{training.CoachId}", dateFrom, dateTo);
            var hallConflict = await HasConflictingDateInStream($"hallTrainings-{training.HallId}", dateFrom, dateTo);

            return coachConflict || hallConflict;
        }

        public async Task Add(TrainingAddModel training)
        {
            var @event = GetEventData("AddTraining", training);
            await _connection.AppendToStreamAsync(TrainingStreamName, ExpectedVersion.Any, @event);
        }

        private async Task<bool> HasConflictingDateInStream(string stream, DateTime dateFrom, DateTime dateTo)
        {
            bool flag;
            do
            {
                var readResult = await _connection.ReadStreamEventsForwardAsync(stream, 0, 100, true, _creds);
                flag = !readResult.IsEndOfStream;

                var isConflictPresent = readResult.Events
                    .Select(e => JsonConvert.DeserializeObject<TrainingAddModel>(Encoding.UTF8.GetString(e.Event.Data))
                        .StartTime
                    )
                    .Any(date => date >= dateFrom && date <= dateTo);

                if (isConflictPresent) return true;
            } while (flag);

            return false;
        }
        
        public static EventData GetEventData(string eventType, object @event, object eventMetadata = null)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var jsonEvent = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, serializerSettings));
            var jsonEventMetadata = eventMetadata == null
                ? null
                : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventMetadata));

            return new EventData(Guid.NewGuid(), eventType, true, jsonEvent, jsonEventMetadata);
        }
    }
}
