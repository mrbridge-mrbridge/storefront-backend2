namespace Application.Orders
{
	public record OrderCancellationDto(Guid OrderId, Guid ProductId);
	
}
