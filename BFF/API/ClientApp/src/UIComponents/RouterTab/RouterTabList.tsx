import clsx from "clsx";
import { AnimateSharedLayout } from "framer-motion";
import React, { useContext, useState } from "react";
import ScrollContainer from "react-indiana-drag-scroll";
import RouterTab from "./RouterTab";

export type RouterTabItemType = {
  label: string;
  path: string;
  pattern?: string;
};

const FLEX_VARIANT = {
  row: "flex-row",
  col: "flex-col space-y-0 divide-y",
  row_md_col: "flex-row space-y-0 md:flex-col md:space-y-0 md:divide-y",
};

export type RouterTabListProps = {
  Tabs: RouterTabItemType[];
  defaultIndex: number;
  hoverEffect?: boolean;
  tabWidth?: string;
  tabStyle?: string;
  flexVariant?: keyof typeof FLEX_VARIANT;
};

export type RouterTabContextType = {
  navigationTo: string | null | undefined;
  setNavigating: (location: string | null | undefined) => void;
};

export const RouterTabContext = React.createContext<RouterTabContextType>({
  setNavigating: () => {},
  navigationTo: undefined,
});

export const useRouterTabContext = () => useContext(RouterTabContext);

function RouterTabList({
  flexVariant = "row",
  Tabs,
  tabWidth,
  tabStyle,
  hoverEffect,
}: RouterTabListProps) {
  const [navigationState, setNavigationState] =
    useState<string | null | undefined>(undefined);

  const flex_Var = flexVariant ? FLEX_VARIANT[flexVariant] : FLEX_VARIANT.row;

  return (
    <RouterTabContext.Provider
      value={{
        navigationTo: navigationState,
        setNavigating: (value: any) => setNavigationState(value),
      }}
    >
      <AnimateSharedLayout>
        <ScrollContainer
          className={clsx("flex w-full", "border-none", flex_Var)}
        >
          {Tabs.map((enity, index) => {
            return (
              <RouterTab
                tabStyle={tabStyle}
                flexVariant={flexVariant}
                key={index}
                hoverEffect={hoverEffect}
                to={enity.path}
                width={tabWidth}
                pattern={enity.pattern}
                name={enity.label}
              />
            );
          })}
        </ScrollContainer>
      </AnimateSharedLayout>
    </RouterTabContext.Provider>
  );
}

export default React.memo(RouterTabList);
