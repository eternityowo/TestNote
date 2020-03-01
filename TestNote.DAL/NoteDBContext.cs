using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TestNote.DAL.Entities;

namespace TestNote.DAL
{
    public partial class NoteDBContext : DbContext
    {
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public NoteDBContext(DbContextOptions<NoteDBContext> options)
            : base(options)
        { }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=NoteDB;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasIndex(e => e.CreateDate)
                    .HasName("IDX_Notes_CreateDate");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_Notes_UserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notes__UserId__5BE2A6F2");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Ip)
                    .HasName("IDX_Users_IP");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BlockDate).HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
