using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARK.Model;

namespace Test
{
    [TestClass]
    public class TripTests
    {   
        [TestMethod]
        public void TripEnded_WithStartAndEndTime_ReturnTrue()
        {
            // Arrange
            Trip trip = new Trip();
            trip.TripStartTime = DateTime.Now;
            trip.TripEndedTime = DateTime.Now.AddHours(1);
            bool expected = true;

            // Act
            bool actual = trip.TripEnded;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TimeBoatOut_Wait5Secs_Return5()
        {
            // Arrange
            Trip trip = new Trip();
            trip.TripStartTime = DateTime.Now;
            int expected = 5;

            // Act
            System.Threading.Thread.Sleep(5000);
            int actual = (int)(DateTime.Now - trip.TripStartTime).TotalSeconds;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
