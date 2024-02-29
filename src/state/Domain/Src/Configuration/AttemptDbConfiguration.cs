using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Src.Dto;

namespace Domain.Src.Configuration
{
    internal class AttemptDbConfiguration : IEntityTypeConfiguration<AttemptDto>
    {
        public void Configure(EntityTypeBuilder<AttemptDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.HistoryId).IsRequired().HasColumnName("history_id");
            builder.Property(t => t.IsCorrect).HasColumnName("is_correct");
            builder.Property(t => t.UserTranslation).HasColumnName("user_translation");
            builder.Property(t => t.ExpectedTranslation).HasColumnName("expected_trasnlation");
            builder.Property(t => t.Word).HasColumnName("word");

            builder.Property(t => t.TotalSeconds).IsRequired().HasColumnName("attempt_time_sec");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
