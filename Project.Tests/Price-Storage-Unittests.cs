using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Logic;
using Project.DataModels;
using Project.DataAccess;

namespace Project.Tests
{
    [TestClass]
    public class BookingLogicPriceTests
    {
        [TestMethod]
        public void BookTicket_ShouldStoreFlightBasePrice_AsTotalPrice()
        {
            // Arrange
            var flight = new FlightModel { Id = 1, BasePrice = 250.0 };
            var account = new AccountModel { Id = 1 };
            var passenger = new PassangerModel("Test", "User", "2000-01-01", 123456);
            var date = "2026-04-10";

            var logic = new BookingLogic(flight, account, passenger, date);
            var bookingsAccess = new BookingsAccess();

            // Act
            var (isSuccess, msg, bookingId) = logic.BookTicket();

            // Assert
            Assert.IsTrue(isSuccess, "Booking should succeed");

            var savedBooking = bookingsAccess.GetById(bookingId);

            Assert.IsNotNull(savedBooking, "Booking should be saved in database");
            Assert.AreEqual(250.0, savedBooking.TotalPrice, "Total price should be stored as flight base price");
        }

        [TestMethod]
        public void ConfirmBooking_ShouldLockPrice()
        {
            // Arrange
            var booking = new BookingsModel(1, "2026-04-10", 100.0, "ongoing");
            var logic = new BookingLogic(null, null, null, null);

            // Act
            logic.ConfirmBooking(booking);
            var originalPrice = booking.TotalPrice;

            logic.UpdateBookingPrice(booking, originalPrice + 50);

            // Assert
            Assert.AreEqual("confirmed", booking.Status);
            Assert.AreEqual(originalPrice, booking.TotalPrice, "Price should remain locked after confirmation");
        }

        [TestMethod]
        public void UpdateBookingPrice_ShouldUpdate_WhenNotConfirmed()
        {
            // Arrange
            var booking = new BookingsModel(1, "2026-04-10", 100.0, "ongoing");
            var logic = new BookingLogic(null, null, null, null);

            // Act
            logic.UpdateBookingPrice(booking, 150.0);

            // Assert
            Assert.AreEqual(150.0, booking.TotalPrice, "Ongoing booking should allow price update");
        }

        [TestMethod]
        public void StoredPrice_ShouldNotChange_WhenFlightPriceChangesLater()
        {
            // Arrange
            var flight = new FlightModel { Id = 1, BasePrice = 200.0 };
            var account = new AccountModel { Id = 1 };
            var passenger = new PassangerModel("Test", "User", "2000-01-01", 123456);
            var date = "2026-04-10";

            var logic = new BookingLogic(flight, account, passenger, date);
            var bookingsAccess = new BookingsAccess();

            // Act
            var (isSuccess, msg, bookingId) = logic.BookTicket();
            Assert.IsTrue(isSuccess, "Booking should succeed");

            var savedBookingBefore = bookingsAccess.GetById(bookingId);
            Assert.IsNotNull(savedBookingBefore);

            // change flight price afterwards
            flight.BasePrice = 9999.0;

            var savedBookingAfter = bookingsAccess.GetById(bookingId);

            // Assert
            Assert.IsNotNull(savedBookingAfter);
            Assert.AreEqual(savedBookingBefore.TotalPrice, savedBookingAfter.TotalPrice,
                "Stored booking price should not change after flight base price changes");
        }
    }
}