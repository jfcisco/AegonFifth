using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
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
                (Airport origin, Airport destination) = await GetAirportsDetails(flight.Origin, flight.Destination);

                yield return new FlightView(
                    flight.FlightNumber.ToString(), 
                    (origin.City, origin.Iata),
                    (destination.City, destination.Iata));
            }
        }

        private async Task<(Airport origin, Airport destination)> GetAirportsDetails(int originId, int destinationId)
        {
            Airport origin;
            Airport destination;
            try
            {
                origin= await _airportRepo.GetAirportByID(originId);
                destination = await _airportRepo.GetAirportByID(destinationId);
            }
            catch (AirportNotFoundException)
            {
                throw new FlightNotFoundException();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (origin, destination);
        }
    }
}