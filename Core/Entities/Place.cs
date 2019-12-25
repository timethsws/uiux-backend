using System;
using System.Collections.Generic;

namespace Core.Entities
{
    /// <summary>
    /// A Touraist Destination
    /// </summary>
    public class Place
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the place
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description for the place
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// Short Description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Main Image
        /// </summary>
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
    }


}
