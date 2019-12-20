using System;
namespace API.DTOs
{
    public class ApplicationUserDTO
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Profile Image
        /// </summary>
        public string ProfileImage { get; set; }

    }
}
