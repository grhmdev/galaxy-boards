import { useDispatch, useSelector } from "react-redux";
import AlertBanner from "./AlertBanner";
import { RootState } from "../../store";
import { useCallback } from "react";
import { Alert, deleteAlert } from "../../store/alerts";

const AlertPrinter = () => {
   const alerts = useSelector((state: RootState) => state.alerts.alerts);
   const dispatch = useDispatch();

   const handleAlertExpired = useCallback(
      (alertId: string) => {
         dispatch(
            deleteAlert({
               alertId,
            })
         );
      },
      [dispatch]
   );

   return (
      <div className="fixed bottom-2 h-full w-full overflow-hidden pointer-events-none z-50">
        <div className="absolute bottom-0 w-full bottom-0">

         {alerts.map((alert: Alert) => (
             <AlertBanner
             key={alert.id}
             alert={alert}
             onAlertExpired={handleAlertExpired}
             />
             ))}
             </div>
      </div>
   );
};

export default AlertPrinter;
