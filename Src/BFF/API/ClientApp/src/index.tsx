import './index.css';
import ReactDOM from 'react-dom';
import React, { lazy, Suspense } from 'react';
import reportWebVitals from './reportWebVitals';
import { createEnvironment } from './Utils/Environment';
import { loadQuery } from "react-relay";
import ErrorBoundary from './UIComponents/ErrorBoundery/ErrorBoundary';
import * as MeQuery from "./Utils/__generated__/UserProviderQuery.graphql";
import GlobalBounderyErrorHandler from './Components/Errors/GlobalBounderyErrorHandler';

const App = lazy(
  () =>
    import(
      /* webpackChunkName: "App" */ "./App"
    )
);

export const RelayEnv = createEnvironment();

const initialQueryRef = loadQuery<MeQuery.UserProviderQuery>(
  RelayEnv,
  MeQuery.default,
  {}
);

ReactDOM.createRoot(document.getElementById("root")!).render(
<React.StrictMode>
    <Suspense fallback={null}>
      <ErrorBoundary
          fallback={<GlobalBounderyErrorHandler /> }
        >
        <App initialQueryRef={initialQueryRef} />
      </ErrorBoundary>
    </Suspense>
</React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();