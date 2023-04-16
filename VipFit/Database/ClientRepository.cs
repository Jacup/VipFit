namespace VipFit.Database
{
    using Microsoft.EntityFrameworkCore;
    using VipFit.Core.Models;

    /// <summary>
    /// Communication with Clients database using EF Core.
    /// </summary>
    public class ClientRepository
    {
        private readonly VipFitContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="db">Client repository.</param>
        public ClientRepository(VipFitContext db) => this.db = db;

        public async Task<IEnumerable<Client>> GetAsync() => await db.Clients.AsNoTracking().ToListAsync();

        public async Task<Client> GetAsync(Guid id) => await db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Client> UpsertAsync(Client client)
        {
            var current = await db.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);

            if (current == null)
                db.Add(client);
            else
                db.Entry(current).CurrentValues.SetValues(client);

            await db.SaveChangesAsync();
            return client;
        }

    }
}
