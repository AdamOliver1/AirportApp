import { useContext, useState } from 'react';
import { ConnectionContext } from "../../contexts/signalR";
import { useEffect } from 'react';
import { HubConnectionBuilder, LogLevel, HubConnection } from '@microsoft/signalr';
import { AirportManager } from '../AirportManager/AirportManager';
import {ErrorMessage} from './../../Extentions/Toaster';
const Airport = () => {
    const connectionContext = useContext(ConnectionContext);
    const [connection, setConnection] = useState<HubConnection | undefined>();
    useEffect(() => {
        try {          
            const newConnection = new HubConnectionBuilder()
                .withUrl('https://localhost:5001/FlightsHub')
                .configureLogging(LogLevel.Information)
                // .withAutomaticReconnect()
                .build();
                setConnection(newConnection)
            connectionContext.setConnection(newConnection);
        } catch (err) {
            ErrorMessage("Not able to connect to the server!")
        }
    }, []);


    return <AirportManager/>
}

export default Airport;
