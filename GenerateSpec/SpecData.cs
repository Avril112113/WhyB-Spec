using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GenerateSpec
{
    public class SpecData
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonPropertyName("mandates")]
        public IReadOnlyList<string> Mandates { get; set; }

        [JsonPropertyName("codes_desc")]
        public string CodesDesc { get; set; }

        [JsonPropertyName("codes")]
        public IReadOnlyDictionary<string, string> Codes { get; set; }

        [JsonPropertyName("types")]
        public IReadOnlyDictionary<string, SpecType> Types { get; set; }

        [JsonPropertyName("requests")]
        public IReadOnlyDictionary<string, Request> Requests { get; set; }

        [JsonPropertyName("events")]
        public IReadOnlyDictionary<string, Event> Events { get; set; }

        [JsonPropertyName("extensions_desc")]
        public string ExtensionsDesc { get; set; }
        [JsonPropertyName("extensions")]
        public IReadOnlyDictionary<string, Extension> Extensions { get; set; }
    }

    public class SpecType
    {
        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonPropertyName("json")]
        public string Json { get; set; }

        [JsonPropertyName("real")]
        public string Real { get; set; }
    }

    public class Event
    {
        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonPropertyName("data")]
        public IReadOnlyDictionary<string, ValueInfo> Data { get; set; }

        [JsonPropertyName("optional")]
        public bool Optional { get; set; }
    }

    public class Request
    {
        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonPropertyName("params")]
        public IReadOnlyDictionary<string, ValueInfo> Params { get; set; }

        [JsonPropertyName("result")]
        public ValueInfo Result { get; set; }

        [JsonPropertyName("optional")]
        public bool Optional { get; set; }
    }

    public class ValueInfo
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [DefaultValue(false)]
        [JsonPropertyName("optional")]
        public bool Optional { get; set; }
    }

    public class Extension
    {
        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonPropertyName("requests")]
        public IReadOnlyDictionary<string, Request> Requests { get; set; }

        [JsonPropertyName("events")]
        public IReadOnlyDictionary<string, Event> Events { get; set; }
    }
}
