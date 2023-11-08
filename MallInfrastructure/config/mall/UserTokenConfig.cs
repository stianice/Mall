﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MallDomain.entity.mall
{
    internal class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> entity)
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("tb_newbee_mall_user_token")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Token, "uq_token").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasComment("用户主键id")
                .HasColumnName("user_id");
            entity.Property(e => e.ExpireTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("token过期时间")
                .HasColumnType("datetime")
                .HasColumnName("expire_time");
            entity.Property(e => e.Token)
                .HasMaxLength(300)
                .HasComment("token值(300位字符串)")
                .HasColumnName("token");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("修改时间")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        }
    }
}
