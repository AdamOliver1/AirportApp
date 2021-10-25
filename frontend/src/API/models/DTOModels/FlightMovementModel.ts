import { Direction } from "../../../Enums/Direction";

export interface IFlightMovement {
    flightId:string,
    direction:Direction,
    name:string,
    oldStationId:number | null,
    newStationId:number | null,
}