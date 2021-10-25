import { IFlightMovement } from "./models/DTOModels/FlightMovementModel";

export default interface IStationState  {
    station:string,
    flightId:string | null,
    name:string | null,
    direction:string | null,
    enteringDate:string | null,
}