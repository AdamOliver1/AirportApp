## AirportApp 🛫
An e2e asynchronic project that handles an airport's incoming and outgoing flights.<br />
Aircraft travel through various ‘stations’ and each ‘station’ is connected to one or more other stations,<br />
that specialize in landing and\or takeoff.<br />
Based on the data received, the system plans an itinerary of stations for each flight.

***Includes:***
* 👓 Rest API that handles all the logic (in Asp.Net core).
* 🎚 Simulator that generate new flights and send them to the API (in WPF core).
* 🖥 Web app for displaying the movement of the planes and the airport control panel (in React).
* 💾 Database (in Sqlite)
* 🧪Unit test project (.NET core)

## Requirements
* .NET 5.0

## Running the project
Clone this repository

## Backend
1. Build the solution using Visual Studio.
2. Run both FlightsSimulator and Server.

## Frontend
In `/frontend/` folder run:

```
npm i
npm start
```
