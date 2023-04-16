using VipFit.Core.Models;

namespace VipFit.Database
{
    public static class DbInitializer
    {
        // TODO: it's not working/used yet.
        public static void Initialize(VipFitContext context) 
        {
            context.Database.EnsureCreated();

            if (context.Clients.Any())
                return; // db has beed seeded

            var clients = new Client[]
            {
                new Client("Janusz", "Kowalski", "12456789", "email@gmail.com"),
            };

            foreach (Client c in clients)
                context.Clients.Add(c);

            context.SaveChanges();
        }
    }
}
