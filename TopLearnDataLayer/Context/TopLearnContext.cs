using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearn.DataLayer.Entities;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;
using TopLearnDataLayer.Entities.Order;
using TopLearnDataLayer.Entities.Question;

namespace TopLearnDataLayer.Context
{
   public class TopLearnContext:DbContext
    {

        public TopLearnContext(DbContextOptions<TopLearnContext> options):base(options)
        {
            
        }



        #region User

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDiscount> UserDiscounts { get; set; }

        #endregion

        
        #region Wallet

        public DbSet<WalletType> WalletTypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion
        
        #region Permission

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion
        
        #region Course
        public DbSet<CourseGroup> courseGroups { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComment> CourseComment { get; set; }
        public DbSet<CourseVote> CourseVotes { get; set; }
        #endregion

        #region Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        #endregion

        #region Answer
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>().HasQueryFilter(x => x.IsDelete == false);
            modelBuilder.Entity<Role>().HasQueryFilter(r=>r.IsDelete == false);
            modelBuilder.Entity<CourseGroup>().HasQueryFilter(r=>r.IsDelete == false);
           
            base.OnModelCreating(modelBuilder);
        }
    }
}
