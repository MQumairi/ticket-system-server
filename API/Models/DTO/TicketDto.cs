using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Models.DTO
{
    public class TicketDto
    {
        public int post_id { get; set; }

        public DateTime date_time { get; set; }

        public string description { get; set; }

        [JsonPropertyName("user")]
        public UserDto user { get; set; }

        public string title { get; set; }

        public ProductDto product { get; set; }

        public StatusDto status { get; set; }

        public List<CommentDto> comments { get; set; }
    }
}