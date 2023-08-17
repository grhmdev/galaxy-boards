import { useCallback } from "react";
import Dialog from "../common/Dialog";
import { useDispatch } from "react-redux";
import { closeEditProjectDialog } from "../../store/UI";
import {
   reloadProjectsFromServer,
   reloadActiveProject,
} from "../../store/projects";
import ProjectForm from "./ProjectForm";
import { ProjectDetail } from "../../api/types";
import { showSuccessAlert } from "../../store/alerts";

const EditProjectDialog = (props: { project: ProjectDetail }) => {
   const dispatch = useDispatch();

   const closeDialog = useCallback(() => {
      dispatch(closeEditProjectDialog({}));
   }, [dispatch]);

   const handleProjectModified = useCallback(() => {
      closeDialog();
      dispatch(reloadProjectsFromServer({}));
      dispatch(reloadActiveProject({}));
      dispatch(
         showSuccessAlert({
            message: `Project updated`,
         })
      );
   }, [dispatch, closeDialog]);

   return (
      <Dialog title="Edit Project" closeDialog={closeDialog}>
         <ProjectForm
            project={props.project}
            onProjectModified={handleProjectModified}
         />
      </Dialog>
   );
};

export default EditProjectDialog;
