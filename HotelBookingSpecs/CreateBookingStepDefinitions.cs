using HotelBooking.Core;
using HotelBookingSpecs.Fakes;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingSpecs
{
    [Binding]
    public class CreateBookingStepDefinitions
    {
        private BookingManager manager;
        private List<Booking> bookings;
        private List<Room> rooms;
        private bool result;

 
        public CreateBookingStepDefinitions()
        {
            bookings = new List<Booking>();
            rooms = new List<Room>
            {
                new Room { Id = 1 }
            };

            var bookingRepo = new FakeBookingRepository(bookings);
            var roomRepo = new FakeRoomRepository(rooms);

            manager = new BookingManager(bookingRepo, roomRepo);
        }

        [Given("no existing bookings")]
        public void GivenNoExistingBookings()
        {
            bookings.Clear();
        }

        [Given("a booking exists from {string} to {string}")]
        public void GivenABookingExistsFromTo(string p0, string p1)
        {
            bookings.Add(new Booking
            {
                RoomId = 1,
                StartDate = DateTime.Parse(p0),
                EndDate = DateTime.Parse(p1),
                IsActive = true
            });
        }

        [When("I create a booking from {string} to {string}")]
        public async Task WhenICreateABookingFromTo(string p0, string p1)
        {
            var booking = new Booking
            {
                StartDate = DateTime.Parse(p0),
                EndDate = DateTime.Parse(p1)
            };

            try
            {
                result = await manager.CreateBooking(booking);
            }
            catch
            {
                result = false; 
            }
        }

        [Then("the booking should be successful")]
        public void ThenTheBookingShouldBeSuccessful()
        {
            Assert.True(result);
        }

        [Then("the booking should fail")]
        public void ThenTheBookingShouldFail()
        {
            Assert.False(result);
        }
    }
}