using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebApplication1.Models
{
    public class UrlUpdateDbContext : DbContext
    {
        public UrlUpdateDbContext() : base("URLUpdatesConnectionString")
        {
            //Database.SetInitializer<UrlUpdateDbContext>(new CreateDatabaseIfNotExists<UrlUpdateDbContext>());
            Database.SetInitializer<UrlUpdateDbContext>(new CreateDatabaseIfNotExists<UrlUpdateDbContext>());

        }

        public DbSet<URLUpdate> URLUpdates { get; set; }
        public DbSet<URL> URLs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}