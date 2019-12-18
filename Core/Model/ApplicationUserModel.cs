using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

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
        public String UserName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public Gender Gender { get; set; }

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

        /// <summary>
        /// Profile picture as base64string
        /// </summary>
        public String ProfilePicture { get; set; }
    }
}
