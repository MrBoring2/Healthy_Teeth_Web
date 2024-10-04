using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Configurators
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "Doctor",
                    NormalizedName = "DOCTOR"
                },
                new IdentityRole
                {
                    Name = "Registrator",
                    NormalizedName = "REGISTRATOR"
                }
            );
        }
    }
}