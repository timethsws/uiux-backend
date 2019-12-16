using System;
using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Core.Model
{
    public class AddReviewModel
    {
        public Guid UserId { get; set; }

        public Guid PlaceId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Rating Rating { get; set; }

        public IFormFile File { get; set; }
    }
}
