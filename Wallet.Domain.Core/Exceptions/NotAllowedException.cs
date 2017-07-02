using System;

namespace Wallet.Domain.Core.Exceptions
{
    /// <summary>
    /// Default exception for Not allowed action
    /// It is used when the user doesn't have the permission to execute an action
    /// </summary>
    public class NotAllowedException : Exception
    {
        public NotAllowedException() : base("You can not make this action.") { }
    }
}