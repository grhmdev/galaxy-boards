import { Middleware } from "@reduxjs/toolkit";
import store, { AppDispatch, RootState } from "..";
import {
   QueryParameters,
   getBoard,
   getProject,
   listProjects,
   queryTickets,
} from "../../api/client";
import {
   activeProjectLoaded,
   projectsLoaded,
   reloadProjectsFromServer,
   changeActiveProject,
   reloadActiveProject,
   boardLoaded,
   ticketsLoaded,
   reloadProjectTickets as reloadProjectTicketsAction,
   loadBoard as loadBoardAction,
} from ".";
import { showErrorAlert } from "../alerts";
import { ProjectDetail } from "../../api/types";

export const dataLoader: Middleware<{}, RootState> =
   (storeApi) => (next) => async (action) => {
      next(action);
      if (action.type === reloadProjectsFromServer({}).type) {
         loadProjectBriefs(storeApi.dispatch);
      } else if (action.type === changeActiveProject({ projectId: "" }).type) {
         loadActiveProject(action.payload.projectId, store.dispatch);
      } else if (action.type === reloadActiveProject({}).type) {
         loadActiveProject(
            store.getState().projects.activeProject!.project.id,
            store.dispatch
         );
      } else if (action.type === reloadProjectTicketsAction({}).type) {
         if (!store.getState().projects.activeProject) return;
         const queryParams: QueryParameters = {
            searchQuery: action.payload.searchQuery,
         };

         reloadProjectTickets(
            store.getState().projects.activeProject!.project.id,
            store.dispatch,
            queryParams
         );
      } else if (action.type === loadBoardAction({ boardId: "" }).type) {
         loadBoard(action.payload.boardId, storeApi.dispatch);
      }
   };

const loadProjectBriefs = (dispatch: AppDispatch) => {
   listProjects().then((result) => {
      if (result.success) {
         dispatch(
            projectsLoaded({
               projects: result.items,
            })
         );
      }
   });
};

const loadActiveProject = (projectId: string, dispatch: AppDispatch) => {
   getProject(projectId).then((result) => {
      if (!result.success || !result.data) {
         dispatch(
            showErrorAlert({
               message: "Failed to retrieve project data from the server",
            })
         );
      }
      const project = result.data!;
      dispatch(
         activeProjectLoaded({
            project,
         })
      );
      reloadProjectTickets(projectId, dispatch);
      loadProjectBoards(project, dispatch);
   });
};

const loadProjectBoards = async (
   project: ProjectDetail,
   dispatch: AppDispatch
) => {
   project.boards.forEach((bb) => {
      loadBoard(bb.id, dispatch);
   });
};

const loadBoard = (boardId: string, dispatch: AppDispatch) => {
   getBoard(boardId).then((result) => {
      if (result.success && result.data) {
         const board = result.data!;
         dispatch(
            boardLoaded({
               board,
            })
         );
      }
   });
};

const reloadProjectTickets = (
   projectId: string,
   dispatch: AppDispatch,
   params?: QueryParameters
) => {
   const paramsWithProjectFilter = params ? params : {};
   if (!paramsWithProjectFilter.filters) {
      paramsWithProjectFilter.filters = [];
   }
   paramsWithProjectFilter.filters.push({ key: "projectId", value: projectId });
   queryTickets(paramsWithProjectFilter).then((result) => {
      if (!result.success) return;
      const tickets = result.items;
      dispatch(
         ticketsLoaded({
            tickets,
         })
      );
   });
};
