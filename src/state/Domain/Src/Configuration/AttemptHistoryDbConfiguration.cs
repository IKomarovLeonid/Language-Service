﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Objects.Src.Models;
using Objects.Src.Primitives;

namespace Domain.Src.Configuration
{
    internal class AttemptHistoryDbConfiguration : IEntityTypeConfiguration<AttemptHistoryDto>
    {
        public void Configure(EntityTypeBuilder<AttemptHistoryDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.UserId).IsRequired().HasColumnName("user_id");
            builder.Property(t => t.TotalAttempts).IsRequired().HasColumnName("total_attempts");
            builder.Property(t => t.Category).IsRequired().HasColumnName("category").HasConversion(new EnumToStringConverter<WordCategory>());
            builder.Property(t => t.WordTypes).IsRequired().HasColumnName("words_types").HasConversion(new EnumToStringConverter<WordType>());
            builder.Property(t => t.CorrectAttempts).IsRequired().HasColumnName("correct_answers");
            builder.Property(t => t.TotalSeconds).IsRequired().HasColumnName("attempt_time_sec");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
