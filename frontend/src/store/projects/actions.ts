import {
   BoardDetail,
   ProjectBrief,
   ProjectDetail,
   TicketBrief,
} from "../../api/types";

export interface ChangeActiveProject {
   projectId: string;
}
export interface ReloadActiveProject {}
export interface ReloadProjectsFromServer {}
export interface ReloadProjectTickets {
   searchQuery?: string;
}
export interface LoadBoard {
   boardId: string;
}
export interface ActiveProjectLoaded {
   project: ProjectDetail;
}
export interface ProjectsLoaded {
   projects: ProjectBrief[];
}
export interface TicketsLoaded {
   tickets: TicketBrief[];
}
export interface BoardLoaded {
   board: BoardDetail;
}
