using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid ProduceListingId { get; private set; }
        public ProduceListing ProduceListing { get; private set; } = null!;
        public Guid BuyerProfileId { get; private set; }
        public BuyerProfile BuyerProfile { get; private set; } = null!;
        public decimal Quantity { get; private set; }
        public ReservationStatus Status { get; private set; }

        private Reservation() { }
        public Reservation(Guid listingId, Guid buyerProfileId, decimal quantity)
        {
            if (listingId == Guid.Empty || buyerProfileId == Guid.Empty) throw new ArgumentException("Listing and buyer are required.");
            if (quantity <= 0) throw new ArgumentException("Reservation quantity must be greater than zero.");
            ProduceListingId = listingId;
            BuyerProfileId = buyerProfileId;
            Quantity = quantity;
            Status = ReservationStatus.Pending;
        }

        public void Accept()
        {
            if (Status != ReservationStatus.Pending) throw new InvalidOperationException("Only pending reservations can be accepted.");
            Status = ReservationStatus.Accepted;
            MarkAsUpdated();
        }

        public void Reject()
        {
            if (Status != ReservationStatus.Pending) throw new InvalidOperationException("Only pending reservations can be rejected.");
            Status = ReservationStatus.Rejected;
            MarkAsUpdated();
        }

        public void Cancel()
        {
            if (Status != ReservationStatus.Pending && Status != ReservationStatus.Accepted)
                throw new InvalidOperationException("This reservation cannot be cancelled.");
            Status = ReservationStatus.Cancelled;
            MarkAsUpdated();
        }
    }
}
