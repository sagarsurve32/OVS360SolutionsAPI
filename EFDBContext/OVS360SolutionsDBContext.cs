using Microsoft.EntityFrameworkCore;
using OVS360SolutionsAPI.Constants;
using OVS360SolutionsAPI.Models;

namespace OVS360SolutionsAPI.EFDBContext
{
    public class OVS360SolutionsDBContext : DbContext
    {
        public OVS360SolutionsDBContext()
        {
        }

        public OVS360SolutionsDBContext(DbContextOptions<OVS360SolutionsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Enquiry> Enquiries { get; set; } = null!;

        public virtual DbSet<Candidate> Candidates { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().AddJsonFile(WebConstants.ConfigurationFileName, false, true);
                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString(WebConstants.ConnectionName);

                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enquiry>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");                
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });
        }
    }
}
