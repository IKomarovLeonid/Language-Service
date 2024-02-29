using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Src.Models;

namespace Domain.Src.Configuration
{
    internal class AttemptHistoryDbConfiguration : IEntityTypeConfiguration<AttemptHistoryDto>
    {
        public void Configure(EntityTypeBuilder<AttemptHistoryDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.UserId).IsRequired().HasColumnName("user_id");
            builder.Property(t => t.TotalWords).IsRequired().HasColumnName("total_words");
            builder.Property(t => t.CorrectAttempts).IsRequired().HasColumnName("correct_answers");
            builder.Property(t => t.TotalSeconds).IsRequired().HasColumnName("attempt_time_sec");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
