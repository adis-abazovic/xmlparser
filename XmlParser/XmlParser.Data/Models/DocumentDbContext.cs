using Microsoft.EntityFrameworkCore;

namespace XmlParser.Data.Models
{
    public partial class DocumentDbContext : DbContext
    {
        public DocumentDbContext()
        {

        }

        public DocumentDbContext(DbContextOptions<DocumentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbDocument> XmlDocuments { get; set; }
        public virtual DbSet<DbElement> XmlElements { get; set; }
        public virtual DbSet<DbWordDuplicate> WordDuplicates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbDocument>(entity =>
            {
                entity.Property(e => e.ClientID)
                    .IsRequired();

                entity.HasMany(e => e.Elements);
            });

            modelBuilder.Entity<DbElement>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.HasMany(w => w.WordDuplicates);
            });

            modelBuilder.Entity<DbWordDuplicate>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired();

                entity.Property(e => e.Frequency)
                    .IsRequired();
            });
        }
    }
}
