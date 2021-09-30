import { lazy } from "react";
import { Route, Routes } from "react-router-dom";

const Home = lazy(
  () =>
    import(
      /* webpackChunkName: "Home" */ "../Components/Home"
    )
);

export default function AppRoutes() {

  return  (  
    <Routes>
      <Route path="/*" element={<Home/>} />
    </Routes> 
  );
  
}
