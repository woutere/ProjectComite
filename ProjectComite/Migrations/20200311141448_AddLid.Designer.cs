﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectComite.data;

namespace ProjectComite.Migrations
{
    [DbContext(typeof(ComiteContext))]
    [Migration("20200311141448_AddLid")]
    partial class AddLid
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProjectComite.Models.Actie", b =>
                {
                    b.Property<int>("actieId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GemeenteId");

                    b.Property<string>("informatie");

                    b.HasKey("actieId");

                    b.HasIndex("GemeenteId");

                    b.ToTable("Actie");
                });

            modelBuilder.Entity("ProjectComite.Models.ActieLid", b =>
                {
                    b.Property<int>("actieLidId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("actieId");

                    b.Property<int>("lidId");

                    b.HasKey("actieLidId");

                    b.HasIndex("actieId");

                    b.HasIndex("lidId");

                    b.ToTable("ActieLid");
                });

            modelBuilder.Entity("ProjectComite.Models.Gemeente", b =>
                {
                    b.Property<int>("gemeenteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("naam");

                    b.Property<string>("postcode");

                    b.HasKey("gemeenteId");

                    b.ToTable("Gemeente");
                });

            modelBuilder.Entity("ProjectComite.Models.Lid", b =>
                {
                    b.Property<int>("lidId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("emailAdres");

                    b.Property<int>("gemeenteId");

                    b.Property<bool>("lidgeldBetaald");

                    b.Property<string>("naam");

                    b.Property<string>("telefoonnummer");

                    b.HasKey("lidId");

                    b.HasIndex("gemeenteId");

                    b.ToTable("Lid");
                });

            modelBuilder.Entity("ProjectComite.Models.Actie", b =>
                {
                    b.HasOne("ProjectComite.Models.Gemeente", "gemeente")
                        .WithMany()
                        .HasForeignKey("GemeenteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectComite.Models.ActieLid", b =>
                {
                    b.HasOne("ProjectComite.Models.Actie", "actie")
                        .WithMany("leden")
                        .HasForeignKey("actieId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectComite.Models.Lid", "lid")
                        .WithMany("acties")
                        .HasForeignKey("lidId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ProjectComite.Models.Lid", b =>
                {
                    b.HasOne("ProjectComite.Models.Gemeente", "gemeente")
                        .WithMany("leden")
                        .HasForeignKey("gemeenteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
