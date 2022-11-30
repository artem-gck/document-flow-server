﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskCrudService.Adapters.DataSource.Context;

#nullable disable

namespace TaskCrudService.Adapters.DataSource.Migrations
{
    [DbContext(typeof(TaskContext))]
    [Migration("20221129065052_AddStatusField")]
    partial class AddStatusField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TaskCrudService.Domain.Entities.ArgumentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArgumentTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArgumentTypeId");

                    b.HasIndex("TaskId");

                    b.ToTable("Arguments");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.ArgumentTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ArgumentTypes");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeadLine")
                        .HasColumnType("datetime2");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.TypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Types");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.ArgumentEntity", b =>
                {
                    b.HasOne("TaskCrudService.Domain.Entities.ArgumentTypeEntity", "ArgumentType")
                        .WithMany("Arguments")
                        .HasForeignKey("ArgumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskCrudService.Domain.Entities.TaskEntity", "Task")
                        .WithMany("Arguments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArgumentType");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.TaskEntity", b =>
                {
                    b.HasOne("TaskCrudService.Domain.Entities.TypeEntity", "Type")
                        .WithMany("Tasks")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.ArgumentTypeEntity", b =>
                {
                    b.Navigation("Arguments");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.TaskEntity", b =>
                {
                    b.Navigation("Arguments");
                });

            modelBuilder.Entity("TaskCrudService.Domain.Entities.TypeEntity", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
