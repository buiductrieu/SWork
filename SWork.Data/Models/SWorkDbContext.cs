using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWork.Data.Entities;

namespace SWork.Data.Models
{
    public class SWorkDbContext : IdentityDbContext<ApplicationUser>
    {
        public SWorkDbContext()
        {

        }
        public SWorkDbContext(DbContextOptions<SWorkDbContext> options) : base(options)
        {

        }


        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<Student> Students { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeTemplate> ResumeTemplates { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<JobBookmark> JobBookmarks { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationUser Reviewee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Employer", NormalizedName = "EMPLOYER" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Student", NormalizedName = "STUDENT" }
            );

            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-1 Relationships
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employer>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employer)
                .HasForeignKey<Employer>(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-n Relationships
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Resumes)
                .WithOne(r => r.Student)
                .HasForeignKey(r => r.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Applications)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.Jobs)
                .WithOne(j => j.Employer)
                .HasForeignKey(j => j.EmployerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Job>()
                .HasMany(j => j.Applications)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Restrict); // tránh vòng xoá

            modelBuilder.Entity<Job>()
                .Property(j => j.Salary)
                .HasPrecision(18, 2); // 18 digits, 2 decimal places

            modelBuilder.Entity<Wallet>()
               .Property(w => w.Balance)
               .HasPrecision(18, 2); // 18 digits, 2 decimal places

            modelBuilder.Entity<Subscription>()
                .Property(s => s.Price)
                .HasPrecision(18, 2); // 18 digits, 2 decimal places

             modelBuilder.Entity<WalletTransaction>()
                .Property(w => w.Amount)
                .HasPrecision(18, 2); // 18 digits, 2 decimal places


            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Applications)
                .WithOne(a => a.Resume)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.Restrict); // tránh vòng xoá

            modelBuilder.Entity<Application>()
                .HasMany(a => a.Interviews)
                .WithOne(i => i.Application)
                .HasForeignKey(i => i.ApplicationID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletID)
                .OnDelete(DeleteBehavior.Cascade);

            // Resume - Template (1-n)
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.ResumeTemplate)
                .WithMany(t => t.Resumes)
                .HasForeignKey(r => r.TemplateID)
                .OnDelete(DeleteBehavior.SetNull); // nếu template bị xoá, resume vẫn tồn tại

            // JobBookmark (n-n với thực thể trung gian)
            modelBuilder.Entity<JobBookmark>()
                .HasKey(jb => jb.BookmarkID);

            modelBuilder.Entity<JobBookmark>()
                .HasOne(jb => jb.Student)
                .WithMany(s => s.JobBookmarks)
                .HasForeignKey(jb => jb.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobBookmark>()
                .HasOne(jb => jb.Job)
                .WithMany(j => j.JobBookmarks)
                .HasForeignKey(jb => jb.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            // Skill - Student (n-n thông qua bảng phụ)
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Skills)
                .WithMany(sk => sk.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentSkill",
                    ss => ss.HasOne<Skill>().WithMany().HasForeignKey("SkillID"),
                    ss => ss.HasOne<Student>().WithMany().HasForeignKey("StudentID"),
                    ss =>
                    {
                        ss.HasKey("StudentID", "SkillID");
                        ss.ToTable("StudentSkills");
                    });

            // Application - Review (1-n)
            modelBuilder.Entity<Application>()
                .HasMany(a => a.Reviews)
                .WithOne(r => r.Application)
                .HasForeignKey(r => r.ApplicationID)
                .OnDelete(DeleteBehavior.SetNull);

            // ApplicationUser - Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // ... existing code ...
            // Report - ApplicationUser
            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Review configuration
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.Reviewer_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Application)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.ApplicationID)
                .OnDelete(DeleteBehavior.SetNull);
        }
        // ... existing code ...
    }

}