using Microsoft.EntityFrameworkCore;
using VipFit.Core.Models;

namespace VipFit.Database
{
    /// <summary>
    /// Communication with Clients database using EF Core.
    /// </summary>
    public class ClientRepository
    {
        private readonly VipFitContext db;

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
