using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace Rholiver.Site.Infrastructure
{
    public class InMemoryUserProvider:IUserProvider
    {
        public ICollection<User> Users { get; set; }

        public InMemoryUserProvider(IEnumerable<User> users)
        {
            Users = new List<User>(users);
        }

        public User GetUser(string id)
        {
            return Users.Where(u => u.Id == id).FirstOrDefault();
        }
    }
}