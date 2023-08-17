import { useCallback } from "react";
import Dialog from "../common/Dialog";
import ProjectForm from "./ProjectForm";
import { useDispatch } from "react-redux";
import { closeCreateProjectDialog } from "../../store/UI";
import {
   reloadProjectsFromServer,
   changeActiveProject,
} from "../../store/projects";

const CreateProjectDialog = () => {
   const dispatch = useDispatch();

   const closeDialog = useCallback(() => {
      dispatch(closeCreateProjectDialog({}));
   }, [dispatch]);

   const handleProjectCreated = useCallback(
      (projectId: string) => {
         closeDialog();
         dispatch(reloadProjectsFromServer({}));
         dispatch(changeActiveProject({ projectId }));
      },
      [dispatch, closeDialog]
   );

   return (
      <Dialog title="Create New Project" closeDialog={closeDialog}>
         <ProjectForm onProjectCreated={handleProjectCreated} />
      </Dialog>
   );
};

export default CreateProjectDialog;
