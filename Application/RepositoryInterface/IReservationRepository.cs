using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RepositoryInterface
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<Reservation?> GetByIdWithDetailsAsync(Guid id);
        IQueryable<Reservation> ForFarmer(Guid farmerProfileId);
        IQueryable<Reservation> ForBuyer(Guid buyerProfileId);
        Task<bool> HasOrderAsync(Guid reservationId);
    }
}
