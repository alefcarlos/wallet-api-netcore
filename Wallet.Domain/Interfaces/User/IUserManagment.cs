using Wallet.Domain.Models;

namespace Wallet.Domain.Interfaces.User
{
    public interface IUserManagment
    {
        LoggedUser User { get; }
        void SetUser(WalletUser user);
        bool IsLogged();

        bool CanExecute(EUserManagmentRole requestedRole);

        bool IsAdmin();
    }

    public class LoggedUser
    {
        public int WalletUserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public EUserManagmentRole Role { get; set; }
    }
}