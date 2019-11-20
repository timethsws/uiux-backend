using System;
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
        public Guid Reviewer { get; set; }

        /// <summary>
        /// Image (Base64 String)
        /// </summary>
        public String Image { get; set; }
    }
}
