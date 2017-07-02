using System;

namespace Wallet.Domain.Core.Exceptions
{
    /// <summary>
    /// Default exception for Not Found record.
    /// It is used when a record wasn't found in the Databse
    /// </summary>
    public class ThereIsNoEnoughLimit : Exception
    {
        public ThereIsNoEnoughLimit() : base("You dont have enough limit to make this purchase.") { }
    }
}