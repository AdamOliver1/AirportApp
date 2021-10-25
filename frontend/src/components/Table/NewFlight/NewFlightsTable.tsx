import React from 'react'
import { IFlightModel } from '../../../API/models/DTOModels/FlightModel';
import { getTime } from '../../../Services/DateConverterService';
import './NewFlight.scss'
import { Direction } from '../../../Enums/Direction';
interface Props {
    rows: IFlightModel[]
}

export const NewFlightsTable = (props: Props) => {
    return (
        <div id="tableNewFlights" >           
            <div className="table-wrapper">
                <table className="fl-table">
                    <thead>
                        <tr>                         
                            <th>Flight</th>
                            <th>Destination</th>
                            <th>Direction</th>
                            <th>Start</th>                        
                        </tr>
                    </thead>
                    <tbody>
                     {props.rows?.map((r) => {
                         return (
                             <tr key={r.id} >
                                 <td>{r.id.substr(r.id.length - 5)}</td>
                                 <td>{r.name}</td>                               
                                 <td>{r.direction == null ? null : Direction[r.direction]}</td>
                                 <td>{getTime(r.startDate)}</td>
                             </tr>
                         )
                     })}                                             
                        </tbody>
                        </table>
                    </div>


            </div>
            )
}
