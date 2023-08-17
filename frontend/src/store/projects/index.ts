import { createSlice } from "@reduxjs/toolkit";
import { INITIAL_STATE } from "./state";
import { projectsReducers } from "./reducers";

const projectsSlice = createSlice({
   name: "projects",
   initialState: INITIAL_STATE,
   reducers: projectsReducers,
});

export const projectsReducer = projectsSlice.reducer;
export const {
   changeActiveProject,
   activeProjectLoaded,
   reloadProjectsFromServer,
   projectsLoaded,
   reloadActiveProject,
   boardLoaded,
   ticketsLoaded,
   reloadProjectTickets,
   loadBoard,
} = projectsSlice.actions;
