import { useEffect } from "react";
import "./App.scss";
import { useDispatch, useSelector } from "react-redux";
import {
   changeActiveProject,
   reloadProjectsFromServer,
} from "./store/projects";
import store, { RootState } from "./store";
import ProjectOverviewPage from "./pages/ProjectOverviewPage";

const App = () => {
   const dispatch = useDispatch();
   const projects = useSelector((state: RootState) => state.projects.projects);
   useEffect(() => {
      dispatch(reloadProjectsFromServer({}));
   }, [dispatch]);

   useEffect(() => {
      // When we do not have an active project selected
      // (e.g. on first page load), then select the first
      // project found as active
      if (!store.getState().projects.activeProject && projects.length > 0) {
         dispatch(
            changeActiveProject({
               projectId: projects[0].id,
            })
         );
      }
   }, [dispatch, projects]);

   return <ProjectOverviewPage></ProjectOverviewPage>;
};

export default App;
