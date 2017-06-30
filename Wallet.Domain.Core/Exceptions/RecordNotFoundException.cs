using System;

namespace Wallet.Domain.Core.Exceptions
{
    /// <summary>
    /// Default exception for Not Found record.
    /// It is used when a record wasn't found in the Databse
    /// </summary>
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base("Registro não encontrado") { }
    }
}