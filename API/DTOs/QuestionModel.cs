using System;
namespace API.DTOs
{
    public class QuestionModel
    {
        public Guid userId { get; set; }

        public Guid placeId { get; set; }

        public string Question { get; set; }
    }
}
