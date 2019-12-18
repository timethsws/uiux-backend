using System;
namespace API.DTOs
{
    public class CommentModel
    {
        public Guid UserId { get; set; }

        public Guid ReviewId { get; set; }

        public string Body { get; set; }
    }
}
