import { useCallback, useEffect, useRef, useState } from "react";
import {
   BoardColumn,
   BoardColumnUpdate,
   BoardDetail,
   BoardUpdate,
} from "../../api/types";
import BoardGridColumn from "./BoardGridColumn";
import { ReactSortable } from "react-sortablejs";
import { DeleteIcon } from "../common/Icons";
import { putBoard } from "../../api/client";
import { showErrorAlert } from "../../store/alerts";
import { useDispatch } from "react-redux";
import { loadBoard } from "../../store/projects";
import store from "../../store";

const BoardGrid = (props: { board: BoardDetail }) => {
   const [displayTicketDragBin, setDisplayTicketDragBin] = useState(false);
   const dispatch = useDispatch();
   const [columns, setColumns] = useState<BoardColumn[]>([]);
   const gridColsClass = `grid-cols-${props.board.columns.length}`;
   const sortableBinId = useRef(`sortable-bin-${props.board.id}`);

   const copyColumns = useCallback((sourceColumns: BoardColumn[]) => {
      return sourceColumns.map((bc) => ({
         ...bc,
         ticketPlacements: bc.ticketPlacements.map((tp) => ({ ...tp })),
      }));
   }, []);

   useEffect(() => {
      setColumns(copyColumns(props.board.columns));
   }, [setColumns, copyColumns, props.board.columns]);

   const reloadBoard = useCallback(() => {
      dispatch(loadBoard({ boardId: props.board.id }));
   }, [dispatch, props.board.id]);

   const updateBoardOnServer = useCallback(
      async (
         boardId: string,
         boardName: string,
         updatedColumns: BoardColumnUpdate[]
      ) => {
         const boardUpdate: BoardUpdate = {
            name: boardName,
            columns: updatedColumns,
         };
         const result = await putBoard(boardId, boardUpdate);

         if (result.success) {
            reloadBoard();
         } else {
            dispatch(
               showErrorAlert({
                  message: `Failed to update ${boardName} on the server`,
               })
            );
         }
      },
      [dispatch, reloadBoard]
   );

   const removeTicketPlacement = useCallback(
      (columns: BoardColumnUpdate[], columnId: string, fromIndex: number) => {
         const column = columns.find((col) => col.id === columnId)!;
         column.ticketPlacements.splice(fromIndex, 1);
      },
      []
   );

   const addTicketPlacement = useCallback(
      (
         columns: BoardColumnUpdate[],
         ticketId: string,
         columnId: string,
         atIndex: number
      ) => {
         columns
            .find((col) => col.id === columnId)!
            .ticketPlacements.push({
               id: null,
               ticketId: ticketId,
               index: atIndex,
            });
      },
      []
   );

   // Converts from BoardColumn[] to BoardColumnUpdate[], used in PUT payload
   const toBoardColumnUpdates = useCallback(
      (columns: BoardColumn[]): BoardColumnUpdate[] => {
         return columns.map((col) => ({
            ...col,
            ticketPlacements: col.ticketPlacements.map((tp) => ({
               id: tp.id,
               index: tp.index,
               ticketId: tp.ticketId,
            })),
         }));
      },
      []
   );

   // Returns true if a ticket has been moved onto this board that is already placed into one
   // of its columns
   const duplicateTicketMovedToThisBoard = useCallback(
      (
         ticketId: string,
         fromBoardId: string | null,
         toBoardId: string | null
      ) => {
         const ticketMovedOntoThisBoard =
            fromBoardId !== props.board.id && toBoardId === props.board.id;
         const ticketOnBoard =
            columns.find(
               (col) =>
                  col.ticketPlacements.find(
                     (tp) => tp.ticketId === ticketId
                  ) !== undefined
            ) !== undefined;
         return ticketMovedOntoThisBoard && ticketOnBoard;
      },
      [columns, props.board.id]
   );

   // Returns true if a ticket has been moved onto this board that is already placed into one
   // of its columns
   const duplicateTicketMovedFromThisBoard = useCallback(
      (
         ticketId: string,
         fromBoardId: string | null,
         toBoardId: string | null
      ) => {
         if (
            fromBoardId !== props.board.id ||
            !toBoardId ||
            toBoardId === props.board.id
         ) {
            return false;
         }
         const toBoard = store
            .getState()
            .projects.activeProject!.boards.find((b) => b.id === toBoardId)!;
         return (
            toBoard.columns.find((col) =>
               col.ticketPlacements.find((tp) => tp.ticketId === ticketId)
            ) !== undefined
         );
      },
      [props.board.id]
   );

   // Handles a ticket move event - tickets are either (re)moved from the board,
   // moved onto the board, or moved within the board (across columns or within a column)
   const handleTicketMoved = useCallback(
      (
         ticketId: string,
         fromBoardId: string | null,
         fromColumnId: string | null,
         fromIndex: number,
         toBoardId: string | null,
         toColumnId: string | null,
         toIndex: number
      ) => {
         // A board can only contain 1 placement per ticket and sortablejs does not provide a way
         // to cancel or revert the DOM change, so this board and potentially the source board
         // are left in a bad state.
         // As a workaround, the destination (to) board and source (from) board must check
         // if the move resulted in a duplicate placement and then reset their column states.
         // No server request is made in this case.
         if (
            duplicateTicketMovedToThisBoard(ticketId, fromBoardId, toBoardId)
         ) {
            dispatch(
               showErrorAlert({
                  message: `Cannot add ticket to board - boards may only hold 1 copy of each ticket.`,
               })
            );
            setColumns(copyColumns(props.board.columns));
            return;
         } else if (
            duplicateTicketMovedFromThisBoard(ticketId, fromBoardId, toBoardId)
         ) {
            setColumns(copyColumns(props.board.columns));
            return;
         }

         const columnUpdates = toBoardColumnUpdates(columns);
         if (fromBoardId === props.board.id) {
            removeTicketPlacement(columnUpdates, fromColumnId!, fromIndex);
         }
         if (toBoardId === props.board.id) {
            addTicketPlacement(columnUpdates, ticketId, toColumnId!, toIndex);
         }
         updateBoardOnServer(props.board.id, props.board.name, columnUpdates);
      },
      [
         addTicketPlacement,
         removeTicketPlacement,
         updateBoardOnServer,
         toBoardColumnUpdates,
         dispatch,
         duplicateTicketMovedToThisBoard,
         duplicateTicketMovedFromThisBoard,
         copyColumns,
         columns,
         props.board,
      ]
   );

   return (
      <>
         <div className={`grid gap-2 ${gridColsClass}`}>
            {columns.map((col) => (
               <BoardGridColumn
                  key={col.id}
                  boardId={props.board.id}
                  column={col}
                  setDisplayTicketDragBin={setDisplayTicketDragBin}
                  onTicketMoved={handleTicketMoved}
               />
            ))}
         </div>
         <div
            className={`relative w-full mt-2 relative w-full ${
               displayTicketDragBin
                  ? "h-10 border-2 border-dashed "
                  : "h-0 invisible"
            }`}
         >
            <ReactSortable
               tag="div"
               id={sortableBinId.current}
               className="h-full w-full"
               group="shared"
               ghostClass="ticket-card-ghost"
               list={[]}
               setList={() => {}}
            ></ReactSortable>
            <div
               className={`absolute left-0 top-1 w-full h-full notSortable text-center ${
                  displayTicketDragBin ? "visible" : "invisible"
               }`}
            >
               <DeleteIcon className="mr-2" />
            </div>
         </div>
      </>
   );
};

export default BoardGrid;
