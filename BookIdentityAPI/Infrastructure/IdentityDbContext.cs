using BookIdentityAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookIdentityAPI.Infrastructure
{
    public class IdentityDbContext:DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserName);
            //or: modelBuilder.Entity<User>().HasAlternateKey(u => new { u.Passport, u.Name})
        }
        public DbSet<User> Users { get; set; }
    }
}
