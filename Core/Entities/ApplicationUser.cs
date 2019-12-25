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
        public string UserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// PasswordHash
        /// </summary>
        public String PasswordHash { get; set; }

        /// <summary>
        /// Profile Image
        /// </summary>
        public Guid ProfilePictureId { get; set; }
        public Image ProfilePicture { get; set; }

        /// <summary>
        /// Gender of the users
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Score of the user
        /// </summary>
        public int Score { get; set; }

    }

    public enum Gender
    {
        Male = 0,
        Female = 1,
        Other = 2
    }
}
