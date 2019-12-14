﻿using System;
using System.Diagnostics.CodeAnalysis;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Database
{
    public class AppDbContext :DbContext
    {
        
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FavouritePlace> Favourits { get; set; }
        public DbSet<ReviewLike> ReviewLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
    }
}
