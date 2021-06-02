using NUnit.Framework;
using SaveEarth.MainClasses;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class AirPlaneTests
    {
        [Test]
        public void Checking_The_Corectness_Of_The_Constructor()
        {
            new AirPlane(0, 10, 2).Should().NotBeNull();
            new AirPlane(5, 12, 6).Should().NotBeNull();
            new AirPlane(5, 22, 0).Should().NotBeNull();
            new AirPlane(5, 0, 82).Should().NotBeNull();
        }

        [Test]
        public void Must_Turn_Left_when_Turn_Is_Left()
        {
            double CheckRadius = 10;
            double CheckDirection = 5;
            double CheckDeltaTime = 0.5;
            double AirTurnVelocity = 0.15;
            var air = new AirPlane(CheckRadius, CheckDirection, 0);
            air.Move(Turn.Left, CheckDeltaTime);


            CheckDirection += -AirTurnVelocity * CheckDeltaTime;
            var LocX = -CheckRadius * Math.Sin(CheckDirection);
            var LocY = CheckRadius * Math.Cos(CheckDirection);

            air.Direction.Should().Be(CheckDirection);
            air.LocationX.Should().Be(LocX);
            air.LocationY.Should().Be(LocY);
        }

        [Test]
        public void Must_Turn_Right_when_Turn_Is_Right()
        {
            double CheckRadius = 20;
            double CheckDirection = 15;
            double CheckDeltaTime = 0.7;
            double AirTurnVelocity = 0.15;
            var air = new AirPlane(CheckRadius, CheckDirection, 0);
            air.Move(Turn.Right, CheckDeltaTime);


            CheckDirection += AirTurnVelocity * CheckDeltaTime;
            var LocX = -CheckRadius * Math.Sin(CheckDirection);
            var LocY = CheckRadius * Math.Cos(CheckDirection);

            air.Direction.Should().Be(CheckDirection);
            air.LocationX.Should().Be(LocX);
            air.LocationY.Should().Be(LocY);
        }

    }
}
