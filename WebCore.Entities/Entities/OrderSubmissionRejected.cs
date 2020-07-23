using System;

namespace WebCore.Entities.Entities
{
    public interface OrderSubmissionRejected
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }

        string CustomerNumber { get; }
        string Reason { get; }
    }
}
