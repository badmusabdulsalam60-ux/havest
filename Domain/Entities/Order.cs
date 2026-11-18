using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid ReservationId { get; private set; }
        public Reservation Reservation { get; private set; } = null!;
        public string OrderNumber { get; private set; } = string.Empty;
        public OrderStatus Status { get; private set; }

        private Order() { }
        public Order(Guid reservationId)
        {
            if (reservationId == Guid.Empty) throw new ArgumentException("Reservation is required.");
            ReservationId = reservationId;
            OrderNumber = $"AG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
            Status = OrderStatus.Confirmed;
        }

        public void UpdateStatus(OrderStatus status)
        {
            if (!System.Enum.IsDefined(typeof(OrderStatus), status))
            {
                throw new ArgumentException("Invalid order status.");
            }

            if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("A completed or cancelled order cannot be changed.");
            Status = status;
            MarkAsUpdated();
        }
    }
}
