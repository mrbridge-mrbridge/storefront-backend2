using Application.ReviewReplies;
using Domain;

namespace Application.Reviews
{
    public class ReviewInternalDto : ReviewAbstract
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public ReplyDto ReviewReply { get; set; } = null;
    }
}