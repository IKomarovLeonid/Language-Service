using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Objects.Dto;
using Objects;
using Objects.Src.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Src.Configuration
{
    class WordStatisticsDbConfiguration : IEntityTypeConfiguration<WordStatisticsDto>
    {
        public void Configure(EntityTypeBuilder<WordStatisticsDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.TotalAnswersCount).HasColumnName("total_answers");
            builder.Property(t => t.CorrectAnswersTotal).HasColumnName("correct_answers");
            builder.Property(t => t.UserId).IsRequired().HasColumnName("user_id");
            builder.Property(t => t.WordId).IsRequired().HasColumnName("word_id");

            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
