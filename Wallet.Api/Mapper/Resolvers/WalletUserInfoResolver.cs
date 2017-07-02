using System;
using AutoMapper;

namespace Wallet.Api.Mapper.Resolvers
{
    using System.Collections.Generic;
    using System.Linq;
    using Wallet.Api.Models.VM;
    using Wallet.Domain.Models;

    public class WalletUserInfoResolver : ITypeConverter<WalletUser, WalletUserVM>
    {
        public WalletUserVM Convert(WalletUser source, WalletUserVM destination, ResolutionContext context)
        {
            var info = new WalletUserVM
            {
                WalletUserId = source.WalletUserId,
                Name = source.Name,
                Email = source.Email,
                RealLimit = source.RealLimit
            };

            // Cards info
            // var cardsInfo = _mapper.Map<IEnumerable<Card>, IEnumerable<CardVM>>(source.Cards);
            info.CardsInfo.AddRange(source.Cards.OrderByDescending(x => x.ExpirationDate).Select(c => new CardVM
            {
                CardId = c.CardId,
                Number = c.Number,
                CCV = c.CCV,
                ExpirationDate = c.ExpirationDate,
                Limit = c.Limit,
                AvailableLimit = c.AvailableLimit,
  
                TransactionsInfo = c.Transactions.OrderByDescending(x => x.Date).Select(t => new CardTransactionVM
                {
                    CardTransactionId = t.CardTransactionId,
                    Date = t.Date,
                    Description = t.Description,
                    Value = t.Value,
                    Type = t.Type
                }).ToList()
            }));

            return info;
        }
    }
}