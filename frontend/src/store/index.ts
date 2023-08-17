import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { Middleware } from "redux";
import { alertsReducer } from "./alerts";
import { projectsReducer } from "./projects";
import { uiReducer } from "./UI";
import { dataLoader } from "./projects/middleware";

export const logger: Middleware<{}, RootState> =
   (storeApi) => (next) => (action) => {
      console.group(action.type);
      console.debug(action.payload);
      next(action);
      console.groupEnd();
   };

const rootReducer = combineReducers({
   alerts: alertsReducer,
   projects: projectsReducer,
   ui: uiReducer,
});
export type RootState = ReturnType<typeof rootReducer>;

const store = configureStore({
   reducer: rootReducer,
   middleware: (getDefaultMiddleware: any) =>
      getDefaultMiddleware().prepend([logger, dataLoader]),
});

export default store;
export type AppDispatch = typeof store.dispatch;
