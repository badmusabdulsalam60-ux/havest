using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enum
{
    public enum OrderStatus
    {
        Confirmed = 1, Preparing = 2, ReadyForPickup = 3, Delivered = 4, Completed = 5, Cancelled = 6
    }
}
