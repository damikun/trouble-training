import React, { Suspense, useMemo } from 'react';
import './App.css';
import { Helmet } from 'react-helmet';
import ContainerSpinner from './UIComponents/Spinner/ContainerSpinner';
import { BrowserRouter as Router } from "react-router-dom";
import Providers from "./Utils/Providers";
import Layout from "./Components/Layout/Layout";
import LayoutBody from "./Components/Layout/LayoutBody";
import AppRoutes from "./Utils/AppRoutes";
import { motion } from "framer-motion";
import clsx from "clsx";
import LayoutHeader from "./Components/Layout/LayautHeader"

function App() {

  const Loader = useMemo(
    () => (
      <motion.div
      className={clsx("flex-1 h-full max-h-full")}
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ type: "tween", duration: 0.3, delay: 0.5 }}
      >
        <div className="w-full h-full">
          <ContainerSpinner />
        </div>
      </motion.div>
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
        content="TroubleTrain, Dalibor Kundrat, Distributed Tracing Workshop"
      />
    </Helmet>

    <Router>
      <Providers fallback={Loader}>
        <Suspense fallback={Loader}>
          <Layout
            header={<LayoutHeader/>}
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
