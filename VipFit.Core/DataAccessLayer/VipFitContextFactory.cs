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
            string appDirectory = VipFitContext.GetApplicationDirectory();

            var optionsBuilder = new DbContextOptionsBuilder<VipFitContext>();
            optionsBuilder
                .UseSqlite($@"Data Source={appDirectory}\vf_db.db;");

            return new VipFitContext(optionsBuilder.Options);
        }
    }
}
