import { Direction } from "../../../Enums/Direction";

export interface IFlightModel{
    id:string,
    direction:Direction,
    name:string,
    startDate:Date,    
}