﻿using System;
using System.Collections.Generic;
using System.Web;

namespace Rholiver.Site.Infrastructure
{
    public interface IUserProvider
    {
        User GetUser(string id);
    }
}