import React, { Suspense } from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { RelayEnvironmentProvider } from "react-relay/hooks";
import { createEnvironment } from './Utils/Environment';



ReactDOM.render(
  <React.StrictMode>
    <RelayEnvironmentProvider environment={createEnvironment()}>
      <Suspense fallback={null}>
        <App />
      </Suspense>
    </RelayEnvironmentProvider>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
