﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARK.Model;

namespace Test
{
    [TestClass]
    public class BoatTests
    {
        [TestMethod]
        public void Usable_WithDamageReportNonFunctional_ReturnFalse()
        {
            // Arrange
            Boat boat = new Boat();
            DamageForm damageform = new DamageForm();
            damageform.Functional = false;
            boat.DamageForms = new List<DamageForm>();
            boat.DamageForms.Add(damageform);
            bool expected = false;

            // Act
            bool actual = boat.Usable;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void Damaged_WithDamageForm_ReturnTrue()
        {            
            // Arrange
            Boat boat = new Boat();
            DamageForm damageform = new DamageForm();
            boat.DamageForms = new List<DamageForm>(); 
            boat.DamageForms.Add(damageform);
            bool expected = true;

            // Act
            bool actual = boat.Damaged;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BoatOut_WithTrip_ReturnTrue()
        {
            // Arrange
            Boat boat = new Boat();
            Trip trip = new Trip();
            boat.Trips = new List<Trip>();
            boat.Trips.Add(trip);
            bool expected = true;

            // Act
            bool actual = boat.BoatOut;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetActiveTrip_WithTrip_ReturnTrip()
        {
            // Arrange
            Boat boat = new Boat();
            Trip trip = new Trip();
            boat.Trips = new List<Trip>();
            boat.Trips.Add(trip);
            Trip expected = trip;

            // Act
            Trip actual = boat.GetActiveTrip;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TripsSailed_With3Trips_Return3()
        {
            // Arange
            Boat boat = new Boat();
            Trip trip1 = new Trip();
            Trip trip2 = new Trip();
            Trip trip3 = new Trip();
            boat.Trips = new List<Trip>();
            boat.Trips.Add(trip1);
            boat.Trips.Add(trip2);
            boat.Trips.Add(trip3);
            int expected = 3;

            // Act
            int actual = boat.TripsSailed;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void KilometersSailed_With3Trips_Return69()
        {
            // Arange
            Boat boat = new Boat();
            Trip trip1 = new Trip();
            Trip trip2 = new Trip();
            Trip trip3 = new Trip();
            trip1.Distance = 35;
            trip2.Distance = 14;
            trip3.Distance = 20;
            boat.Trips = new List<Trip>();
            boat.Trips.Add(trip1);
            boat.Trips.Add(trip2);
            boat.Trips.Add(trip3);
            double expected = 69;

            // Act
            double actual = boat.KilometersSailed;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LongDistanceTripsSailed_With1NormalAnd1LongDistance_Return1()
        {
            // Arange
            Boat boat = new Boat();
            Trip trip1 = new Trip();
            Trip trip2 = new Trip();
            trip2.LongTrip = true;
            boat.Trips = new List<Trip>();
            boat.Trips.Add(trip1);
            boat.Trips.Add(trip2);
            int expected = 1;

            // Act
            int actual = boat.LongDistanceTripsSailed;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
