using AutoMapper;
using RocketFinApp.Models;
using RocketFinDomain;
using RocketFinDomain.Entities;
using RocketFinDomain.Requests;
using RocketFinDomain.Responses;

namespace RocketFinApi.Core.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {
            CreateMap<Security, InstrumentResponse>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.shortName))
           .ForMember(dest => dest.Bid, opt => opt.MapFrom(src => src.bid))
           .ForMember(dest => dest.Ask, opt => opt.MapFrom(src => src.ask))
           .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.regularMarketPrice))
           .ForMember(dest => dest.ChangeValue, opt => opt.MapFrom(src => src.regularMarketChange))
           .ForMember(dest => dest.ChangePercentage, opt => opt.MapFrom(src => src.regularMarketChangePercent));

            CreateMap<Trade, TransactionResponse>()
                .ForMember(dest => dest.Operation, opt => opt.MapFrom(src => src.TradeType.ToString()));

            CreateMap<Trade, TransactionSummaryResponse>()
                .ForMember(dest => dest.Transaction, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.Operation, opt => opt.MapFrom(src => src.TradeType.ToString()));

            CreateMap<(Holding holding, InstrumentResponse instrument), PortfolioResponse>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.instrument.Symbol))
                .ForMember(dest => dest.MarketValue, opt => opt.MapFrom(src => src.holding.Quantity * src.instrument.CurrentPrice))
                .ForMember(dest => dest.TotalShares, opt => opt.MapFrom(src => src.holding.TotalShares))
                .ForMember(dest => dest.CostBasis, opt => opt.MapFrom(src => src.holding.CostBasis))
                .ForMember(dest => dest.UnrealizedReturnRate, opt => opt.MapFrom(src => src.holding.CostBasis == 0 ? 0 : ((src.holding.Quantity * src.instrument.CurrentPrice - src.holding.CostBasis) / src.holding.CostBasis) * 100))
                .ForMember(dest => dest.UnrealizedProfitLoss, opt => opt.MapFrom(src => (src.holding.Quantity * src.instrument.CurrentPrice) - src.holding.CostBasis));
               
           CreateMap<BuyInstrumentRequest, Trade>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.PriceAtTransaction, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.TradeType, opt => opt.MapFrom(src => TradeType.Buy));

            CreateMap<SellInstrumentRequest, Trade>()
               .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => -src.Quantity))
               .ForMember(dest => dest.PriceAtTransaction, opt => opt.Ignore())
               .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
               .ForMember(dest => dest.TradeType, opt => opt.MapFrom(src => TradeType.Sell));


        }

    }
}
