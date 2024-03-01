Endpoint: candidate - returns a candidate name and phone info

Endpoint: Location - takes an ip address as parameter and returns the corresponding name of the city.

Endpoint: Listings - takes a parameter for the number of passengers and calls the api/QuoteRequest endpoint of JayRide that returns a listings (sample below). Using the inputted number of passengers, the api will filter out the listings that can support the number of passengers requested with a computed total for each listing and returns it with the cheapest listings on top.

  "from": "Sydney Airport (SYD), T1 International Terminal",
  "to": "46 Church Street, Parramatta NSW, Australia",
  "listings": [
    {
      "name": "Listing 1",
      "pricePerPassenger": 89.5,
      "vehicleType": {
        "name": "Sedan",
        "maxPassengers": 3
      }
    },
    {
      "name": "Listing 2",
      "pricePerPassenger": 0.1,
      "vehicleType": {
        "name": "Hatchback",
        "maxPassengers": 2
      }
    },
