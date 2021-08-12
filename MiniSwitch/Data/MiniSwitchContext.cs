using System;
using MiniSwitch.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MiniSwitch.Data
{
    public class MiniSwitchContext : IdentityDbContext<User>
    {
        public MiniSwitchContext(DbContextOptions options) : base(options)
        {
        }

        public new DbSet<User> Users { get; set; }
    }
}
