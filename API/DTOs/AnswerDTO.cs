using System;
namespace API.DTOs
{
    public class AnswerDTO
    {
        public Guid Id { get; set; }

        public UserThumbDTO Owner { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Liked { get; set; }

        public int LikesCount { get; set; }

        public string Body { get; set; }
    }
}
