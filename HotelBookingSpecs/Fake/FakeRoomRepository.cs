using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core;

namespace HotelBookingSpecs.Fakes
{
    public class FakeRoomRepository : IRepository<Room>
    {
        private List<Room> _rooms;

        public FakeRoomRepository(List<Room> rooms)
        {
            _rooms = rooms;
        }

        public Task<IEnumerable<Room>> GetAllAsync()
        {
            return Task.FromResult(_rooms.AsEnumerable());
        }

        public Task<Room> GetAsync(int id)
        {
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }

        public Task AddAsync(Room room)
        {
            _rooms.Add(room);
            return Task.CompletedTask;
        }

        public Task EditAsync(Room room)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAsync(int id)
        {
            return Task.CompletedTask;
        }
    }
}