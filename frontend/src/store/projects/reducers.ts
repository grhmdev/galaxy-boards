import { PayloadAction } from "@reduxjs/toolkit";
import { ProjectsState } from "./state";
import {
   ActiveProjectLoaded,
   BoardLoaded,
   ChangeActiveProject,
   ReloadProjectTickets,
   ProjectsLoaded,
   ReloadProjectsFromServer,
   TicketsLoaded,
} from "./actions";

export const projectsReducers = {
   activeProjectLoaded: (
      state: ProjectsState,
      action: PayloadAction<ActiveProjectLoaded>
   ) => {
      if (state.activeProject === null) {
         state.activeProject = {
            project: action.payload.project,
            tickets: [],
            boards: [],
         };
      } else {
         state.activeProject.project = action.payload.project;
      }
   },
   projectsLoaded: (
      state: ProjectsState,
      action: PayloadAction<ProjectsLoaded>
   ) => {
      state.projects = action.payload.projects;
   },
   clearTickets: (
      state: ProjectsState,
      action: PayloadAction<TicketsLoaded>
   ) => {
      if (!state.activeProject) return;
      state.activeProject.tickets = [];
   },
   ticketsLoaded: (
      state: ProjectsState,
      action: PayloadAction<TicketsLoaded>
   ) => {
      if (!state.activeProject) return;
      action.payload.tickets.forEach((ticket) => {
         const index = state.activeProject!.tickets.findIndex(
            (t) => t.id === ticket.id
         );
         if (index !== -1) {
            state.activeProject!.tickets[index] = ticket;
         } else {
            state.activeProject!.tickets.push(ticket);
         }
      });
   },
   boardLoaded: (state: ProjectsState, action: PayloadAction<BoardLoaded>) => {
      if (!state.activeProject) return;

      // Update existing board if held
      const index = state.activeProject.boards.findIndex(
         (b) => b.id === action.payload.board.id
      );
      if (index !== -1) {
         state.activeProject.boards[index] = action.payload.board;
      } else {
         state.activeProject.boards.push(action.payload.board);
      }
   },
   changeActiveProject: (
      state: ProjectsState,
      action: PayloadAction<ChangeActiveProject>
   ) => {
      // Reset active project state, to be replaced in `activeProjectLoaded`
      state.activeProject = null;
   },
   // The reducers below are provided only to be able to export
   // their action creation functions - the actions are processed
   // by the middleware, not here
   reloadProjectsFromServer: (
      state: ProjectsState,
      action: PayloadAction<ReloadProjectsFromServer>
   ) => {},
   reloadActiveProject: (
      state: ProjectsState,
      action: PayloadAction<ReloadProjectsFromServer>
   ) => {},
   reloadProjectTickets: (
      state: ProjectsState,
      action: PayloadAction<ReloadProjectTickets>
   ) => {},
   loadBoard: (
      state: ProjectsState,
      action: PayloadAction<ReloadProjectsFromServer>
   ) => {},
};
