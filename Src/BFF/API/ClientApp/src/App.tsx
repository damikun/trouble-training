// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

import React, { Suspense, useMemo } from 'react';
import './App.css';
import { Helmet } from 'react-helmet';
import { PreloadedQuery } from "react-relay";
import ContainerSpinner from './UIComponents/Spinner/ContainerSpinner';
import { BrowserRouter as Router } from "react-router-dom";
import Providers from "./Utils/Providers";
import Layout from "./Components/Layout/Layout";
import LayoutBody from "./Components/Layout/LayoutBody";
import AppRoutes from "./Utils/AppRoutes";
import LayoutHeader from "./Components/Layout/LayautHeader"
import * as MeQuery from "./Utils/__generated__/UserProviderQuery.graphql";

interface Props {
  initialQueryRef: PreloadedQuery<MeQuery.UserProviderQuery>;
}

function App({ initialQueryRef }: Props) {

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
      <title>Trouble Training - Demo</title>
      <meta
        name="description"
        content="Trouble training demo - workshop for full stack app"
      />
      <meta name="author" content="TroubleDev" />
      <meta
        name="keywords"
        content="TroubleTraining, Distributed Tracing Workshop"
      />
    </Helmet>

    <Router>
      <Providers initialQueryRef={initialQueryRef} fallback={Loader}>
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
