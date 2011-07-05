using System;

namespace Rholiver.Site.Infrastructure
{
    public interface IUserProvider
    {
        User GetUser(string id);
    }
}