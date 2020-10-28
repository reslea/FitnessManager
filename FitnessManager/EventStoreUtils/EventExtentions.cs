using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FitnessManager.EventStoreUtils
{
    public static class EventExtentions
    {
        public static EventData GetEventData(this object @event, string eventType, object eventMetadata = null)
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
