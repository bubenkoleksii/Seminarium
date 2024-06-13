﻿// <auto-generated />
using System;
using CourseService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CourseService.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(CommandContext))]
    partial class CommandContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CourseCourseGroup", b =>
                {
                    b.Property<Guid>("CoursesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupsId")
                        .HasColumnType("uuid");

                    b.HasKey("CoursesId", "GroupsId");

                    b.HasIndex("GroupsId");

                    b.ToTable("CourseCourseGroup", "public");
                });

            modelBuilder.Entity("CourseCourseTeacher", b =>
                {
                    b.Property<Guid>("CoursesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeachersId")
                        .HasColumnType("uuid");

                    b.HasKey("CoursesId", "TeachersId");

                    b.HasIndex("TeachersId");

                    b.ToTable("CourseCourseTeacher", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StudyPeriodId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Courses", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("CourseGroups", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseTeacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCreator")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("CourseTeachers", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Lesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Homework")
                        .HasColumnType("text");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lesson", "public");
                });

            modelBuilder.Entity("CourseCourseGroup", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CourseService.Domain.Entities.CourseGroup", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseCourseTeacher", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CourseService.Domain.Entities.CourseTeacher", null)
                        .WithMany()
                        .HasForeignKey("TeachersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Lesson", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Course", b =>
                {
                    b.Navigation("Lessons");
                });
#pragma warning restore 612, 618
        }
    }
}
