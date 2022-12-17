﻿// <auto-generated />
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
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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

                    b.Property<Guid>("OpenLoopId")
                        .HasColumnType("uuid");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OpenLoopId");

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

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.UserNotes", b =>
                {
                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.ToTable("UserNotes", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.Attachment", b =>
                {
                    b.HasOne("AutomaticBroccoli.DataAccess.Postgres.Entities.OpenLoop", "OpenLoop")
                        .WithMany("Attachments")
                        .HasForeignKey("OpenLoopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OpenLoop");
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

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.OpenLoop", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("AutomaticBroccoli.DataAccess.Postgres.Entities.User", b =>
                {
                    b.Navigation("OpenLoops");
                });
#pragma warning restore 612, 618
        }
    }
}
