using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateGenSearch.Services
{
    using UltimateGenSearch.Services.Login;

    public interface IAccountService
    {
        IEnumerable<Account> GetAccounts();

        bool UpdateAccount(Account account);
    }
}
