using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PocoDb;

namespace Rholiver.Site.Infrastructure
{
    public interface IUserProvider
    {
        User GetUser(string id);
    }

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