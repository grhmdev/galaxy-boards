import { AddIcon } from "../common/Icons";
import { useCallback } from "react";
import { useDispatch } from "react-redux";
import { openCreateProjectDialog } from "../../store/UI";
import { useSelector } from "react-redux";
import { RootState } from "../../store";
import ColorPip from "../common/ColorPip";
import { changeActiveProject } from "../../store/projects";
import { IconButton } from "../common/Buttons";

const ProjectSelector = () => {
   const dispatch = useDispatch();
   const projectsState = useSelector((state: RootState) => state.projects);
   const handleCreateProject = useCallback(() => {
      dispatch(openCreateProjectDialog({}));
   }, [dispatch]);

   const handleSelectProject = useCallback(
      (projectId: string) => {
         dispatch(
            changeActiveProject({
               projectId,
            })
         );
      },
      [dispatch]
   );

   const projectIsActive = useCallback(
      (projectId: string): boolean => {
         return (
            projectsState.activeProject !== null &&
            projectId === projectsState.activeProject.project.id
         );
      },
      [projectsState.activeProject]
   );

   return (
      <>
         <div className="grid grid-cols-2 px-1">
            <div>Projects</div>
            <div className="text-right">
               <IconButton
                  title="Create new project"
                  className="text-white hover:bg-transparent p-0"
                  onClick={handleCreateProject}
               >
                  <AddIcon className="" />
               </IconButton>
            </div>
         </div>
         {projectsState.projects.length > 0 && (
            <ul className="">
               {projectsState.projects.map((project) => (
                  <li key={project.id} className="">
                     <button
                        className={`p-1 text-left text-sm w-full ${
                           projectIsActive(project.id)
                              ? "bg-gradient-to-r from-violet-600 to-blue-500"
                              : "hover:bg-slate-700"
                        }`}
                        onClick={() => {
                           handleSelectProject(project.id);
                        }}
                        disabled={projectIsActive(project.id)}
                     >
                        <ColorPip
                           hexCodeCode={project.hexColorCode}
                           className="mr-2"
                        />
                        {project.name}
                     </button>
                  </li>
               ))}
            </ul>
         )}
         {projectsState.projects.length === 0 && (
            <p className="text-center text-xs">No projects found</p>
         )}
      </>
   );
};

export default ProjectSelector;
