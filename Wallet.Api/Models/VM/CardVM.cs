using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wallet.Api.Models.VM
{
    public class CardVM
    {
        public int CardId { get; set; }

        public string Number { get; set; }

        [JsonIgnore]
        public DateTime ExpirationDate { get; set; }

        //MM/YY
        public string ExpirationDateString => ExpirationDate.ToString("dd/MM");

        public string CCV { get; set; }

        public decimal Limit { get; set; }
        public decimal AvailableLimit { get; set; }


        public List<CardTransactionVM> TransactionsInfo { get; set; } = new List<CardTransactionVM>();
    }
}