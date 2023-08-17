import { useSelector } from "react-redux";
import { RootState } from "../store";
import CreateProjectDialog from "../components/project/CreateProjectDialog";
import RootLayout from "./RootLayout";
import EditProjectDialog from "../components/project/EditProjectDialog";
import ProjectCard from "../components/project/ProjectCard";
import TicketListCard from "../components/ticket/TicketListCard";
import BoardList from "../components/board/BoardList";
import EditTicketDialog from "../components/ticket/EditTicketDialog";
import CreateTicketDialog from "../components/ticket/CreateTicketDialog";
import CreateBoardDialog from "../components/board/CreateBoardDialog";
import EditBoardDialog from "../components/board/EditBoardDialog";
import { ActiveProject } from "../store/projects/state";

const ProjectOverviewPage = () => {
   const uiState = useSelector((state: RootState) => state.ui);
   const activeProject: ActiveProject | null = useSelector(
      (state: RootState) => state.projects.activeProject
   );

   return (
      <RootLayout>
         {uiState.showCreateProjectDialog && <CreateProjectDialog />}
         {uiState.showEditProjectDialog && activeProject && (
            <EditProjectDialog project={activeProject.project} />
         )}
         {uiState.showEditTicketDialog && <EditTicketDialog />}
         {uiState.showCreateTicketDialog && <CreateTicketDialog />}
         {uiState.showCreateBoardDialog && <CreateBoardDialog />}
         {uiState.showEditBoardDialog && <EditBoardDialog />}
         {activeProject && (
            <>
               <ProjectCard project={activeProject.project} />
               <TicketListCard
                  projectId={activeProject.project.id}
                  tickets={activeProject.tickets}
               />
               <BoardList boards={activeProject.boards} />
            </>
         )}
      </RootLayout>
   );
};

export default ProjectOverviewPage;
