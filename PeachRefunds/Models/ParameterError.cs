﻿using Newtonsoft.Json;

namespace PeachPayments.Models
{
    public class ParameterError
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
