using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Objects;
using Objects.Dto;

namespace Domain.Configuration
{
    internal class WordsDbConfiguration : IEntityTypeConfiguration<WordDto>
    {
        public void Configure(EntityTypeBuilder<WordDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.Word).HasColumnName("word");
            builder.Property(t => t.Attributes).HasColumnName("attributes");
            builder.Property(t => t.Translation).IsRequired().HasColumnName("translation");
            builder.Property(t => t.Conjugation).HasColumnName("conjugation");
            builder.Property(t => t.LanguageType).HasColumnName("language_type").HasConversion(new EnumToStringConverter<WordLanguageType>());
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
