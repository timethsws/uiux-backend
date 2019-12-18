using System;
namespace Core.Entities
{
    public class QuestionLikes
    {
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }

        public Question Question { get; set; }
        public Guid QuestionId { get; set; }
    }
}
