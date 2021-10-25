import './AirportManager.css';
import { useContext, useEffect, useRef, useState } from 'react';
import { ConnectionContext } from '../../contexts/signalR';
import { IStationModel } from '../../API/models/DTOModels/IStationModel';
import { IFlightModel } from '../../API/models/DTOModels/FlightModel';
import { emitCustomEvent } from 'react-custom-events';
import axios from 'axios';
import { IFlightMovement } from '../../API/models/DTOModels/FlightMovementModel';
import { StationStatesTable } from '../Table/AirportState/AirportStateTable';
import IStationState from '../../API/IStateState';
import { Direction } from '../../Enums/Direction';
import { DraggableBoard } from '../DraggablrBoard/DraggableBoard';
import { ErrorMessage } from '../../Extentions/Toaster';
import { NewFlightsTable } from '../Table/NewFlight/NewFlightsTable';
import { getTime } from '../../Services/DateConverterService';

export const AirportManager = () => {

    const { connection } = useContext(ConnectionContext);
    const [stations, setStations] = useState<IStationModel[]>([]);
    const [render, setRender] = useState(true);
    const [stationStates, setStationsStates] = useState<IStationState[]>([]);
    const [flightModels, setFlightModels] = useState<IFlightModel[]>([]);
    const stationRef = useRef<IStationModel[]>([]);
    const disconnetedAlert = useRef(false);
    const URL = 'https://localhost:5001/api/airport/';
    useEffect(() => {
        getStationsAsync().then((res: any) => saveStations(res?.data));
        getUnfinishedFlightsAsync().then((res: any) => ContinueUnfinisedFlights(res?.data))
    }, [])

    useEffect(() => {
        connection?.start().then(() => {
            connection?.on('RecieveFlightMovement', onRecieveFlightMovement);
            connection?.on('RecieveNewFlight', onRecieveNewFlight);
        }).catch(handleError)
        connection?.onclose(handleError)
    }, [connection])


    const getStationsAsync = async () => {
        return await axios.get<IStationModel[]>(URL + "stations").catch(handleError)
    }

    const GetStationNameById = (id: number): string => {

        try {
            return stationRef.current.filter(s => s.id === id)[0].name
        } catch (err) {
            handleError("Logic Error");
            return "Unabled to fins station";
        }
    };

    const GetStationName = (flightMovement: IFlightMovement, isOldStation: boolean): string => {
        if (isOldStation) {
            if (flightMovement.oldStationId) return GetStationNameById(flightMovement.oldStationId)
            return flightMovement.direction === Direction.Landing ? "Starts Landing" : "Starts Departure";
        }
        else {
            if (flightMovement.newStationId) return GetStationNameById(flightMovement.newStationId)
            return flightMovement.direction === Direction.Takeoff ? "Plane Landed" : "Plane Took Of";
        }

    }

    const saveStations = (stations: IStationModel[]) => {
        if (stations == null) return;
        stationRef.current = stations;
        setStations(stations);
        setStationsStateRows(stations)
    }

    const setStationsStateRows = (stations: IStationModel[]) => {
        if (stations == null) return;
        let rows: IStationState[] = [];
        stations.forEach(s => {
            rows.push(
                {
                    flightId: null,
                    name: null,
                    direction: null,
                    enteringDate: null,
                    station: s.name,
                }
            );
        })
        setStationsStates(rows)
    }
  
    const replaceItemsInArray = (tableRows: IStationState[], ...rowsToReplace: IStationState[]): IStationState[] => {
        tableRows = tableRows.map(tableRow => {
            rowsToReplace.forEach(row => {
                tableRow = tableRow.station === row.station ? row : tableRow;
            })
            return tableRow
        });
        return tableRows;
    }
  
    const getUnfinishedFlightsAsync = async () => {
        return await axios.get<IFlightMovement[]>("https://localhost:5001/api/airport/unfinishedflights").catch(handleError)
    }

    const onRecieveFlightMovement = (flightMovement: IFlightMovement) => {

        if (!flightMovement.flightId) return;
        let newRow: IStationState = IflightMovement_ToIStationState_NewStation(flightMovement);
        let oldRow: IStationState = IflightMovement_ToIStationState_OldStation(flightMovement);

        //delete flight from new flights table i needed
        if (flightMovement.oldStationId == null)
            removeFlightFromFlightModels(flightMovement);

        //update stations state table
        setStationsStates(prev => replaceItemsInArray(prev, ...[oldRow, newRow]))

        //move the flights
        if (flightMovement.oldStationId)
            emitCustomEvent(flightMovement.oldStationId + 'old', flightMovement);
        else if (flightMovement.newStationId)
            emitCustomEvent(flightMovement.newStationId + 'new', flightMovement);
        setRender(true);
    }

    const onRecieveNewFlight = (flightModel: IFlightModel) => {
        flightModel.startDate = new Date(flightModel.startDate);
        if (flightModel.startDate.getTime() > Date.now()) {
            setFlightModels(prev => [flightModel, ...prev])
        }
    }

    const ContinueUnfinisedFlights = (flights: IFlightMovement[]) => {
        flights?.forEach(onRecieveFlightMovement);
    }


    const removeFlightFromFlightModels = (flightModel: IFlightMovement) => {
        setFlightModels(prev => removeItemFromArrayById(flightModel, prev))
    }

    const removeItemFromArrayById = (toRemove: IFlightMovement, flights: IFlightModel[]): IFlightModel[] => {
        flights = flights.filter(f => {
            return f.id !== toRemove.flightId;
        });
        return flights;
    }

    const IflightMovement_ToIStationState_NewStation = (flightMovement: IFlightMovement): IStationState => {
        let enteringDate = getTime();
        return {
            flightId: flightMovement.flightId,
            name: flightMovement.name,
            direction: Direction[flightMovement.direction],
            enteringDate: enteringDate,
            station: GetStationName(flightMovement, false),
        }

    }

    const IflightMovement_ToIStationState_OldStation = (flightMovement: IFlightMovement): IStationState => {
        return {
            flightId: null,
            name: null,
            direction: null,
            enteringDate: null,
            station: GetStationName(flightMovement, true),
        }
    }

    const handleError = (err: any) => {
        if (disconnetedAlert.current) return;
        disconnetedAlert.current = true;
        ErrorMessage("There is aproblem with the airport.\nPlease come back later")
    }

    return (<div className="container">

        <div className="title"><div>Airport Manager</div></div>
        <div className='parent'>
            <div className='child StationStatesTable'> <StationStatesTable rows={stationStates} /></div>
            <div className='child NewFlightsTable'> <NewFlightsTable rows={flightModels} /></div>
        </div>
        <DraggableBoard setRender={setRender} stations={stations} />
    </div>
    )
}
