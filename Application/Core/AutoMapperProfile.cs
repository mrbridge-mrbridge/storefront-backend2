using Application.DiscountAndPromotions;
using Application.Orders;
using Application.Page;
using Application.Photos;
using Application.Product;
using Application.Purchases;
using Application.ReviewReplies;
using Application.Reviews;
using Application.Shipping;
using Application.Store;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Domain.Product>();
            CreateMap<Domain.Product, ProductCreateParam>().ReverseMap();
            // CreateMap<Domain.Product, ProductDetail>()
            //     .ForMember(p =>p.Reviews, opt => opt.MapFrom(p => p.Reviews))
            //     .ForMember(p =>p.ProductPhotos, opt => opt.MapFrom(p => p.ProductPhotos));
                

            CreateMap<ProductPhoto, PhotoUploadResult>();
            CreateMap<PagePhoto, PhotoUploadResult>();


            CreateMap<Domain.Product, ProductDto>()
                .ForMember(pd => pd.ProductPhotos, opt => opt.MapFrom(p => p.ProductPhotos))
                .ForMember(p =>p.Reviews, opt => opt.MapFrom(p => p.Reviews))
                .ForMember(pd => pd.DefaultImage, opt => opt.MapFrom(p => p.ProductPhotos.Aggregate("", (url, next)=> next.Url != null? next.Url: "" )))
                .ForMember(pd => pd.StoreName, opt => opt.MapFrom(p => p.Store.StoreName));

            CreateMap<Domain.Store, StoreDto>();
            CreateMap<Domain.Store, GetStoreDto>()
                .ForMember(sd => sd.Pages, opt => opt.MapFrom(s => s.Pages));

            CreateMap<Domain.Page, PageDto>()
                .ForMember(
                    td => td.HeroImage,
                    opt =>
                        opt.MapFrom(
                            t =>
                                t.PagePhotos.Any()
                                    ? t.PagePhotos.First(tp => tp.Id == t.HeroImage).Url
                                    : ""
                        )
                )
                .ForMember(td => td.StoreName, opt => opt.MapFrom(t => t.Store.StoreName))
                .ForMember(
                    td => td.Logo,
                    opt =>
                        opt.MapFrom(
                            t =>
                                t.PagePhotos.Any()
                                    ? t.PagePhotos.First(tp => tp.Id == t.Logo).Url
                                    : ""
                        )
                );

            CreateMap<CreatePageParam, Domain.Page>();

            CreateMap<Purchase, PurchaseCreateParam>().ReverseMap();
            CreateMap<Purchase, PurchaseDto>()
                .ForMember(pd => pd.ProductDescription, opt => opt.MapFrom(p => p.Product.ProductName))
                .ForMember(pd => pd.QuantityRemaining, opt => opt.MapFrom(p => p.Product.Quantity))
                .ForMember(pd => pd.ProductDefaultImage, opt => opt.MapFrom(p => p.Product.ProductPhotos.Aggregate("", (url, next)=> next.Url != null? next.Url: "" )))
                .ForMember(pd => pd.PurchaseState, opt => opt.MapFrom(p => p.Order.OrderState))
                .ForMember(
                    pd => pd.DiscountAmount,
                    opt =>
                        opt.MapFrom(
                            p => (p.Product.UnitPrice * p.QuantityPurchased) * (p.Discount / 100)
                        )
                )
                .ForMember(
                    pd => pd.AmountDue,
                    opt =>
                        opt.MapFrom(
                            p =>
                                (p.Product.UnitPrice * p.QuantityPurchased)
                                * ((100 - p.Discount) / 100)
                        )
                )
                .ForMember(pd => pd.Order, opt => opt.MapFrom(p => p.Order.OrderId))
                .ForMember(
                    pd => pd.UnitOfMeasurement,
                    opt => opt.MapFrom(p => p.Product.UnitOfMeasurement)
                )
                .ForMember(pd => pd.CustomerId, opt => opt.MapFrom(p => p.Order.CustomerId));

            CreateMap<Order, OrderDto>()
                .ForMember(od => od.Purchases, opt => opt.MapFrom(o => o.Purchases))
                .ForMember(
                    od => od.TotalAmount,
                    opt =>
                        opt.MapFrom(
                            o =>
                                o.Purchases.Aggregate(
                                    (decimal)0,
                                    (purchase, next) =>
                                        purchase + (next.QuantityPurchased * next.Product.UnitPrice)
                                )
                        )
                )
                .ForMember(od => od.Customer, opt => opt.MapFrom(o => o.Customer.FirstName));

            //CreateMap<CreateCustomerParam, CreditCardDetail>()
            //             .ForMember(cc => cc.ExpiryMonth, opt => opt.MapFrom(c => c.Card.ExpiryMonth))
            //             .ForMember(cc => cc.ExpiryYear, opt => opt.MapFrom(c => c.Card.ExpiryYear))
            //             .ForMember(cc => cc.Cvc, opt => opt.MapFrom(c => c.Card.Cvc));

            CreateMap<ShippingDetails, ShippingParam>()
                .ReverseMap();
            CreateMap<ShippingDetails, ShippingDto>();

            CreateMap<CreateParam, Discount>();
            CreateMap<Discount, DiscountDto>()
                .ForMember(
                    dd => dd.Expires,
                    opt => opt.MapFrom(d => d.Expires.ToShortDateString())
                );

            CreateMap<CustomerReview, ReviewInternalDto>()
            .ForMember(r => r.Customer, opt => opt.MapFrom(cr => cr.Customer.FirstName));
            CreateMap<ReviewReply, ReplyDto>();

        }

    }
}
