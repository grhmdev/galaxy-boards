import { ChangeEvent, FormEvent, useCallback, useState } from "react";
import LabelledContent from "../common/LabelledContent";
import { Button } from "../common/Buttons";
import { useDispatch, useSelector } from "react-redux";
import { showErrorAlert } from "../../store/alerts";
import { PostResult, PutResult, postTicket, putTicket } from "../../api/client";
import { TicketDetail, TicketStatus, TicketStatusType } from "../../api/types";
import { RootState } from "../../store";

interface FormField {
   value: string;
   validationMessage: string | null;
}

interface FormData {
   name: FormField;
   description: FormField;
   status: FormField;
}

const TicketForm = (props: {
   ticket?: TicketDetail;
   onTicketCreated?: (ticketId: string) => void;
   onTicketModified?: () => void;
}) => {
   const dispatch = useDispatch();
   const activeProjectId = useSelector(
      (state: RootState) => state.projects.activeProject!.project.id
   );
   const [formData, setFormData] = useState<FormData>({
      name: {
         value: props.ticket ? props.ticket.name : "",
         validationMessage: null,
      },
      description: {
         value: props.ticket ? props.ticket.description : "",
         validationMessage: null,
      },
      status: {
         value: props.ticket ? props.ticket.status : TicketStatus.Open,
         validationMessage: null,
      },
   });

   const formDataValid = useCallback(() => {
      const name = formData.name.value.trim();
      return name.length > 0;
   }, [formData]);

   const createTicketOnServer = useCallback(
      async (name: string, description: string, status: TicketStatusType) => {
         const postResult: PostResult = await postTicket(
            name,
            description,
            status,
            activeProjectId
         );
         if (!postResult.success) {
            dispatch(
               showErrorAlert({
                  message: "Failed to create ticket on server",
               })
            );
            return;
         }
         props.onTicketCreated!(postResult.createdId);
      },
      [dispatch, props.onTicketCreated, activeProjectId]
   );

   const updateTicketOnServer = useCallback(
      async (
         id: string,
         name: string,
         description: string,
         status: TicketStatusType
      ) => {
         const result: PutResult = await putTicket(
            id,
            name,
            description,
            status
         );
         if (!result.success) {
            dispatch(
               showErrorAlert({
                  message: "Failed to update ticket on server",
               })
            );
            return;
         }
         props.onTicketModified!();
      },
      [dispatch, props.onTicketModified]
   );

   const handleSubmit = useCallback(
      async (event: FormEvent) => {
         event.preventDefault();
         const name = formData.name.value.trim();
         const description = formData.description.value.trim();
         const status = formData.status.value as TicketStatusType;

         if (!formDataValid()) {
            return;
         }

         if (!props.ticket) {
            createTicketOnServer(name, description, status);
         } else {
            updateTicketOnServer(props.ticket.id, name, description, status);
         }
      },
      [
         formData,
         formDataValid,
         props.ticket,
         createTicketOnServer,
         updateTicketOnServer,
      ]
   );

   const handleNameChanged = (event: ChangeEvent<HTMLInputElement>) => {
      const name = event.target.value;
      let validationMessage: string | null = null;
      if (name.trim().length === 0) {
         validationMessage = "Required field";
      }
      setFormData((formData) => ({
         ...formData,
         name: { value: name, validationMessage: validationMessage },
      }));
   };

   const handleDescriptionChanged = (
      event: ChangeEvent<HTMLTextAreaElement>
   ) => {
      setFormData((formData) => ({
         ...formData,
         description: { value: event.target.value, validationMessage: null },
      }));
   };

   const handleStatusChanged = (event: ChangeEvent<HTMLSelectElement>) => {
      setFormData((formData) => ({
         ...formData,
         status: { value: event.target.value, validationMessage: null },
      }));
   };

   return (
      <form onSubmit={handleSubmit}>
         <LabelledContent
            label="Name"
            content={
               <>
                  <input
                     type="text"
                     className={`w-full rounded ${
                        formData.name.validationMessage ? "bg-red-100" : ""
                     }`}
                     value={formData.name.value}
                     onChange={handleNameChanged}
                  />
                  <span className="text-red-500">
                     {formData.name.validationMessage}
                  </span>
               </>
            }
         />
         <LabelledContent
            label="Status"
            content={
               <>
                  <select
                     className={`w-full rounded ${
                        formData.status.validationMessage ? "bg-red-100" : ""
                     }`}
                     value={formData.status.value}
                     onChange={handleStatusChanged}
                  >
                     {Object.keys(TicketStatus).map((status) => (
                        <option key={`opt-${status}`} value={status}>
                           {status}
                        </option>
                     ))}
                  </select>
                  <span className="text-red-500">
                     {formData.status.validationMessage}
                  </span>
               </>
            }
         />
         <LabelledContent
            label="Description"
            content={
               <>
                  <textarea
                     rows={15}
                     className={`w-full rounded ${
                        formData.description.validationMessage
                           ? "bg-red-100"
                           : ""
                     }`}
                     value={formData.description.value}
                     onChange={handleDescriptionChanged}
                  />
                  <span className="text-red-500">
                     {formData.description.validationMessage}
                  </span>
               </>
            }
         />
         <div className="text-right pt-2">
            <Button
               title=""
               className="w-full"
               disabled={!formDataValid()}
               isSubmit={true}
            >
               Save
            </Button>
         </div>
      </form>
   );
};

export default TicketForm;
