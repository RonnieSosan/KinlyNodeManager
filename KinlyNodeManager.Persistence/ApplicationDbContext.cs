using KinlyNodeManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace KinlyNodeManager.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration Configuration;
        private readonly string connectionString;

        public ApplicationDbContext()
        {
            Configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

            connectionString = Configuration.GetConnectionString("Kinly_DB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Node>().HasData(
         new Node
         {
             Id = 1,
             Name = "FirstNode",
             Port = "7070",
             Maintainer = "j@mail.cu"
         }
     );
            modelBuilder.Entity<NodeLabel>().HasData(
                new NodeLabel { Id = 1, Key = "Group", Value = "API", NodeId = 1 },
                new NodeLabel { Id = 2, Key = "Type", Value = "Plartform", NodeId = 1 }
            );
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //create databse sercer connection
            optionsBuilder.UseSqlServer(connectionString);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // List of dbsets to create as tables in the database
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeLabel> NodeLabels { get; set; }
    }
}
