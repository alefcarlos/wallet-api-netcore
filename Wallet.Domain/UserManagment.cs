using Wallet.Domain.Interfaces.User;
using Wallet.Domain.Models;

namespace Wallet.Domain
{
    public class UserManagment : IUserManagment
    {

        private WalletUser _userField;
        public WalletUser User => _userField;
        public bool IsLogged() => _userField != null;
        public void SetUser(WalletUser user)
        {
            _userField = user;
        }

        public bool IsAdmin() => User.Role == EUserManagmentRole.Admin;

        public bool CanExecute(EUserManagmentRole requestedRole)
        {
            if (!IsLogged()) return false;
            
            if (IsAdmin()) return true;
            //if (User.Role == EUserManagmentRole.ReadAndWrite) return true;

            return (User.Role == requestedRole);
        }
    }
}