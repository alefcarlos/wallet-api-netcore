using System;

namespace Wallet.Domain.Core.Exceptions
{
    /// <summary>
    /// Default exception for Not Found record.
    /// It is used when a record wasn't found in the Databse
    /// </summary>
    public class ThereIsNoLimitEnough : Exception
    {
        public ThereIsNoLimitEnough() : base("You dont have limit enough to make this purchase.") { }
    }
}