import { ReactSortable, SortableEvent } from "react-sortablejs";
import { BoardColumn } from "../../api/types";
import TicketCard from "../ticket/TicketCard";
import { useCallback, useEffect, useRef, useState } from "react";

const BoardGridColumn = (props: {
   boardId: string;
   column: BoardColumn;
   setDisplayTicketDragBin: (display: boolean) => void;
   onTicketMoved: (
      ticketId: string,
      fromBoardId: string | null,
      fromColumnId: string | null,
      fromIndex: number,
      toBoardId: string | null,
      toColumnId: string | null,
      toIndex: number
   ) => void;
}) => {
   const [tickets, setTickets] = useState<
      { id: string; ticketPlacementId: string }[]
   >([]);
   const sortableId = useRef(`${props.boardId}|${props.column.id}`);

   useEffect(() => {
      setTickets(
         props.column.ticketPlacements.map((tp) => ({
            id: tp.ticketId,
            ticketPlacementId: tp.id,
         }))
      );
   }, [setTickets, props.column.ticketPlacements]);

   const setTicketsSortable = useCallback(
      (items: Array<{ id: string; ticketPlacementId?: string }>) => {
         setTickets(
            items.map((item) => ({
               id: item.id,
               ticketPlacementId: item.ticketPlacementId
                  ? item.ticketPlacementId
                  : item.id,
            }))
         );
      },
      [setTickets]
   );

   const handleTicketMoved = useCallback(
      (event: SortableEvent) => {
         let fromBoardId = null;
         let fromColumnId = null;
         let toBoardId = null;
         let toColumnId = null;

         if (event.from.id.indexOf("|") !== -1) {
            [fromBoardId, fromColumnId] = event.from.id.split("|");
         }
         if (event.to.id.indexOf("|") !== -1) {
            [toBoardId, toColumnId] = event.to.id.split("|");
         }

         if (
            fromBoardId === toBoardId &&
            fromColumnId !== toColumnId &&
            toColumnId !== props.column.id
         ) {
            // Ticket moved from this column to another (or same) column
            // on the same board, let that column handle the
            // update to not process it twice
            return;
         }

         const ticketId = event.item.attributes.getNamedItem("data-id")!.value;
         props.onTicketMoved(
            ticketId,
            fromBoardId,
            fromColumnId,
            event.oldIndex || 0,
            toBoardId,
            toColumnId,
            event.newIndex || 0
         );
      },
      [props]
   );

   return (
      <div className="bg-gray-50 rounded border flex flex-col">
         <div className="w-full text-xs text-center pt-1 grow-1">
            {props.column.name}
         </div>
         <div className="w-full min-h-[60px] p-2 grow-999">
            <ReactSortable
               id={sortableId.current}
               tag={"div"}
               className="w-full h-full"
               group="shared"
               animation={150}
               list={tickets}
               setList={setTicketsSortable}
               onSort={handleTicketMoved}
               ghostClass="ticket-card-ghost"
               onStart={() => props.setDisplayTicketDragBin(true)}
               onEnd={() => props.setDisplayTicketDragBin(false)}
            >
               {tickets.map(({ id, ticketPlacementId }) => (
                  <TicketCard
                     key={ticketPlacementId}
                     ticketId={id}
                     className="mx-auto"
                  />
               ))}
            </ReactSortable>
         </div>
      </div>
   );
};

export default BoardGridColumn;
