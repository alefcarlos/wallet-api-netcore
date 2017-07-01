using System;

namespace Wallet.Domain.Core.Exceptions
{
    /// <summary>
    /// </summary>
    public class ThereIsNoCardException : Exception
    {
        public ThereIsNoCardException() : base("You dont't have any card in your Wallet.") { }
    }
}