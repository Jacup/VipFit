using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VipFit.DataAccessLayer;

namespace VipFit.Core.DataAccessLayer
{
    public class VipFitContextFactory : IDesignTimeDbContextFactory<VipFitContext>
    {
        public VipFitContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VipFitContext>();
            optionsBuilder.UseSqlite($@"Data Source=C:\Users\jacub\vipfit\db\mydb.db;");

            return new VipFitContext(optionsBuilder.Options);
        }
    }
}
