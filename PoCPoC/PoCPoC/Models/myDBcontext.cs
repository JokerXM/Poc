using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace PoCPoC.Models
{
    public class myDBcontext:DbContext
    {

        public DbSet<User> User { get; set; }

        public DbSet<Friend> Friend { get; set; }

        public DbSet<Events> Events { get; set; }

        public DbSet<Contribution> Contribute { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<EType> Types { get; set; }

        public DbSet<ContributionType> ContributionTypes { get; set; }
    }
}