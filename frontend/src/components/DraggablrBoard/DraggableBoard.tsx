import { MutableRefObject, useRef, createRef, Dispatch, SetStateAction } from 'react';
import Draggable, { ControlPosition, DraggableEventHandler, DraggableData, DraggableEvent } from 'react-draggable';
import { IStationModel } from '../../API/models/DTOModels/IStationModel';
import { Station } from '../Station/Station';
import './DraggableBoard.css'

interface Props {
    stations: IStationModel[],
    setRender:Dispatch<SetStateAction<boolean>>
}

export const DraggableBoard = (props: Props) => {

    const nodeRefArray = useRef<MutableRefObject<any>[]>([]);  

    const setRefs = () => {  
        if(nodeRefArray.current.length > 0) return;
        props.stations?.forEach(s => {
            nodeRefArray.current.push(createRef());
           })
    }   

    const onStop: DraggableEventHandler = (e: DraggableEvent, data: DraggableData) => {    
        console.log(" data.x, y: data.y", data.x, data.y);
                 
        let station =  data.node;        
        if(station !== undefined)
            setStationPosition(parseInt(station.id), { x: data.x, y: data.y })
    }


    const getStationPosition = (id: number): ControlPosition => {
        let data = localStorage.getItem("airportStationPosition" + id.toString());     
        if (data != null) {
            let nums: number[] = data.split(',').map(s => parseInt(s))
            console.log("nums",nums);
            
            let position: ControlPosition = {
                x: nums[0],
                y: nums[1],
            }
            console.log("position",position);
            
            return position;
        }
        return ({ x: 0, y: 0 })
    }

 
    const setStationPosition = (id: number, position: ControlPosition) => {
        let str = position.x.toString() + "," + position.y.toString();
        localStorage.setItem("airportStationPosition" + id.toString(), str);
    }

    const renderStations = (stations: IStationModel[]) => {
        setRefs()
        return (
            stations?.map((s,i) => {
                return (
                    <Draggable key={s.id} defaultPosition={getStationPosition(s.id)} nodeRef={nodeRefArray.current[i]} bounds="parent" onStop={onStop}>
                        <div ref={nodeRefArray.current[i]} className="draggable" id={s.id.toString()}>
                            <Station id={s.id} name={s.name} />                          
                        </div>
                    </Draggable>
                )
            })
        )
    }



    return (      
        <div className="box">
            {renderStations(props.stations)}      
        </div>


    )
}
