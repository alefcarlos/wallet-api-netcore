using Wallet.Domain.Models;

namespace Wallet.Domain.Interfaces.User
{
    public interface IUserManagment
    {
        WalletUser User {get;}
        void SetUser(WalletUser user);
        bool IsLogged();

        bool CanExecute(EUserManagmentRole requestedRole);

        bool IsAdmin();
    }
}