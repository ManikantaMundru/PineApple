using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;
using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit
{
   public static class Extensions
    {
        public static T BodyToModel<T>(this IEventModel eventmodel)
        {
            return JsonConvert.DeserializeObject<T>(eventmodel.Body, new JsonSerializerSettings
            {
                MissingMemberHandling=MissingMemberHandling.Ignore,
                NullValueHandling=NullValueHandling.Ignore
            });
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new StringEnumConverter());
        }

        public static void ForEach<T>(this ICollection<T> seq, Action<T> action)
        {
            foreach (var item in seq)
            {
                action(item);
            }
        }

        internal static bool IsConnected(this IConnection connection) => connection?.IsOpen ?? false;
    }
}
