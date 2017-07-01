using Wallet.Api.Models.VM;
using AutoMapper;
using Wallet.Domain.Models;
using Wallet.Api.Mapper.Resolvers;

namespace Wallet.Api.Mapper.MapperProfiles
{
    /// <summary>
    /// A mapper configuration for AutoMaper
    /// </summary>
    public class MainProfile : Profile
    {

        public MainProfile()
        {
            //WalletUser to WalletUserVM, and vice-versa
            CreateMap<WalletUser, WalletUserVM>().ConvertUsing(new WalletUserInfoResolver());

            //Card to CardVM, and vice-versa
            CreateMap<Card, CardVM>().ReverseMap();

            //CardTransaction to CardTransactionVM, and vice-versa
            CreateMap<CardTransaction, CardTransactionVM>().ReverseMap();

            CreateMap<NewCardTransactionInfoVM, CardTransaction>();
        }
    }
}
