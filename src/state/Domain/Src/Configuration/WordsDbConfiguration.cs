using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Objects.Src.Dto;
using Objects.Src.Primitives;

namespace Domain.Src.Configuration
{
    internal class WordsDbConfiguration : IEntityTypeConfiguration<WordDto>
    {
        public void Configure(EntityTypeBuilder<WordDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.Category).IsRequired().HasColumnName("category").HasConversion(new EnumToStringConverter<WordCategory>());
            builder.Property(t => t.Type).IsRequired().HasColumnName("type").HasConversion(new EnumToStringConverter<WordType>());
            builder.Property(t => t.Language).IsRequired().HasColumnName("language").HasConversion(new EnumToStringConverter<LanguageType>());
            builder.Property(t => t.Word).HasColumnName("word");
            builder.Property(t => t.Translation).IsRequired().HasColumnName("translation");
            builder.Property(t => t.Conjugation).HasColumnName("conjugation");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
