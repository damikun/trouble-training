import React, { Suspense, useMemo } from 'react';
import './App.css';
import { Helmet } from 'react-helmet';
import ContainerSpinner from './UIComponents/Spinner/ContainerSpinner';
import { BrowserRouter as Router } from "react-router-dom";
import Providers from "./Utils/Providers";
import Layout from "./Components/Layout/Layout";
import LayoutBody from "./Components/Layout/LayoutBody";
import AppRoutes from "./Utils/AppRoutes";

function App() {

  const Loader = useMemo(
    () => (
      <div className="w-full h-full">
        <ContainerSpinner />
      </div>
    ),
    []
  );
  return (
    <>
    <Helmet>
      <title>Trouble Train - Demo</title>
      <meta
        name="description"
        content="Trouble train demo - workshop for full stack app"
      />
      <meta name="author" content="TroubleDev, Dalibor Kundrat" />
      <meta
        name="keywords"
        content="TroubleTrain, Dalibor Kundratm, Distributed Tracing Workshop"
      />
    </Helmet>

    <Router>
      <Providers fallback={Loader}>
        <Suspense fallback={Loader}>
          <Layout
            content={
              <LayoutBody>
                <Suspense fallback={Loader}>
                  <AppRoutes />
                </Suspense>
              </LayoutBody>
            }
          />
        </Suspense>
      </Providers>
    </Router>
  </>
  );
}

export default App;
