using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YiQiKan.DotNet.Model.DBModel;

namespace YiQiKan.DotNet.DataBase.DBContexts {
    public class VideoDBContext : DbContext {
        public DbSet<VideoHistory> VideoHistories { get; set; }

        private readonly string connectString;

        public VideoDBContext(string connectString)
        {
            this.connectString = connectString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Filename={connectString}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<VideoHistory>().HasKey(b => b.MovieId);
        }
    }
}
