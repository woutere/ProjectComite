using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectComite.Areas.Identity.Data;
using ProjectComite.Models;

namespace ProjectComite.data
{

    public class ComiteContext :IdentityDbContext<CustomUser>
    {
        public ComiteContext(DbContextOptions<ComiteContext> options) : base(options)
        {

        }


        public DbSet<Lid> leden { get; set; }
        public DbSet<Gemeente> gemeenten { get; set; }
        public DbSet<Actie> acties { get; set; }
        public DbSet<ActieLid> actieleden { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lid>().ToTable("Lid").HasOne(g => g.gemeente);
            modelBuilder.Entity<Gemeente>().ToTable("Gemeente");
            //modelBuilder.Entity<Gemeente>().HasMany(a => a.acties);
            modelBuilder.Entity<Actie>().ToTable("Actie").HasOne(g=>g.gemeente);
            modelBuilder.Entity<ActieLid>().ToTable("ActieLid");
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ActieLid>().HasKey(al => new { al.actieId, al.lidId });
            modelBuilder.Entity<ActieLid>().HasOne(o => o.actie).WithMany(s => s.leden).HasForeignKey(k => k.actieId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ActieLid>().HasOne(o => o.lid).WithMany(s => s.actieleden).HasForeignKey(k => k.lidId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
