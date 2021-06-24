using System.Collections.Generic;
using Catalog.API.Model;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void Seed(IMongoCollection<User> users)
        {
            var documents = new List<User>()
            {
                new()
                {
                    Name = "Fariz",
                    Surname = "Huseynov",
                    Age = 28
                },
                new ()
                {
                    Name = "Murtuz",
                    Surname = "Huseynov",
                    Age = 30
                },
                new()
                {
                    Name = "Shamama",
                    Surname = "Aliyeva",
                    Age = 51
                }
            };
            if (!users.Find(f => true).Any())
                users.InsertManyAsync(documents);

        }
    }
}
