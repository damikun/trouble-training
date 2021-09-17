import { Navigate, Route, Routes } from "react-router";
import RouterTabList, {
  RouterTabItemType,
} from "../../UIComponents/RouterTab/RouterTabList";
import ViewContainer from "../../UIComponents/ViewContainer/ViewContainer";
import clsx from "clsx";
import SettingsFilters from "./SettingsFilters";
import { Suspense, useContext, useMemo, useState } from "react";
import Welcome from "./Info/Welcome";
import FourOhOne from "../Errors/FourOhOne";
import PrivateRoute from "../../Utils/PrivateRouter";
import ContainerSpinner from "../../UIComponents/Spinner/ContainerSpinner";
import SettingsHooks from "./WebHooks/SettingsHooks";
import SettingsHooksEdit from "./WebHooks/SettingsHooksEdit";
import SettingsHooksNew from "./WebHooks/SettingsHooksNew";
import SettingsHooksLogs from "./WebHooks/SettingsHooksLogs";
import React from "react";
import ErrorBoundary from "../../UIComponents/ErrorBoundery/ErrorBoundary";
import GlobalBounderyErrorHandler from "../Errors/GlobalBounderyErrorHandler";

const view_WebHooks = true;

export const SettingsTabs = [
  {
    label: "Welcome",
    path: ``,
  },
  {
    label: "WebHooks",
    path: `Hooks`,
    pattern: "Hooks/*",
  },
] as RouterTabItemType[];

export type HooksContextType = {
  connection_id: string;
  setConnectionId: (connection: string) => void;
};

export const HooksContext = React.createContext<
HooksContextType | undefined
>(undefined);

export const useHooksContext = () => useContext(HooksContext);

export default function Settings() {

  const [state, setState] = useState("");
  
  const ctx = useMemo(() => {
    return {
      connection_id: state,
      setConnectionId: (connection: string) => setState(connection),
    };
  }, [state, setState]);

  return (

    <div
      className={clsx(
        "flex h-full w-full",
        "text-xs md:text-sm max-h-full space-y-1",
        "p-5"
      )}
    >

      <div className="flex w-full max-w-5xl mx-auto mt-14">
      <div
        className={clsx(
          "flex flex-col md:flex-row space-x-0 md:space-x-5",
          "space-y-2 md:space-y-0 w-full h-full xl:space-x-10"
        )}
      >
        <div className={clsx("flex w-full md:w-64 2xl:w-72 md:h-full")}>
          <TabsSection />
        </div>
        <div className="flex-1">
        <ErrorBoundary
          fallback={<GlobalBounderyErrorHandler /> }
        >
            <Suspense fallback={<ContainerSpinner />}>
              <Routes>
                <PrivateRoute
                  path={SettingsTabs[0].path}
                  authorised={true}
                  element={<Welcome />}
                />

                <PrivateRoute
                  path={`${SettingsTabs[1].path}/*`}
                  authorised={view_WebHooks}
                  unauthorisedComponent={<FourOhOne />}
                  element={

                    <HooksContext.Provider value={ctx}>
                      <Routes>
                        <Route path={"Edit/:hookid"}>
                          <SettingsHooksEdit />
                        </Route>
                        <Route path={"Logs/:hookid"}>
                          <SettingsHooksLogs />
                        </Route>
                        <Route path={"New"}>
                          <SettingsHooksNew />
                        </Route>
                        <Route path={"/"}>
                          <SettingsHooks />
                        </Route>
                      </Routes>
                    </HooksContext.Provider>
                  }
                />

                <Route path={"/*"} element={<Navigate to="" />} />
              </Routes>
            </Suspense>
          </ErrorBoundary>
        </div>
      </div>

      </div>
      
    </div>
  );
}

///////////////////////////////////////////////////
///////////////////////////////////////////////////

function TabsSection() {
  return (
    <ViewContainer
      bgcolor="bg-transparent"
      shadow={false}
      border={false}
      padding={false}
      filters={<SettingsFilters />}
      content={
        <div className={clsx("flex flex-col  h-full")}>
          <div
            className={clsx(
              "flex w-full flex-row flex-wrap-reverse",
              "justify-center sm:justify-between",
              "overflow-x-scroll scrollbarwidth scrollbarhide",
              "scrollbarhide2 scrolling-touch items-end"
            )}
          >
            <div
              className={clsx(
                "flex flex-wrap w-full",
                "truncate justify-center"
              )}
            >
              <RouterTabList
                hoverEffect
                tabStyle={"bg-white hover:bg-gray-50 h-11"}
                flexVariant="row_md_col"
                defaultIndex={0}
                Tabs={SettingsTabs}
              />
            </div>
          </div>
        </div>
      }
    />
  );
}
