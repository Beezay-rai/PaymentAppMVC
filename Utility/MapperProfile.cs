using AutoMapper;
using PayementMVC.Data;
using PayementMVC.Models;

namespace PayementMVC.Utility
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {

            CreateMap<Balance, BalanceViewModel>().ReverseMap();
            CreateMap<Transaction, TransactionViewModel>().ReverseMap();
        }
    }
}
