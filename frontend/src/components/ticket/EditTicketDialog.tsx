import { useCallback, useEffect, useState } from "react";
import Dialog from "../common/Dialog";
import { useDispatch, useSelector } from "react-redux";
import { closeEditTicketDialog } from "../../store/UI";
import TicketForm from "./TicketForm";
import { TicketDetail } from "../../api/types";
import { RootState } from "../../store";
import { getTicket } from "../../api/client";
import { showErrorAlert, showSuccessAlert } from "../../store/alerts";
import { reloadProjectTickets } from "../../store/projects";

const EditTicketDialog = () => {
   const ticketId = useSelector((state: RootState) => state.ui.editTicketId);
   const [ticket, setTicket] = useState<TicketDetail | null>(null);
   const dispatch = useDispatch();

   useEffect(() => {
      if (!ticketId) return;
      getTicket(ticketId).then((result) => {
         if (!result || !result.data) {
            dispatch(
               showErrorAlert({
                  message: "Failed to retrieve ticket data from server",
               })
            );
            return;
         }
         setTicket(result.data);
      });
   }, [dispatch, ticketId]);

   const closeDialog = useCallback(() => {
      dispatch(closeEditTicketDialog({}));
   }, [dispatch]);

   const handleTicketModified = useCallback(() => {
      dispatch(reloadProjectTickets({}));
      closeDialog();
      dispatch(showSuccessAlert({ message: `Ticket updated` }));
   }, [dispatch, closeDialog]);

   return (
      <Dialog
         title={ticket === null ? "Edit Ticket" : ticket.name}
         closeDialog={closeDialog}
      >
         {ticket && (
            <TicketForm
               ticket={ticket}
               onTicketModified={handleTicketModified}
            />
         )}
      </Dialog>
   );
};

export default EditTicketDialog;
