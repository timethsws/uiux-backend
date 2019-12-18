using System;
namespace API.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public UserThumbDTO Owner { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
