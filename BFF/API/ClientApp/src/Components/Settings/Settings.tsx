import { Navigate, Route, Routes, useParams } from "react-router";
import RouterTabList, {
  RouterTabItemType,
} from "../../UIComponents/RouterTab/RouterTabList";
import ViewContainer from "../../UIComponents/ViewContainer/ViewContainer";
import clsx from "clsx";
import SettingsFilters from "./SettingsFilters";
import { Suspense } from "react";
import SettingsInfo from "./Info/SettingsInfo";
import FourOhOne from "../Errors/FourOhOne";
import PrivateRoute from "../../Utils/PrivateRouter";
import ContainerSpinner from "../../UIComponents/Spinner/ContainerSpinner";

// const SettingsMainQueryTag = graphql`
//   query SettingsMainQuery($Name: String!) {
//     hooks {
//       ...SettingsBodyFragment_failedJobs
//     }
//   }
// `;

const view_WebHooks = true;

export const SettingsTabs = [
  {
    label: "Informations",
    path: ``,
  },
  {
    label: "WebHooks",
    path: `Hooks`,
    pattern: "Hooks/*",
  },
] as RouterTabItemType[];

export default function Settings() {
  const { name }: any = useParams();

  // const root_data = useLazyLoadQuery<SettingsMainQuery>(
  //   SettingsMainQueryTag,
  //   { Name: name },
  //   { fetchPolicy: "store-or-network" }
  // );

  return (
    <div
      className={clsx(
        "flex flex-col md:flex-row space-x-0 md:space-x-5 xl:space-x-10",
        "space-y-2 md:space-y-0 w-full h-full"
      )}
    >
      <div className={clsx("flex w-full md:w-64 2xl:w-72 md:h-full")}>
        <TabsSection />
      </div>
      <div className="flex-1">
        <Suspense fallback={<ContainerSpinner />}>
          <Routes>
            <PrivateRoute
              path={SettingsTabs[1].path}
              authorised={true}
              element={<SettingsInfo />}
            />

            {/* <PrivateRoute
              path={`${SettingsTabs[3].path}/*`}
              authorised={view_WebHooks}
              unauthorisedComponent={<FourOhOne />}
              element={
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
                  <Route path={""}>
                    <SettingsHooks />
                  </Route>
                </Routes>
              }
            /> */}

            <Route path={"/*"} element={<Navigate to="" />} />
          </Routes>
        </Suspense>
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
