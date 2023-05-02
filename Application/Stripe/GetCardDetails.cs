using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Stripe
{
    public class GetCardDetails
    {
        public class Query : IRequest<Result<CustomerResource>>
        {
            public Guid CustomerId { get; set; }
            public Guid StoreId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CustomerResource>>
        {
            private readonly AppDataContext _context;
            private readonly IStripeService _stripeService;
            private readonly IMapper _mapper;

            public Handler(AppDataContext context, IStripeService stripeService, IMapper mapper)
            {
                _context = context;
                _stripeService = stripeService;
                _mapper = mapper;
            }

            public async Task<Result<CustomerResource>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                if (request.StoreId != Guid.Empty)
                {
                    var individualStoreCardDetail = await _context.CreditCardDetails.FindAsync(
                        request.CustomerId,
                        request.StoreId
                    );

                    if (individualStoreCardDetail == null)
                    {
                        return Result<CustomerResource>.Failure("The customer has no details yet");
                    }

                    var CardReource = _mapper.Map<CustomerResource>(individualStoreCardDetail);
                    return Result<CustomerResource>.Success(CardReource);
                }

                // Since we treating merchant card details as one for now
                var response = _context.CreditCardDetails.First(
                    c => c.CustomerId == request.CustomerId
                );

                if (response == null)
                {
                    return Result<CustomerResource>.Failure("The customer has no details yet");
                }
                var cardDetails = _mapper.Map<CustomerResource>(response);

                return Result<CustomerResource>.Success(cardDetails);
            }
        }
    }
}
