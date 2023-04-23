namespace VipFit.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;
    using VipFit.Core.Models;

    /// <summary>
    /// Communication with Clients database using EF Core.
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly VipFitContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="db">Client repository.</param>
        public ClientRepository(VipFitContext db) => this.db = db;

        /// <inheritdoc/>
        public async Task<IEnumerable<Client>> GetAsync() => await db.Clients.AsNoTracking().ToListAsync();

        /// <inheritdoc/>
        public async Task<Client?> GetAsync(Guid id) => await db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        /// <inheritdoc/>
        public async Task<IEnumerable<Client>> GetAsync(string args)
        {
            string[] parameters = args.Split(' ');
            return await db.Clients
                .Where(c =>
                    parameters.Any(parameter =>
                        c.FirstName.StartsWith(parameter) ||
                        c.LastName.StartsWith(parameter) ||
                        (c.Email != null && c.Email.StartsWith(parameter)) ||
                        (c.Phone != null && c.Phone.StartsWith(parameter))))
                .OrderByDescending(c =>
                    parameters.Count(parameter =>
                        c.FirstName.StartsWith(parameter) ||
                        c.LastName.StartsWith(parameter) ||
                        (c.Email != null && c.Email.StartsWith(parameter)) ||
                        (c.Phone != null && c.Phone.StartsWith(parameter))))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var client = await db.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (client != null)
            {
                // Remove dependencies? Passes and Payments.
                db.Clients.Remove(client);
                await db.SaveChangesAsync();
            }
        }
    }
}
