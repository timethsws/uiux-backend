using System;
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
        public String Name { get; set; }

        /// <summary>
        /// Description for the place
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Main Image
        /// </summary>
        public String Image { get; set; }

    }
}
