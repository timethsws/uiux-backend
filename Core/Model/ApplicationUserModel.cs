using System;
namespace Core.Model
{
    public class ApplicationUserModel
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// PasswordHash
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        public String PhoneNumber { get; set; }
    }
}
