using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Data{
    public class ApplicationDbContext : DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base (options)
            {

            }

            public DbSet<Models.Task> Tasks {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                //changed the name of the table to avoid comflicts 
            modelBuilder.Entity<Models.Task>().ToTable("Tasks");

            modelBuilder.Entity<Models.Task>().HasData(
                new Models.Task
                {
                    Id = 1,
                    Title = "Finish Enterprise Project",
                    Description = "Project 4 for CNT4714.",
                    DueDate = new DateTime(2025,4,24),
                    Priority = "high",
                    Status = "in-progress",
                    CreatedAt = new DateTime(2025,4,4)
                },
                new Models.Task
                {
                    Id = 2,
                    Title = "Put in Workorder",
                    Description = "Theres a hole in my wall. I need to ge it fixed.",
                    DueDate = new DateTime(2025,5,1),
                    Priority = "medium",
                    Status = "todo",
                    CreatedAt = new DateTime(2025,4,4)
                }
            );

        }


    }


}