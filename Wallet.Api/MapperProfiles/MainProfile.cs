using Wallet.Api.Models.VM;
using AutoMapper;
using Wallet.Domain.Models;

namespace Wallet.Api.MapperProfiles
{
    /// <summary>
    /// A mapper configuration for AutoMaper
    /// </summary>
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            //WalletUser to WalletUserVM, and vice-versa
            CreateMap<WalletUser, WalletUserVM>().ReverseMap();

            //Card to CardVM, and vice-versa
            CreateMap<Card, CardVM>().ReverseMap();
        }
    }
}
