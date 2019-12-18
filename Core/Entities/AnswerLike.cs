using System;
namespace Core.Entities
{
    public class AnswerLike
    {
        public Guid Id { get; set; }

        public Answer Answer { get; set; }
        public Guid AnswerId { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
    }
}
