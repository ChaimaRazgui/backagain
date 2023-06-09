﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable($"{nameof(User)}s");
        builder.HasKey(x => x.UserId);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.PasswordSalt)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.Active)
            .IsRequired()
            .HasDefaultValue(false);
    }
}