using System;
namespace Core.Entities
{
    public class CommentLike
    {
        public Guid Id { get; set; }

        public Comment Comment { get; set; }
        public Guid CommentId { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
    }
}
