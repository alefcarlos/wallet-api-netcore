using System;

namespace Wallet.Domain.Core.Exceptions
{
    /// <summary>
    /// </summary>
    public class ThereIsNoCardAvailableException : Exception
    {
        public ThereIsNoCardAvailableException() : base("You dont't have any card in your Wallet whose limit is avaliable.") { }
    }
}