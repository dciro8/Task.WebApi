using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Task.DataInfrastructure;
using Task.Domain.Entities;
using Task.Utilities.Encrypt;
using Task.Utilities.Enums;

namespace Task.DataInfrastructure.Context;

public partial class TaskDbContext : DbContext
{
    public TaskDbContext()
    {
    }
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<TASK> TASK { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TASK>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date_Created).HasColumnType("datetime");
            entity.Property(e => e.Date_Updated).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Developer).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(8);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string key = Params.KeyEncrypt;

        string connectionString = Encrypt.DecryptString(key, Params.ConecctionString);

        optionsBuilder.UseSqlServer(connectionString);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
