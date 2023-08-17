import { useCallback } from "react";
import Dialog from "../common/Dialog";
import { useDispatch } from "react-redux";
import TicketForm from "./TicketForm";
import { reloadProjectTickets } from "../../store/projects";
import { closeCreateTicketDialog } from "../../store/UI";

const CreateTicketDialog = () => {
   const dispatch = useDispatch();

   const closeDialog = useCallback(() => {
      dispatch(closeCreateTicketDialog({}));
   }, [dispatch]);

   const handleTicketCreated = useCallback(
      (ticketId: string) => {
         dispatch(reloadProjectTickets({}));
         closeDialog();
      },
      [dispatch, closeDialog]
   );

   return (
      <Dialog title="Create Ticket" closeDialog={closeDialog}>
         <TicketForm onTicketCreated={handleTicketCreated} />
      </Dialog>
   );
};

export default CreateTicketDialog;
