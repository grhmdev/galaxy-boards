import { ReactNode } from "react";
import { CloseIcon } from "./Icons";

const DismissOverlay = (props: { closeDialog: () => void }) => {
   return (
      <div
         className="fixed top-0 left-0 w-full h-full bg-black bg-opacity-30 z-40"
         onClick={props.closeDialog}
      ></div>
   );
};

const Dialog = (props: {
   children: ReactNode;
   title: string;
   closeDialog: () => void;
}) => {
   return (
      <>
         <DismissOverlay closeDialog={props.closeDialog} />
         <dialog
            className="shadow-lg w-3/4 lg:w-1/2 rounded z-50 overflow-hidden"
            open
         >
            <div className="grid grid-cols-8 border-b-2 p-2 bg-slate-800 text-white">
               <h2 className="col-span-7 font-mono">{props.title}</h2>
               <div className="text-right">
                  <button onClick={() => props.closeDialog()}>
                     <CloseIcon />
                  </button>
               </div>
            </div>
            <div className="p-3">{props.children}</div>
         </dialog>
      </>
   );
};

export default Dialog;
