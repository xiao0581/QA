using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core;

namespace HotelBookingSpecs.Fakes
{
    public class FakeBookingRepository : IRepository<Booking>
    {
        private List<Booking> _bookings;

        public FakeBookingRepository(List<Booking> bookings)
        {
            _bookings = bookings;
        }

        public Task<IEnumerable<Booking>> GetAllAsync()
        {
            return Task.FromResult(_bookings.AsEnumerable());
        }

        public Task<Booking> GetAsync(int id)
        {
            return Task.FromResult(_bookings.FirstOrDefault());
        }

        public Task AddAsync(Booking booking)
        {
            _bookings.Add(booking);
            return Task.CompletedTask;
        }

        public Task EditAsync(Booking booking)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAsync(int id)
        {
            return Task.CompletedTask;
        }
    }
}