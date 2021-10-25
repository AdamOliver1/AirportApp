import { createContext } from 'react';
import { HubConnection } from '@microsoft/signalr';

export interface IConnectionProps {
  connection: HubConnection | undefined,
  setConnection: ((con: HubConnection) => void)
}

export const ConnectionContext = createContext<IConnectionProps>({
  connection: undefined,
  setConnection: ((con: any) => { })
})

export const ConnectionContextConsumer = ConnectionContext.Consumer;
export const ConnectionContextProvider = ConnectionContext.Provider;

