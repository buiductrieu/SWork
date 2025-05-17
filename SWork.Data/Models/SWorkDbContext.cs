using Microsoft.EntityFrameworkCore;
using SWork.Data.Entities;

namespace SWork.Data.Models
{
    public class SWorkDbContext : DbContext
    {
        public SWorkDbContext(DbContextOptions<SWorkDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. Quan hệ 1-1 ---
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

            // --- 2. Quan hệ 1-nhiều ---
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

            // --- 3. JobBookmark (nhiều-nhiều qua thực thể) ---
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

            // --- 5. Resume - Template ---
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.ResumeTemplate)
                .WithMany(t => t.Resumes)
                .HasForeignKey(r => r.TemplateID)
                .OnDelete(DeleteBehavior.SetNull);

            // --- 6. Review ---
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Application)
                .WithMany()
                .HasForeignKey(r => r.ApplicationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.Reviewer_id)
                .OnDelete(DeleteBehavior.Restrict);

            // --- 7. Notifications & Reports ---
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Subscription)
                .WithMany(s => s.Jobs)
                .HasForeignKey(j => j.SubscriptionID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.JobCategory)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CategoryID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Application)
                .WithMany(a => a.Reviews) // Đảm bảo `Application` có `Reviews`
                .HasForeignKey(r => r.ApplicationID)
                .OnDelete(DeleteBehavior.Restrict);
            // --- 8. Unique constraints ---
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Skill>()
                .HasIndex(s => s.SkillName)
                .IsUnique();

            modelBuilder.Entity<JobCategory>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();
        }

    }
}