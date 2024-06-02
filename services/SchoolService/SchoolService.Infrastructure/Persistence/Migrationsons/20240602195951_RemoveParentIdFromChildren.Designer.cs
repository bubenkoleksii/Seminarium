﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SchoolService.Infrastructure.Persistence;

#nullable disable

namespace SchoolService.Infrastructure.Persistence.Migrationsons
{
    [DbContext(typeof(CommandContext))]
    [Migration("20240602195951_RemoveParentIdFromChildren")]
    partial class RemoveParentIdFromChildren
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ParentChild", b =>
                {
                    b.Property<Guid>("ChildId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uuid");

                    b.HasKey("ChildId", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("ParentChild", "public");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClassTeacherId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Img")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

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

                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid");

                    b.Property<byte>("StudyPeriodNumber")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("ClassTeacherId")
                        .IsUnique();

                    b.HasIndex("SchoolId");

                    b.ToTable("Groups", "public");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.JoiningRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<bool>("AreOccupied")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("GradingSystem")
                        .HasColumnType("bigint");

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

                    b.Property<int>("OwnershipType")
                        .HasColumnType("integer");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Region")
                        .HasColumnType("integer");

                    b.Property<string>("RegisterCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RequesterEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RequesterFullName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("RequesterPhone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid");

                    b.Property<string>("ShortName")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<long>("StudentsQuantity")
                        .HasColumnType("bigint");

                    b.Property<string>("TerritorialCommunity")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId")
                        .IsUnique();

                    b.ToTable("JoiningRequests", "public");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.School", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<bool>("AreOccupied")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("GradingSystem")
                        .HasColumnType("bigint");

                    b.Property<string>("Img")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<Guid>("JoiningRequestId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastArchivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("OwnershipType")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Region")
                        .HasColumnType("integer");

                    b.Property<string>("RegisterCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ShortName")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("SiteUrl")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("StudentsQuantity")
                        .HasColumnType("bigint");

                    b.Property<string>("TerritorialCommunity")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Schools", "public");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.SchoolProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClassTeacherGroupId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Details")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Email")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("Img")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

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

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SchoolId");

                    b.ToTable("SchoolProfiles", "public");
                });

            modelBuilder.Entity("ParentChild", b =>
                {
                    b.HasOne("SchoolService.Domain.Entities.SchoolProfile", null)
                        .WithMany()
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("SchoolService.Domain.Entities.SchoolProfile", null)
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.Group", b =>
                {
                    b.HasOne("SchoolService.Domain.Entities.SchoolProfile", "ClassTeacher")
                        .WithOne("ClassTeacherGroup")
                        .HasForeignKey("SchoolService.Domain.Entities.Group", "ClassTeacherId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SchoolService.Domain.Entities.School", "School")
                        .WithMany("Groups")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassTeacher");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.JoiningRequest", b =>
                {
                    b.HasOne("SchoolService.Domain.Entities.School", "School")
                        .WithOne("JoiningRequest")
                        .HasForeignKey("SchoolService.Domain.Entities.JoiningRequest", "SchoolId");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.SchoolProfile", b =>
                {
                    b.HasOne("SchoolService.Domain.Entities.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SchoolService.Domain.Entities.School", "School")
                        .WithMany("Teachers")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Group");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.Group", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.School", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("JoiningRequest")
                        .IsRequired();

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("SchoolService.Domain.Entities.SchoolProfile", b =>
                {
                    b.Navigation("ClassTeacherGroup");
                });
#pragma warning restore 612, 618
        }
    }
}
