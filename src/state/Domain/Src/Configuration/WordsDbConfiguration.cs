using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Objects.Src;
using Objects.Src.Primitives;

namespace Domain.Src.Configuration
{
    internal class WordsDbConfiguration : IEntityTypeConfiguration<WordDto>
    {
        public void Configure(EntityTypeBuilder<WordDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.Type).IsRequired().HasColumnName("type").HasConversion(new EnumToStringConverter<WordType>());
            builder.Property(t => t.LanguageFrom).IsRequired().HasColumnName("language_from").HasConversion(new EnumToStringConverter<LanguageType>());
            builder.Property(t => t.LanguageTo).IsRequired().HasColumnName("language_to").HasConversion(new EnumToStringConverter<LanguageType>());
            builder.Property(t => t.Word).HasColumnName("word");
            builder.Property(t => t.Translation).HasColumnName("translation");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
