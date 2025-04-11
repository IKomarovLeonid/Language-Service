using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Src.Dto;

namespace Domain.Src.Configuration
{
    class UserStatisticsDbConfiguration : IEntityTypeConfiguration<UserStatisticsDto>
    {
        public void Configure(EntityTypeBuilder<UserStatisticsDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.UserId).IsRequired().HasColumnName("user_id");
            builder.Property(t => t.UserRating).HasColumnName("user_rating");
            builder.Property(t => t.TotalAttempts).HasColumnName("total_attempts");
            builder.Property(t => t.ErrorAttempts).HasColumnName("error_attempts");
            builder.Property(t => t.LastAttempt).IsRequired().HasColumnName("last_time_utc");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
