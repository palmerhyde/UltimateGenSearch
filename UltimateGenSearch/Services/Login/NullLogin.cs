﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateGenSearch.Services.Login
{
    using System.Net;

    public class NullLogin : ILogin
    {
        public void Login(CookieContainer cookies)
        {
        }
    }
}