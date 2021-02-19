using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ObserverLab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Publisher
            using (var locationPublisher = new LocationPublisher())
            {
                // Observer
                var locationObserver = new LocationObserver();

                // Subscribe
                var locationObserverUnsubscriber = locationPublisher.Subscribe(locationObserver);
                locationPublisher.Write(new Location() { X = 1, Y = 1 });
                locationPublisher.Write(new Location() { X = 2, Y = 2 });

                // Unsubscribe
                locationObserverUnsubscriber.Dispose();
                locationPublisher.Write(new Location() { X = 3, Y = 3 });
            }
        }
    }

    public class Location
    {
        // Properties
        public int X { get; set; }

        public int Y { get; set; }
    }

    public class LocationPublisher : Observable<Location>
    {

    }

    public class LocationObserver : IObserver<Location>
    {
        // Methods
        public void OnNext(Location location)
        {
            #region Contracts

            if (location == null) throw new ArgumentException(nameof(location));

            #endregion

            // Display
            Console.WriteLine($"Location: X={location.X}, Y={location.Y}");
        }

        public void OnCompleted()
        {
            // Nothing

        }

        public void OnError(Exception error)
        {
            #region Contracts

            if (error == null) throw new ArgumentException(nameof(error));

            #endregion

            // Nothing

        }
    }
}
