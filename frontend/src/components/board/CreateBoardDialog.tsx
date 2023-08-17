import { useCallback } from "react";
import Dialog from "../common/Dialog";
import { useDispatch } from "react-redux";
import { closeCreateBoardDialog } from "../../store/UI";
import BoardForm from "./BoardForm";
import { loadBoard } from "../../store/projects";
import { showSuccessAlert } from "../../store/alerts";

const CreateBoardDialog = () => {
   const dispatch = useDispatch();

   const closeDialog = useCallback(() => {
      dispatch(closeCreateBoardDialog({}));
   }, [dispatch]);

   const handleBoardCreated = useCallback(
      (boardId: string) => {
         closeDialog();
         dispatch(loadBoard({ boardId }));
         dispatch(showSuccessAlert({ message: `Board created` }));
      },
      [closeDialog]
   );

   return (
      <Dialog title="Create New Board" closeDialog={closeDialog}>
         <BoardForm onBoardCreated={handleBoardCreated} />
      </Dialog>
   );
};

export default CreateBoardDialog;
