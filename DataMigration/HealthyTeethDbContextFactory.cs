using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigration
{
    public class HealthyTeethDbContextFactory : IDesignTimeDbContextFactory<HealthyTeethDbContext>
    {
        public HealthyTeethDbContextFactory()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public HealthyTeethDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HealthyTeethDbContext>();

            const string connectionString = "Host=localhost;Port=5433;Database=healthy-teeth;Persist Security Info=True;User ID=postgres;Password=postgres";
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
            optionsBuilder.UseNpgsql(connectionString, options => options
                                    .CommandTimeout(1000)
                                    .MigrationsAssembly("DataMigration")
                                    .MigrationsHistoryTable("EntMigrations", "public"));
            return new HealthyTeethDbContext(optionsBuilder.Options);
        }

    }
}
