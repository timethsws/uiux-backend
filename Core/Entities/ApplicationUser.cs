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

    }
}
