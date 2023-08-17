import { ChangeEvent, FormEvent, useCallback, useState } from "react";
import LabelledContent from "../common/LabelledContent";
import { Button, IconButton } from "../common/Buttons";
import { useDispatch, useSelector } from "react-redux";
import { showErrorAlert } from "../../store/alerts";
import { PostResult, PutResult, postBoard, putBoard } from "../../api/client";
import {
   BoardDetail,
   BoardUpdate,
   TicketPlacementUpdate,
} from "../../api/types";
import { RootState } from "../../store";
import {
   AddIcon,
   DeleteIcon,
   DownArrowIcon,
   UpArrowIcon,
} from "../common/Icons";

interface FormField {
   value: string;
   validationMessage: string | null;
}

interface FormData {
   name: FormField;
   columns: Array<{
      id?: string;
      name: FormField;
      index: number;
   }>;
}

const BoardForm = (props: {
   board?: BoardDetail;
   onBoardCreated?: (boardId: string) => void;
   onBoardModified?: () => void;
}) => {
   const dispatch = useDispatch();
   const activeProjectId = useSelector(
      (state: RootState) => state.projects.activeProject!.project.id
   );
   const [formData, setFormData] = useState<FormData>({
      name: {
         value: props.board ? props.board.name : "",
         validationMessage: null,
      },
      columns: props.board
         ? props.board.columns.map((c) => ({
              id: c.id,
              name: { value: c.name, validationMessage: null },
              index: c.index,
           }))
         : [],
   });

   const formDataValid = useCallback(() => {
      const name = formData.name.value.trim();
      const columnWithoutName = formData.columns.find(
         (c) => c.name.value.trim().length === 0
      );
      return name.length > 0 && !columnWithoutName;
   }, [formData]);

   const createBoardOnServer = useCallback(async () => {
      const postResult: PostResult = await postBoard(
         formData.name.value.trim(),
         activeProjectId,
         formData.columns.map((c) => ({
            name: c.name.value.trim(),
            index: c.index,
            tickets: [],
         }))
      );
      if (!postResult.success) {
         dispatch(
            showErrorAlert({
               message: "Failed to create board on server",
            })
         );
         return;
      }
      props.onBoardCreated!(postResult.createdId);
   }, [dispatch, formData, props.onBoardCreated, activeProjectId]);

   // Finds existing ticket placements for a column and converts
   // them to 'update' objects sent with PUT requests
   const ticketPlacementUpdatesForColumn = useCallback(
      (columnId: string): TicketPlacementUpdate[] => {
         return props
            .board!.columns.find((col) => col.id === columnId)!
            .ticketPlacements.map((tp) => ({
               id: tp.id,
               index: tp.index,
               ticketId: tp.ticketId,
            }));
      },
      [props.board]
   );

   const updateBoardOnServer = useCallback(async () => {
      const boardUpdate: BoardUpdate = {
         name: formData.name.value.trim(),
         columns: formData.columns.map((c) => ({
            id: c.id ? c.id : null,
            name: c.name.value.trim(),
            index: c.index,
            ticketPlacements:
               c.id === undefined ? [] : ticketPlacementUpdatesForColumn(c.id!),
         })),
      };

      const result: PutResult = await putBoard(props.board!.id, boardUpdate);
      if (!result.success) {
         dispatch(
            showErrorAlert({
               message: "Failed to update board on server",
            })
         );
         return;
      }
      props.onBoardModified!();
   }, [
      dispatch,
      ticketPlacementUpdatesForColumn,
      formData,
      props.onBoardModified,
      props.board,
   ]);

   const handleSubmit = useCallback(
      async (event: FormEvent) => {
         event.preventDefault();
         if (!formDataValid()) {
            return;
         }

         if (!props.board) {
            createBoardOnServer();
         } else {
            updateBoardOnServer();
         }
      },
      [formDataValid, props.board, createBoardOnServer, updateBoardOnServer]
   );

   const handleNameChanged = useCallback(
      (event: ChangeEvent<HTMLInputElement>) => {
         const name = event.target.value;
         setFormData((formData) => ({
            ...formData,
            name: {
               value: name,
               validationMessage:
                  name.trim().length === 0 ? "Required field" : null,
            },
         }));
      },
      [setFormData]
   );

   const handleAddColumn = useCallback(() => {
      setFormData((formData) => ({
         ...formData,
         columns: [
            ...formData.columns,
            {
               name: { value: "", validationMessage: null },
               index: formData.columns.length,
            },
         ],
      }));
   }, [setFormData]);

   const handleDeleteColumn = useCallback(
      (index: number) => {
         setFormData((formData) => {
            const updatedColumns = [...formData.columns];
            updatedColumns.splice(index, 1);
            return {
               ...formData,
               columns: updatedColumns,
            };
         });
      },
      [setFormData]
   );

   const handleColumnNameChanged = useCallback(
      (name: string, index: number) => {
         setFormData((formData) => {
            const updatedColumns = [...formData.columns];
            updatedColumns[index].name.value = name;
            updatedColumns[index].name.validationMessage =
               name.trim().length === 0 ? "Required field" : null;
            return {
               ...formData,
               columns: updatedColumns,
            };
         });
      },
      [setFormData]
   );

   const handleColumnMove = useCallback(
      (startIndex: number, moveUp: boolean) => {
         if (moveUp && startIndex === 0) {
            // Already at start (index 0)
            return;
         }
         setFormData((formData: FormData) => {
            if (!moveUp && startIndex === formData.columns.length - 1) {
               // Already at end (last index)
               return formData;
            }
            // Swap column at startIndex with the one above or below
            const destIndex = moveUp ? startIndex - 1 : startIndex + 1;
            const updatedColumns = [...formData.columns];
            const columnToMove = updatedColumns[startIndex];
            updatedColumns[startIndex] = updatedColumns[destIndex];
            updatedColumns[destIndex] = columnToMove;
            // Also update the 2 moved columns index values
            updatedColumns[startIndex].index = startIndex;
            columnToMove.index = destIndex;
            return {
               ...formData,
               columns: updatedColumns,
            };
         });
      },
      [setFormData]
   );

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
            label="Columns"
            content={
               <>
                  {formData.columns.map((formColumnData) => (
                     <div
                        key={"col" + formColumnData.index}
                        className="grid grid-cols-10 mt-1 gap-2"
                     >
                        <div className="col-span-1 grid grid-rows-2 text-right h-8 -mt-1">
                           <IconButton
                              className=""
                              title={"Move up"}
                              disabled={formColumnData.index === 0}
                              onClick={() =>
                                 handleColumnMove(formColumnData.index, true)
                              }
                           >
                              <UpArrowIcon />
                           </IconButton>
                           <IconButton
                              className=""
                              title={"Move down"}
                              disabled={
                                 formColumnData.index ===
                                 formData.columns.length - 1
                              }
                              onClick={() =>
                                 handleColumnMove(formColumnData.index, false)
                              }
                           >
                              <DownArrowIcon />
                           </IconButton>
                        </div>
                        <div className="col-span-8">
                           <input
                              className={`p-1 w-full rounded ${
                                 formColumnData.name.validationMessage
                                    ? "bg-red-100"
                                    : ""
                              }`}
                              type="text"
                              value={formColumnData.name.value}
                              onChange={(
                                 event: ChangeEvent<HTMLInputElement>
                              ) => {
                                 handleColumnNameChanged(
                                    event.target.value,
                                    formColumnData.index
                                 );
                              }}
                           ></input>
                           <span className="text-red-500">
                              {formColumnData.name.validationMessage}
                           </span>
                        </div>
                        <div className="col-span-1 text-right">
                           <IconButton
                              title="Remove column"
                              onClick={() => {
                                 handleDeleteColumn(formColumnData.index);
                              }}
                           >
                              <DeleteIcon />
                           </IconButton>
                        </div>
                     </div>
                  ))}
                  <div className="grid grid-cols-10 mt-1">
                     <div className="col-start-2 col-span-8">
                        <button
                           className="w-full border-2 hover:border-gray-700 rounded"
                           onClick={handleAddColumn}
                        >
                           <AddIcon />
                        </button>
                     </div>
                  </div>
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

export default BoardForm;
