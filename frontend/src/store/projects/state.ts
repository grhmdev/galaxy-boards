import {
   BoardDetail,
   ProjectBrief,
   ProjectDetail,
   TicketBrief,
} from "../../api/types";

export interface ActiveProject {
   project: ProjectDetail;
   boards: BoardDetail[];
   tickets: TicketBrief[];
}

export interface ProjectsState {
   projects: ProjectBrief[];
   activeProject: ActiveProject | null;
}

export const INITIAL_STATE: ProjectsState = {
   projects: [],
   activeProject: null,
};
