using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealRept.Data
{


    public class RoleConfiguration:IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "ContractStaff",
                    NormalizedName = "ContractStaff".ToUpper()
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "Administrator".ToUpper()
                },
                new IdentityRole
                {
                    Name = "BranchStaff",
                    NormalizedName = "BranchStaff".ToUpper()
                },
                new IdentityRole
                {
                    Name = "JustRegistered",
                    NormalizedName = "JustRegistered".ToUpper()

                },
                new IdentityRole
                {
                    Name = "Suspended",
                    NormalizedName = "Suspended".ToUpper()
                }
                );

        }
    }
}
