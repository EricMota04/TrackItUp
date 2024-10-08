﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrackItUpDAL.Context;

#nullable disable

namespace TrackItUpDAL.Migrations
{
    [DbContext(typeof(TrackItUpContext))]
    [Migration("20240920055709_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TrackItUpDAL.Entities.Habit", b =>
                {
                    b.Property<int>("HabitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HabitId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeactivatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Frequency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HabitName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("ReminderTime")
                        .HasColumnType("time");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("HabitId");

                    b.HasIndex("UserId");

                    b.ToTable("Habits");
                });

            modelBuilder.Entity("TrackItUpDAL.Entities.HabitTracking", b =>
                {
                    b.Property<int>("HabitTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HabitTrackingId"));

                    b.Property<DateTime>("DateTracked")
                        .HasColumnType("datetime2");

                    b.Property<int>("HabitId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.HasKey("HabitTrackingId");

                    b.HasIndex("HabitId");

                    b.ToTable("HabitTrackings");
                });

            modelBuilder.Entity("TrackItUpDAL.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TrackItUpDAL.Entities.Habit", b =>
                {
                    b.HasOne("TrackItUpDAL.Entities.User", "User")
                        .WithMany("Habits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrackItUpDAL.Entities.HabitTracking", b =>
                {
                    b.HasOne("TrackItUpDAL.Entities.Habit", "Habit")
                        .WithMany("HabitTrackings")
                        .HasForeignKey("HabitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Habit");
                });

            modelBuilder.Entity("TrackItUpDAL.Entities.Habit", b =>
                {
                    b.Navigation("HabitTrackings");
                });

            modelBuilder.Entity("TrackItUpDAL.Entities.User", b =>
                {
                    b.Navigation("Habits");
                });
#pragma warning restore 612, 618
        }
    }
}
