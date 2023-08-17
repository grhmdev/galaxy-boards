import { ChangeEvent, FormEvent, useCallback, useState } from "react";
import LabelledContent from "../common/LabelledContent";
import { Button } from "../common/Buttons";
import { useDispatch } from "react-redux";
import { showErrorAlert } from "../../store/alerts";
import {
   PostResult,
   PutResult,
   postProject,
   putProject,
} from "../../api/client";
import { ProjectDetail } from "../../api/types";

interface FormField {
   value: string;
   validationMessage: string | null;
}

interface FormData {
   name: FormField;
   hexColorCode: FormField;
}

const ProjectForm = (props: {
   project?: ProjectDetail;
   onProjectCreated?: (projectId: string) => void;
   onProjectModified?: () => void;
}) => {
   const dispatch = useDispatch();
   const [formData, setFormData] = useState<FormData>({
      name: {
         value: props.project ? props.project.name : "",
         validationMessage: null,
      },
      hexColorCode: {
         value: props.project ? props.project.hexColorCode : "#9010EE",
         validationMessage: null,
      },
   });

   const formDataValid = useCallback(() => {
      const name = formData.name.value.trim();
      const color = formData.hexColorCode.value.trim();
      return name.length > 0 && color.length > 0;
   }, [formData]);

   const createProjectOnServer = useCallback(
      async (name: string, color: string) => {
         const postResult: PostResult = await postProject(name, color);
         if (!postResult.success) {
            dispatch(
               showErrorAlert({
                  message: "Failed to create project on server",
               })
            );
            return;
         }
         props.onProjectCreated!(postResult.createdId);
      },
      [dispatch, props.onProjectCreated]
   );

   const updateProjectOnServer = useCallback(
      async (id: string, name: string, color: string) => {
         const result: PutResult = await putProject(id, name, color);
         if (!result.success) {
            dispatch(
               showErrorAlert({
                  message: "Failed to update project on server",
               })
            );
            return;
         }
         props.onProjectModified!();
      },
      [dispatch, props.onProjectModified]
   );

   const handleSubmit = useCallback(
      async (event: FormEvent) => {
         event.preventDefault();
         const name = formData.name.value.trim();
         const color = formData.hexColorCode.value.trim();

         if (!formDataValid()) {
            return;
         }

         if (!props.project) {
            createProjectOnServer(name, color);
         } else {
            updateProjectOnServer(props.project.id, name, color);
         }
      },
      [
         formData,
         formDataValid,
         props.project,
         createProjectOnServer,
         updateProjectOnServer,
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

   const handleColorChanged = (event: ChangeEvent<HTMLInputElement>) => {
      const color = event.target.value;
      let validationMessage: string | null = null;
      if (color.trim().length === 0) {
         validationMessage = "Required field";
      }
      setFormData((formData) => ({
         ...formData,
         hexColorCode: { value: color, validationMessage: validationMessage },
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
            label="Color"
            content={
               <>
                  <input
                     type="color"
                     className={`w-full rounded ${
                        formData.hexColorCode.validationMessage
                           ? "bg-red-100"
                           : ""
                     }`}
                     value={formData.hexColorCode.value}
                     onChange={handleColorChanged}
                  />
                  <span className="text-red-500">
                     {formData.hexColorCode.validationMessage}
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

export default ProjectForm;
