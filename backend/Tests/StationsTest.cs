using BL.Services;
using BL.Services.StationServices;
using BL.StationBinders;
using Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class StationsTest
    {
        [TestMethod]
        public void StationNotAvailable_PlaneCanNotAccesstation()
        {
            IFlightService flightService1 = new FlightService(new Flight());

            IStationService stationsService = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 1) }, new OneOptionStationBinder());
            stationsService.RegisterFlightToStation(flightService1);
            Assert.IsFalse(stationsService.CurrentFlightService == null);
            stationsService.ExitStation();
            Assert.IsTrue(stationsService.CurrentFlightService == null);
        }

        [TestMethod]
        public void StationAvailable_PlaneCanAccesstation()
        {
            IStationService stationsService = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 1) }, new OneOptionStationBinder());
            Assert.IsTrue(stationsService.CurrentFlightService == null);
        }

        [TestMethod]
        public void FlightStayAtStationIfNoAvailable()
        {
            IStationService stationsService1 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 0) }, new OneOptionStationBinder());

            IStationService stationsService2 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 3) }, new OneOptionStationBinder());
            stationsService1.NextLandingStations = new List<IStationService> { stationsService2 };

            IFlightService flightService1 = new FlightService(new Flight { Direction = FlightDirection.Landing });
            IFlightService flightService2 = new FlightService(new Flight { Direction = FlightDirection.Landing });

            stationsService2.RegisterFlightToStation(flightService2);
            stationsService1.RegisterFlightToStation(flightService1);

            Assert.IsTrue(stationsService1.CurrentFlightService == flightService1);                  
            Assert.IsTrue(stationsService2.CurrentFlightService != flightService1);
            Thread.Sleep(500);
            Assert.IsTrue(stationsService1.CurrentFlightService == flightService1);
            Assert.IsTrue(stationsService2.CurrentFlightService != flightService1);
        }

        [TestMethod]
        public void GetNextStationOneConnection()
        {
            IStationService stationsService1 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 0) }, new OneOptionStationBinder());

            IStationService stationsService2 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 3) }, new OneOptionStationBinder());
            IFlightService flightService = new FlightService(new Flight { Direction = FlightDirection.Landing });
            stationsService1.RegisterFlightToStation(flightService);
            stationsService1.NextLandingStations = new List<IStationService> { stationsService2 };
            Assert.IsTrue(stationsService1.GetNextStation(flightService.Direction) == stationsService2);
          
        }

        [TestMethod]
        public void FlightContinuesToNextStationsIfAvailable()
        {
            IStationService stationsService1 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 0) }, new OneOptionStationBinder());

            IStationService stationsService2 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 3) }, new OneOptionStationBinder());
            stationsService1.NextLandingStations = new List<IStationService> { stationsService2 };

            IFlightService flightService = new FlightService(new Flight { Direction = FlightDirection.Landing });

            stationsService1.RegisterFlightToStation(flightService);
            Assert.IsTrue(stationsService1.CurrentFlightService != null);
            stationsService1.ExitStation();
            stationsService2.RegisterFlightToStation(flightService);
            Assert.IsTrue(stationsService1.CurrentFlightService == null);
           
        }

        [TestMethod]
        public void LastStationGetsEmptyWhenFlightFinished()
        {          
            IStationService stationsService = new StationService(new Station { WaitingTime = new TimeSpan(0, 0,1) }, new OneOptionStationBinder());          
            IFlightService flightService = new FlightService(new Flight { Direction = FlightDirection.Landing });

            stationsService.RegisterFlightToStation(flightService);            
            Assert.IsTrue(stationsService.CurrentFlightService != null);
           var next = stationsService.GetNextStation(flightService.Direction);
            Assert.IsTrue(next == null);
            stationsService.ExitStation();
            Assert.IsTrue(stationsService.CurrentFlightService == null);
        }

        [TestMethod]
        public void FlightThatFinisehdWaitingTime_ContinuesWhenNextStationEmpty()
        {
            IStationService stationsService1 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 1) }, new OneOptionStationBinder());

            IStationService stationsService2 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 1) }, new OneOptionStationBinder());
            stationsService1.NextLandingStations = new List<IStationService> { stationsService2 };

            IFlightService flightService1 = new FlightService(new Flight { Direction = FlightDirection.Landing });
            IFlightService flightService2 = new FlightService(new Flight { Direction = FlightDirection.Landing });

            stationsService1.RegisterFlightToStation(flightService1);
            stationsService2.RegisterFlightToStation(flightService2);
            stationsService1.ExitStation();
            Assert.IsTrue(stationsService2.CurrentFlightService == flightService2);
            stationsService2.ExitStation();
            stationsService2.RegisterFlightToStation(flightService1);
            Assert.IsTrue(stationsService2.CurrentFlightService == flightService1);
        }

        [TestMethod]
        public void NextStationIsBydDirection()
        {
            IStationService stationsService1 = new StationService(new Station(),new MultipleOptionStationBinder());

            IStationService stationsService2 = new StationService(new Station(), new OneOptionStationBinder());

            IStationService stationsService3 = new StationService(new Station(), new OneOptionStationBinder());        

            stationsService1.NextLandingStations = new List<IStationService> { stationsService2 };
            stationsService1.NextTakeoffStations = new List<IStationService> { stationsService3 };

            var nextLanding = stationsService1.GetNextStation(FlightDirection.Landing); 
            var nextTakeoff = stationsService1.GetNextStation(FlightDirection.Takeoff);

            Assert.IsTrue(nextLanding == stationsService2);
            Assert.IsTrue(nextTakeoff == stationsService3);
        }

        [TestMethod]
        public void FlightWillContinueToSameDirectionStation()
        {
            IStationService stationsService1 = new StationService(new Station {WaitingTime=new TimeSpan(0,0,0) }, new MultipleOptionStationBinder());

            IStationService stationsService2 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 0) }, new OneOptionStationBinder());

            IStationService stationsService3 = new StationService(new Station { WaitingTime = new TimeSpan(0, 0, 0) }, new OneOptionStationBinder());

            stationsService1.NextLandingStations = new List<IStationService> { stationsService2 };
            stationsService1.NextTakeoffStations = new List<IStationService> { stationsService3 };
           
            stationsService3.RegisterFlightToStation(new FlightService(new Flight()));
          
            var nextTakeoff = stationsService1.GetNextStation(FlightDirection.Takeoff);

            Assert.IsTrue(nextTakeoff == stationsService3);
            Assert.IsTrue(stationsService3.CurrentFlightService != null);         
        }
    }
}
