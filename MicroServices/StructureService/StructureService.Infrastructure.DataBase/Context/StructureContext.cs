﻿using Microsoft.EntityFrameworkCore;
using StructureService.Domain.Entities;

namespace StructureService.Infrastructure.DataBase.Context
{
    public class StructureContext : DbContext
    {
        public DbSet<PositionEntity> Positions { get; set; } = null!;
        public DbSet<DepartmentEntity> Departments { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;

        public StructureContext(DbContextOptions<StructureContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PositionEntity>()
                        .HasIndex(us => us.Name)
                        .IsUnique();

            modelBuilder.Entity<UserEntity>()
                        .HasIndex(us => us.Id)
                        .IsUnique();

            modelBuilder.Entity<DepartmentEntity>()
                        .HasIndex(us => us.Name)
                        .IsUnique();

            modelBuilder.Entity<DepartmentEntity>()
                        .HasIndex(us => us.CheifUserId)
                        .IsUnique();
        }
    }
}
