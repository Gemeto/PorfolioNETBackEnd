using Microsoft.EntityFrameworkCore;
using PorfolioNETBackEnd.Models;

namespace PorfolioNETBackEnd.Services.Context;

public partial class PortafolioContextService : DbContext
{
    public PortafolioContextService()
    {
    }

    public PortafolioContextService(DbContextOptions<PortafolioContextService> options)
        : base(options)
    {
    }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<JobExperience> JobExperiences { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=127.0.0.1;User ID=root;Password=rufo4321;Port=3306;Database=portafolio");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("category");

            entity.HasIndex(e => e.ParentId, "parent_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("parent_id");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("image");

            entity.Property(e => e.Path)
                .HasColumnType("text")
                .HasColumnName("path");
        });

        modelBuilder.Entity<JobExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("job_experience");

            entity.HasIndex(e => e.CategoryId, "category_id");

            entity.HasIndex(e => e.ImageId, "image_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.Title)
                .HasMaxLength(256)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasOne(d => d.Image).WithMany(p => p.JobExperiences)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("image_id");

            entity.HasOne(d => d.User).WithMany(p => p.JobExperiences)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.JobExperiences)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("cateogry_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.MainImageId, "main_image_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.MainImageId).HasColumnName("main_image_id");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(256)
                .HasColumnName("surname");

            entity.HasOne(d => d.MainImage).WithMany(p => p.Users)
                .HasForeignKey(d => d.MainImageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("main_image_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
