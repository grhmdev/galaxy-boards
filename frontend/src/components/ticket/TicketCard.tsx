import { useCallback } from "react";
import { TicketStatus } from "../../api/types";
import { useDispatch, useSelector } from "react-redux";
import { openEditTicketDialog } from "../../store/UI";
import { RootState } from "../../store";

const TicketCard = (props: { ticketId: string; className?: string }) => {
   const dispatch = useDispatch();
   const ticket = useSelector((state: RootState) =>
      state.projects.activeProject!.tickets.find((t) => t.id === props.ticketId)
   );

   const handleOpenTicket = useCallback(() => {
      if (!ticket) return;
      dispatch(
         openEditTicketDialog({
            ticketId: ticket.id,
         })
      );
   }, [dispatch, ticket]);

   if (!ticket) {
      return <></>;
   }

   return (
      <div
         data-id={ticket.id}
         className={`p-1 text-sm border border-t-4 border-sky-500 break-all border mb-1 mx-1 w-full rounded shadow cursor-grab ${
            props.className ? props.className : ""
         } ${
            ticket.status === TicketStatus.Closed
               ? "bg-gray-200 text-gray-400"
               : "bg-white"
         }`}
      >
         <button
            className="underline text-left"
            title="View ticket"
            onClick={handleOpenTicket}
         >
            {ticket.name}
         </button>
      </div>
   );
};

export default TicketCard;
