using System;
namespace Core.Entities
{
    public class ReviewLike
    {
        public Guid Id { get; set; }

        public Review Review { get; set; }
        public Guid ReviewId { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
    }
}
