using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlyingDutchmanAirlines_Tests.Stubs
{
    class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext 
    { 
        public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options) : base (options) 
        { 
            base.Database.EnsureDeleted();
        }

        public bool WillErrorOnGetFlights { get; set; } = false;
        public override DbSet<Flight> Flights
        {
            get
            {
                // Throw database error when prompted
                if (WillErrorOnGetFlights) throw new Exception("Database Error");
                return base.Flights;
            }
            set => base.Flights = value;
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            IEnumerable<Booking> bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();

            if (bookings.Any(b => b.CustomerId != 1))
            {
                throw new Exception("Database Error!");
            }

            // IEnumerable<Airport> airports = pendingChanges.Select(e => e.Entity).OfType<Airport>();

            // if (airports.Any(a => a.AirportId == 10))
            // {
            //     throw new Exception("Database Error!");
            // }

            // IEnumerable<Flight> flights = pendingChanges.Select(e => e.Entity).OfType<Flight>();

            // if (flights.Any(f => f.FlightNumber == 2))
            // {
            //     throw new Exception("Database Error!");
            // }

            await base.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}