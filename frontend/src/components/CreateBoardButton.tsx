import { useDispatch } from "react-redux";
import { AddIcon, BoardIcon } from "./common/Icons";
import { openCreateBoardDialog } from "../store/UI";

const CreateBoardButton = () => {
   const dispatch = useDispatch();

   return (
      <section className="p-3">
         <button
            title="Create board"
            className="w-full p-3 border-box border-4 hover:border-gray-300"
            onClick={() => {
               dispatch(openCreateBoardDialog({}));
            }}
         >
            <BoardIcon />
            <AddIcon />
         </button>
      </section>
   );
};

export default CreateBoardButton;
