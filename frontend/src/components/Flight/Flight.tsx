import './Flight.css'
import { Direction } from '../../Enums/Direction';
import { IFlightMovement } from '../../API/models/DTOModels/FlightMovementModel';
import { Icon } from '@iconify/react';

export const Flight = (props: IFlightMovement) => {

    const getClass = () => {
        return "flightSize " +  (props?.direction === Direction.Landing ? "landing" : "takeoff");
    }

    const getIcon = () => {        
        return props?.direction === Direction.Landing ? "mdi:airplane-landing" : "fa-solid:plane-departure"
    }

    return (
        <div>
            {/* <div className="flightName">{props?.name}</div>                */}
            <Icon className={getClass()} icon={getIcon()} />
         </div>
    )
}
