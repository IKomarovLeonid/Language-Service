using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Objects.Dto;
using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Objects.Src.Dto;

namespace Domain.Src.Configuration
{
    class UsersDbConfiguration : IEntityTypeConfiguration<UserDto>
    {
        public void Configure(EntityTypeBuilder<UserDto> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.IsAdmin).IsRequired().HasColumnName("is_admin");
            builder.Property(t => t.Email).IsRequired().HasColumnName("email");
            builder.Property(t => t.UserName).IsRequired().HasColumnName("user_name");
            builder.Property(t => t.CreatedTime).IsRequired().HasColumnName("created_utc");
            builder.Property(t => t.UpdatedTime).IsRequired().HasColumnName("updated_utc");
        }
    }
}
