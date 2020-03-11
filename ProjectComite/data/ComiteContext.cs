using Microsoft.EntityFrameworkCore;
using ProjectComite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.data
{
    public class ComiteContext :DbContext
    {
        public ComiteContext(DbContextOptions<ComiteContext> options) : base(options)
        {

        }

        public DbSet<Lid> leden { get; set; }
        public DbSet<Gemeente> gemeenten { get; set; }
        public DbSet<Actie> acties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lid>().ToTable("Lid");
            modelBuilder.Entity<Gemeente>().ToTable("Gemeente");

        }
    }
}
