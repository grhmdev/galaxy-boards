import { useCallback, useEffect, useState } from "react";
import Dialog from "../common/Dialog";
import { useDispatch, useSelector } from "react-redux";
import { closeEditBoardDialog } from "../../store/UI";
import { BoardDetail } from "../../api/types";
import { showErrorAlert, showSuccessAlert } from "../../store/alerts";
import BoardForm from "./BoardForm";
import { RootState } from "../../store";
import { getBoard } from "../../api/client";
import { loadBoard } from "../../store/projects";

const EditBoardDialog = () => {
   const boardId = useSelector((state: RootState) => state.ui.editBoardId);
   const [board, setBoard] = useState<BoardDetail | null>(null);
   const dispatch = useDispatch();

   useEffect(() => {
      if (!boardId) return;
      getBoard(boardId).then((result) => {
         if (!result || !result.data) {
            dispatch(
               showErrorAlert({
                  message: "Failed to retrieve board data from server",
               })
            );
            return;
         }
         setBoard(result.data);
      });
   }, [dispatch, boardId]);

   const closeDialog = useCallback(() => {
      dispatch(closeEditBoardDialog({}));
   }, [dispatch]);

   const handleBoardModified = useCallback(() => {
      closeDialog();
      dispatch(loadBoard({ boardId }));
      dispatch(showSuccessAlert({ message: `Board updated` }));
   }, [dispatch, closeDialog, boardId]);

   return (
      <Dialog title="Edit Board" closeDialog={closeDialog}>
         {board && (
            <BoardForm board={board} onBoardModified={handleBoardModified} />
         )}
      </Dialog>
   );
};

export default EditBoardDialog;
