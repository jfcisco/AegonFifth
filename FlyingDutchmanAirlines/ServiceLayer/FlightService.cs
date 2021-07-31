using System.Collections.Generic;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class FlightService
    {
        private readonly FlightRepository _flightRepo;
        private readonly AirportRepository _airportRepo;
        
        public FlightService(FlightRepository flightRepo, AirportRepository airportRepo)
        {
            _flightRepo = flightRepo;
            _airportRepo = airportRepo;
        }

        public async IAsyncEnumerable<FlightView> GetFlights()
        {
            foreach (Flight flight in _flightRepo.GetFlights())
            {
                Airport origin = await _airportRepo.GetAirportByID(flight.Origin);
                Airport destination = await _airportRepo.GetAirportByID(flight.Destination);
                
                yield return new FlightView(
                    flight.FlightNumber.ToString(), 
                    (origin.City, origin.Iata),
                    (destination.City, destination.Iata));
            }
        }
    }
}