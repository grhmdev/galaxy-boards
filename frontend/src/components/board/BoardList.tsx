import { BoardDetail } from "../../api/types";
import CreateBoardButton from "../CreateBoardButton";
import BoardCard from "./BoardCard";

const BoardList = (props: { boards: BoardDetail[] }) => {
   return (
      <>
         {props.boards.map((board) => (
            <BoardCard key={board.id} boardId={board.id} />
         ))}
         <CreateBoardButton />
      </>
   );
};

export default BoardList;
