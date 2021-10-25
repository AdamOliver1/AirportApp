import IStationState from '../../../API/IStateState';
import './AirportState.scss'

interface Props {
    rows: IStationState[]
}


export const StationStatesTable = (props: Props) => {
    return (
        <div id="tableState" >
            <div className="table-wrapper">
                <table className="fl-table">
                    <thead>
                        <tr>
                            <th>Station</th>
                            <th>Flight</th>
                            <th>Destination</th>
                            <th>Direction</th>
                            <th>Entance Time</th>
                        </tr>
                    </thead>
                    <tbody>
                        {props.rows?.map((r, i) => {
                            return (
                                <tr key={i}>
                                    <td>{r.station}</td>
                                    <td>{r.flightId?.substr(r.flightId.length - 5)}</td>
                                    <td>{r.name}</td>
                                    <td>{r.direction}</td>
                                    <td>{r.enteringDate}</td>
                                </tr>
                            )
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    )
}
