import { lazy } from "react";
import { Route, Routes } from "react-router-dom";
import PrivateRoute from "./PrivateRouter";
import FourOhFour from "../Components/Errors/FourOhFour";
import { useUserStore } from "./UserProvider";
import Login from "../Components/Login";
import Logout from "../Components/Logout";

const Settings = lazy(
  () =>
    import(
      /* webpackChunkName: "Settings" */ "../Components/Settings/Settings"
    )
);

export default function AppRoutes() {
  const userStore = useUserStore();

  if (!userStore?.user?.me) {
    return (
      <Routes>
        <Route path="/*" element={<Login/>} />
      </Routes>
    );
  } else {
    return (
      <Routes>

        <PrivateRoute  path={"/*"} element={<Settings />} />
        
        <PrivateRoute  path={"/logout"} element={<Logout />} />

        <PrivateRoute path="*" element={<FourOhFour />} />

        

      </Routes>
    );
  }
}
