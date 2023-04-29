namespace VipFit.Core.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    /// <summary>
    /// Context factory.
    /// </summary>
    public class VipFitContextFactory : IDesignTimeDbContextFactory<VipFitContext>
    {
        /// <summary>
        /// Creates new DB.
        /// </summary>
        /// <param name="args">Args.</param>
        /// <returns>Created VipFit context.</returns>
        public VipFitContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VipFitContext>();
            optionsBuilder.UseSqlite($@"Data Source=C:\Users\jacub\vipfit\db\mydb.db;");

            return new VipFitContext(optionsBuilder.Options);
        }
    }
}
