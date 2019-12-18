using System;
using Core.Entities;

namespace API.DTOs
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public UserThumbDTO Owner { get; set; }

        public DateTime CreatedOn { get; set; }

        public int LikesCount {get;set;}

        public int AnswerCount { get; set; }

        public bool Liked { get; set; }

        public bool Answered { get; set; }

    }
}
