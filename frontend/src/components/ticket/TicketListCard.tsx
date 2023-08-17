import Card from "../common/Card";
import TicketCard from "./TicketCard";
import { AddIcon, TicketIcon } from "../common/Icons";
import { useCallback, useEffect, useRef, useState } from "react";
import { IconButton } from "../common/Buttons";
import { TicketBrief } from "../../api/types";
import { useDispatch } from "react-redux";
import { openCreateTicketDialog } from "../../store/UI";
import { QueryParameters, queryTickets } from "../../api/client";
import { ReactSortable } from "react-sortablejs";

const TicketListCard = (props: {
   projectId: string;
   tickets: TicketBrief[];
}) => {
   const [searchTimeoutId, setSearchTimeoutId] = useState<any>(null);
   const [tickets, setTickets] = useState<TicketBrief[]>([]);
   const [searchQuery, setSearchQuery] = useState<string>("");
   const dispatch = useDispatch();

   useEffect(() => {
      // Make a copy of tickets to allow ReactSortable
      // to modify/extend them
      setTickets(props.tickets.map((t) => ({ ...t })));
   }, [setTickets, props.tickets]);

   const reloadTickets = useCallback(() => {
      const params: QueryParameters = {
         searchQuery: searchQuery,
         filters: [
            {
               key: "projectId",
               value: props.projectId,
            },
         ],
      };

      queryTickets(params).then((result) => {
         if (!result.success) return;
         setTickets(result.items);
      });
   }, [searchQuery, props.projectId]);

   const searchTickets = () => {
      if (searchTimeoutId !== null) {
         clearTimeout(searchTimeoutId);
      }
      setSearchTimeoutId(
         setTimeout(() => {
            reloadTickets();
         }, 300) as any
      );
   };

   const firstRender = useRef(true);
   useEffect(
      () => {
         if (firstRender.current) {
            firstRender.current = false;
            return;
         }
         searchTickets();
      },
      // eslint-disable-next-line
      [searchQuery]
   );

   const handleAddTicket = useCallback(() => {
      dispatch(openCreateTicketDialog({}));
   }, [dispatch]);

   return (
      <Card>
         <div className="grid grid-cols-3 mb-2">
            <h2 className="">
               <TicketIcon className="mr-2" />
               Tickets
            </h2>
            <div className="text-center">
               <input
                  type="text"
                  className="text-center p-0.5 border-0 focus:ring-0 text-sm ml-1"
                  placeholder="Search.."
                  value={searchQuery}
                  onChange={(event) => setSearchQuery(event.target.value)}
               />
            </div>
            <div className="text-right">
               <IconButton title="Add ticket" onClick={handleAddTicket}>
                  <AddIcon />
               </IconButton>
            </div>
         </div>

         <ReactSortable
            id={"sortabable-ticket-list"}
            group={{ name: "shared", pull: "clone", put: false }}
            animation={150}
            tag="div"
            className="flex flex-wrap"
            list={tickets}
            setList={setTickets}
            ghostClass="ticket-card-ghost"
         >
            {tickets.map((ticket) => (
               <TicketCard
                  key={`tli-${ticket.id}`}
                  ticketId={ticket.id}
                  className="max-w-[200px]"
               />
            ))}
         </ReactSortable>
         {tickets.length === 0 && (
            <div className="w-full text-center text-sm">No tickets found</div>
         )}
      </Card>
   );
};

export default TicketListCard;
