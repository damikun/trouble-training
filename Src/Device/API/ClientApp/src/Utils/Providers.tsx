import React, { Suspense, useState, useContext, useMemo, useEffect } from "react";
import { Environment } from "react-relay";
import { RelayEnvironmentProvider } from "react-relay/hooks";
import ToastProvider from "../UIComponents/Toast/ToastProvider";
import { createEnvironment } from "./Environment";
import TraceProvider from "./TraceProvider"

type Props = {
  children: React.ReactNode;
  fallback?: React.ReactNode;
};

type EnviromentContextType = {
  env: Environment;
  reset(): void;
};

export const EnviromentContext = React.createContext<
  EnviromentContextType | undefined
>(undefined);

export const useEnvirometHandler = () => useContext(EnviromentContext);

export default function Providers({ children, fallback }: Props) {
  const [envState, setEnvState] = useState(createEnvironment());

  useEffect(() => {
  }, [])

  const providerInit = useMemo(() => {
    return {
      env: envState,
      reset() {
        setEnvState(createEnvironment());
      },
    };
  }, [envState, setEnvState]);

  return (
    <EnviromentContext.Provider value={providerInit}>
      <EnviromentContext.Consumer>
        {(state) =>
          state && (
            <RelayEnvironmentProvider environment={state?.env}>
              <Suspense fallback={fallback ? fallback : null}>
                <TraceProvider>
                    <ToastProvider>{children}</ToastProvider>
                </TraceProvider>
              </Suspense>
            </RelayEnvironmentProvider>
          )
        }
      </EnviromentContext.Consumer>
    </EnviromentContext.Provider>
  );
}