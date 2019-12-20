using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Question
    {
        /// <summary>
        /// Identification
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Question Body
        /// </summary>
        public string QuestionBody { get; set; }

        /// <summary>
        /// Creater of ghe Question
        /// </summary>
        public ApplicationUser Owner { get; set; }
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Place
        /// </summary>
        public Place Place { get; set; }
        public Guid PlaceId { get; set; }

        /// <summary>
        /// Created on date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        public virtual List<Answer> Answers { get; set; }

        public virtual List<QuestionLikes> Likes { get; set; }

    }
}
