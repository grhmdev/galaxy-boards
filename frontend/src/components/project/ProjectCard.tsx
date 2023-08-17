import Card from "../common/Card";
import ColorPip from "../common/ColorPip";
import { useCallback } from "react";
import { reloadActiveProject } from "../../store/projects";
import { EditIcon, ProjectIcon, RefreshIcon } from "../common/Icons";
import { IconButton } from "../common/Buttons";
import { showSuccessAlert } from "../../store/alerts";
import { openEditProjectDialog } from "../../store/UI";
import { ProjectDetail } from "../../api/types";
import { useDispatch } from "react-redux";

const ProjectCard = (props: { project: ProjectDetail }) => {
   const dispatch = useDispatch();

   const handleReloadProject = useCallback(() => {
      dispatch(reloadActiveProject({}));
      dispatch(
         showSuccessAlert({ message: `Reloaded project ${props.project.name}` })
      );
   }, [dispatch, props.project.name]);

   const handleEditProject = useCallback(() => {
      dispatch(openEditProjectDialog({}));
   }, [dispatch]);

   return (
      <Card className="grid grid-cols-8">
         <div className="col-span-6">
            <ColorPip
               hexCodeCode={props.project.hexColorCode}
               className="mr-2"
            />
            <ProjectIcon className="mr-2" />
            <span className="">{props.project.name}</span>
         </div>

         <div className="text-right col-span-2">
            <IconButton
               title="Edit project"
               className="leading-[0px] p-1 rounded hover:bg-gray-100"
               onClick={handleEditProject}
            >
               <EditIcon className="" />
            </IconButton>
            <IconButton
               title="Reload project"
               className=""
               onClick={handleReloadProject}
            >
               <RefreshIcon className="" />
            </IconButton>
         </div>
      </Card>
   );
};

export default ProjectCard;
