using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string CommentBody { get; set; }

        public ApplicationUser Owner { get; set; }
        public Guid OwnerId { get; set; }

        public Review Review { get; set; }
        public Guid ReviewId { get; set; }

        public virtual List<CommentLike> Likes { get; set; }

    }
}
