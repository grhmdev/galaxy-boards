import { PayloadAction } from "@reduxjs/toolkit";
import { UIState } from "./state";
import {
   CloseCreateBoardDialog,
   CloseCreateProjectDialog,
   CloseCreateTicketDialog,
   CloseEditBoardDialog,
   CloseEditProjectDialog,
   CloseEditTicketDialog,
   OpenCreateBoardDialog,
   OpenCreateProjectDialog,
   OpenCreateTicketDialog,
   OpenEditBoardDialog,
   OpenEditProjectDialog,
   OpenEditTicketDialog,
} from "./actions";

export const uiReducers = {
   openCreateProjectDialog: (
      state: UIState,
      action: PayloadAction<OpenCreateProjectDialog>
   ) => {
      state.showCreateProjectDialog = true;
   },
   closeCreateProjectDialog: (
      state: UIState,
      action: PayloadAction<CloseCreateProjectDialog>
   ) => {
      state.showCreateProjectDialog = false;
   },
   openEditProjectDialog: (
      state: UIState,
      action: PayloadAction<OpenEditProjectDialog>
   ) => {
      state.showEditProjectDialog = true;
   },
   closeEditProjectDialog: (
      state: UIState,
      action: PayloadAction<CloseEditProjectDialog>
   ) => {
      state.showEditProjectDialog = false;
   },
   openEditTicketDialog: (
      state: UIState,
      action: PayloadAction<OpenEditTicketDialog>
   ) => {
      state.showEditTicketDialog = true;
      state.editTicketId = action.payload.ticketId;
   },
   closeEditTicketDialog: (
      state: UIState,
      action: PayloadAction<CloseEditTicketDialog>
   ) => {
      state.showEditTicketDialog = false;
      state.editTicketId = null;
   },
   openCreateTicketDialog: (
      state: UIState,
      action: PayloadAction<OpenCreateTicketDialog>
   ) => {
      state.showCreateTicketDialog = true;
   },
   closeCreateTicketDialog: (
      state: UIState,
      action: PayloadAction<CloseCreateTicketDialog>
   ) => {
      state.showCreateTicketDialog = false;
   },
   openEditBoardDialog: (
      state: UIState,
      action: PayloadAction<OpenEditBoardDialog>
   ) => {
      state.showEditBoardDialog = true;
      state.editBoardId = action.payload.boardId;
   },
   closeEditBoardDialog: (
      state: UIState,
      action: PayloadAction<CloseEditBoardDialog>
   ) => {
      state.showEditBoardDialog = false;
      state.editBoardId = null;
   },
   openCreateBoardDialog: (
      state: UIState,
      action: PayloadAction<OpenCreateBoardDialog>
   ) => {
      state.showCreateBoardDialog = true;
   },
   closeCreateBoardDialog: (
      state: UIState,
      action: PayloadAction<CloseCreateBoardDialog>
   ) => {
      state.showCreateBoardDialog = false;
   },
};
