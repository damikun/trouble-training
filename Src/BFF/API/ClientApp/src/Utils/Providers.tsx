import React, { Suspense, useState, useContext, useMemo } from "react";
import { Environment } from "react-relay";
import { PreloadedQuery, RelayEnvironmentProvider } from "react-relay/hooks";
import ToastProvider from "../UIComponents/Toast/ToastProvider";
import { createEnvironment } from "./Environment";
import UserProvider from "./UserProvider";
import TraceProvider from "./TraceProvider"
import * as MeQuery from "../Utils/__generated__/UserProviderQuery.graphql";
import {RelayEnv} from "../index"

type Props = {
  children: React.ReactNode;
  fallback?: React.ReactNode;
  initialQueryRef: PreloadedQuery<MeQuery.UserProviderQuery>;
};

type EnviromentContextType = {
  env: Environment;
  reset(): void;
};

export const EnviromentContext = React.createContext<
  EnviromentContextType | undefined
>(undefined);

export const useEnvirometHandler = () => useContext(EnviromentContext);

export default function Providers({ children, fallback, initialQueryRef }: Props) {
  const [envState, setEnvState] = useState(RelayEnv);

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
                  <UserProvider initialQueryRef={initialQueryRef}>
                      <ToastProvider>{children}</ToastProvider>
                  </UserProvider>
                </TraceProvider>
              </Suspense>
            </RelayEnvironmentProvider>
          )
        }
      </EnviromentContext.Consumer>
    </EnviromentContext.Provider>
  );
}
