import { createSlice } from "@reduxjs/toolkit";
import { INITIAL_STATE } from "./state";
import { uiReducers } from "./reducers";

const uiSlice = createSlice({
   name: "UI",
   initialState: INITIAL_STATE,
   reducers: uiReducers,
});

export const uiReducer = uiSlice.reducer;
export const {
   openCreateProjectDialog,
   closeCreateProjectDialog,
   openEditProjectDialog,
   closeEditProjectDialog,
   openEditTicketDialog,
   closeEditTicketDialog,
   openCreateTicketDialog,
   closeCreateTicketDialog,
   openCreateBoardDialog,
   closeCreateBoardDialog,
   openEditBoardDialog,
   closeEditBoardDialog,
} = uiSlice.actions;
