using System;
namespace Core.Entities
{
    public class Answer
    {
        public Guid Id { get; set; }

        public Question Question { get; set; }
        public Guid QuestionId { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
