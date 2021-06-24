using Catalog.API.Data;
using Catalog.API.Model;
using Microsoft.Extensions.Options;

namespace Catalog.API.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {

        }
    }
}
