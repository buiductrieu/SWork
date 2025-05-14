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

            // Cấu hình quan hệ một-một
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

            // Cấu hình quan hệ một-nhiều
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Resumes)
                .WithOne(r => r.Student)
                .HasForeignKey(r => r.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình Application để tránh multiple cascade paths
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Applications)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Cascade); // Giữ cascade cho Student

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.Jobs)
                .WithOne(j => j.Employer)
                .HasForeignKey(j => j.EmployerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Job>()
                .HasMany(j => j.Applications)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.NoAction); // Đổi thành NoAction

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Applications)
                .WithOne(a => a.Resume)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.NoAction); // Đổi thành NoAction

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

            // Cấu hình quan hệ nhiều-nhiều giữa Student và Job (qua JobBookmark)
            modelBuilder.Entity<JobBookmark>()
                .HasKey(jb => jb.BookmarkID);

            modelBuilder.Entity<JobBookmark>()
                .HasOne(jb => jb.Student)
                .WithMany(s => s.JobBookmarks)
                .HasForeignKey(jb => jb.StudentID)
                .OnDelete(DeleteBehavior.NoAction); // Đổi thành NoAction

            modelBuilder.Entity<JobBookmark>()
                .HasOne(jb => jb.Job)
                .WithMany(j => j.JobBookmarks)
                .HasForeignKey(jb => jb.JobID)
                .OnDelete(DeleteBehavior.Cascade); // Giữ nguyên cascade

            // Cấu hình Subscription và Job
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Subscription)
                .WithMany()
                .HasForeignKey(j => j.SubscriptionID)
                .OnDelete(DeleteBehavior.SetNull); // Nếu Subscription bị xóa, Job vẫn tồn tại

            // Cấu hình JobCategory và Job
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CategoryID)
                .OnDelete(DeleteBehavior.SetNull); // Nếu Category bị xóa, Job vẫn tồn tại

            // Cấu hình Resume và ResumeTemplate
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.ResumeTemplate)
                .WithMany(t => t.Resumes)
                .HasForeignKey(r => r.TemplateID)
                .OnDelete(DeleteBehavior.SetNull); // Nếu Template bị xóa, Resume vẫn tồn tại

            // Cấu hình Review với Application
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Application)
                .WithMany()
                .HasForeignKey(r => r.ApplicationID)
                .OnDelete(DeleteBehavior.NoAction); // Đổi thành NoAction

            // Cấu hình Review với User (Reviewer và Reviewee)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.Reviewer_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewee)
                .WithMany()
                .HasForeignKey(r => r.Reviewee_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Review với Job
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Job)
                .WithMany(j => j.Reviews)
                .HasForeignKey(r => r.Job_id)
                .OnDelete(DeleteBehavior.NoAction); // Đổi thành NoAction

            // Các ràng buộc khoá ngoại khác 
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

            // Các ràng buộc khác
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