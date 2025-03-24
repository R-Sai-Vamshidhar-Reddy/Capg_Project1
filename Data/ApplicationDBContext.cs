using Microsoft.EntityFrameworkCore;
using SubjectTopicApp.Models;
using System.Collections.Generic;


namespace SubjectTopicApp.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Topics> Topics { get; set; }
        public DbSet<SubTopics> SubTopics { get; set; }
    }
}