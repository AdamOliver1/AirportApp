import { HubConnection } from '@microsoft/signalr';
import { useState } from 'react';
import './App.css';
import Airport from './components/Airport/Airport';
import { ConnectionContextProvider, IConnectionProps } from './contexts/signalR';
export interface IApp { }
function App(props: IApp) {

  const [connection, setConnection] = useState<HubConnection>();
  
  const connectionContextValues = {
    connection,
    setConnection,
   }

  return (
    <ConnectionContextProvider value={connectionContextValues}>
    <div className="App">
      <Airport />
    </div>
    </ConnectionContextProvider>
  );
}

export default App;
