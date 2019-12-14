using System;
using Core.Enums;

namespace Core.Model
{
    public class AddReviewModel
    {
        public Guid UserId { get; set; }

        public Guid PlaceId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Rating Rating { get; set; }
    }
}
