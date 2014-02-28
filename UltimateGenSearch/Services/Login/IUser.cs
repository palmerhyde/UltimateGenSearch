using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateGenSearch.Services.Login
{
    public interface IUser
    {
        IEnumerable<Account> GetAccounts();
    }
}
