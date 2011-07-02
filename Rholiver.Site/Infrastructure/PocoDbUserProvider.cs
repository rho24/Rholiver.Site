using System.Linq;
using PocoDb;

namespace Rholiver.Site.Infrastructure
{
    class PocoDbUserProvider : IUserProvider
    {
        readonly PocoDbClient _pocoDb;

        public PocoDbUserProvider(PocoDbClient pocoDb)
        {
            _pocoDb = pocoDb;
        }

        public User GetUser(string id)
        {
            using (var session = _pocoDb.BeginSession())
            {
                return session.Get<User>().FirstOrDefault();
            }
        }
    }
}