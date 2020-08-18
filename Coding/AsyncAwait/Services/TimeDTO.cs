using System;
using System.Text.Json.Serialization;

namespace AsyncAwait
{
    public sealed class TimeDTO
    {
        [JsonIgnore]
        public string Location { get; set; }

        [JsonPropertyName("abbreviation")]
        public string Code { get; set; }

        [JsonPropertyName("datetime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonPropertyName("week_number")]
        public int WeekNum { get; set; }

        [JsonIgnore]
        public int CompletionThreadId { get; set; }

        [JsonIgnore]
        public int InitialThreadId { get; set; }
    }
}