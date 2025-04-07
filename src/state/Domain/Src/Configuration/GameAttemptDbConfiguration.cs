using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Dto;

namespace Domain.Configuration
{
    internal class GameAttemptDbConfiguration : IEntityTypeConfiguration<GameAttemptDto>
    {
        public void Configure(EntityTypeBuilder<GameAttemptDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.UserId).IsRequired().HasColumnName("user_id");
            builder.Property(t => t.TotalAnswersCount).HasColumnName("total_answers");
            builder.Property(t => t.CorrectAnswersCount).HasColumnName("correct_answers");
            builder.Property(t => t.MaxStreak).HasColumnName("max_streak");
            builder.Property(t => t.UserRatingChange).HasColumnName("rating_change");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
