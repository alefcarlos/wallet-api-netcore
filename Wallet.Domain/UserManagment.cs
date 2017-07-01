using Wallet.Domain.Interfaces.User;
using Wallet.Domain.Models;

namespace Wallet.Domain
{
    public class UserManagment : IUserManagment
    {

        private LoggedUser _userField;
        public LoggedUser User => _userField;
        public bool IsLogged() => _userField != null;
        public void SetUser(WalletUser user)
        {
            _userField = new LoggedUser
            {
                WalletUserId = user.WalletUserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
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