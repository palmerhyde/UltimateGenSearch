using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateGenSearch.Services.Login
{
    public class NullLogin : ILogin
    {
        public object Login(string username, string password)
        {
            return null;
        }
    }
}