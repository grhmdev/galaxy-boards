import { useEffect, useRef, MouseEvent } from "react";
import { Alert, AlertType } from "../../store/alerts";
import { CloseIcon, SuccessIcon, WarningIcon } from "../common/Icons";

interface AlertBannerProps {
   alert: Alert;
   onAlertExpired: (alertId: string) => void;
}

const AlertBanner = ({ alert, onAlertExpired }: AlertBannerProps) => {
   const alertRef = useRef<HTMLDialogElement>(null);

   useEffect(() => {
      // Begin fadeout in 3s (1s transition duration), then remove alert after 4s
      const fadeoutTimer = setTimeout(() => {
         if (!alertRef.current) return;
         alertRef.current!.className +=
            "transition-opacity duration-1000 opacity-0";
      }, 3000);
      const expireTimer = setTimeout(() => {
         onAlertExpired(alert.id);
      }, 4000);
      return () => {
         clearTimeout(fadeoutTimer);
         clearTimeout(expireTimer);
      };
   }, [onAlertExpired, alertRef, alert]);

   const handleUserDismiss = (event: MouseEvent<HTMLButtonElement>) => {
      event.stopPropagation();
      onAlertExpired(alert.id);
   };

   return (
      <dialog
         open
         ref={alertRef}
         className={`w-2/3 rounded p-1 mb-2 pointer-events-auto bottom-0 relative text-sm ${
            alert.type === AlertType.Success ? "bg-green-200" : "bg-red-100"
         }`}
         role="alert"
      >
         <button
            type="button"
            className="float-right cursor-pointer"
            aria-label="Close"
            onClick={handleUserDismiss}
         >
            <CloseIcon />
         </button>
         <span className="mr-1">
            {alert.type === AlertType.Error && <WarningIcon />}
            {alert.type === AlertType.Success && <SuccessIcon />}
         </span>
         {alert.message}
      </dialog>
   );
};

export default AlertBanner;
