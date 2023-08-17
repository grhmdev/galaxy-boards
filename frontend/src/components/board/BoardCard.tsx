import { useCallback } from "react";
import { IconButton } from "../common/Buttons";
import Card from "../common/Card";
import { BoardIcon, EditIcon, RefreshIcon } from "../common/Icons";
import BoardGrid from "./BoardGrid";
import { useDispatch, useSelector } from "react-redux";
import { showSuccessAlert } from "../../store/alerts";
import { RootState } from "../../store";
import { openEditBoardDialog } from "../../store/UI";
import { loadBoard } from "../../store/projects";

const BoardCard = (props: { boardId: string }) => {
   const board = useSelector(
      (state: RootState) =>
         state.projects.activeProject!.boards.find(
            (b) => b.id === props.boardId
         )!
   );
   const dispatch = useDispatch();

   const reloadBoard = useCallback(() => {
      dispatch(
         loadBoard({
            boardId: board.id,
         })
      );
   }, [dispatch, board.id]);

   const handleReloadBoard = useCallback(() => {
      reloadBoard();
      dispatch(
         showSuccessAlert({
            message: `Reloaded board`,
         })
      );
   }, [dispatch, reloadBoard]);

   const handleEditBoard = useCallback(() => {
      dispatch(openEditBoardDialog({ boardId: board.id }));
   }, [dispatch, board.id]);

   return (
      <Card>
         <div className="grid grid-cols-8 mb-2">
            <h2 className="col-span-6"><BoardIcon className="mr-2"/>{board.name}</h2>
            <div className="col-span-2 text-right">
               <IconButton title="Edit board" onClick={handleEditBoard}>
                  <EditIcon />
               </IconButton>
               <IconButton title="Reload board" onClick={handleReloadBoard}>
                  <RefreshIcon />
               </IconButton>
            </div>
         </div>
         <BoardGrid board={board} />
      </Card>
   );
};

export default BoardCard;
