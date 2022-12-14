// <auto-generated />
using System;
using AutomaticBroccoli.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutomaticBroccoli.DataAccess.Postgres.Migrations
{
    [DbContext(typeof(AutomaticBroccoliDbContext))]
    partial class AutomaticBroccoliDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AttachmentOpenLoop", b =>
                {
                    b.Property<Guid>("AttachmentsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OpenLoopsId")
                        .HasColumnType("uuid");

                    b.HasKey("AttachmentsId", "OpenLoopsId");

                    b.HasIndex("OpenLoopsId");

                    b.ToTable("AttachmentOpenLoop");
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.OpenLoop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OpenLoops");
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AttachmentOpenLoop", b =>
                {
                    b.HasOne("AutomaticBroccoli.DataAccess.Postgres.Entities.Attachment", null)
                        .WithMany()
                        .HasForeignKey("AttachmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutomaticBroccoli.DataAccess.Postgres.Entities.OpenLoop", null)
                        .WithMany()
                        .HasForeignKey("OpenLoopsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.OpenLoop", b =>
                {
                    b.HasOne("AutomaticBroccoli.DataAccess.Postgres.Entities.User", "User")
                        .WithMany("OpenLoops")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.User", b =>
                {
                    b.Navigation("OpenLoops");
                });
#pragma warning restore 612, 618
        }
    }
}
