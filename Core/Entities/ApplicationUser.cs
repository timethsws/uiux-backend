//-----------------------------------------------------------------------
// <copyright file="ApplicationUser.cs" company="Team Traveller">
//     Copyright (c) Traveller. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Core.Entities
{
    using System;

    /// <summary>
    /// Application User
    /// </summary>
    public class ApplicationUser
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
        public String PasswordHash { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// Profile Image
        /// </summary>
        public Guid ProfilePictureId { get; set; }
        public Image ProfilePicture { get; set; }

        public Gender Gender { get; set; }

    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
