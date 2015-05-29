namespace SystemDot.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using SystemDot.EventSourcing.Streams;
    using Newtonsoft.Json;

    public class JsonSerializer
    {
        private readonly IEnumerable<Type> knownTypes = new[]
        {
            typeof(List<SourcedEvent>), 
            typeof(Dictionary<string, object>)
        };

        private readonly Newtonsoft.Json.JsonSerializer typedSerializer = new Newtonsoft.Json.JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.All,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly Newtonsoft.Json.JsonSerializer untypedSerializer = new Newtonsoft.Json.JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.Auto,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public virtual void Serialize<T>(Stream output, T graph)
        {
            using (var streamWriter = new StreamWriter(output, Encoding.UTF8))
            {
                Serialize(new JsonTextWriter(streamWriter), graph);
            }
        }

        public virtual T Deserialize<T>(Stream input)
        {
            using (var streamReader = new StreamReader(input, Encoding.UTF8))
            {
                return Deserialize<T>(new JsonTextReader(streamReader));
            }
        }

        protected virtual void Serialize(JsonWriter writer, object graph)
        {
            using (writer)
            {
                GetSerializer(graph.GetType()).Serialize(writer, graph);
            }
        }

        protected virtual T Deserialize<T>(JsonReader reader)
        {
            using (reader)
            {
                return (T)GetSerializer(typeof(T)).Deserialize(reader, typeof(T));
            }
        }

        protected virtual Newtonsoft.Json.JsonSerializer GetSerializer(Type typeToSerialize)
        {
            return knownTypes.Contains(typeToSerialize) 
                ? untypedSerializer 
                : typedSerializer;
        }
    }
}