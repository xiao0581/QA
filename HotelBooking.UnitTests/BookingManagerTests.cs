using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        //private IBookingManager bookingManager;
        //IRepository<Booking> bookingRepository;

        //public BookingManagerTests(){
        //    DateTime start = DateTime.Today.AddDays(10);
        //    DateTime end = DateTime.Today.AddDays(20);
        //    bookingRepository = new FakeBookingRepository(start, end);
        //    IRepository<Room> roomRepository = new FakeRoomRepository();
        //    bookingManager = new BookingManager(bookingRepository, roomRepository);
        //}

        //[Fact]
        //public async Task FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        //{
        //    // Arrange
        //    DateTime date = DateTime.Today;

        //    // Act
        //    Task result() => bookingManager.FindAvailableRoom(date, date);

        //    // Assert
        //    await Assert.ThrowsAsync<ArgumentException>(result);
        //}

        //[Fact]
        //public async Task FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        //{
        //    // Arrange
        //    DateTime date = DateTime.Today.AddDays(1);
        //    // Act
        //    int roomId = await bookingManager.FindAvailableRoom(date, date);
        //    // Assert
        //    Assert.NotEqual(-1, roomId);
        //}

        //[Fact]
        //public async Task FindAvailableRoom_RoomAvailable_ReturnsAvailableRoom()
        //{
        //    // This test was added to satisfy the following test design
        //    // principle: "Tests should have strong assertions".

        //    // Arrange
        //    DateTime date = DateTime.Today.AddDays(1);
            
        //    // Act
        //    int roomId = await bookingManager.FindAvailableRoom(date, date);

        //    var bookingForReturnedRoomId = (await bookingRepository.GetAllAsync()).
        //        Where(b => b.RoomId == roomId
        //                   && b.StartDate <= date
        //                   && b.EndDate >= date
        //                   && b.IsActive);
            
        //    // Assert
        //    Assert.Empty(bookingForReturnedRoomId);
        //}


        // =========================================================================================================
        // Mock-based tests

        // Input validation Boundary Test
        [Theory]
        [InlineData(0, 0)]    // today
        [InlineData(-1, 1)]   // start in past
        [InlineData(5, 2)]    // start > end
        public async Task FindAvailableRoom_InvalidDates(
            int startOffset,
            int endOffset)

        {
            // Arrange
            var mockRoomRepo = new Mock<IRepository<Room>>();

            mockRoomRepo.Setup(x => x.GetAllAsync())
                        .ReturnsAsync(new List<Room> { new Room { Id = 1 } });

            var mockBookingRepo = new Mock<IRepository<Booking>>();

            mockBookingRepo.Setup(x => x.GetAllAsync())
                           .ReturnsAsync(new List<Booking>());

            var manager = new BookingManager(
                mockBookingRepo.Object,
                mockRoomRepo.Object);

            DateTime start = DateTime.Today.AddDays(startOffset);
            DateTime end = DateTime.Today.AddDays(endOffset);

            // Act
            Task result() => manager.FindAvailableRoom(start, end);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }



        // Feature 1: Booking succeeds £¨Happy Path£©


        [Fact]
        public async Task CreateBooking_AvailableRoomReturnsTrue()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { Id = 1 }
            };

            var mockRoomRepo = new Mock<IRepository<Room>>();

            mockRoomRepo.Setup(x => x.GetAllAsync())
                        .ReturnsAsync(rooms);

            var mockBookingRepo = new Mock<IRepository<Booking>>();

            mockBookingRepo.Setup(x => x.GetAllAsync())
                           .ReturnsAsync(new List<Booking>());

            var manager = new BookingManager(
                mockBookingRepo.Object,
                mockRoomRepo.Object);

            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2)
            };

            // Act
            bool result = await manager.CreateBooking(booking);

            // Assert
            Assert.True(result);

            //  Verify
            mockBookingRepo.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Once);
        }



        // Feature 1: Booking fails 


        [Fact]
        public async Task CreateBooking_NoRooms_ReturnsFalse()
        {
            // Arrange
            var mockRoomRepo = new Mock<IRepository<Room>>();

            mockRoomRepo.Setup(x => x.GetAllAsync())
                        .ReturnsAsync(new List<Room>());

            var mockBookingRepo = new Mock<IRepository<Booking>>();

            mockBookingRepo.Setup(x => x.GetAllAsync())
                           .ReturnsAsync(new List<Booking>());

            var manager = new BookingManager(
                mockBookingRepo.Object,
                mockRoomRepo.Object);

            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2)
            };

            // Act
            bool result = await manager.CreateBooking(booking);

            // Assert
            Assert.False(result);

            mockBookingRepo.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Never);
        }


        // ======================================================
        // Feature 2: Fully occupied dates
     

        [Fact]
        public async Task GetFullyOccupiedDates_AllRoomsBooked_ReturnsDates()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { Id = 1 },
                new Room { Id = 2 }
            };

            var bookings = new List<Booking>
            {
                new Booking
                {
                    RoomId = 1,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(3),
                    IsActive = true
                },
                new Booking
                {
                    RoomId = 2,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(3),
                    IsActive = true
                }
            };

            var mockRoomRepo = new Mock<IRepository<Room>>();

            mockRoomRepo.Setup(x => x.GetAllAsync())
                        .ReturnsAsync(rooms);

            var mockBookingRepo = new Mock<IRepository<Booking>>();

            mockBookingRepo.Setup(x => x.GetAllAsync())
                           .ReturnsAsync(bookings);

            var manager = new BookingManager(
                mockBookingRepo.Object,
                mockRoomRepo.Object);

            // Act
            var result = await manager.GetFullyOccupiedDates(
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(3));

            // Assert
            Assert.NotEmpty(result);

            // strong assertions example
            Assert.Equal(3, result.Count);
        }
    }

}

