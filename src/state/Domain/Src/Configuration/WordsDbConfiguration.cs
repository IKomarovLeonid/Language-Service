using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Src.Dto;

namespace Domain.Src.Configuration
{
    internal class WordsDbConfiguration : IEntityTypeConfiguration<WordDto>
    {
        public void Configure(EntityTypeBuilder<WordDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.Word).HasColumnName("word");
            builder.Property(t => t.Attributes).IsRequired().HasColumnName("attributes");
            builder.Property(t => t.Translation).IsRequired().HasColumnName("translation");
            builder.Property(t => t.Conjugation).HasColumnName("conjugation");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
