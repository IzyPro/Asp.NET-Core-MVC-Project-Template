using System;
using Template.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Template.Data
{
    public class MiniSwitchContext : IdentityDbContext<User>
    {
        public MiniSwitchContext(DbContextOptions options) : base(options)
        {
        }

        public new DbSet<User> Users { get; set; }
    }
}
