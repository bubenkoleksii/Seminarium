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

            modelBuilder.Entity("CourseService.Domain.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LessonItemId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PracticalLessonItemSubmitId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.HasKey("Id");

                    b.HasIndex("LessonItemId");

                    b.HasIndex("PracticalLessonItemSubmitId");

                    b.ToTable("Attachments", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

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

            modelBuilder.Entity("CourseService.Domain.Entities.CourseGroupCourse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("GroupId");

                    b.ToTable("CourseGroupCourse", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseTeacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCreator")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("CourseTeachers", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseTeacherCourse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("CourseTeacherCourse", "public");
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

                    b.Property<string>("Homework")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Number")
                        .HasColumnType("bigint");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lessons", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.LessonItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AuthorId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LessonId")
                        .HasColumnType("uuid");

                    b.Property<int>("LessonItemType")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("LessonId");

                    b.ToTable("LessonItems", "public");

                    b.HasDiscriminator<int>("LessonItemType").HasValue(3);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("CourseService.Domain.Entities.PracticalLessonItemSubmit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Attempt")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PracticalLessonItemId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<string>("TeacherComment")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Text")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.HasKey("Id");

                    b.HasIndex("PracticalLessonItemId");

                    b.ToTable("PracticalLessonItemSubmits", "public");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.PracticalLessonItem", b =>
                {
                    b.HasBaseType("CourseService.Domain.Entities.LessonItem");

                    b.Property<bool>("AllowSubmitAfterDeadline")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("Attempts")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("CourseService.Domain.Entities.TheoryLessonItem", b =>
                {
                    b.HasBaseType("CourseService.Domain.Entities.LessonItem");

                    b.Property<bool>("IsGraded")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Attachment", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.LessonItem", "LessonItem")
                        .WithMany("Attachments")
                        .HasForeignKey("LessonItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CourseService.Domain.Entities.PracticalLessonItemSubmit", "PracticalLessonItemSubmit")
                        .WithMany("Attachments")
                        .HasForeignKey("PracticalLessonItemSubmitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("LessonItem");

                    b.Navigation("PracticalLessonItemSubmit");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseGroupCourse", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CourseService.Domain.Entities.CourseGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Course");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseTeacherCourse", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CourseService.Domain.Entities.CourseTeacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Course");

                    b.Navigation("Teacher");
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

            modelBuilder.Entity("CourseService.Domain.Entities.LessonItem", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.CourseTeacher", "Author")
                        .WithMany("LessonItems")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CourseService.Domain.Entities.Lesson", "Lesson")
                        .WithMany("LessonItems")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.PracticalLessonItemSubmit", b =>
                {
                    b.HasOne("CourseService.Domain.Entities.PracticalLessonItem", "PracticalLessonItem")
                        .WithMany("Submits")
                        .HasForeignKey("PracticalLessonItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PracticalLessonItem");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Course", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.CourseTeacher", b =>
                {
                    b.Navigation("LessonItems");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.Lesson", b =>
                {
                    b.Navigation("LessonItems");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.LessonItem", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.PracticalLessonItemSubmit", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("CourseService.Domain.Entities.PracticalLessonItem", b =>
                {
                    b.Navigation("Submits");
                });
#pragma warning restore 612, 618
        }
    }
}
