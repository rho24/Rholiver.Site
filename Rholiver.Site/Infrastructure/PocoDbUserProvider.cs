using System;
using System.Linq;
using PocoDb;

namespace Rholiver.Site.Infrastructure
{
    internal class PocoDbUserProvider : IUserProvider
    {
        readonly PocoDbClient _pocoDb;

        public PocoDbUserProvider(PocoDbClient pocoDb) {
            _pocoDb = pocoDb;
        }

        public User GetUser(string id) {
            using (var session = _pocoDb.BeginSession()) {
                return session.Get<User>().Where(u => u.Id == id).FirstOrDefault();
            }
        }
    }
}