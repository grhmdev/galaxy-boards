export enum AlertType {
   Success,
   Error,
}

export interface Alert {
   id: string;
   type: AlertType;
   message: string;
}

export interface AlertState {
   alerts: Array<Alert>;
}

export const INITIAL_STATE: AlertState = {
   alerts: [],
};
