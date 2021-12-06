import RouterTabList from "../../UIComponents/RouterTab/RouterTabList";
import ViewContainer from "../../UIComponents/ViewContainer/ViewContainer";
import clsx from "clsx";
import TabsFilters from "./TabsFilters";
import { TabsSettings } from "./TabsSettings";


export default function TabsSection() {
    return (
      <ViewContainer
        bgcolor="bg-transparent"
        shadow={false}
        border={false}
        padding={false}
        filters={<TabsFilters />}
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
                  Tabs={TabsSettings}
                />
              </div>
            </div>
          </div>
        }
      />
    );
  }
  