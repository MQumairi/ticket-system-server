using System;
using System.Collections.Generic;

namespace API.Models.DTO
{
    public class CommentDto
    {
        public int post_id { get; set; }

        public DateTime date_time { get; set; }

        public string description { get; set; }

        public UserDto author { get; set; }

        public AttachmentDto attachment {get; set; }

    }
}