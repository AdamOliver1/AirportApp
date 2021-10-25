import { useState } from 'react'
import './Station.css'
import { useCustomEventListener, emitCustomEvent } from 'react-custom-events';
import { IFlightMovement } from '../../API/models/DTOModels/FlightMovementModel';
import { Flight } from '../Flight/Flight';
import { IStationModel } from '../../API/models/DTOModels/IStationModel';


export const Station = (props: IStationModel) => {
    const [flight, setFlight] = useState<IFlightMovement | null>(null)

    const getFlight = () => {
        return flight === null ? null : <Flight {...flight} />;
    }

    useCustomEventListener(props.id.toString() + 'old', (flightMovement: IFlightMovement) => { 
        setFlight(null);
        if (flightMovement.newStationId) emitCustomEvent(flightMovement.newStationId.toString() + 'new', flightMovement);

    });

    useCustomEventListener(props.id.toString() + 'new', (flightMovement: IFlightMovement) => {  
        setFlight(flightMovement);
    });


    return (<>
        <div className="station">
            <div className='stationTitle'>
               <div>{props.name}</div> 
                 <div>  {flight?.name}</div>   
                 
            {getFlight()}
            </div>
        </div>
    </>
    )
}
