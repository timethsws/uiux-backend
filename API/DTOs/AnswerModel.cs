using System;
namespace API.DTOs
{
    public class AnswerModel
    {
        public Guid questionId { get; set; }

        public Guid userId { get; set; }

        public string body { get; set; }
    }
}
