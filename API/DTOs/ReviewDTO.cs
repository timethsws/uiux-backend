using System;
using System.Collections.Generic;
using Core.Enums;

namespace API.DTOs
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public Rating Rating { get; set; }

        public int LikesCount { get; set; }

        public int CommentCount { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public bool Liked { get; set; }

        public UserThumbDTO Owner { get; set; }
    }
}
