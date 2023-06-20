using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackProject.DataAccessLayer.Entities
{
	public class AppDbContext : IdentityDbContext<User>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Footer> Footers { get; set; }

        public DbSet<Information> Informations { get; set; }

        public DbSet<Links> Links { get; set; }

        public DbSet<Touch> Touches { get; set; }

        public DbSet<Courses> Courses { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<NoticeBoard> NoticeBoards { get; set; }

        public DbSet<NoticeBoard2> NoticeBoard2s { get; set; }

        public DbSet<SessionOne> SessionOnes { get; set; }

        public DbSet<SessionTwo> SessionTwos { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Header> Headers { get; set; }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}

