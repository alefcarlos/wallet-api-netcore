using System;
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

        public int CCV { get; set; }

        public decimal Limit { get; set; }
    }
}