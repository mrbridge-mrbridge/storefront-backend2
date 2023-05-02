namespace Application.Stripe
{
	public record CustomerResource
    (
    string CustomerStripeId, 
    string Email, 
    string Name
    );
}
