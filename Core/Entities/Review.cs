using System;
using System.Collections.Generic;
using Core.Enums;

namespace Core.Entities
{
    public class Review
    {
        /// <summary>
        /// Identification
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Review Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Review Data
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        /// Rating data
        /// </summary>
        public Rating Rating { get; set; }

        /// <summary>
        /// The date the review was added
        /// </summary>
        public DateTime ReviewedOn { get; set; }

        /// <summary>
        /// The person who added the review
        /// </summary>
        public Guid ReviewerId { get; set; }
        public ApplicationUser Reviewer { get; set; }

        /// <summary>
        /// Image (Base64 String)
        /// </summary>
        public Image Image { get; set; }

        public Place Place { get; set; }
        public Guid PlaceId { get; set; }

        public virtual List<ReviewLike> Likes { get; set; }
        public virtual List<Comment> Comments { get; set; }

    }
}
