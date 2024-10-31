using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Dto;

namespace Domain.Configuration
{
    internal class AttemptHistoryDbConfiguration : IEntityTypeConfiguration<AttemptHistoryDto>
    {
        public void Configure(EntityTypeBuilder<AttemptHistoryDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.TotalAttempts).IsRequired().HasColumnName("total_attempts");
            builder.Property(t => t.WordsErrors).HasColumnName("words_errors");
            builder.Property(t => t.AttemptAttributes).HasColumnName("attributes");
            builder.Property(t => t.CorrectAttempts).IsRequired().HasColumnName("correct_answers");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
